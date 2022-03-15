namespace Devshed.Csv.ClosedXml.Tests
{
    using Devshed.Shared;
    using Devshed.Csv;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;
    using System.Text;

    [TestClass]
    public class XlsxReaderTests
    {
        #region Constants

        private static TestRow[] oneRow = new[] { new TestRow { Id = 1, Name = "OK_NAME", IsActive = true } };

        private static TestRow[] twoRows = new[] {
                new TestRow { Id = 1, Name = "OK_NAME1", IsActive = true },
                new TestRow { Id = 2, Name = "OK_NAME2", IsActive = false } };


        #endregion

        [TestMethod]
        public void Build_OneTestRow_CreatesCsv()
        {
            var stream = FullDefinition().WriteAsXlsx(twoRows);

            //stream.Flush();

            //stream.Position = 0;


            //using (var s = new FileStream("D:\\" + DateTime.Now.Ticks + ".xlsx", FileMode.CreateNew))
            //{
            //    s.Write(stream.GetBytes(), 0, (int)stream.Length);
            //}

            stream.Position = 0;
            var result = FullDefinition().ReadXlsx(stream);

            Assert.AreEqual(1, result[0].Id);
            Assert.AreEqual("OK_NAME1", result[0].Name);
            Assert.AreEqual(true, result[0].IsActive);
        }

        
        private static CsvDefinition<TestRow> NameDefinition()
        {
            return new CsvDefinition<TestRow>(
                new TextCsvColumn<TestRow>(e => e.Name)
                {
                    HeaderName = "OK_NAME_HEADER"
                })
            {
                FirstRowContainsHeaders = true,
                WriteBitOrderMarker = false,
                Encoding = Encoding.UTF8
            };
        }

        private static CsvDefinition<TestRow> FullDefinition()
        {
            return new CsvDefinition<TestRow>(
                  new NumberCsvColumn<TestRow>(e => e.Id),
                  new TextCsvColumn<TestRow>(e => e.Name),
                  new BooleanCsvColumn<TestRow>(e => e.IsActive))
            {
                FirstRowContainsHeaders = true,
                WriteBitOrderMarker = false
            };
        }

        private static CsvDefinition<TestRow> FullDefinitionWithHeaders()
        {
            return new CsvDefinition<TestRow>(
                 new NumberCsvColumn<TestRow>(e => e.Id)
                 {
                     HeaderName = "OK_ID_HEADER"
                 },
                 new TextCsvColumn<TestRow>(e => e.Name)
                 {
                     HeaderName = "OK_NAME_HEADER"
                 },
                 new BooleanCsvColumn<TestRow>(e => e.IsActive)
                 {
                     HeaderName = "OK_ISACTIVE_HEADER"
                 })
            {
                FirstRowContainsHeaders = true,
                WriteBitOrderMarker = false
            };
        }

        private sealed class TestRow
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public bool IsActive { get; set; }
        }
    }
}
