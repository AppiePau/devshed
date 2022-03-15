namespace Devshed.Csv.Tests
{
    using System.Linq;
    using System.Collections.Generic;
    using Devshed.Shared;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TypedCsvColumnTests
    {
        [TestMethod]
        public void New_Type_ResolvesHeaderName()
        {
            var column = new TypedCsvColumn<CompositeTestModel, TestColor>(e => e.TestColor);
            Assert.AreEqual("TestColor", column.HeaderName);

        }

        [TestMethod]
        public void Composition_Type_ResolvesHeaderName()
        {
            var models = new[]
            {
                new CompositeTestModel
                {
                    Name = "OK_NAME",
                    TestColors = new[] { new CompositeColumnValue<string>("Kleur1", "Blue"),
                    new CompositeColumnValue<string>("Kleur2", "Green"),
                    new CompositeColumnValue<string>("Kleur3", "Red")}
                }
            };

            var definition =
                new CsvDefinition<CompositeTestModel>(
                    new TextCsvColumn<CompositeTestModel>(e => e.Name),
                    new ObjectCsvColumn<CompositeTestModel>(e => e.TestColor),
                    new CompositeCsvColumn<CompositeTestModel, string>(e => e.TestColors, "Kleur1", "Kleur2"))
                {
                    FirstRowContainsHeaders = true
                };

            var result = CsvWriter.CreateStream(definition, models).GetString();

            Assert.AreEqual("\"Name\";\"TestColor\";\"Kleur1\";\"Kleur2\"\r\n"
                            + "\"OK_NAME\";0;\"Blue\";\"Green\"\r\n", result);
        }

        [TestMethod]
        public void Composition_FromMethod_ResolvesHeaderName()
        {
             var models = new[]
            {
                new CompositeTestModel
                {
                    Name = "OK_NAME",
                    TestColors = new[] { new CompositeColumnValue<string>("Kleur1", "Blue"),
                    new CompositeColumnValue<string>("Kleur2", "Green"),
                    new CompositeColumnValue<string>("Kleur3", "Red")}
                }
            };

            var definition =
                new CsvDefinition<CompositeTestModel>(
                    new TextCsvColumn<CompositeTestModel>(e => e.Name),
                    new ObjectCsvColumn<CompositeTestModel>(e => e.TestColor),
                    new CompositeCsvColumn<CompositeTestModel, string>(e => e.TestColors, "Kleur1", "Kleur2"))
                {
                    FirstRowContainsHeaders = true
                };

            var result = CsvWriter.CreateStream(definition, models).GetString();

            Assert.AreEqual("\"Name\";\"TestColor\";\"Kleur1\";\"Kleur2\"\r\n"
                            + "\"OK_NAME\";0;\"Blue\";\"Green\"\r\n", result);
        }

        [TestMethod]
        public void Composition_NotInCollection_ResolvesHeaderName()
        {
            var models = new[]
            {
                new CompositeTestModel
                {
                    Name = "OK_NAME",
                    TestColors = new[] { new CompositeColumnValue<string>("Kleur1", "Blue"),
                    new CompositeColumnValue<string>("Kleur2", "Green"),
                    new CompositeColumnValue<string>("Kleur3", "Red")}
                }
            };

            var definition =
                new CsvDefinition<CompositeTestModel>(
                    new TextCsvColumn<CompositeTestModel>(e => e.Name),
                    new ObjectCsvColumn<CompositeTestModel>(e => e.TestColor),
                    new CompositeCsvColumn<CompositeTestModel, string>(e => e.TestColors, "Kleur1", "Kleur2"))
                {
                    FirstRowContainsHeaders = true
                };

            var result = CsvWriter.CreateStream(definition, models).GetString();

            Assert.AreEqual("\"Name\";\"TestColor\";\"Kleur1\";\"Kleur2\"\r\n"
                            + "\"OK_NAME\";0;\"Blue\";\"Green\"\r\n", result);
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
