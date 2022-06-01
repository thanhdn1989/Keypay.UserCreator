using System;
using System.Globalization;
using UserCreator.ParsingRules;
using Xunit;

namespace UserCreator.Test.Presentation
{
    public class DateOfBirthParserTest
    {
        private DateOfBirthParser CreateSut()
        {
            return new DateOfBirthParser();
        }

        [Fact]
        public void Parse_ValidDateTime_ShouldReturnCorrectValue()
        {
            // Arrange
            var sut = CreateSut();
            var input = DateTime.Now;
            
            // Act
            var result = sut.TryParse(input.ToString(CultureInfo.CurrentCulture), out var data);

            // Verify
            Assert.True(result);
            Assert.NotEqual(input, data);
        }

        [Fact]
        public void Parse_InvalidDateTime_ShouldReturnFalse()
        {
            // Arrange
            var sut = CreateSut();
            var input = Guid.NewGuid().ToString();
            
            // Act
            var result = sut.TryParse(input, out var data);

            // Verify
            Assert.False(result);
            Assert.Equal(default(DateTime), data);
        }
    }
}