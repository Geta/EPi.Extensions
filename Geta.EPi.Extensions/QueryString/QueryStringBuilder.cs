using EPiServer;
using System.Web;
namespace Geta.EPi.Cms
{
	public class QueryStringBuilder : IHtmlString
	{
		protected readonly UrlBuilder UrlBuilder;
		public static readonly QueryStringBuilder Empty;

		static QueryStringBuilder()
		{
			Empty = new QueryStringBuilder(string.Empty);	
		}

		public QueryStringBuilder(string url)
		{
			UrlBuilder = new UrlBuilder(url);
		}

		public static QueryStringBuilder Create(string url)
		{
			return new QueryStringBuilder(url);
		}

		public QueryStringBuilder Add(string name, string value)
		{
			UrlBuilder.QueryCollection[name] = HttpUtility.UrlEncode(value);
			return this;
		}

		public QueryStringBuilder Remove(string name)
		{
			UrlBuilder.QueryCollection.Remove(name);
			return this;
		}

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

		public override string ToString()
		{
			return UrlBuilder.ToString();
		}

		public string ToHtmlString()
		{
			return ToString();
		}
	}
}