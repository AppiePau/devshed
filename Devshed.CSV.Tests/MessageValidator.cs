namespace Devshed.Csv.Tests
{
    using System;

    public sealed class MessageValidator
    {
        private readonly Exception exception;

        public MessageValidator(Exception exception)
        {
            this.exception = exception;
        }

        public string Message
        {
            get
            {
                return this.exception.Message;
            }
        }
    }
}