namespace Autofac.Extras.NLog.Tests
{
   using System;
   using System.Diagnostics;
   using System.Linq;
   using System.Text.RegularExpressions;
   using FluentAssertions;
   using global::NLog;
   using global::NLog.Config;
   using global::NLog.Targets;
   using NUnit.Framework;

   public class LoggerAdapterTestsBase
   {
      private static readonly string layout = "${longdate} ${uppercase:${level}} ${callsite} ${message}${onexception:${newline}${exception}${newline}}";
      private readonly Regex logLinePattern = new Regex( @"^(?<date>[\d-]*)\s(?<time>[\d:.]*)\s(?<level>[^\s]+)\s(?<callsite>[\w.]+)\s(?<message>[^$]*)$", RegexOptions.Compiled );

      private IContainer container;
      private MemoryTarget inMemory;
      protected NLog.ILogger Logger;
      protected string TestMessage;

      [ TestFixtureSetUp ]
      public void Set_up_test_fixture()
      {
         // Configure NLog to log to memory to simplify testing
         inMemory = new MemoryTarget { Layout = layout, Name = "memory" };
         SimpleConfigurator.ConfigureForTargetLogging( inMemory, LogLevel.Trace );

         // Configure Autofac container
         ContainerBuilder containerBuilder = new ContainerBuilder();

         containerBuilder.RegisterModule<NLogModule>();
         containerBuilder.RegisterType<SampleClassWithConstructorDependency>().Named<ISampleClass>( "constructor" );
         containerBuilder.RegisterType<SampleClassWithPropertyDependency>().Named<ISampleClass>( "property" );

         container = containerBuilder.Build();

         // Grab an ILogger which should be an instance of LoggerAdapter
         Logger = container.ResolveNamed<ISampleClass>( "property" ).GetLogger();
         Logger.Should().NotBeNull( because: "should be provided by NLogModule" )
               .And.BeOfType<LoggerAdapter>( because: "the LoggerAdapter should be provided here" );
      }

      [ TestFixtureTearDown ]
      public void Tear_down_test_fixture()
      {
         container.Dispose();
         inMemory.Dispose();
      }

      [ SetUp ]
      public void Set_up_test()
      {
         inMemory.Logs.Clear();
         inMemory.Logs.Count.Should().Be( 0, because: "logs should have been cleared" );

         TestMessage = $"{Guid.NewGuid():N}";
      }

      protected void Confirm_callsite_value( LogLevel level = null )
      {
         var logEntry = inMemory.Logs.Last( x => x.Contains( TestMessage ) );
         logEntry.Should().NotBeNull( because: "should have logged the message" );

         Match match = logLinePattern.Match( logEntry );
         match.Success.Should().BeTrue( because: "log entry layout should match the regex pattern" );

         Console.WriteLine( $"Log entry: {logEntry}" );
         Console.WriteLine( $"Callsite: {match.Groups[ "callsite" ].Value}" );

         match.Groups[ "callsite" ].Value.Should().Be( $"{GetType().FullName}.{new StackFrame( 1 ).GetMethod().Name}",
                                                       because: "should use the test method as the callsite context" );

         if( level == null ) return;

         match.Groups[ "level" ].Value.Should().NotBeNull( because: "this test specifies a log level" );

         var loggedLevel = LogLevel.FromString( match.Groups[ "level" ].Value );
         Console.WriteLine( $"Level: {loggedLevel} (expected: {level})" );

         loggedLevel.Should().Be( level, because: $"the test expects it to be {level}" );
      }
   }
}
