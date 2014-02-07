using FluentAssertions;
using Xunit;
using Xunit.Extensions;

namespace Geta.EPi.Extensions.Tests
{
    public class StringExtensionsTests
    {
        public class GenerateSlugTests
        {

            [InlineData(0)]
            [InlineData(10)]
            [InlineData(100)]
            [Theory]
            public void it_should_return_slug_with_less_or_equal_number_chars_than_max_length(int expected)
            {
                var result = "Some long string here to use. And some more text.".GenerateSlug(expected);

                result.Length.Should().BeLessOrEqualTo(expected);
            }

            [Fact]
            public void it_should_separate_words_in_camel_case_with_dash()
            {
                var result = "SomeCamelCaseString".GenerateSlug();

                result.Should().Be("some-camel-case-string");
            }

            [Fact]
            public void it_should_remove_non_alphanumeric_chars()
            {
                var result = "String with %&*( some 23 &%$ symbols.".GenerateSlug();

                result.Should().Be("string-with-some-23-symbols");
            }

            [Fact]
            public void it_should_replace_whitespace_with_dash()
            {
                var result = "I have spaces and\ttabs here".GenerateSlug();

                result.Should().Be("i-have-spaces-and-tabs-here");
            }

            [Fact]
            public void it_should_lowercase_letters()
            {
                var result = "I have UPPER and Camel".GenerateSlug();

                result.Should().Be("i-have-upper-and-camel");
            }
        }
    }
}
