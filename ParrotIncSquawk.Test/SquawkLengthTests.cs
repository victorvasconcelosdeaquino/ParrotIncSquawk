using FluentValidation.Results;
using ParrotIncSquawk.Models;
using ParrotIncSquawk.Validation;
using System.Threading.Tasks;
using Xunit;

namespace ParrotIncSquawk.Test
{
    public class SquawkLengthTests
    {
        [Fact]
        public void SquawkWhenTheTextIsLessThan_1_ShouldFail()
        {
            // Arrange
            SquawkRequest model = new()
            {
                Text = string.Empty,
            };
            SquawkValidation validationRules = new();
            int count = model.Text.Length;

            // Act
            ValidationResult validationResult = validationRules.Validate(model);

            // Assert
            Assert.True(count < 1);
            Assert.False(validationResult.IsValid);
        }

        [Fact]
        public void SquawkWhenTheTextIsGreaterThan_1_ShouldPass()
        {
            // Arrange
            SquawkRequest model = new()
            {
                Text = "Hello!",
            };
            SquawkValidation validationRules = new();
            int count = model.Text.Length;

            // Act
            ValidationResult validationResult = validationRules.Validate(model);

            // Assert
            Assert.True(count > 1);
            Assert.True(validationResult.IsValid);
        }

        [Fact]
        public void SquawkWhenTheTextContainsWordsNotAllowed()
        {
            // Arrange
            SquawkRequest model = new()
            {
                Text = "Tweet",
            };
            SquawkValidation validationRules = new();

            // Act
            ValidationResult validationResult = validationRules.Validate(model);

            // Assert
            Assert.False(validationResult.IsValid);
        }
    }
}
