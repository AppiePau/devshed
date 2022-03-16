namespace Devshed.Csv.Tests
{
    using System.Collections.Generic;
    using Devshed.Shared;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Text;
    using System.Linq;

    [TestClass]
    public class NumberArrayCsvColumnTests
    {
        [TestMethod]
        public void Read_NumberArray_ReadsNumbersFromQuotedString()
        {
            var definition = this.GetNumberDefinition();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes($"\"Name\";\"Numbers\"{Environment.NewLine}"
                            + $"\"OK_NAME\";\"1,2,3\"{Environment.NewLine}"));

            var rows = CsvReader.ReadAsCsv<NumberArrayModel>(definition, stream);
            var row = rows.Single();
            var testColors = row.Numbers.ToArray();

            Assert.AreEqual("OK_NAME", row.Name);
            Assert.AreEqual(1, testColors[0]);
            Assert.AreEqual(2, testColors[1]);
            Assert.AreEqual(3, testColors[2]);
        }
        
        private CsvDefinition<NumberArrayModel> GetNumberDefinition()
        {
            return new CsvDefinition<NumberArrayModel>(
                                new TextCsvColumn<NumberArrayModel>(e => e.Name),
                                new ArrayCsvColumn<NumberArrayModel, int>(e => e.Numbers))
            {
                FirstRowContainsHeaders = true,
                WriteBitOrderMarker = false
            };
        }
        
        private sealed class NumberArrayModel
        {
            public string Name { get; set; }

            public int[] Numbers { get; set; }
        }
    }
}
