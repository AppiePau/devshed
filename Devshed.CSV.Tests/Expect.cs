namespace Devshed.Csv.Tests
{
    using System;
    using System.Diagnostics;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class Expect
    {
        [DebuggerStepThrough]
        public static ThrownException<TException> Throws<TException>(Action test) where TException : Exception
        {
            try
            {
                test();
            }
            catch (TException ex)
            {
                return new ThrownException<TException>(ex);
            }

            throw new AssertFailedException("Test did not throw exception of type '" + typeof(TException).Name + "'.");
        }
    }
}