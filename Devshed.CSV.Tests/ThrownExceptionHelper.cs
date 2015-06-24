namespace Devshed.Csv.Tests
{
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class ThrownExceptionHelper
    {

        [DebuggerStepThrough]
        public static MessageValidator Contains(this MessageValidator exception, string message)
        {
            if (exception.Message.Contains(message))
            {
                return exception;
            }

            throw new AssertFailedException("Message does not contain '" + message + "'. The original message was: " + exception.Message);
        }

        [DebuggerStepThrough]
        public static MessageValidator Equals(this MessageValidator exception, string message)
        {
            if (exception.Message.Equals(message))
            {
                return exception;
            }

            throw new AssertFailedException("Message does not equal '" + message + "'. The original message was: " + exception.Message);
        }
    }
}