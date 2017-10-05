namespace Devshed.Csv.Tests
{
    using System.Collections.Generic;
    using Devshed.Shared;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CompositeCsvColumnTests
    {
        private const string Header1 = "Header1";

        private const string Header2 = "Header2";
        
        private const string Header3 = "Header3";

        [TestMethod]
        public void Composition_FromPropertyWithAllHeaderNames_ResolvesAllHeaderName()
        {
            var rows = this.GetCompositeRows();

            var definition =
                new CsvDefinition<CompositeTestModel>(
                    new TextCsvColumn<CompositeTestModel>(e => e.Name),
                    new ObjectCsvColumn<CompositeTestModel>(e => e.TestColor),
                    new CompositeCsvColumn<CompositeTestModel, string>(e => e.TestColors, Header1, Header2, Header3))
                    {
                        FirstRowContainsHeaders = true,
                        WriteBitOrderMarker = false
                    };

            var result = CsvWriter.CreateStream(definition, rows).GetString();

            Assert.AreEqual("\"Name\";\"TestColor\";\"Header1\";\"Header2\";\"Header3\"\r\n"
                            + "\"OK_NAME\";0;\"ValueOne\";\"ValueTwo\";\"ValueThree\"\r\n", result);
        }

        [TestMethod]
        public void CompositeProperty_FromPropertyWithMissingHeaderNames_ResolvesOnlySpecifiedHeaderNames()
        {
            var rows = this.GetCompositeRows();

            var definition =
                new CsvDefinition<CompositeTestModel>(
                    new TextCsvColumn<CompositeTestModel>(e => e.Name),
                    new ObjectCsvColumn<CompositeTestModel>(e => e.TestColor),
                    new CompositeCsvColumn<CompositeTestModel, string>(e => e.GetTestColors(), Header1, Header2)) //// Purple is not in the source
                {
                    FirstRowContainsHeaders = true,
                    WriteBitOrderMarker = false
                };

            var result = CsvWriter.CreateStream(definition, rows).GetString();

            Assert.AreEqual("\"Name\";\"TestColor\";\"Header1\";\"Header2\"\r\n"
                            + "\"OK_NAME\";0;\"ValueOne\";\"ValueTwo\"\r\n", result);
        }


        [TestMethod]
        public void CompositeProperty_WithoutHeadernames_ShouldGenerateHeadernames()
        {
            var rows = this.GetCompositeRows();

            var definition =
                new CsvDefinition<CompositeTestModel>(
                    new TextCsvColumn<CompositeTestModel>(e => e.Name),
                    new ObjectCsvColumn<CompositeTestModel>(e => e.TestColor),
                    new CompositeCsvColumn<CompositeTestModel, string>(e => e.GetTestColors())) //// Purple is not in the source
                {
                    FirstRowContainsHeaders = true,
                    WriteBitOrderMarker = false
                };

            var result = CsvWriter.CreateStream(definition, rows).GetString();

            Assert.AreEqual("\"Name\";\"TestColor\";\"Header1\";\"Header2\";\"Header3\"\r\n"
                            + "\"OK_NAME\";0;\"ValueOne\";\"ValueTwo\";\"ValueThree\"\r\n", result);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void Composition_NotInCollection_ThrowsException()
        {
            var rows = this.GetCompositeRows();

            var definition =
                new CsvDefinition<CompositeTestModel>(
                    new TextCsvColumn<CompositeTestModel>(e => e.Name),
                    new ObjectCsvColumn<CompositeTestModel>(e => e.TestColor),
                    new CompositeCsvColumn<CompositeTestModel, string>(e => e.GetTestColors(), Header1, "NotInCollection"))
                {
                    FirstRowContainsHeaders = true
                };

            var result = CsvWriter.CreateStream(definition, rows).GetString();
        }

        private CompositeTestModel[] GetCompositeRows()
        {
            return new[] 
            {
                new CompositeTestModel
                {
                    Name = "OK_NAME",
                    TestColors = new[] { new CompositeColumnValue<string>(Header1, "ValueOne"),
                    new CompositeColumnValue<string>(Header2, "ValueTwo"),
                    new CompositeColumnValue<string>(Header3, "ValueThree")}
                }
            };
        }

        private sealed class CompositeTestModel
        {
            public string Name { get; set; }

            public TestColor TestColor { get; set; }

            public IEnumerable<CompositeColumnValue<string>> TestColors { get; set; }

            public IEnumerable<CompositeColumnValue<string>> GetTestColors()
            {
                return this.TestColors;
            }
        }

        private enum TestColor
        {
            Green = 1
        }
    }
}
