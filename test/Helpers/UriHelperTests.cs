using BVNetwork.NotFound.Tests.Base.Http;
using EPiServer.Web;
using Geta.EPi.Extensions.Helpers;
using System;
using Xunit;

namespace Geta.EPi.Extensions.Tests.Helpers
{
    public class UriHelperTests
    {

        [Fact]
        public void It_returns_base_Url_if_request_contains_base_Url()
        {
            var expected = "http://www.sample.com/";
            var request = new FakeHttpRequest().WithUrl(expected);
            var httpContext = FakeHttpContext.CreateWithRequest(request);

            var result = UriHelpers.GetBaseUri(httpContext, SiteDefinition.Empty);

            Assert.Equal(expected, result.ToString());
        }

        [Fact]
        public void It_adds_slash_to_the_base_Url()
        {
            var expected = "http://www.sample.com/";
            var request = new FakeHttpRequest().WithUrl("http://www.sample.com");
            var httpContext = FakeHttpContext.CreateWithRequest(request);

            var result = UriHelpers.GetBaseUri(httpContext, SiteDefinition.Empty);

            Assert.Equal(expected, result.ToString());
        }

        [Fact]
        public void It_sets_X_Forwarded_Proto_scheme()
        {
            var expected = "https://www.sample.com/";
            var request = new FakeHttpRequest()
                .WithUrl("http://www.sample.com/")
                .WithHeader("X-Forwarded-Proto", "https");
            var httpContext = FakeHttpContext.CreateWithRequest(request);

            var result = UriHelpers.GetBaseUri(httpContext, SiteDefinition.Empty);

            Assert.Equal(expected, result.ToString());
        }

        [Fact]
        public void It_sets_first_X_Forwarded_Proto_scheme()
        {
            var expected = "https://www.sample.com/";
            var request = new FakeHttpRequest()
                .WithUrl("http://www.sample.com/")
                .WithHeader("X-Forwarded-Proto", "https, http");
            var httpContext = FakeHttpContext.CreateWithRequest(request);

            var result = UriHelpers.GetBaseUri(httpContext, SiteDefinition.Empty);

            Assert.Equal(expected, result.ToString());
        }

        [Fact]
        public void It_uses_site_definition_scheme_when_no_http_context()
        {
            var expected = "https://www.sample.com/";
            var siteDefinition = new SiteDefinition() { SiteUrl = new Uri(expected) };

            var result = UriHelpers.GetBaseUri(null, siteDefinition);

            Assert.Equal(expected, result.ToString());
        }
    }
}
