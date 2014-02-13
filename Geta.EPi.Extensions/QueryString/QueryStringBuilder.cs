using System.Web;
using EPiServer;

namespace Geta.EPi.Extensions.QueryString
{
    /// <summary>
    ///     Helper class for creating and modifying URL's QueryString.
    /// </summary>
    public class QueryStringBuilder : IHtmlString
    {
        protected readonly UrlBuilder UrlBuilder;

        /// <summary>
        ///     Represents the empty query string. Field is read-only.
        /// </summary>
        public static readonly QueryStringBuilder Empty = new QueryStringBuilder(string.Empty);

        /// <summary>
        ///     Instantiates new QueryStringBuilder with provided URL.
        /// </summary>
        /// <param name="url">URL for which to build query.</param>
        public QueryStringBuilder(string url)
        {
            UrlBuilder = new UrlBuilder(url);
        }

        /// <summary>
        ///     Factory method for instantiating new QueryStringBuilder with provided URL.
        /// </summary>
        /// <param name="url">URL for which to build query.</param>
        /// <returns>Instance of QueryStringBuilder.</returns>
        public static QueryStringBuilder Create(string url)
        {
            return new QueryStringBuilder(url);
        }

        /// <summary>
        ///     Adds query string parameter to query URL encoded.
        /// </summary>
        /// <param name="name">Name of parameter.</param>
        /// <param name="value">Value of parameter.</param>
        /// <returns>Instance of modified QueryStringBuilder.</returns>
        public QueryStringBuilder Add(string name, string value)
        {
            UrlBuilder.QueryCollection[name] = HttpUtility.UrlEncode(value);
            return this;
        }

        /// <summary>
        ///     Removes query string parameter from query.
        /// </summary>
        /// <param name="name">Name of parameter to remove.</param>
        /// <returns>Instance of modified QueryStringBuilder.</returns>
        public QueryStringBuilder Remove(string name)
        {
            UrlBuilder.QueryCollection.Remove(name);
            return this;
        }

        /// <summary>
        ///     Adds query string parameter to query string if it is not already present, otherwise it removes it.
        /// </summary>
        /// <param name="name">Name of parameter to add or remove.</param>
        /// <param name="value">Value of parameter to add.</param>
        /// <returns>Instance of modified QueryStringBuilder.</returns>
        public QueryStringBuilder Toggle(string name, string value)
        {
            var currVal = HttpUtility.UrlDecode(UrlBuilder.QueryCollection[name]);
            var exists = currVal != null && currVal == value;

            if (exists)
                Remove(name);
            else
                Add(name, value);

            return this;
        }

        /// <summary>
        ///     Returns string representation of URL with query string.
        /// </summary>
        /// <returns>String representation of URL with query string.</returns>
        public override string ToString()
        {
            return UrlBuilder.ToString();
        }

        /// <summary>
        ///     Returns string representation of URL with query string. This is implementation of IHtmlString.
        /// </summary>
        /// <returns>String representation of URL with query string.</returns>
        public string ToHtmlString()
        {
            return ToString();
        }
    }
}