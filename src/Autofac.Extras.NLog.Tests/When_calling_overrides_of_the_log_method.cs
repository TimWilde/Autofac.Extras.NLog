namespace Autofac.Extras.NLog.Tests
{
   using System;
   using System.Globalization;
   using NUnit.Framework;
   using NLogFramework = global::NLog;

   [ TestFixture ]
   public class When_calling_overrides_of_the_log_method: LoggerAdapterTestsBase
   {
      [ Test ]
      public void Calling_log_exception()
      {
         Logger.LogException( NLogFramework.LogLevel.Debug, TestMessage, new Exception( "This is a test exception!" ) );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_a_log_event_info_instance()
      {
         Logger.Log( new NLogFramework.LogEventInfo( NLogFramework.LogLevel.Debug, "Something", new CultureInfo( "en-GB" ), TestMessage, null ) );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_log_level_and_value()
      {
         Logger.Log( NLogFramework.LogLevel.Debug, new { test = TestMessage } );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_format_and_value()
      {
         Logger.Log( NLogFramework.LogLevel.Debug, new CultureInfo( "ja-JP" ), new { Timestamp = DateTime.Now, test = TestMessage } );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_format_message_and_args()
      {
         Logger.Log( NLogFramework.LogLevel.Debug, new CultureInfo( "ja-JP" ), "Date: {0}, test: {1}", new object[] { DateTime.Now, TestMessage } );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_and_message()
      {
         Logger.Log( NLogFramework.LogLevel.Debug, TestMessage );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_message_and_args()
      {
         Logger.Log( NLogFramework.LogLevel.Debug, "Date: {0}, Message: {1}", new object[] { DateTime.Now, TestMessage } );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_format_message_and_typed_arg()
      {
         Logger.Log( NLogFramework.LogLevel.Debug, new CultureInfo( "ja-JP" ), $"Date: {{0}}, Message: {TestMessage}", DateTime.Now );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_message_and_typed_argument()
      {
         Logger.Log( NLogFramework.LogLevel.Debug, $"Date: {{0}}, Message: {TestMessage}", DateTime.Now );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_format_message_and_two_typed_arguments()
      {
         Logger.Log( NLogFramework.LogLevel.Debug, new CultureInfo( "ja-JP" ), $"Start: {{0}}, End: {{1}}, {TestMessage}", DateTime.Today, DateTime.Now );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_message_and_two_typed_arguments()
      {
         Logger.Log( NLogFramework.LogLevel.Debug, $"Start: {{0}}, End: {{1}}, {TestMessage}", DateTime.Today, DateTime.Now );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_format_message_and_three_typed_arguments()
      {
         Logger.Log( NLogFramework.LogLevel.Debug, new CultureInfo( "ja-JP" ), $"Start: {{0}}, End: {{1}}, Max: {{2}}, {TestMessage}",
                     DateTime.Today, DateTime.Now, DateTime.MaxValue );

         Confirm_callsite_value();
      }

      [ Test ]
      public void Passing_level_message_and_three_typed_arguments()
      {
         Logger.Log( NLogFramework.LogLevel.Debug, $"Start: {{0}}, End: {{1}}, Max: {{2}}, {TestMessage}", DateTime.Today, DateTime.Now, DateTime.MaxValue );

         Confirm_callsite_value();
      }
   }
}
