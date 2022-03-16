namespace Devshed.Csv.Tests
{
    using System.IO;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Devshed.Shared;
    using System.Linq;
    using System.Text;
    using Reading;

    [TestClass]
    public class CsvStreamMapperTests
    {
        private static readonly string Bom = Encoding.Unicode.GetString(Encoding.Unicode.GetPreamble());

        private readonly string CsvHeader = "\"Id\";\"Name\";\"IsActive\"" + Environment.NewLine;

        private readonly string CsvHeaderDuplicates = "\"Id\";\"Name\";\"Name\"" + Environment.NewLine;


        private const string CsvData = "1;\"John Doe\";TRUE";

        [TestMethod]
        public void Read_CsvWithoutHeader_CreatesUser()
        {
            var definition = this.GetUnicodeDefinition();

            var users = Read(definition, Bom + CsvData);

            var user = users.Single();
            Assert.AreEqual(1, user.Id, "User Id has been read.");
            Assert.AreEqual("John Doe", user.Name, "Name has been read.");
            Assert.AreEqual(true, user.IsActive, "IsActive has been read.");
        }

        [TestMethod]
        public void Read_CsvWithHeader_CreatesUser()
        {
            var definition = this.GetUnicodeDefinition();
            definition.FirstRowContainsHeaders = true;

            var users = Read(definition, Bom + CsvHeader + CsvData);

            var user = users.Single();
            Assert.AreEqual(1, user.Id, "User Id has been read.");
            Assert.AreEqual("John Doe", user.Name, "Name has been read.");
            Assert.AreEqual(true, user.IsActive, "IsActive has been read.");
        }


        [TestMethod]
        public void Read_LowerCaseHeaderNames_CreatesUser()
        {
            var definition = this.GetUnicodeDefinition();
            definition.FirstRowContainsHeaders = true;

            var users = Read(definition, Bom + $"\"id\";\"name\";\"isactive\"{Environment.NewLine}" + CsvData);

            var user = users.Single();
            Assert.AreEqual(1, user.Id, "User Id has been read.");
            Assert.AreEqual("John Doe", user.Name, "Name has been read.");
            Assert.AreEqual(true, user.IsActive, "IsActive has been read.");
        }


        [TestMethod]
        [ExpectedException(typeof(DuplicateHeaderException))]
        public void Read_WithDuplicateHeaderNames_ThrowsException()
        {
            var definition = this.GetUnicodeDefinition();
            definition.FirstRowContainsHeaders = true;

            var users = Read(definition, Bom + CsvHeaderDuplicates + CsvData);

        }


        private CsvDefinition<UserView> GetUnicodeDefinition()
        {
            return new CsvDefinition<UserView>(
                new NumberCsvColumn<UserView>(e => e.Id),
                new TextCsvColumn<UserView>(e => e.Name),
                new BooleanCsvColumn<UserView>(e => e.IsActive))
                {
                    WriteBitOrderMarker = false,
                    Encoding = Encoding.Unicode
                };
        }

        private static UserView[] Read(CsvDefinition<UserView> definition, string data)
        {
            using (var stream = new MemoryStream(data.GetUnicodeBytes()))
            {
                return CsvReader.Read(stream, definition);
            }
        }

        private sealed class UserView
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public bool IsActive { get; set; }
        }
    }
}
