using Devshed.Csv.Writing;
using System.Text;

namespace Devshed.Csv
{
    internal sealed class CsvStringFormatter : IStringFormatter
    {
        public string FormatCell(string value)
        {
            return FormatCell(value, false);
        }

        public string FormatCell(string value, bool removeNewLineCharacters)
        {
            var sb = new StringBuilder(value);
            sb.Replace("\"", "");

            if (removeNewLineCharacters)
            {
                sb.Replace("\r\n", "");
                sb.Replace("\n", "");
            }

            return sb.ToString();
        }

        public string FormatStringCell(string value)
        {
            return FormatStringCell(value, false);
        }

        public string FormatStringCell(string value, bool removeNewLineCharacters)
        {
            var sb = new StringBuilder(value);
            sb.Replace("\"", "\"\"");

            if (removeNewLineCharacters)
            {
                sb.Replace("\r\n", "");
                sb.Replace("\n", "");
            }

            sb.Insert(0, "\"");
            sb.Append("\"");

            return sb.ToString();
        }
    }
}