using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Utility.ExtensionMethods
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string thisString)
        {
            return string.IsNullOrEmpty(thisString);
        }
        public static bool IsNotNullOrEmpty(this string thisString)
        {
            return !string.IsNullOrEmpty(thisString);
        }

        public static bool Contains(this string thisString, string thatString, bool isCaseSensitive)
        {
            return thisString.IndexOf(
                        thatString,
                        isCaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}
