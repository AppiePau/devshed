using System;
using System.Linq;
using System.Collections.Generic;

namespace Devshed.Csv
{
    public enum ColumnDataType
    {
        Text = 1,
        Number = 2,
        Decimal = 3,
        DateTime = 4,
        Time = 5,
        Currency = 6,
        Boolean = 7,
        Composite = 8,
        StrongTyped = 9,
        Object = 10,
        Dynamic = 11
    }
}