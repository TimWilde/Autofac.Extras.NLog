namespace Autofac.Extras.NLog.Tests
{
   using System;
   using System.Globalization;
   using System.Linq;
   using System.Text.RegularExpressions;
   using FluentAssertions;
   using global::NLog.Config;
   using global::NLog.Targets;
   using NUnit.Framework;
   using NLogFramework = global::NLog;

   [ TestFixture ]
   public class LoggerAdapterTests
   {
      private static readonly string layout = "${longdate} ${uppercase:${level}} ${callsite} ${message}${onexception:${newline}${exception}${newline}}";
      private readonly Regex logLinePattern = new Regex( @"^(?<date>[\d-]*)\s(?<time>[\d:.]*)\s(?<level>[^\s]+)\s(?<callsite>[\w.]+)\s(?<message>[^$]*)$", RegexOptions.Compiled );

      private IContainer container;
      private MemoryTarget inMemory;

      [ TestFixtureSetUp ]
      public void Set_up_test_fixture()
      {
         inMemory = new MemoryTarget { Layout = layout, Name = "memory" };
         SimpleConfigurator.ConfigureForTargetLogging( inMemory, NLogFramework.LogLevel.Trace );

         ContainerBuilder containerBuilder = new ContainerBuilder();

         containerBuilder.RegisterModule<NLogModule>();
         containerBuilder.RegisterType<SampleClassWithConstructorDependency>().Named<ISampleClass>( "constructor" );
         containerBuilder.RegisterType<SampleClassWithPropertyDependency>().Named<ISampleClass>( "property" );

         container = containerBuilder.Build();
      }

      [ SetUp ]
      public void Set_up_test_context()
      {
         inMemory.Logs.Clear();
         inMemory.Logs.Count.Should().Be( 0, because: "logs should have been cleared." );
      }

      [ TestFixtureTearDown ]
      public void Tear_down_test_fixture()
      {
         container.Dispose();
         inMemory.Dispose();
      }

      [ Test ]
      public void Log_message_contains_correct_callsite()
      {
         ILogger logger = container.ResolveNamed<ISampleClass>( "property" ).GetLogger();
         logger.Should().NotBeNull( because: "should be provided by NLogModule" );

         string testId = $"{Guid.NewGuid():N}";

         logger.Log( new NLogFramework.LogEventInfo( NLogFramework.LogLevel.Debug, "Something", new CultureInfo( "en-GB" ), testId, null ) );

         var logEntry = inMemory.Logs.Last( x => x.Contains( testId ) );
         logEntry.Should().NotBeNull( because: "should have logged the message" );

         Match match = logLinePattern.Match( logEntry );
         match.Success.Should().BeTrue( because: "log entry should match the regex pattern" );

         match.Groups[ "callsite" ].Value.Should().Be( $"{GetType().FullName}.{nameof(Log_message_contains_correct_callsite)}",
                                                       because: "should use the client code calling context" );
      }
   }
}
