using System.Globalization;
using System.Text;

namespace Devshed.Csv
{
    /// <summary>Represents the outline of a CSV defition.</summary>
    public interface ICsvDefinition
    {
        /// <summary>Gets the element delimiter between columns.</summary>
        /// <value>The element delimiter.</value>
        string ElementDelimiter { get; }

        /// <summary>Gets the element processing method.</summary>
        /// <value>The element processing.</value>
        ElementProcessing ElementProcessing { get; }

        /// <summary>Gets the encoding for streams.</summary>
        /// <value>The encoding.</value>
        Encoding Encoding { get; }

        /// <summary>Gets a value indicating whether first row contains headers.</summary>
        /// <value>
        ///   <c>true</c> if [first row contains headers]; otherwise, <c>false</c>.</value>
        bool FirstRowContainsHeaders { get; }

        /// <summary>Gets a value indicating whether this instance has fields enclosed in quotes.</summary>
        /// <value>
        ///   <c>true</c> if this instance has fields enclosed in quotes; otherwise, <c>false</c>.</value>
        bool HasFieldsEnclosedInQuotes { get; }

        /// <summary>Gets a value indicating whether to ignore readonly properties in the model.</summary>
        /// <value>
        ///   <c>true</c> if [ignore readonly properties]; otherwise, <c>false</c>.</value>
        bool IgnoreReadonlyProperties { get; }

        /// <summary>Gets a value indicating whether remove new line characters on writing.</summary>
        /// <value>
        ///   <c>true</c> if [remove new line characters]; otherwise, <c>false</c>.</value>
        bool RemoveNewLineCharacters { get; }

        /// <summary>Gets a value indicating whether to throw an exception on error.</summary>
        /// <value>
        ///   <c>true</c> if [throw exception on error]; otherwise, <c>false</c>.</value>
        bool ThrowExceptionOnError { get; }

        /// <summary>Gets a value indicating whether to write the bit order marker (BOM).</summary>
        /// <value>
        ///   <c>true</c> if [write bit order marker]; otherwise, <c>false</c>.</value>
        bool WriteBitOrderMarker { get; }

        /// <summary>Gets the formatting culture.</summary>
        /// <value>The formatting culture.</value>
        CultureInfo FormattingCulture { get; }
    }
}