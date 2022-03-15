using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuizK101.Helpers
{
    public static class Utilities
    {
        public static string GetSubstringText(this string Str, string Start, string End)
        {
            try
            {
                var StartingIndex = !string.IsNullOrEmpty(Start) ? Str.IndexOf(Start) + 1 : 0;

                var EndingIndex = (!string.IsNullOrEmpty(End) ? Str.IndexOf(End) : Str.Length);

                var Length = EndingIndex - StartingIndex;

                return Str.Substring(StartingIndex, Length).Trim();
            }
            catch
            {
                return null;
            }
        }
    }
}