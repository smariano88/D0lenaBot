using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D0lenaBot.Server.Tests.Integration.Helpers
{
    public static class ExtendedAssert
    {
        public static void ContainsTextOnlyOnce(this string target, string textToSearch)
        {
            var timesContained = target.ContainsCount(textToSearch);
            if (timesContained == 1)
            {
                ExtendedAssert.Pass();
                return;
            }

            Assert.Fail($"{textToSearch} found {timesContained} times in string {target}");
        }

        /// <summary>
        /// Method used to make more explicit when a test passes. There is no implementation needed
        /// </summary>
        public static void Pass()
        {
        }
    }
}
