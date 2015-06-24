namespace Devshed.Csv
{

    /// <summary>
    ///
    /// </summary>
    public enum ElementProcessing
    {
        /// <summary>
        /// Not set. By default strict is used.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Validate all constraint when reading a CSV file.
        /// </summary>
        Strict = 1,

        /// <summary>
        /// Only check for less elements than headers (recommended at least).
        /// </summary>
        OnlyTooFew = 2,

        /// <summary>
        /// Only check for more elements than headers. When undefined, string.Empty will be returned.
        /// </summary>
        OnlyTooMany = 3,

        /// <summary>
        /// No validation, either too many or too few will result in string.Empty values.
        /// </summary>
        Loose = 4
    }
}