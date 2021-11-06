using D0lenaBot.Server.App.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace D0lenaBot.Server.UnitTest.App.Infrastructure.DolarSi
{
    [TestClass]
    public class DolarSiValuesParserTests
    {
        private DolarSiValuesParser target;

        [TestInitialize]
        public void TestInit()
        {
            this.target = new DolarSiValuesParser();
        }

        [TestMethod]
        public void It_parses_a_DolarSi_date_content_into_a_valid_date()
        {
            // Arrange
            var content = "ACTUALIZADO: 05/11/2021 17:00";

            // Act
            var result = this.target.ParseDate(content);

            // Assert
            Assert.AreEqual(new DateTime(2021, 11, 5, 0, 0, 0, DateTimeKind.Utc), result);
        }


        [TestMethod]
        public void It_parses_a_DolarSi_exchange_content_that_has_a_comma_as_decimal_separator_into_a_valid_decimal()
        {
            // Arrange
            var content = "$ 196,50";

            // Act
            var result = this.target.ParseRateToDecimal(content);

            // Assert
            Assert.AreEqual(196.5m, result);
        }

        [TestMethod]
        public void It_parses_a_DolarSi_exchange_content_that_has_a_dot_as_decimal_separator_into_a_valid_decimal()
        {
            // Arrange
            var content = "$ 196.50";

            // Act
            var result = this.target.ParseRateToDecimal(content);

            // Assert
            Assert.AreEqual(196.5m, result);
        }
    }
}
