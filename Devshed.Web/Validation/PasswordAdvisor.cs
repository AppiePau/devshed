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


    /// <summary>
    /// </summary>
    /// <seealso cref="https://stackoverflow.com/questions/12899876/checking-strings-for-a-strong-enough-password"/>
    /// <seealso cref="https://social.msdn.microsoft.com/Forums/vstudio/en-US/5e3f27d2-49af-410a-85a2-3c47e3f77fb1/how-to-check-for-password-strength?forum=csharpgeneral"/>
    public static class PasswordAdvisor
    {
        public static PasswordResult CheckStrength(string password, int minLength = 8)
        {
            if (minLength < 4) { throw new ArgumentOutOfRangeException("Minimum length must be at least 4 characters."); }

            var result = new PasswordResult { MinimumLength = minLength };
            int score = 0;

            if (password.Length < 1)
                return new PasswordResult { Score = PasswordScore.Blank };

            if (password.Length < 4)
                return new PasswordResult { Score = PasswordScore.VeryWeak };

            if (password.Length >= minLength)
            {
                result.OverMinimumLength = true;
                score++;
            }

            if (password.Length >= minLength + 3)
            {
                score++;
            }

            if (MatchPattern(password, @"\d+").Success)
            {
                result.HasOneDigit = true;
                score++;
            }

            if (MatchPattern(password, @"[a-z]").Success && MatchPattern(password, @"[A-Z]").Success)
            {
                result.HasUpperAndLowerCaseLetter = true;
                score++;
            }

            if (MatchPattern(password, @".[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]").Success)
            {
                result.OverMinimumLength = true;
                score++;
            }


            result.Score = (PasswordScore)score;

            return result;
        }

        private static Match MatchPattern(string password, string pattern)
        {
            return Regex.Match(password, pattern, RegexOptions.ECMAScript);
        }
    }

}