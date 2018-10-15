namespace Devshed.Csv.Writing
{
    public interface IStringFormatter
    {
        string FormatForcedExcelStringCell(string value);
        string FormatForcedExcelStringCell(string value, bool removeEnters);
        string FormatStringCell(string value);
        string FormatStringCell(string value, bool removeEnters);
    }
}