using System.Text.RegularExpressions;

namespace DaftAppleGames.SubnauticaPets.Extensions
{
    /// <summary>
    /// Useful static extension methods to String
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Adds spaces in CaselCase strings.
        /// So the above becomes "Camel Case".
        /// Used to "prettify" enum strings, for example.
        /// </summary>
        internal static string AddSpacesInCamelCase(this string enumString)
        {
            return string.IsNullOrEmpty(enumString) ? "" : Regex.Replace(enumString, "([A-Z])", " $1").Trim();
        }
    }
}