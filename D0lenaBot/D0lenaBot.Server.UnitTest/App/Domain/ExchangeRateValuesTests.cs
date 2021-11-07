using D0lenaBot.Server.App.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D0lenaBot.Server.UnitTest.App.Domain
{
    [TestClass]
    public class ExchangeRateValuesTests
    {
        [DataTestMethod]
        [DataRow(197, 201, 199)]
        [DataRow(197.5, 201.67, 199.59)]
        [DataRow(197.102, 201, 199.05)]
        [DataRow(197.798, 201, 199.4)]

        public void It_rounds_the_average_to_two_decimals(double buy, double sell, double expected)
        {
            // Arrange
            var target = new ExchangeRateValues((decimal)buy, (decimal)sell);

            // Act
            var result = target.Average;

            // Assert
            Assert.AreEqual((decimal)expected, result);
        }
    }
}
