using System;
using System.Linq;
using System.Collections.Generic;

namespace Devshed.Csv
{
    /// <summary>
    /// The CSV value types that are supported.
    /// </summary>
    public enum ColumnDataType
    {
        /// <summary>
        /// Textual value.
        /// </summary>
        Text = 1,

        /// <summary>
        /// Number integer value
        /// </summary>
        Number = 2,

        /// <summary>
        /// Number decimal value
        /// </summary>
        Decimal = 3,

        /// <summary>
        /// Datetime value
        /// </summary>
        DateTime = 4,

        /// <summary>
        /// Time value
        /// </summary>
        Time = 5,

        /// <summary>
        /// Current decimal value
        /// </summary>
        Currency = 6,

        /// <summary>
        /// Boolean true/false value
        /// </summary>
        Boolean = 7,

        /// <summary>
        /// A mixed content type value
        /// </summary>
        Composite = 8,

        /// <summary>
        /// A column defined by a strong type value.
        /// </summary>
        StrongTyped = 9,

        /// <summary>
        /// Object unspecified value
        /// </summary>
        Object = 10,

        /// <summary>
        /// Dynamic value
        /// </summary>
        Dynamic = 11
    }
}