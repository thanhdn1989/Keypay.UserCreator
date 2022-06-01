using System.Threading.Tasks;
using UserCreator.Core;
using UserCreator.Core.Constants;
using Xunit;

namespace UserCreator.Test.Core
{
    public class RecoveryServiceTests
    {
        [Fact]
        public void RecoverFromLastSession_ShouldEnsureCorrectId()
        {
            // Arrange
            var identityManager = new IdentityManager();
            var sut = new RecoveryService(identityManager);

            // Act
            sut.TryRecoverLastSession(@"Data/Users.txt");

            // Verify
            Assert.Equal(5, identityManager.GetNext(FieldConstants.DateOfBirth));
            Assert.Equal(5, identityManager.GetNext(FieldConstants.Salary));
            Assert.Equal(2, identityManager.GetNext(FieldConstants.DataField));
        }
    }
}