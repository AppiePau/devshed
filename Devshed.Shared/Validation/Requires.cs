namespace Devshed.Shared
{
    using System;

    /// <summary>
    /// Helps validating values.
    /// </summary>
    public static class Requires
    {
        /// <summary>
        /// Determines whether the value is not null.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException"> In cas of a null value an ArgumentNullException is thrown. </exception>
        public static void IsNotNull(object value, string name)
        {
            if (value == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Determines whether the value starts with the specified string seqence.
        /// </summary>
        /// <exception cref="System.FormatException"> A FormatException is thrown when the string does not start with the specified string. </exception>
        public static void StartsWith(string value, string start, string name)
        {
            Requires.IsNotNull(value, "value");

            if (!value.StartsWith(start))
            {
                throw new FormatException("The parameter '" + name + "' does not start with '" + start + "'.");
            }
        }

        /// <summary>
        /// Determines whether the value ends with the specified string seqence.
        /// </summary>
        /// <exception cref="System.FormatException"> A FormatException is thrown when the string does not start with the specified string. </exception>
        public static void EndsWith(string value, string end, string name)
        {
            Requires.IsNotNull(value, "value");

            if (!value.EndsWith(end))
            {
                throw new FormatException("The parameter '" + name + "' does not start with '" + end + "'.");
            }
        }

        /// <summary>
        /// Determines whether the value is not null or an empty string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentNullException"> An ArgumentNullException is thrown when the value is null or empty. </exception>
        public static void IsNotNullOrEmpty(string value, string name)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Determines whether the value is not null or an white space string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="name">The name.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <exception cref="System.ArgumentNullException"> An ArgumentNullException is thrown when the value is null or white space. </exception>
        public static void IsNotNullOrWhiteSpace(string value, string name)
        {
            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(value.Trim()))
            {
                throw new ArgumentNullException(name);
            }
        }
    }
}
