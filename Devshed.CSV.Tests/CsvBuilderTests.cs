namespace Devshed.Csv.Tests
{
    using Devshed.Shared;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Text;

    [TestClass]
    public class CsvBuilderTests
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
            var result = CsvWriter.CreateStream<TestRow>(NameDefinition(), oneRow).GetString();
            Assert.AreEqual("\"OK_NAME_HEADER\"\r\n\"OK_NAME\"\r\n", result);
        } 
        
        [TestMethod]
        public void Build_BigEndianUnicode_AddsBigEndianUnicodeBom()
        {
            var definition = NameDefinition();
            definition.WriteBitOrderMarker = true;
            var result = CsvWriter.CreateStream<TestRow>(definition, oneRow, Encoding.BigEndianUnicode).GetString();
            Assert.AreEqual(Encoding.BigEndianUnicode.GetString(Encoding.BigEndianUnicode.GetPreamble()) + "\"OK_NAME_HEADER\"\r\n\"OK_NAME\"\r\n", result);
        }

        [TestMethod]
        public void Build_UTF32_AddsUTF32Bom()
        {
            var definition = NameDefinition();
            definition.WriteBitOrderMarker = true;
            var result = CsvWriter.CreateStream<TestRow>(definition, oneRow, Encoding.UTF32).GetString();
            Assert.AreEqual(Encoding.UTF32.GetString(Encoding.UTF32.GetPreamble()) + "\"OK_NAME_HEADER\"\r\n\"OK_NAME\"\r\n", result);
        }

        [TestMethod]
        public void Build_UTF8_AddsUTF8Bom()
        {
            var definition = NameDefinition();
            definition.WriteBitOrderMarker = true;
            var result = CsvWriter.CreateStream<TestRow>(definition, oneRow, Encoding.UTF8).GetString();
            Assert.AreEqual(Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble()) + "\"OK_NAME_HEADER\"\r\n\"OK_NAME\"\r\n", result);
        }


        [TestMethod]
        public void Build_OneTestRowNoEnters_CreatesCsv()
        {
            var def = NameDefinition();
            def.RemoveNewLineCharacters = true;

            var newlines = new[] { new TestRow { Id = 1, Name = "OK_NAME\r\nENTER", IsActive = true } };

            var result = CsvWriter.CreateStream<TestRow>(def, newlines).GetString();
            Assert.AreEqual("\"OK_NAME_HEADER\"\r\n\"OK_NAMEENTER\"\r\n", result);
        }

        [TestMethod]
        public void Build_OneTestRowWithHeader_CreatesCsv()
        {
            var result = CsvWriter.CreateStream<TestRow>(NameDefinition(), oneRow).GetString();
            Assert.AreEqual("\"OK_NAME_HEADER\"\r\n\"OK_NAME\"\r\n", result);
        }

        [TestMethod]
        public void Build_TwoTestRowsWithHeader_CreatesCsv()
        {
            var result = CsvWriter.CreateStream<TestRow>(FullDefinitionWithHeaders(), twoRows).GetString();
            var expected = "\"OK_ID_HEADER\";\"OK_NAME_HEADER\";\"OK_ISACTIVE_HEADER\"\r\n"
                + "1;\"OK_NAME1\";True\r\n"
                + "2;\"OK_NAME2\";False\r\n";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Build_NoSpecifiedHeaders_InfersMemberNames()
        {
            var result = CsvWriter.CreateStream<TestRow>(FullDefinition(), twoRows).GetString();
            var expected = "\"Id\";\"Name\";\"IsActive\"\r\n"
                + "1;\"OK_NAME1\";True\r\n"
                + "2;\"OK_NAME2\";False\r\n";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Build_WithoutHeaderNames_NamesAreReflected()
        {
            var definition = new CsvDefinition<TestRow>(
                new TextCsvColumn<TestRow>(e => e.Name))
                {
                    FirstRowContainsHeaders = true,
                    WriteBitOrderMarker = false
                };

            var result = CsvWriter.CreateStream<TestRow>(definition, oneRow).GetString();
            Assert.AreEqual("\"Name\"\r\n\"OK_NAME\"\r\n", result);
        }

        [TestMethod]
        public void Build_NoHeaderNamesInFirstRow_HeaderRowIsSkipped()
        {
            var definition = new CsvDefinition<TestRow>(
                new TextCsvColumn<TestRow>(e => e.Name))
            {
                FirstRowContainsHeaders = false,
                WriteBitOrderMarker = false
            };

            var result = CsvWriter.CreateStream<TestRow>(definition, oneRow).GetString();
            Assert.AreEqual("\"OK_NAME\"\r\n", result);
        }

        [TestMethod]
        public void Build_NoHeaderNamesInFirstRowWithoutTextForcing_HeaderRowIsSkipped()
        {
            var definition = new CsvDefinition<TestRow>(
                new TextCsvColumn<TestRow>(e => e.Name) { ForceNumberToTextFormatting = false })
            {
                FirstRowContainsHeaders = false,
                WriteBitOrderMarker = false
            };

            var result = CsvWriter.CreateStream<TestRow>(definition, oneRow).GetString();
            Assert.AreEqual("\"OK_NAME\"\r\n", result);
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
                    WriteBitOrderMarker = false
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
