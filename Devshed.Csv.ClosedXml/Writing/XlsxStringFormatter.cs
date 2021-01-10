using Devshed.Csv.Writing;

namespace Devshed.Csv.ClosedXml
{
    internal class XlsxStringFormatter : IStringFormatter
    {
        public string FormatCell(string value)
        {
            return value;
        }

        public string FormatStringCell(string value)
        {
            return FormatStringCell(value, false);
        }

        public string FormatStringCell(string value, bool removeEnters)
        {
            if (value == null) return string.Empty;

            return value.Replace("\r\n", "");
        }
    }
}