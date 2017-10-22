namespace Autofac.Extras.NLog.Tests
{
   using System;
   using System.Diagnostics;
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
      private ILogger logger;
      private string testMessage;

      [ TestFixtureSetUp ]
      public void Set_up_test_fixture()
      {
         // Configure NLog to log to memory to simplify testing
         inMemory = new MemoryTarget { Layout = layout, Name = "memory" };
         SimpleConfigurator.ConfigureForTargetLogging( inMemory, NLogFramework.LogLevel.Trace );

         // Configure Autofac container
         ContainerBuilder containerBuilder = new ContainerBuilder();

         containerBuilder.RegisterModule<NLogModule>();
         containerBuilder.RegisterType<SampleClassWithConstructorDependency>().Named<ISampleClass>( "constructor" );
         containerBuilder.RegisterType<SampleClassWithPropertyDependency>().Named<ISampleClass>( "property" );

         container = containerBuilder.Build();

         // Grab an ILogger which should be an instance of LoggerAdapter
         logger = container.ResolveNamed<ISampleClass>( "property" ).GetLogger();
         logger.Should().NotBeNull( because: "should be provided by NLogModule" )
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

         testMessage = $"{Guid.NewGuid():N}";
      }

      private void Confirm_callsite_value()
      {
         var logEntry = inMemory.Logs.Last( x => x.Contains( testMessage ) );
         logEntry.Should().NotBeNull( because: "should have logged the message" );

         Match match = logLinePattern.Match( logEntry );
         match.Success.Should().BeTrue( because: "log entry layout should match the regex pattern" );

         Console.WriteLine( $"Log entry: {logEntry}" );
         Console.WriteLine( $"Callsite: {match.Groups[ "callsite" ].Value}" );

         match.Groups[ "callsite" ].Value.Should().Be( $"{GetType().FullName}.{new StackFrame(1).GetMethod().Name}",
                                                       because: "should use the test method as the callsite context" );
      }

      [ Test ]
      public void Passing_a_log_event_info_instance()
      {
         logger.Log( new NLogFramework.LogEventInfo( NLogFramework.LogLevel.Debug, "Something", new CultureInfo( "en-GB" ), testMessage, null ) );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_log_level_and_value()
      {
         logger.Log( NLogFramework.LogLevel.Debug, new { test = testMessage } );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_format_and_value()
      {
         logger.Log( NLogFramework.LogLevel.Debug, new CultureInfo( "ja-JP" ), new { Timestamp = DateTime.Now, test = testMessage } );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Calling_log_exception()
      {
         logger.LogException( NLogFramework.LogLevel.Debug, testMessage, new Exception( "This is a test exception!" ) );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_format_message_and_args()
      {
         logger.Log( NLogFramework.LogLevel.Debug, new CultureInfo( "ja-JP" ), "Date: {0}, test: {1}", new object[] { DateTime.Now, testMessage } );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_and_message()
      {
         logger.Log( NLogFramework.LogLevel.Debug, testMessage );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_message_and_args()
      {
         logger.Log( NLogFramework.LogLevel.Debug, "Date: {0}, Message: {1}", new object[] { DateTime.Now, testMessage } );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_format_message_and_typed_arg()
      {
         logger.Log( NLogFramework.LogLevel.Debug, new CultureInfo( "ja-JP" ), $"Date: {{0}}, Message: {testMessage}", DateTime.Now );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_message_and_typed_argument()
      {
         logger.Log( NLogFramework.LogLevel.Debug, $"Date: {{0}}, Message: {testMessage}", DateTime.Now );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_format_message_and_two_typed_arguments()
      {
         logger.Log( NLogFramework.LogLevel.Debug, new CultureInfo( "ja-JP" ), $"Start: {{0}}, End: {{1}}, {testMessage}", DateTime.Today, DateTime.Now );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_message_and_two_typed_arguments()
      {
         logger.Log( NLogFramework.LogLevel.Debug, $"Start: {{0}}, End: {{1}}, {testMessage}", DateTime.Today, DateTime.Now );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_format_message_and_three_typed_arguments()
      {
         logger.Log( NLogFramework.LogLevel.Debug, new CultureInfo( "ja-JP" ), $"Start: {{0}}, End: {{1}}, Max: {{2}}, {testMessage}",
                     DateTime.Today, DateTime.Now, DateTime.MaxValue );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_message_and_three_typed_arguments()
      {
         logger.Log( NLogFramework.LogLevel.Debug, $"Start: {{0}}, End: {{1}}, Max: {{2}}, {testMessage}", DateTime.Today, DateTime.Now, DateTime.MaxValue );

         Confirm_callsite_value();
      }
   }
}
