namespace Devshed.Csv.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ObjectCsvColumnTests
    {
        [TestMethod]
        public void New_TestDecimal_ResolvesHeaderName()
        {
            var column = new ObjectCsvColumn<Model>(e => e.TestDecimal);
            Assert.AreEqual("TestDecimal", column.HeaderName);
        }

        [TestMethod]
        public void New_TestObject_ResolvesHeaderName()
        {
            var column = new ObjectCsvColumn<Model>(e => e.TestObject);
            Assert.AreEqual("TestObject", column.HeaderName);
        }

        [TestMethod]
        public void New_TestFunc_ResolvesHeaderName()
        {
            var column = new ObjectCsvColumn<Model>(e => e.TestFunc);
            Assert.AreEqual("TestFunc", column.HeaderName);
        }

        private sealed class Model
        {
            public decimal TestDecimal { get; set; }

            public object TestObject { get; set; }

            public Func<object> TestFunc { get; set; }
        }
    }
}
