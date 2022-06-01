using System;
using System.Linq;
using UserCreator.Core;
using UserCreator.Core.Constants;
using Xunit;

namespace UserCreator.Test.Core
{
    public class IdentityManagerTest
    {
        private IdentityManager CreateSut()
        {
            return new IdentityManager();
        }
        
        [Fact]
        public void GetId_NotExistFieldName_ShouldReturnOne()
        {
            // Arrange
            var sut = CreateSut();

            // Act
            var firstId = sut.GetNext(Guid.NewGuid().ToString());
            var secondId = sut.GetNext(FieldConstants.DateOfBirth);

            // Verify
            Assert.Equal(1, firstId);
            Assert.Equal(1, secondId);
        }

        [Fact]
        public void GetId_ExistFieldName_ShouldIncreaseIdValue()
        {
            // Arrange
            var sut = CreateSut();
            sut.GetNext(FieldConstants.DateOfBirth);
            
            // Act
            var ids = Enumerable
                .Range(0, 1000)
                .AsParallel()
                .WithDegreeOfParallelism(32)
                .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                .Select(_ => sut.GetNext(FieldConstants.DateOfBirth))
                .ToList(); // Force parallelism 
            
            // Verify
            Assert.DoesNotContain(ids.GroupBy(id => id), g => g.Count() > 1); // Ensure unique Id
            Assert.Equal(1001, ids.Max()); // Ensure id will be generated unique and sequentially
        }
    }
}