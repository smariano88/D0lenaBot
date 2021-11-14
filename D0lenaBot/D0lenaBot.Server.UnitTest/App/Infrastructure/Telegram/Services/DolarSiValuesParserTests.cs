using D0lenaBot.Server.App.Infrastructure.Telegram.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace D0lenaBot.Server.UnitTest.App.Infrastructure.Telegram.Services
{
    [TestClass]
    public class TelegramMessageBuilderTests
    {
        private TelegramMessageBuilder target;

        [TestInitialize]
        public void TestInit()
        {
            this.target = new TelegramMessageBuilder();
        }

        [TestMethod]
        public void It_adds_the_provided_text_in_bold()
        {
            // Arrange
            var content = "Some text with a unsanitized character!";

            // Act
            var result = this.target.AddBoldText(content).ToString();

            // Assert
            Assert.AreEqual("*Some text with a unsanitized character\\!*", result);
        }

        [TestMethod]
        public void It_adds_the_provided_text_in_italic()
        {
            // Arrange
            var content = "Some text with a unsanitized character!";

            // Act
            var result = this.target.AddItalicText(content).ToString();

            // Assert
            Assert.AreEqual("_Some text with a unsanitized character\\!_", result);
        }

        [TestMethod]
        public void It_adds_a_new_line_character()
        {
            // Act
            var result = this.target.AddNewLine().ToString();

            // Assert
            Assert.AreEqual("%0A", result);
        }


        [TestMethod]
        public void It_adds_the_provided_text_as_it_is()
        {
            // Arrange
            var content = "Some text";

            // Act
            var result = this.target.AddText(content).ToString();

            // Assert
            Assert.AreEqual("Some text", result);
        }

        [TestMethod]
        public void It_adds_a_bullet()
        {
            // Act
            var result = this.target.AddBullet().ToString();

            // Assert
            Assert.AreEqual("● ", result);
        }

        [TestMethod]
        public void It_sanitizes_all_the_special_characters_that_it_needs()
        {
            // Arrange
            var content = "!().°#$%&/=?¡*¨][_:;,-{}´+'¿<>¬~`^";

            // Act
            var result = this.target.AddText(content).ToString();

            // Assert
            Assert.AreEqual("\\!\\(\\)\\.°#$%&/=?¡*¨][_:;,-{}´+'¿<>¬~`^", result);
        }
    }
}
