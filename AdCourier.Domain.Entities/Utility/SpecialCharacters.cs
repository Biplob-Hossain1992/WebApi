using System.Text.RegularExpressions;

namespace AdCourier.Domain.Entities.Utility
{
    public static class SpecialCharacters
    {
        public static string RemoveSpecialCharacters(string stringValue)
        {
            string[] chars = new string[] { "\n", "\t", ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", @"\", ";", "_", "(", ")", ":", "|", "[", "]" };
            for (int i = 0; i < chars.Length; i++)
            {
                if (stringValue.Contains(chars[i]))
                {
                    stringValue = stringValue.Replace(chars[i], "");
                }
            }

            stringValue = Regex.Replace(stringValue, @"\s+", " ", RegexOptions.Multiline);
            return stringValue.Trim();
        }
    }
}
