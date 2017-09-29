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
        Currency = 5,
        Boolean = 6,
        Composite = 7,
        StrongTyped = 8,
        Object = 9
    }

}