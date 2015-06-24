using System.Text;

namespace Devshed.Csv.Writing
{
    internal static class CsvString
    {
        internal static string FormatStringCell(string value)
        {
            return FormatStringCell(value, true);
        }

        internal static string FormatForcedExcelStringCell(string value)
        {
            return FormatForcedExcelStringCell(value, true);
        }

        internal static string FormatForcedExcelStringCell(string value, bool removeEnters)
        {
            return string.Format("={0}", FormatStringCell(value, removeEnters));
        }

        internal static string FormatStringCell(string value, bool removeEnters)
        {
            var sb = new StringBuilder(value);
            sb.Replace("\"", "\"\"");

            if (removeEnters)
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