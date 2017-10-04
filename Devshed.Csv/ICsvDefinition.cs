using System.Text;

namespace Devshed.Csv
{
    public interface ICsvDefinition
    {
        string ElementDelimiter { get; set; }
        ElementProcessing ElementProcessing { get; set; }
        Encoding Encoding { get; set; }
        bool FirstRowContainsHeaders { get; set; }
        bool HasFieldsEnclosedInQuotes { get; set; }
        bool IgnoreReadonlyProperties { get; set; }
        bool RemoveNewLineCharacters { get; set; }
        bool ThrowExceptionOnError { get; set; }
        bool WriteBitOrderMarker { get; set; }
    }
}