using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AMS.Helpers
{
    public static class ProjectExtensionMethod
    {
        public static IEnumerable<TO> Map<TI, TO>(this IEnumerable<TI> seznam, Func<TI, TO> mapper)
        {
            return seznam.Select(mapper);
        }

        public static string RemoveSpecialCharacters(this string _string)
        {
            return Regex.Replace(_string, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
    }
}
