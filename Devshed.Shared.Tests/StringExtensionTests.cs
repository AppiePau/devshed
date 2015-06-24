namespace Devshed.Shared.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringExtensionTests
    {
        [TestMethod]
        public void RemoveBegin_ByLength_RemovesBegin()
        {
            var test = "BeginRemove";

            var result = StringExtensions.RemoveBegin(test, 5);

            Assert.AreEqual("Remove", result);
        }

        [TestMethod]
        public void RemoveBegin_ByBeginString_RemovesBegin()
        {
            var test = "BeginRemove";

            var result = StringExtensions.RemoveBegin(test, "Begin");

            Assert.AreEqual("Remove", result);
        }

        [TestMethod]
        public void RemoveBegin_ByBeginStringCaseSensitive_DoesNotRemoveBegin()
        {
            var test = "BeginRemove";

            var result = StringExtensions.RemoveBegin(test, "begin", StringComparison.Ordinal);

            Assert.AreEqual("BeginRemove", result);
        }

        [TestMethod]
        public void RemoveEnd_ByLength_RemovesEnd()
        {
            var test = "RemoveEnd";

            var result = StringExtensions.RemoveEnd(test, 3);

            Assert.AreEqual("Remove", result);
        }

        [TestMethod]
        public void RemoveEnd_ByEndString_RemovesEnd()
        {
            var test = "RemoveEnd";

            var result = StringExtensions.RemoveEnd(test, "End");

            Assert.AreEqual("Remove", result);
        }

        [TestMethod]
        public void RemoveEnd_ByEndStringCaseSensitive_DoesNotRemoveEnd()
        {
            var test = "RemoveEnd";

            var result = StringExtensions.RemoveEnd(test, "end", StringComparison.Ordinal);

            Assert.AreEqual("RemoveEnd", result);
        }
    }
}
