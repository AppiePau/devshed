namespace Devshed.Csv.Tests
{
    using System.Collections.Generic;
    using Devshed.Shared;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TypedCsvColumnTests
    {
        [TestMethod]
        public void New_Type_ResolvesHeaderName()
        {
            var column = new TypedCsvColumn<Model, Color>(e => e.TestColor);
            Assert.AreEqual("TestColor", column.HeaderName);

        }

        [TestMethod]
        public void Composition_Type_ResolvesHeaderName()
        {
            var models = new[] 
            {
                new Model
                {
                    Name = "OK_NAME",
                    TestColors = new[] { new KeyValuePair<string, string>("Green", "One"),
                    new KeyValuePair<string, string>("Black", "Two"),
                    new KeyValuePair<string, string>("Red", "Three")}
                }
            };

            var definition =
                new CsvDefinition<Model>(
                    new TextCsvColumn<Model>(e => e.Name),
                    new ObjectCsvColumn<Model>(e => e.TestColor),
                    new CompositeCsvColumn<Model>(e => e.TestColors, "Green", "Black"))
                    {
                        FirstRowContainsHeaders = true
                    };

            var result = CsvBuilder.Build(definition, models).GetString();

            Assert.AreEqual("\"Name\";\"TestColor\";\"Green\";\"Black\"\r\n"
                            + "=\"OK_NAME\";0;\"One\";\"Two\"\r\n", result);
        }

        [TestMethod]
        public void Composition_FromMethod_ResolvesHeaderName()
        {
            var models = new[] 
            {
                new Model
                {
                    Name = "OK_NAME",
                    TestColors = new[] { new KeyValuePair<string, string>("Green", "One"),
                    new KeyValuePair<string, string>("Black", "Two"),
                    new KeyValuePair<string, string>("Red", "Three")}
                }
            };

            var definition =
                new CsvDefinition<Model>(
                    new TextCsvColumn<Model>(e => e.Name),
                    new ObjectCsvColumn<Model>(e => e.TestColor),
                    new CompositeCsvColumn<Model>(e => e.GetTestColors(), "Green", "Black"))
                {
                    FirstRowContainsHeaders = true
                };

            var result = CsvBuilder.Build(definition, models).GetString();

            Assert.AreEqual("\"Name\";\"TestColor\";\"Green\";\"Black\"\r\n"
                            + "=\"OK_NAME\";0;\"One\";\"Two\"\r\n", result);
        }

        [TestMethod]
        public void Composition_NotInCollection_ResolvesHeaderName()
        {
            var models = new[] 
            {
                new Model
                {
                    Name = "OK_NAME",
                    TestColors = new[] { new KeyValuePair<string, string>("Green", "One"),
                    new KeyValuePair<string, string>("Black", "Two"),
                    new KeyValuePair<string, string>("Red", "Three")}
                }
            };

            var definition =
                new CsvDefinition<Model>(
                    new TextCsvColumn<Model>(e => e.Name),
                    new ObjectCsvColumn<Model>(e => e.TestColor),
                    new CompositeCsvColumn<Model>(e => e.GetTestColors(), "Green", "Purple"))
                {
                    FirstRowContainsHeaders = true
                };

            var result = CsvBuilder.Build(definition, models).GetString();

            Assert.AreEqual("\"Name\";\"TestColor\";\"Green\";\"Purple\"\r\n"
                            + "=\"OK_NAME\";0;\"One\";\"\"\r\n", result);
        }

        private sealed class Model
        {
            public string Name { get; set; }

            public Color TestColor { get; set; }

            public IEnumerable<KeyValuePair<string, string>> TestColors { get; set; }

            public IEnumerable<KeyValuePair<string, string>> GetTestColors()
            {
                return this.TestColors;
            }
        }

        private enum Color
        {
            Green = 1
        }
    }
}
