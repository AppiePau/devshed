namespace Devshed.Csv.ClosedXml.Tests
{
    using Devshed.Shared;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    [TestClass]
    public class XlsxWriterTests
    {
        #region Constants

        private static readonly string UTF8Bom = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

        private static TestRow[] oneRow = new[] { new TestRow { Id = 1, Name = "OK_NAME", Getallen = new[] {
             new CompositeColumnValue<decimal>("COL1", 1M) }, IsActive = true } };

        private static TestRow[] twoRows = new[] {
                new TestRow { Id = 1, Name = "OK_NAME1", Getallen = new [] {
                        new CompositeColumnValue<decimal>("COL1", 1.23M),
                        new CompositeColumnValue<decimal>("COL2", 2.345M) }, IsActive = true },
                new TestRow { Id = 2, Name = "OK_NAME2", Getallen = new [] {
                        new CompositeColumnValue<decimal>("COL1",  3.45M),
                        new CompositeColumnValue<decimal>("COL2", 5.567M) }, IsActive = false } };
       
        private static TestRow[] incompleteRows = new[] {
                new TestRow { Id = 1, Name = "OK_NAME1", Getallen = new [] {
                        new CompositeColumnValue<decimal>("COL1", 1.23M),
                        new CompositeColumnValue<decimal>("COL2", 2.345M) }, IsActive = true },
                new TestRow { Id = 2, Name = "OK_NAME2", Getallen = new [] {
                        new CompositeColumnValue<decimal>("COL1",  3.45M),
                        new CompositeColumnValue<decimal>("COL2", 5.567M) }, IsActive = false },
                new TestRow { Id = 2, Name = "OK_NAME2", Getallen = new [] {
                        new CompositeColumnValue<decimal>("COL2", 5.567M) }, IsActive = false } };


        #endregion

        [TestMethod]
        public void Build_OneTestRow_CreatesCsv()
        {
            var result = NameDefinition().WriteAsXlsx(oneRow);

            using (var s = new FileStream(".\\Test_" + DateTime.Now.Ticks + ".xlsx", FileMode.CreateNew))
            {
                s.Write(result.GetBytes(), 0, (int)result.Length);
            }

        }


        [TestMethod]
        public void Build_TwoTestRowsWithHeader_CreatesCsv()
        {
            var result = FullDefinitionWithHeaders(twoRows).WriteAsXlsx(twoRows);
            using (var s = new FileStream(".\\Test_" + DateTime.Now.Ticks + ".xlsx", FileMode.CreateNew))
            {
                s.Write(result.GetBytes(), 0, (int)result.Length);
            }
        }

        [TestMethod]
        public void Build_IncompleteComposite_CreatesCsv()
        {
            var result = FullDefinitionWithHeadersAndIncomplete(incompleteRows).WriteAsXlsx(incompleteRows);
            using (var s = new FileStream(".\\Test_" + DateTime.Now.Ticks + ".xlsx", FileMode.CreateNew))
            {
                s.Write(result.GetBytes(), 0, (int)result.Length);
            }
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

        private static CsvDefinition<TestRow> FullDefinitionWithHeaders(TestRow[] rows)
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
                 new CompositeCsvColumn<TestRow, decimal>(e => e.Getallen,
                    rows.SelectMany(e => e.Getallen))
                 {
                     HeaderName = "OK_GETALLEN_HEADER"
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

        private static CsvDefinition<TestRow> FullDefinitionWithHeadersAndIncomplete(TestRow[] rows)
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
                 new CompositeCsvColumn<TestRow, decimal>(e => e.Getallen,
                    rows.SelectMany(e => e.Getallen))
                 {
                     HeaderName = "OK_GETALLEN_HEADER",
                     AllowUndefinedColumnsInCollection =true
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

            public CompositeColumnValue<decimal>[] Getallen { get; set; }

            public bool IsActive { get; set; }
        }
    }
}
