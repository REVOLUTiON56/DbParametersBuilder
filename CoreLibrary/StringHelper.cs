using System;
using System.Text;

namespace CoreLibrary
{
    public static class StringHelper
    {
        /// <summary>
        /// Returns the input string with the first character converted to uppercase
        /// </summary>
        public static string FirstLetterToUpperCase(this string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                throw new ArgumentException("There is no first letter");
            }
            char[] chars = source.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);

            return new string(chars);
        }

        public static string ToUnderscoreCase(this string input)
        {
            if (input == null)
                return null;

            if (input.Length == 0)
                return string.Empty;

            var builder = new StringBuilder();
            builder.Append(char.ToLower(input[0]));

            for (var i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i]))
                    builder.Append('_');

                builder.Append(char.ToLower(input[i]));
            }

            return builder.ToString();
        }
    }
}