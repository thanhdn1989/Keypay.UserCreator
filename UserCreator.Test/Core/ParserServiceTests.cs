using System;
using System.Globalization;
using Moq;
using UserCreator.Core;
using UserCreator.Core.Constants;
using UserCreator.Core.Contracts;
using UserCreator.ParsingRules;
using Xunit;

namespace UserCreator.Test.Core
{
    public class ParserServiceTests
    {
        private readonly Mock<IServiceProvider> _serviceProvider;
        private readonly ParserProvider _parserProvider;

        public ParserServiceTests()
        {
            _serviceProvider = new Mock<IServiceProvider>();
        }

        private void RegisterRule()
        {
            _serviceProvider.Setup(s => s.GetService(typeof(DateOfBirthParser)))
                .Returns(new DateOfBirthParser());
            
            
            _serviceProvider.Setup(s => s.GetService(typeof(SalaryParser)))
                .Returns(new SalaryParser());
        }

        private ParserService CreateSut()
        {
            return new ParserService(new ParserProvider(_serviceProvider.Object));
        }
        
        [Fact]
        public void ParseInput_WithEmptyParsingRule_ShouldReturnInputAsString()
        {
            // Arrange
            var guidInput = Guid.NewGuid().ToString();
            var stringInput = "This is a test";
            var decimalInput = "1.2";
            var sut = CreateSut();

            // Act
            var guidParseResult =
                sut.TryParse(new Field(FieldConstants.DateOfBirth, guidInput), out var guidParsed);

            var stringParseResult =
                sut.TryParse(new Field(FieldConstants.DateOfBirth, stringInput), out var stringParsed);

            var decimalParseResult = sut.TryParse(new Field(FieldConstants.DateOfBirth, decimalInput),
                out var decimalParsed);

            // Verify
            Assert.True(guidParseResult && stringParseResult && decimalParseResult);
            Assert.Equal(guidInput, guidParsed);
            Assert.Equal(stringInput, stringParsed);
            Assert.Equal(decimalInput, decimalParsed);
        }
        
        [Fact]
        public void ParseInput_WithExistedParsingRule_ShouldRespectParserRule()
        {
            // Arrange
            var decimalInput = "1.2";
            var dateOfBirth = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            RegisterRule();
            var sut = CreateSut();
            
            // Act
            var dateOfBirthParseResult =
                sut.TryParse(new Field(FieldConstants.DateOfBirth, dateOfBirth), out var dateOfBirthParsed);

            var stringParseResult =
                sut.TryParse(new Field(FieldConstants.DateOfBirth, Guid.NewGuid().ToString()), out var stringParsed);

            var salaryParseResult = sut.TryParse(new Field(FieldConstants.Salary, decimalInput),
                out var decimalParsed);
            

            // Verify
            Assert.False(stringParseResult);
            Assert.True(dateOfBirthParseResult);
            Assert.True(salaryParseResult);
            Assert.Equal(dateOfBirth, dateOfBirthParsed);
            Assert.Equal(default(DateTime).ToString(CultureInfo.InvariantCulture), stringParsed);
            Assert.Equal(decimalInput, decimalParsed);
        }
    }
}