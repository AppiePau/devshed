using System.Text;

namespace Devshed.Csv.Writing
{
    //internal class CsvStringFormatter : IStringFormatter
    //{
    //    public  string FormatStringCell(string value)
    //    {
    //        return FormatStringCell(value, true);
    //    }

    //    public string FormatForcedExcelStringCell(string value)
    //    {
    //        return FormatForcedExcelStringCell(value, true);
    //    }

    //    public string FormatForcedExcelStringCell(string value, bool removeEnters)
    //    {
    //        return string.Format("={0}", FormatStringCell(value, removeEnters));
    //    }

    //    public string FormatStringCell(string value, bool removeEnters)
    //    {
    //        var sb = new StringBuilder(value);
    //        sb.Replace("\"", "\"\"");

    //        if (removeEnters)
    //        {
    //            sb.Replace("\r\n", "");
    //            sb.Replace("\n", "");
    //        }

    //        sb.Insert(0, "\"");
    //        sb.Append("\"");

    //        return sb.ToString();
    //    }
    //}
}