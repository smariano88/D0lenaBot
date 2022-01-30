using D0lenaBot.Server.App.Infrastructure.CotizacionCo.Services;
using D0lenaBot.Server.App.Infrastructure.DolarSi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace D0lenaBot.Server.UnitTest.App.Infrastructure.DolarSi.Services
{
    [TestClass]
    public class CotizacionCoValuesParserTests
    {
        private CotizacionCoValuesParser target;

        [TestInitialize]
        public void TestInit()
        {
            this.target = new CotizacionCoValuesParser();
        }

        [TestMethod]
        public void It_parses_the_date_content_into_a_valid_date()
        {
            // Arrange
            var content = "Valor del Dolar Blue hoy 30/01/2022 en Rosario";

            // Act
            var result = this.target.ParseDate(content);

            // Assert
            Assert.AreEqual(new DateTime(2022, 1, 30, 0, 0, 0, DateTimeKind.Utc), result);
        }


        [TestMethod]
        public void It_parses_a_exchange_rate_content_that_has_a_comma_as_decimal_separator_into_a_valid_decimal()
        {
            // Arrange
            var content = "$ 206,50";

            // Act
            var result = this.target.ParseRateToDecimal(content);

            // Assert
            Assert.AreEqual(206.5m, result);
        }

        [TestMethod]
        public void It_parses_a_exchange_rate_content_that_has_a_dot_as_decimal_separator_into_a_valid_decimal()
        {
            // Arrange
            var content = "$ 206.50";

            // Act
            var result = this.target.ParseRateToDecimal(content);

            // Assert
            Assert.AreEqual(206.5m, result);
        }
    }
}
