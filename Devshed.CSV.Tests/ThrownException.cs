namespace Devshed.Csv.Tests
{
    using System;

    public sealed class ThrownException<TException> where TException : Exception
    {
        private readonly TException exception;

        public ThrownException(TException exception)
        {
            this.exception = exception;
        }

        public MessageValidator Message
        {
            get
            {
                return new MessageValidator(this.exception);
            }
        }
    }
}