namespace Devshed.Csv.Writing
{
    /// <summary>Formats string output values that are being written into the output files.</summary>
    public interface IStringFormatter
    {
        /// <summary>Formats the string for cell wrinting output as text.</summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        string FormatForcedExcelStringCell(string value);

        /// <summary>Formats the string for cell wrinting output as text.</summary>
        /// <param name="value">The value.</param>
        /// <param name="removeEnters">if set to <c>true</c> [remove enters].</param>
        /// <returns>
        ///   <br />
        /// </returns>
        string FormatForcedExcelStringCell(string value, bool removeEnters);

        /// <summary>Formats the string for cell wrinting output.</summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        string FormatStringCell(string value);

        /// <summary>Formats the string for cell wrinting output.</summary>
        /// <param name="value">The value.</param>
        /// <param name="removeEnters">if set to <c>true</c> to remove enters.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        string FormatStringCell(string value, bool removeEnters);
    }
}