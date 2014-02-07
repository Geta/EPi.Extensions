using FluentAssertions;
using Xunit;

namespace Geta.EPi.Extensions.Tests
{
    public class StringExtensionsTests
    {
        public class GenerateSlugTests
        {
            [Fact]
            public void it_should_return_slug_with_10_chars_if_max_length_is_10()
            {
                var expected = 10;

                var result = "Some long string here to use".GenerateSlug(expected);

                result.Length.Should().BeLessOrEqualTo(expected);
            }
        }
    }
}
