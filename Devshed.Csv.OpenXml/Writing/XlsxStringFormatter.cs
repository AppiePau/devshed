using Devshed.Csv.Writing;

namespace Devshed.Csv.ClosedXml
{
    internal class XlsxStringFormatter : IStringFormatter
    {
        public string FormatForcedExcelStringCell(string value)
        {
            return value;
        }

        public string FormatForcedExcelStringCell(string value, bool removeEnters)
        {
            return value;
        }

        public string FormatStringCell(string value)
        {
            return value;
        }

        public string FormatStringCell(string value, bool removeEnters)
        {
            return value;
        }
    }
}