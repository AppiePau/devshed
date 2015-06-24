namespace Devshed.Csv.Tests
{
    using System.Collections.Generic;
    using Devshed.Shared;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ArrayCsvColumnTests
    {
        [TestMethod]
        public void Render_ColorNamesArray_WritesAllColorsInElement()
        {
            var rows = this.GetModelRows();

            var definition =
                new CsvDefinition<ArrayContainingModel>(
                    new TextCsvColumn<ArrayContainingModel>(e => e.Name),
                    new ArrayCsvColumn<ArrayContainingModel, string>(e => e.TestColors))                    
                    {
                        FirstRowContainsHeaders = true,
                        WriteBitOrderMarker = false
                    };

            var result = CsvWriter.CreateStream(definition, rows).GetString();

            Assert.AreEqual("\"Name\";\"TestColors\"\r\n"
                            + "\"OK_NAME\";Red,Green,Blue\r\n", result);
        }

        [TestMethod]
        public void Render_ForceExcelTextCell_AddsSingleQuote()
        {
            var rows = this.GetModelRows();

            var definition =
                new CsvDefinition<ArrayContainingModel>(
                    new TextCsvColumn<ArrayContainingModel>(e => e.Name) {  ForceNumberToTextFormatting = true} )
                {
                    FirstRowContainsHeaders = true,
                    WriteBitOrderMarker = false
                };

            var result = CsvWriter.CreateStream(definition, rows).GetString();

            Assert.AreEqual("\"Name\"\r\n"
                            + "=\"OK_NAME\"\r\n", result);
        }

        private ArrayContainingModel[] GetModelRows()
        {
            return new[] 
            {
                new ArrayContainingModel
                {
                    Name = "OK_NAME",
                    TestColors = new[] { "Red", "Green", "Blue" }
                }
            };
        }

        private sealed class ArrayContainingModel
        {
            public string Name { get; set; }

            public IEnumerable<string> TestColors { get; set; }
        }
    }
}
