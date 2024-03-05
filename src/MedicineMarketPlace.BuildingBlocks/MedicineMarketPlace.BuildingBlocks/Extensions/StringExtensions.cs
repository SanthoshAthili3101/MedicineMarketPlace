using System.Text.RegularExpressions;

namespace MedicineMarketPlace.BuildingBlocks.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNotBlank(this string value)
        {
            return !value.IsBlank();
        }

        public static bool IsBlank(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool CompareString(this string thisString, string compareString)
        {
            return string.Equals(thisString, compareString, StringComparison.CurrentCultureIgnoreCase);
        }

        public static string GenerateRoute(this string inputString)
        {
            inputString = new Regex("[*'\"/.,_~`#^@$%!?+=|:;-<>()[{}]").Replace(inputString.ToLower(), " ");
            inputString = inputString.Replace("\\", " ");
            inputString = inputString.Replace("/", " ");
            inputString = inputString.Replace("@", " ");
            inputString = inputString.Replace("-", " ");
            inputString = new Regex("]").Replace(inputString, " ");
            inputString = new Regex("-").Replace(inputString, " ");
            inputString = new Regex("&").Replace(inputString, "and");
            inputString = new Regex("\\s+").Replace(inputString.Trim(), "-");
            return inputString;
        }

        public static string GeneratePassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            Random random = new Random();
            char[] password = new char[length];
            int index = 0;

            // Generate at least one uppercase letter
            password[index++] = validChars[random.Next(26)];
            // Generate at least one lowercase letter
            password[index++] = validChars[random.Next(26, 52)];
            // Generate at least one digit
            password[index++] = validChars[random.Next(52, 62)];
            // Generate at least one special character
            password[index++] = validChars[random.Next(62, validChars.Length)];

            for (; index < length; index++)
            {
                password[index] = validChars[random.Next(validChars.Length)];
            }

            // Shuffle the characters in the password
            for (int i = 0; i < length; i++)
            {
                int j = random.Next(i, length);
                char temp = password[i];
                password[i] = password[j];
                password[j] = temp;
            }

            return new string(password);
        }

        public static string GenerateUniqueKey()
        {
            var unixTimestamp = ((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).Ticks).ToString();
            return new Random().Next(0, 9999).ToString("D4") + unixTimestamp.Substring(unixTimestamp.Length - 6) + new Random().Next(0, 9999).ToString("D4");
        }
    }
}
