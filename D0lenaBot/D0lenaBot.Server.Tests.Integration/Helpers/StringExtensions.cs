namespace D0lenaBot.Server.Tests.Integration.Helpers
{
    public static class StringExtensions
    {
        public static bool ContainsOnlyOnce(this string strings, string text)
        {
            int count = 0;
            int i = 0;
            while ((i = strings.IndexOf(text, i)) != -1)
            {
                i += text.Length;
                count++;
            }
            if (count == 1)
            {
                return true;
            }

            return false;
        }
    }
}
