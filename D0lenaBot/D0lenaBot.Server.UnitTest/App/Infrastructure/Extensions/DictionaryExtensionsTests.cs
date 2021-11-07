using D0lenaBot.Server.App.Infrastructure.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace D0lenaBot.Server.UnitTest.App.Infrastructure.Extensions
{
    [TestClass]
    public class DictionaryExtensionsTests
    {
        [TestMethod]
        public void It_parses_a_key_value_pair_dictionary_into_query_strings_arguments()
        {
            // Arrange
            var values = new Dictionary<string, string>();
            values.Add("id", "123");
            values.Add("text", "This is a message with <html> code");

            // Act
            var result = values.ToQueryStringArgs();

            // Assert
            var expectedResult = "id=123&text=This is a message with &lt;html&gt; code";
            Assert.AreEqual(expectedResult, result);
        }
    }
}