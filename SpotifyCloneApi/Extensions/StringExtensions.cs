using System.Collections.Generic;
using System.Linq;

namespace SpotifyCloneApi.Extensions
{
    public static class StringExtensions
    {
        public static string RemoveWhiteSpaceAndSpecialCharacters(this string value)
            => string.Join("", value?.Where(x => !char.IsWhiteSpace(x) && char.IsLetterOrDigit(x)))?.ToLower();

        public static string RemoveWhiteSpaceAndSpecialCharacters(this IEnumerable<char> value)
            => string.Join("", value?.Where(x => !char.IsWhiteSpace(x) && char.IsLetterOrDigit(x)))?.ToLower();

        public static string RemoveWhiteSpace(this string value)
            => string.Join("", value?.Where(x => !char.IsWhiteSpace(x)))?.ToLower();
    }
}
