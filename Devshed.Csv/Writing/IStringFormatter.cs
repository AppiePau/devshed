namespace Devshed.Csv.Writing
{
    /// <summary>Formats string output values that are being written into the output files.</summary>
    public interface IStringFormatter
    {
        /// <summary>Formats the string for cell writing output.</summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        string FormatStringCell(string value);

        /// <summary>Formats the string for cell writing output.</summary>
        /// <param name="value">The value.</param>
        /// <param name="removeEnters">if set to <c>true</c> to remove enters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        string FormatStringCell(string value, bool removeEnters);

        /// <summary>Formats the cell for writing output.</summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        string FormatCell(string value);

        /// <summary>Formats the cell for writing output.</summary>
        /// <param name="text">The text.</param>
        /// <param name="removeNewLineCharacters">if set to <c>true</c> [remove new line characters].</param>
        /// <returns>
        ///   <br />
        /// </returns>
        string FormatCell(string text, bool removeNewLineCharacters);
    }
}