using System;
using Xunit;

namespace Geta.EPi.Extensions.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("http://mysite", "/")]
        [InlineData("http://mysite.com", "/")]
        [InlineData("http://mysite.com/page", "/page")]
        [InlineData("https://mysite", "/")]
        [InlineData("https://mysite.com", "/")]
        [InlineData("https://mysite.com/page", "/page")]
        [InlineData("https://mysite.com/page?q=123", "/page?q=123")]
        public void RemoveHost_removes_host_from_absolute_url(string url, string expected)
        {
            var result = url.RemoveHost();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/page")]
        [InlineData("/page?q=123")]
        public void RemoveHost_returns_original_url_for_relative_url(string url)
        {
            var expected = url;

            var result = url.RemoveHost();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void RemoveHost_returns_empty_string_for_invalid_url(string url)
        {
            var expected = string.Empty;

            var result = url.RemoveHost();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("/", "http://mysite.com/")]
        [InlineData("/page", "http://mysite.com/page")]
        [InlineData("/page?q=123", "http://mysite.com/page?q=123")]
        [InlineData("", "http://mysite.com/")]
        [InlineData("page", "http://mysite.com/page")]
        [InlineData("page?q=123", "http://mysite.com/page?q=123")]
        public void AddHost_returns_absolute_url_for_relative_url(string url, string expected)
        {
            var baseUrl = "http://mysite.com";

            var result = url.AddHost(() => new Uri(baseUrl));

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("http://mysite.com/")]
        [InlineData("http://mysite.com/page")]
        [InlineData("http://mysite.com/page?q=123")]
        public void AddHost_returns_same_absolute_url(string url)
        {
            var expected = url;
            var baseUrl = "http://mysite.com";

            var result = url.AddHost(() => new Uri(baseUrl));

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("https://mysite.com/", "http://mysite.com/", "https://mysite.com/")]
        [InlineData("https://mysite.com/", "http://mysite.com/page", "https://mysite.com/page")]
        [InlineData("http://mysite.com/", "https://mysite.com/", "http://mysite.com/")]
        [InlineData("http://mysite.com/", "https://mysite.com/page", "http://mysite.com/page")]
        public void AddHost_updates_absolute_url_scheme_from_base_url(string baseUrl, string url, string expected)
        {
            var result = url.AddHost(() => new Uri(baseUrl));

            Assert.Equal(expected, result);
        }
    }
}