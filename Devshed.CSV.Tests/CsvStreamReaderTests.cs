namespace Devshed.Csv.Tests
{
    using System.IO;
    using System.Text;
    using Devshed.Csv.Reading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CsvStreamReaderTests
    {
        [TestMethod]
        public void Build_OneTestRow_CreatesCsv()
        {
            var data = "CELL1;CELL2";

            using (var reader = GetReader(data))
            {
                var line = reader.ReadLine();

                Assert.AreEqual("CELL1", line.Elements[0]);
                Assert.AreEqual("CELL2", line.Elements[1]);
            }
        }

        [TestMethod]
        public void Build_TwoTestRows_CreatesCsv()
        {
            var data = "CELL1;CELL2\r\nCELL3;CELL4";

            using (var reader = GetReader(data))
            {
                var line1 = reader.ReadLine();
                var line2 = reader.ReadLine();

                Assert.AreEqual("CELL1", line1.Elements[0]);
                Assert.AreEqual("CELL2", line1.Elements[1]);

                Assert.AreEqual("CELL3", line2.Elements[0]);
                Assert.AreEqual("CELL4", line2.Elements[1]);
            }
        }

        [TestMethod]
        public void Build_OneCellWithLineFeed_KeepsEnterInCell()
        {
            var data = "CELL1;\"CELL2\nDATA\"";

            using (var reader = GetReader(data))
            {
                var line = reader.ReadLine();

                Assert.AreEqual("CELL1", line.Elements[0]);
                Assert.AreEqual("CELL2\nDATA", line.Elements[1]);
            }
        }


        [TestMethod]
        public void Build_OneCellWithEnter_KeepsEnterInCell()
        {
            var data = "CELL1;\"CELL2\r\nDATA\"";

            using (var reader = GetReader(data))
            {
                var line = reader.ReadLine();

                Assert.AreEqual("CELL1", line.Elements[0]);
                Assert.AreEqual("CELL2\r\nDATA", line.Elements[1]);
            }
        }


        [TestMethod]
        public void Build_OneCellWithEnterAndQuotes_KeepsEnterAndQuoteInCell()
        {
            var data = "CELL1;\"CELL2\"\"\r\nDATA\"";

            using (var reader = GetReader(data))
            {
                var line = reader.ReadLine();

                Assert.AreEqual("CELL1", line.Elements[0]);
                Assert.AreEqual("CELL2\"\r\nDATA", line.Elements[1]);
            }
        }


        [TestMethod]
        public void Build_EmptyTextCell_IsReadAsEmpty()
        {
            var data = "\"\";\"CELL2\"";

            using (var reader = GetReader(data))
            {
                var line = reader.ReadLine();

                Assert.AreEqual("", line.Elements[0]);
                Assert.AreEqual("CELL2", line.Elements[1]);
            }
        }

        private static CsvStreamReader GetReader(string data)
        {
            return new CsvStreamReader(new MemoryStream(Encoding.Unicode.GetBytes(data)), Encoding.Unicode);
        }
    }
}
