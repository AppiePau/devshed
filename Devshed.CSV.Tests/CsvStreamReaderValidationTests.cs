namespace Devshed.Csv.Tests
{
    using System.Linq;
    using System.IO;
    using System.Text;
    using Devshed.Csv.Reading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Diagnostics;

    [TestClass]
    public class CsvStreamReaderValidationTests
    {
        private const string HEADER1 = "HEADER1";

        private const string HEADER2 = "HEADER2";

        private const string CELL1 = "CELL1";

        private const string CELL2 = "CELL2";

        [TestMethod]
        public void GetRows_StrictElementProcessingNormalData_ReturnsElements()
        {
            var data = "HEADER1;HEADER2\r\nCELL1;CELL2";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(ElementProcessing.Strict, HEADER1, HEADER2);
                var lines = parser.GetRows(reader).ToList();

                Assert.AreEqual(HEADER1, lines[0][HEADER1]);
                Assert.AreEqual(HEADER2, lines[0][HEADER2]);
                Assert.AreEqual(CELL1, lines[1][HEADER1]);
                Assert.AreEqual(CELL2, lines[1][HEADER2]);
            }
        }

        [TestMethod]
        public void GetRows_StrictElementProcessingTooManyColumns_ThrowsException()
        {
            var data = "HEADER1;HEADER2\r\nCELL1;CELL2;CELL3";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(ElementProcessing.Strict, HEADER1, HEADER2);
                Expect.Throws<InvalidOperationException>(() => parser.GetRows(reader).ToList())
                    .Message.Contains("The line (2) contains more elements (3) than headers (2) available.");
            }
        }

        [TestMethod]
        public void GetRows_StrictElementProcessingTooFewColumns_ThrowsException()
        {
            var data = "HEADER1;HEADER2\r\nCELL1";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(ElementProcessing.Strict, HEADER1, HEADER2);

                Expect.Throws<InvalidOperationException>(() => parser.GetRows(reader).ToList())
                    .Message.Contains("The line (2) contains not enough elements (1) than headers (2) available.");
            }
        }


        [TestMethod]
        public void GetRows_TooFewValidationElementProcessingTooFewColumns_ThrowsException()
        {
            var data = "HEADER1;HEADER2\r\nCELL1";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(ElementProcessing.OnlyTooFew, HEADER1, HEADER2);

                Expect.Throws<InvalidOperationException>(() => parser.GetRows(reader).ToList())
                    .Message.Contains("The line (2) contains not enough elements (1) than headers (2) available.");
            }
        }

        [TestMethod]
        public void GetRows_TooFewValidationElementProcessingTooManyColumns_Succeeds()
        {
            var data = "HEADER1;HEADER2\r\nCELL1;nCELL2;nCELL3";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(ElementProcessing.OnlyTooFew, HEADER1, HEADER2);
                var lines = parser.GetRows(reader).ToList();

                Assert.AreEqual(2, lines.Count());
            }
        }

        [TestMethod]
        public void GetRows_TooManyValidationElementProcessingTooFewColumns_Succeeds()
        {
            var data = "HEADER1;HEADER2\r\nCELL1";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(ElementProcessing.OnlyTooMany, HEADER1, HEADER2);
                var lines = parser.GetRows(reader).ToList();

                Assert.AreEqual(2, lines.Count());
            }
        }

        [TestMethod]
        public void GetRows_TooManyValidationElementProcessingTooManyColumns_ThrowsException()
        {
            var data = "HEADER1;HEADER2\r\nCELL1;nCELL2;nCELL3";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(ElementProcessing.OnlyTooMany, HEADER1, HEADER2);

                Expect.Throws<InvalidOperationException>(() => parser.GetRows(reader).ToList())
                    .Message.Contains("The line (2) contains more elements (3) than headers (2) available.");
            }
        }

        [TestMethod]
        public void GetRows_LooseElementProcessingTooFewColumns_ReturnsStringEmpty()
        {
            var data = "HEADER1;HEADER2\r\nCELL1";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(ElementProcessing.Loose, HEADER1, HEADER2);
                var lines = parser.GetRows(reader).ToList();

                Assert.AreEqual(string.Empty, lines[1][HEADER2]);
            }
        }

        [TestMethod]
        public void GetRows_LooseElementProcessingTooManyColumns_IgnoresExcess()
        {
            var data = "HEADER1;HEADER2\r\nCELL1;CELL2;CELL3";

            using (var reader = GetReader(data))
            {
                var parser = new CsvStreamLineReader(ElementProcessing.Loose, HEADER1, HEADER2);
                var lines = parser.GetRows(reader).ToList();

                Assert.AreEqual(HEADER1, lines[0][HEADER1]);
                Assert.AreEqual(HEADER2, lines[0][HEADER2]);
                Assert.AreEqual(CELL1, lines[1][HEADER1]);
                Assert.AreEqual(CELL2, lines[1][HEADER2]);
            }
        }

        private static CsvStreamReader GetReader(string data)
        {
            return new CsvStreamReader(new MemoryStream(CsvConfiguration.DefaultEncoding.GetBytes(data)), CsvConfiguration.DefaultEncoding);
        }
    }
}
