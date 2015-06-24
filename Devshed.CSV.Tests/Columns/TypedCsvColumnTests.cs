namespace Devshed.Csv.Tests
{
    using System.Linq;
    using System.Collections.Generic;
    using Devshed.Shared;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TypedCsvColumnTests
    {
        [TestMethod]
        public void New_Type_ResolvesHeaderName()
        {
            var column = new TypedCsvColumn<CompositeTestModel, TestColor>(e => e.TestColor);
            Assert.AreEqual("TestColor", column.HeaderName);
        }
        
        private sealed class CompositeTestModel
        {
            public string Name { get; set; }

            public TestColor TestColor { get; set; }

            public IEnumerable<CompositeColumnValue<string>> TestColors { get; set; }

            public IEnumerable<CompositeColumnValue<string>> GetTestColors()
            {
                return this.TestColors;
            }
        }

        private enum TestColor
        {
            Green = 1
        }
    }
}
