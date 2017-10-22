namespace Autofac.Extras.NLog.Tests
{
   using System;
   using System.Globalization;
   using global::NLog;
   using NUnit.Framework;

   [ TestFixture ]
   public class When_calling_overrides_of_the_trace_method: LoggerAdapterTestsBase
   {
      [ Test ]
      public void Passing_a_typed_value()
      {
         Logger.Trace( new { message = TestMessage } );

         Confirm_callsite_value( LogLevel.Trace );
      }

      [ Test ]
      public void Passing_a_format_provider_and_a_typed_value()
      {
         // NLog bug? Seems like the format provider is ignored.
         Logger.Trace( new CultureInfo( "ja-JP" ), new { date = DateTime.Now, message = TestMessage } );

         Confirm_callsite_value( LogLevel.Trace );
      }

      [ Test ]
      public void Passing_a_message_and_an_exception()
      {
         Logger.Trace( TestMessage, new Exception( "This is a test!" ) );

         Confirm_callsite_value( LogLevel.Trace );
      }

      [ Test ]
      public void Passing_message_exception_and_arguments()
      {
         Logger.Trace( new Exception( "This is a test!" ), $"Start: {{0}}, End: {{1}}, {TestMessage}", DateTime.Today, DateTime.Now );

         Confirm_callsite_value( LogLevel.Trace );
      }

      [ Test ]
      public void Passing_exception_format_message_and_arguments()
      {
         Logger.Trace( new Exception( "This is a test!" ), new CultureInfo( "ja-JP" ), $"Start: {{0}}, End: {{1}}, {TestMessage}", DateTime.Today, DateTime.Now );

         Confirm_callsite_value( LogLevel.Trace );
      }

      [ Test ]
      public void Passing_format_message_and_arguments()
      {
         Logger.Trace( new CultureInfo( "ja-JP" ), $"Start: {{0}}, {TestMessage}", new object[] { DateTime.Today } );

         Confirm_callsite_value( LogLevel.Trace );
      }

      [ Test ]
      public void Passing_message()
      {
         Logger.Trace( TestMessage );

         Confirm_callsite_value( LogLevel.Trace );
      }

      [ Test ]
      public void Passing_message_and_arguments()
      {
         Logger.Trace( $"{{0}}, {TestMessage}", new object[] { DateTime.Now } );

         Confirm_callsite_value( LogLevel.Trace );
      }

      [ Test ]
      public void Passing_format_and_message_and_one_typed_argument()
      {
         Logger.Trace( new CultureInfo( "ja-JP" ), $"{{0}}, {TestMessage}", DateTime.Now );

         Confirm_callsite_value( LogLevel.Trace );
      }

      [ Test ]
      public void Passing_message_and_typed_argument()
      {
         Logger.Trace( $"{{0}}, {TestMessage}", DateTime.Now );

         Confirm_callsite_value( LogLevel.Trace );
      }

      [ Test ]
      public void Passing_format_and_message_and_two_typed_arguments()
      {
         Logger.Trace( new CultureInfo( "ja-JP" ), $"{{0}}, {{1}}, {TestMessage}", DateTime.Today, DateTime.Now );

         Confirm_callsite_value( LogLevel.Trace );
      }

      [ Test ]
      public void Passing_message_and_two_typed_arguments()
      {
         Logger.Trace( $"{{0}}, {{1}}, {TestMessage}", DateTime.Today, DateTime.Now );

         Confirm_callsite_value( LogLevel.Trace );
      }

      [ Test ]
      public void Passing_format_and_message_and_three_typed_arguments()
      {
         Logger.Trace( new CultureInfo( "ja-JP" ), $"{{0}}, {{1}}, {{2}}, {TestMessage}", DateTime.Today, DateTime.Now, DateTime.MaxValue );

         Confirm_callsite_value( LogLevel.Trace );
      }

      [ Test ]
      public void Passing_message_and_three_typed_arguments()
      {
         Logger.Trace( $"{{0}}, {{1}}, {{2}}, {TestMessage}", DateTime.Today, DateTime.Now, DateTime.MaxValue );

         Confirm_callsite_value( LogLevel.Trace );
      }
   }
}
