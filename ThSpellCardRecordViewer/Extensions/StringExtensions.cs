namespace ThSpellCardRecordViewer.Extensions
{
    internal static class StringExtensions
    {
        public static string TranslateUnixTime(this int unixTime)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime.ToString();
        }

        public static int TranslateHexadecimal(this string hexadecimal)
        {
            return Convert.ToInt32(hexadecimal, 16);
        }

        public static string RemoveRightOf(this string text, string removeLetter)
        {
            int length = text.IndexOf(removeLetter);
            if (length < 0)
            {
                return text;
            }

            return text.Substring(0, length);
        }
    }
}
