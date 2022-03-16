namespace Devshed.Csv.Tests
{
    using System;
    using System.Collections.Generic;
    using Devshed.Shared;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DynamicCsvColumnTests
    {
        private const string Header1 = "Header1";

        private const string Header2 = "Header2";

        private const string Header3 = "Header3";

        [TestMethod]
        public void Composition_FromPropertyWithAllHeaderNames_ResolvesAllHeaderName()
        {
            var rows = new[] {
                new DynamicTestModel
                {
                    Name = "Main name",
                    Columns = new[] { new TestColumn { Name = "Test One", Type = 1, Value = "One" }, new TestColumn { Name = "Test Two", Type = 2, Value = "Two" } }
                }
            };

            var definition =
                new CsvDefinition<DynamicTestModel>(
                    new TextCsvColumn<DynamicTestModel>(e => e.Name),
                    new DynamicCsvColumn<DynamicTestModel, TestColumn>(e => e.Columns, column =>
                    {
                        if (column.Type == 1)
                        {
                            return new TextCsvColumn<TestColumn>(sf => sf.Value) { HeaderName = column.Name };
                        }
                        else if (column.Type == 2)
                        {
                            return new TextCsvColumn<TestColumn>(sf => sf.Value) { HeaderName = column.Name, Format = (v, c) => v.ToString() };
                        }

                        throw new InvalidOperationException();
                    }))
                {
                    FirstRowContainsHeaders = true,
                    WriteBitOrderMarker = false
                };

            var result = CsvWriter.WriteAsCsv(definition, rows).GetString();

            Assert.AreEqual($"\"Name\";\"Test One\";\"Test Two\"{Environment.NewLine}\"Main name\";\"One\";\"Two\"{Environment.NewLine}", result);
        }

        private sealed class DynamicTestModel
        {
            public string Name { get; set; }

            public TestColumn[] Columns { get; set; }
        }

        private class TestColumn
        {
            public string Name { get; set; }

            public string Value { get; set; }

            public int Type { get; set; }

        }
    }
}
