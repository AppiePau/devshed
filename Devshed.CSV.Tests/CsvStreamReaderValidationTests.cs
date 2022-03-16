namespace Devshed.Csv.Tests
{
    using System.Linq;
    using System.IO;
    using System.Text;
    using Devshed.Csv.Reading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Diagnostics;
    using System.Globalization;

    public sealed class TestCsvDefinition : ICsvDefinition
    {
        public string ElementDelimiter { get; set; }

        public ElementProcessing ElementProcessing { get; set; }

        public Encoding Encoding { get; set; }

        public bool FirstRowContainsHeaders { get; set; }

        public CultureInfo FormattingCulture { get; set; } = CultureInfo.CurrentCulture;

        public bool HasFieldsEnclosedInQuotes { get; set; }

        public bool IgnoreReadonlyProperties { get; set; }

        public bool RemoveNewLineCharacters { get; set; }

        public bool ThrowExceptionOnError { get; set; } = true; // Default behavior

        public bool WriteBitOrderMarker { get; set; }
    }


    [TestClass]
    public class CsvStreamReaderValidationTests
    {
        private static readonly Header HEADER1 = new Header("HEADER1");

        private static readonly Header HEADER2 = new Header("HEADER2");

        private static readonly Header CELL1 = new Header("CELL1");

        private static readonly Header CELL2 = new Header("CELL2");

        [TestMethod]
        public void GetRows_StrictElementProcessingNormalData_ReturnsElements()
        {
            var data = $"HEADER1;HEADER2{Environment.NewLine}CELL1;CELL2";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(new TestCsvDefinition { ElementProcessing = ElementProcessing.Strict }, HEADER1.Name, HEADER2.Name);
                var lines = parser.GetRows(reader).ToList();

                Assert.AreEqual(HEADER1.Name, lines[0][HEADER1]);
                Assert.AreEqual(HEADER2.Name, lines[0][HEADER2]);
                Assert.AreEqual(CELL1.Name, lines[1][HEADER1]);
                Assert.AreEqual(CELL2.Name, lines[1][HEADER2]);
            }
        }

        [TestMethod]
        public void GetRows_StrictElementProcessingTooManyColumns_ThrowsException()
        {
            var data = $"HEADER1;HEADER2{Environment.NewLine}CELL1;CELL2;CELL3";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(new TestCsvDefinition { ElementProcessing = ElementProcessing.Strict }, HEADER1.Name, HEADER2.Name);
                Expect.Throws<InvalidOperationException>(() => parser.GetRows(reader).ToList())
                    .Message.Contains("The line (2) contains more elements (3) than headers (2) available.");
            }
        }

        [TestMethod]
        public void GetRows_StrictElementProcessingTooFewColumns_ThrowsException()
        {
            var data = $"HEADER1;HEADER2{Environment.NewLine}CELL1";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(new TestCsvDefinition { ElementProcessing = ElementProcessing.Strict }, HEADER1.Name, HEADER2.Name);

                Expect.Throws<InvalidOperationException>(() => parser.GetRows(reader).ToList())
                    .Message.Contains("The line (2) contains not enough elements (1) than headers (2) available.");
            }
        }


        [TestMethod]
        public void GetRows_TooFewValidationElementProcessingTooFewColumns_ThrowsException()
        {
            var data = $"HEADER1;HEADER2{Environment.NewLine}CELL1";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(new TestCsvDefinition { ElementProcessing = ElementProcessing.OnlyTooFew }, HEADER1.Name, HEADER2.Name);

                Expect.Throws<InvalidOperationException>(() => parser.GetRows(reader).ToList())
                    .Message.Contains("The line (2) contains not enough elements (1) than headers (2) available.");
            }
        }

        [TestMethod]
        public void GetRows_TooFewValidationElementProcessingTooManyColumns_Succeeds()
        {
            var data = $"HEADER1;HEADER2{Environment.NewLine}CELL1;nCELL2;nCELL3";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(new TestCsvDefinition { ElementProcessing = ElementProcessing.OnlyTooFew }, HEADER1.Name, HEADER2.Name);
                var lines = parser.GetRows(reader).ToList();

                Assert.AreEqual(2, lines.Count());
            }
        }

        [TestMethod]
        public void GetRows_TooManyValidationElementProcessingTooFewColumns_Succeeds()
        {
            var data = $"HEADER1;HEADER2{Environment.NewLine}CELL1";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(new TestCsvDefinition { ElementProcessing = ElementProcessing.OnlyTooMany }, HEADER1.Name, HEADER2.Name);
                var lines = parser.GetRows(reader).ToList();

                Assert.AreEqual(2, lines.Count());
            }
        }

        [TestMethod]
        public void GetRows_TooManyValidationElementProcessingTooManyColumns_ThrowsException()
        {
            var data = $"HEADER1;HEADER2{Environment.NewLine}CELL1;nCELL2;nCELL3";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(new TestCsvDefinition { ElementProcessing = ElementProcessing.OnlyTooMany }, HEADER1.Name, HEADER2.Name);

                Expect.Throws<InvalidOperationException>(() => parser.GetRows(reader).ToList())
                    .Message.Contains("The line (2) contains more elements (3) than headers (2) available.");
            }
        }

        [TestMethod]
        public void GetRows_LooseElementProcessingTooFewColumns_ReturnsStringEmpty()
        {
            var data = $"HEADER1;HEADER2{Environment.NewLine}CELL1";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(new TestCsvDefinition { ElementProcessing = ElementProcessing.Loose }, HEADER1.Name, HEADER2.Name);
                var lines = parser.GetRows(reader).ToList();

                Assert.AreEqual(string.Empty, lines[1][HEADER2]);
            }
        }

        [TestMethod]
        public void GetRows_LooseElementProcessingTooManyColumns_IgnoresExcess()
        {
            var data = $"HEADER1;HEADER2{Environment.NewLine}CELL1;CELL2;CELL3";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(new TestCsvDefinition { ElementProcessing = ElementProcessing.Loose }, HEADER1.Name, HEADER2.Name);
                var lines = parser.GetRows(reader).ToList();

                Assert.AreEqual(HEADER1.Name, lines[0][HEADER1]);
                Assert.AreEqual(HEADER2.Name, lines[0][HEADER2]);
                Assert.AreEqual(CELL1.Name, lines[1][HEADER1]);
                Assert.AreEqual(CELL2.Name, lines[1][HEADER2]);
            }
        }


        [TestMethod]
        [ExpectedException(typeof(DuplicateHeaderException))]
        public void GetRows_WithDuplicateHeaderNames_ThrowsException()
        {
            var data = $"HEADER1;HEADER1{Environment.NewLine}CELL1;CELL2";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(new TestCsvDefinition { ElementProcessing = ElementProcessing.Loose }, HEADER1.Name, HEADER1.Name);
                var lines = parser.GetRows(reader).ToList();
                
            }
        }

        private static CsvStreamReader GetReader(string data)
        {
            return new CsvStreamReader(new MemoryStream(CsvConfiguration.DefaultEncoding.GetBytes(data)), CsvConfiguration.DefaultEncoding);
        }
    }
}
