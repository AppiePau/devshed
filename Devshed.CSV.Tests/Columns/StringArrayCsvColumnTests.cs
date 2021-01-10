namespace Devshed.Csv.Tests
{
    using Devshed.Shared;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Linq;
    using System.Text;

    [TestClass]
    public class StringArrayCsvColumnTests
    {
        [TestMethod]
        public void Render_ColorNamesArray_WritesAllColorsInQuotedElement()
        {
            var rows = this.GetModelRows();

            var definition = this.GetDefinition();

            var result = CsvWriter.CreateStream(definition, rows).GetString();

            Assert.AreEqual("\"Name\";\"TestColors\"\r\n"
            + "\"OK_NAME\";\"Red,Green,Blue\"\r\n", result);
        }

        [TestMethod]
        public void Render_ColorNamesArray_DoesNotEscapeSemicolon()
        {
            var rows = new[]
                {
                    new ColorArrayModel
                    {
                          Name = "OK_NAME",
                         TestColors = new[] { "Red;Test", "Green,Test", "Blue" }
                    }
                };

            var definition = this.GetDefinition();

            var result = CsvWriter.CreateStream(definition, rows).GetString();

            Assert.AreEqual("\"Name\";\"TestColors\"\r\n"
            + "\"OK_NAME\";\"Red;Test,Green_Test,Blue\"\r\n", result);
        }

        [TestMethod]
        public void Read_ColorNamesArrayWithSeparator_ReadsColorsFromQuotedString()
        {
            var definition = this.GetDefinition();
            var stream = new MemoryStream(Encoding.UTF8.GetBytes("\"Name\";\"TestColors\"\r\n"
                + "\"OK_NAME\";\"Red;Test,Green_Test,Blue\"\r\n"));

            var rows = CsvReader.Read<ColorArrayModel>(stream, definition);
            var row = rows.Single();
            var testColors = row.TestColors.ToArray();

            Assert.AreEqual("OK_NAME", row.Name);
            Assert.AreEqual("Red;Test", testColors[0]);
            Assert.AreEqual("Green_Test", testColors[1]);
            Assert.AreEqual("Blue", testColors[2]);
        }


        [TestMethod]
        public void Render_ForceExcelTextCell_AddsSingleQuote()
        {
            var rows = this.GetModelRows();

            var definition =
            new CsvDefinition<ColorArrayModel>(
            new TextCsvColumn<ColorArrayModel>(e => e.Name) { ForceNumberToTextFormatting = true })
            {
                FirstRowContainsHeaders = true,
                WriteBitOrderMarker = false
            };

            var result = CsvWriter.CreateStream(definition, rows).GetString();

            Assert.AreEqual("\"Name\"\r\n"
            + "\"OK_NAME\"\r\n", result);
        }

        private ColorArrayModel[] GetModelRows()
        {
            return new[]
                {
                new ColorArrayModel
                {
                Name = "OK_NAME",
                TestColors = new[] { "Red", "Green", "Blue" }
                }
                };
        }

        private CsvDefinition<ColorArrayModel> GetDefinition()
        {
            return new CsvDefinition<ColorArrayModel>(
            new TextCsvColumn<ColorArrayModel>(e => e.Name),
            new ArrayCsvColumn<ColorArrayModel, string>(e => e.TestColors))
            {
                FirstRowContainsHeaders = true,
                WriteBitOrderMarker = false
            };
        }

        private sealed class ColorArrayModel
        {
            public string Name { get; set; }

            public string[] TestColors { get; set; }
        }
    }
}