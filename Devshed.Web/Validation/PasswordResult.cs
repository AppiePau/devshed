using System;
using System.Linq;
using System.Collections.Generic;
namespace Devshed.Web
{
    using System.Globalization;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Text.RegularExpressions;
    using System.ComponentModel;


    public sealed class PasswordResult
    {
        public bool HasUpperAndLowerCaseLetter { get; internal set; }

        public bool HasOneDigit { get; internal set; }

        public PasswordScore Score { get; internal set; }

        public bool OverMinimumLength { get; internal set; }

        public int MinimumLength { get; internal set; }
    }

}