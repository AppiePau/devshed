using System.Globalization;
using System.Text;

namespace Devshed.Csv
{
    public interface ICsvDefinition
    {
        string ElementDelimiter { get; }

        ElementProcessing ElementProcessing { get; }

        Encoding Encoding { get; }

        bool FirstRowContainsHeaders { get; }

        bool HasFieldsEnclosedInQuotes { get; }

        bool IgnoreReadonlyProperties { get; }

        bool RemoveNewLineCharacters { get; }

        bool ThrowExceptionOnError { get; }

        bool WriteBitOrderMarker { get; }

        CultureInfo FormattingCulture { get; }
    }
}