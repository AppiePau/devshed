namespace Devshed.Csv.Tests
{
    using Devshed.Shared;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Reading;

    [TestClass]
    public class TextCsvColumnTests
    {
        [TestMethod]
        public void Reading_WithStringBasedProperty_ReturnsResult()
        {
            var def = new CsvDefinition<TestModel>(new TextCsvColumn<TestModel>("Name"))
            {
                FirstRowContainsHeaders = true,
                ElementDelimiter = ";",
                HasFieldsEnclosedInQuotes = true,
                Encoding = Encoding.UTF8
            };

            var data = $"\"Name\";\"TestColors\"{Environment.NewLine}\"OK_NAME\";\"Red\"";

            var mapper = new TableDataMapper<TestModel>(def);
            var result = mapper.FromStream(new MemoryStream(Encoding.UTF8.GetBytes(data)));

            Assert.AreEqual("OK_NAME", result[0].Name);
        }

        public sealed class TestModel
        {
            public string Name { get; set; }
        }
    }
}