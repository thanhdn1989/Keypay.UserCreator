using System;
using UserCreator.ParsingRules;
using Xunit;

namespace UserCreator.Test.Presentation
{
    public class SalaryParserTests
    {
        private SalaryParser CreateSut()
        {
            return new SalaryParser();
        }

        [Fact]
        public void Parse_ValidDecimal_ShouldReturnValue()
        {
            // Arrange
            var sut = CreateSut();
            const string input = "1.2";
            
            // Act
            var result = sut.Parse(input);

            // Verify
            Assert.Equal((decimal)1.2, result);
        }
        
        [Fact]
        public void Parse_InValidDecimal_ShouldRaiseException()
        {
            // Arrange
            var sut = CreateSut();
            var input = Guid.NewGuid().ToString();
            
            // Act

            // Verify
            Assert.Throws<FormatException>(() => sut.Parse(input));
        }
    }
}