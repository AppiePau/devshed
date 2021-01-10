namespace Devshed.Csv.Writing
{
    public interface IStringFormatter
    {
        string FormatStringCell(string value);
        string FormatStringCell(string value, bool removeEnters);
        string FormatCell(string value);
        string FormatCell(string text, bool removeNewLineCharacters);
    }
}