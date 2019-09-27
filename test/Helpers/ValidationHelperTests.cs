using FluentAssertions;
using Geta.EPi.Extensions.Helpers;
using Xunit;

namespace Geta.EPi.Extensions.Tests.Helpers
{
    public class ValidationHelperTests
    {
        public class IsValidEmailTests
        {
            [InlineData("marti@moto.com")]
            [InlineData("marti@moto.no")]
            [InlineData("marti@moto")]
            [InlineData("#$%^&*()@moto.com")]
            [Theory]
            public void it_should_validate_valid_email(string validEmail)
            {
                var result = ValidationHelper.IsValidEmail(validEmail);

                result.Should().BeTrue();
            }

            [InlineData("marti@")]
            [InlineData("marti")]
            [InlineData("@")]
            [InlineData("@marti")]
            [InlineData("@marti.com")]
            [Theory]
            public void it_should_invalidate_invalid_email(string invalidEmail)
            {
                var result = ValidationHelper.IsValidEmail(invalidEmail);

                result.Should().BeFalse();
            }
        }
    }
}