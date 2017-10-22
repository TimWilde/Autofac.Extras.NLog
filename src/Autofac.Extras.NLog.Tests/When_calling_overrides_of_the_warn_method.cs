namespace Autofac.Extras.NLog.Tests
{
   using System;
   using System.Globalization;
   using global::NLog;
   using NUnit.Framework;

   [ TestFixture ]
   public class When_calling_overrides_of_the_warn_method: LoggerAdapterTestsBase
   {
      [ Test ]
      public void Passing_a_typed_value()
      {
         Logger.Debug( new { message = TestMessage } );

         Confirm_callsite_value( LogLevel.Debug );
      }

      [ Test ]
      public void Passing_a_format_and_a_typed_value()
      {
         Logger.Debug( new CultureInfo( "ja-JP" ), new { message = TestMessage } );

         Confirm_callsite_value( LogLevel.Debug );
      }

      [ Test ]
      public void Passing_a_message_and_an_exception()
      {
         Logger.Debug( TestMessage, new Exception( "This is a test!" ) );

         Confirm_callsite_value( LogLevel.Debug );
      }

      [ Test ]
      public void Passing_an_exception_and_a_message_and_arguments()
      {
         Logger.Debug( new Exception( "This is a test!" ), $"{{0}}, {TestMessage}", DateTime.Now );

         Confirm_callsite_value( LogLevel.Debug );
      }

      [ Test ]
      public void Passing_an_exception_and_a_format_and_a_message_and_arguments()
      {
         Logger.Debug( new Exception( "This is a test!" ), new CultureInfo( "ja-JP" ), $"{{0}}, {TestMessage}", DateTime.Now );

         Confirm_callsite_value( LogLevel.Debug );
      }

      [ Test ]
      public void Passing_a_format_and_a_message_and_arguments()
      {
         Logger.Debug( new CultureInfo( "ja-JP" ), $"{{0}}, {TestMessage}", new object[] { DateTime.Now } );

         Confirm_callsite_value( LogLevel.Debug );
      }

      [ Test ]
      public void Passing_a_message()
      {
         Logger.Debug( TestMessage );

         Confirm_callsite_value( LogLevel.Debug );
      }

      [ Test ]
      public void Passing_a_message_and_arguments()
      {
         Logger.Debug( $"{{0}}, {TestMessage}", new object[] { DateTime.Now } );

         Confirm_callsite_value( LogLevel.Debug );
      }

      [ Test ]
      public void Passing_a_format_and_a_message_and_a_typed_argument()
      {
         Logger.Debug( new CultureInfo( "ja-JP" ), $"{{0}}, {TestMessage}", DateTime.Now );

         Confirm_callsite_value( LogLevel.Debug );
      }

      [ Test ]
      public void Passing_a_message_and_a_typed_argument()
      {
         Logger.Debug( $"{{0}}, {TestMessage}", DateTime.Now );

         Confirm_callsite_value( LogLevel.Debug );
      }

      [ Test ]
      public void Passing_a_format_and_a_message_and_two_typed_arguments()
      {
         Logger.Debug( new CultureInfo( "js-JP" ), $"{{0}}, {{1}}, {TestMessage}", DateTime.Today, DateTime.Now );

         Confirm_callsite_value( LogLevel.Debug );
      }

      [ Test ]
      public void Passing_a_message_and_two_typed_arguments()
      {
         Logger.Debug( $"{{0}}, {{1}}, {TestMessage}", DateTime.Today, DateTime.Now );

         Confirm_callsite_value( LogLevel.Debug );
      }

      [ Test ]
      public void Passing_a_format_and_a_message_and_three_typed_arguments()
      {
         Logger.Debug( new CultureInfo( "ja-JP" ), $"{{0}}, {{1}}, {{2}}, {TestMessage}", DateTime.Today, DateTime.Now, DateTime.MaxValue );

         Confirm_callsite_value( LogLevel.Debug );
      }

      [ Test ]
      public void Passing_a_message_and_three_typed_arguments()
      {
         Logger.Debug( $"{{0}}, {{1}}, {{2}}, {TestMessage}", DateTime.Today, DateTime.Now, DateTime.MaxValue );

         Confirm_callsite_value( LogLevel.Debug );
      }
   }
}
