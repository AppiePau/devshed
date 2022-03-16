namespace Devshed.Csv.Tests
{
    using Devshed.Shared;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Text;

    [TestClass]
    public class CsvBuilderTests
    {
        #region Constants

        private static readonly string UTF8Bom = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());

        private static TestRow[] oneRow = new[] { new TestRow { Id = 1, Name = "OK_NAME", IsActive = true } };

        private static TestRow[] twoRows = new[] {
                new TestRow { Id = 1, Name = "OK_NAME1", IsActive = true },
                new TestRow { Id = 2, Name = "OK_NAME2", IsActive = false } };


        #endregion

        [TestMethod]
        public void Build_OneTestRow_CreatesCsv()
        {
            var result = CsvWriter.WriteAsCsv<TestRow>(NameDefinition(), oneRow).GetString();
            Assert.AreEqual($"\"OK_NAME_HEADER\"{Environment.NewLine}\"OK_NAME\"{Environment.NewLine}", result);
        }

        [TestMethod]
        public void Build_WithWithUTF8Bom_WritesUTF8Bom()
        {
            var definition = NameDefinition();
            definition.WriteBitOrderMarker = true;
     
            var result = CsvWriter.WriteAsCsv<TestRow>(definition, oneRow).GetString();
            Assert.AreEqual(UTF8Bom + $"\"OK_NAME_HEADER\"{Environment.NewLine}\"OK_NAME\"{Environment.NewLine}", result);
        }

        [TestMethod]
        public void Build_BigEndianUnicode_AddsBigEndianUnicodeBom()
        {
            var definition = NameDefinition();
            definition.WriteBitOrderMarker = true;
            definition.Encoding = Encoding.BigEndianUnicode;
            var result = CsvWriter.WriteAsCsv<TestRow>(definition, oneRow).GetString();
            Assert.AreEqual(Encoding.BigEndianUnicode.GetString(Encoding.BigEndianUnicode.GetPreamble()) + $"\"OK_NAME_HEADER\"{Environment.NewLine}\"OK_NAME\"{Environment.NewLine}", result);
        }

        [TestMethod]
        public void Build_UTF32_AddsUTF32Bom()
        {
            var definition = NameDefinition();
            definition.WriteBitOrderMarker = true;
            definition.Encoding = Encoding.UTF32;
            var result = CsvWriter.WriteAsCsv<TestRow>(definition, oneRow).GetString();
            Assert.AreEqual(Encoding.UTF32.GetString(Encoding.UTF32.GetPreamble()) + $"\"OK_NAME_HEADER\"{Environment.NewLine}\"OK_NAME\"{Environment.NewLine}", result);
        }

        [TestMethod]
        public void Build_UTF8_AddsUTF8Bom()
        {
            var definition = NameDefinition();
            definition.WriteBitOrderMarker = true;
            definition.Encoding = Encoding.UTF8;
            var result = CsvWriter.WriteAsCsv<TestRow>(definition, oneRow).GetString();
            Assert.AreEqual(Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble()) + $"\"OK_NAME_HEADER\"{Environment.NewLine}\"OK_NAME\"{Environment.NewLine}", result);
        }


        [TestMethod]
        public void Build_OneTestRowNoEnters_CreatesCsv()
        {
            var def = NameDefinition();
            def.RemoveNewLineCharacters = true;

            var newlines = new[] { new TestRow { Id = 1, Name = $"OK_NAME{Environment.NewLine}ENTER", IsActive = true } };

            var result = CsvWriter.WriteAsCsv<TestRow>(def, newlines).GetString();
            Assert.AreEqual($"\"OK_NAME_HEADER\"{Environment.NewLine}\"OK_NAMEENTER\"{Environment.NewLine}", result);
        }

        [TestMethod]
        public void Build_OneTestRowWithHeader_CreatesCsv()
        {
            var result = CsvWriter.WriteAsCsv<TestRow>(NameDefinition(), oneRow).GetString();
            Assert.AreEqual($"\"OK_NAME_HEADER\"{Environment.NewLine}\"OK_NAME\"{Environment.NewLine}", result);
        }

        [TestMethod]
        public void Build_TwoTestRowsWithHeader_CreatesCsv()
        {
            var result = CsvWriter.WriteAsCsv<TestRow>(FullDefinitionWithHeaders(), twoRows).GetString();
            var expected = $"\"OK_ID_HEADER\";\"OK_NAME_HEADER\";\"OK_ISACTIVE_HEADER\"{Environment.NewLine}"
                + $"1;\"OK_NAME1\";True{Environment.NewLine}"
                + $"2;\"OK_NAME2\";False{Environment.NewLine}";

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void Build_NoSpecifiedHeaders_InfersMemberNames()
        {
            var result = CsvWriter.WriteAsCsv<TestRow>(FullDefinition(), twoRows).GetString();
            var expected = $"\"Id\";\"Name\";\"IsActive\"{Environment.NewLine}"
                + $"1;\"OK_NAME1\";True{Environment.NewLine}"
                + $"2;\"OK_NAME2\";False{Environment.NewLine}";

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

            var result = CsvWriter.WriteAsCsv<TestRow>(definition, oneRow).GetString();
            Assert.AreEqual($"\"Name\"{Environment.NewLine}\"OK_NAME\"{Environment.NewLine}", result);
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

            var result = CsvWriter.WriteAsCsv<TestRow>(definition, oneRow).GetString();
            Assert.AreEqual($"\"OK_NAME\"{Environment.NewLine}", result);
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

            var result = CsvWriter.WriteAsCsv<TestRow>(definition, oneRow).GetString();
            Assert.AreEqual($"\"OK_NAME\"{Environment.NewLine}", result);
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
