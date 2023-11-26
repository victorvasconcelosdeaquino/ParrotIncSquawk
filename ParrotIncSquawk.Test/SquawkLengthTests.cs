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
        public void SquawkWhenTheTextIsGreaterThan_400_ShouldFail()
        {
            // Arrange
            SquawkRequest model = new()
            {
                Text = $"This is an example text for Squawk to test whether it is possible to exceed the " +
                $"400 character limit in a Squawk, which should not be possible, since our application is " +
                $"very reliable, scalable, uses good practices and our company has the best and the most " +
                $"qualified professionals on the market. Unfortunately, with this beautiful text, we need " +
                $"to make it fail, but it's for a good reason, it's to prove that our test is efficient.",
            };
            SquawkValidation validationRules = new();
            int count = model.Text.Length;

            // Act
            ValidationResult validationResult = validationRules.Validate(model);

            // Assert
            Assert.True(count > 400);
            Assert.False(validationResult.IsValid);
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
