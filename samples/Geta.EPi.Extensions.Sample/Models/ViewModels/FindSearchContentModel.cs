using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Geta.EPi.Extensions.Sample.Models.Pages;
using EPiServer.Find.UnifiedSearch;
using Geta.EPi.Extensions.Sample.Models.ViewModels;
using EPiServer.ServiceLocation;
using EPiServer.Find.Framework.Statistics;
using EPiServer;

namespace Geta.EPi.Extensions.Sample.Models.ViewModels
{
    public class FindSearchContentModel : PageViewModel<FindSearchPage>
    {
        public FindSearchContentModel(FindSearchPage currentPage)
            : base(currentPage)
        {
        }

        /// <summary>
        /// Search hits
        /// </summary>
        public UnifiedSearchResults Hits { get; set; }

        /// <summary>
        /// Public proxy path mainly used for constructing url's in javascript
        /// </summary>
        public string PublicProxyPath { get; set; }

        /// <summary>
        /// Flag to indicate if both Find serviceUrl and defaultIndex are configured
        /// </summary>
        public bool IsConfigured { get; set; }

        /// <summary>
        /// Constructs a url for a section group
        /// </summary>
        /// <param name="groupName">Name of group</param>
        /// <returns>Url</returns>
        public string GetSectionGroupUrl(string groupName)
        {
            return UriSupport.AddQueryString(RemoveQueryStringByKey(HttpContext.Current.Request.Url.AbsoluteUri,"p"), "t", HttpContext.Current.Server.UrlEncode(groupName));
        }

        /// <summary>
        /// Removes specified query string from url
        /// </summary>
        /// <param name="url">Url from which to remove query string</param>
        /// <param name="key">Key of query string to remove</param>
        /// <returns>New url that excludes the specified query string</returns>
        private string RemoveQueryStringByKey(string url, string key)
        {
            var uri = new Uri(url);
            var newQueryString = HttpUtility.ParseQueryString(uri.Query);
            newQueryString.Remove(key);
            string pagePathWithoutQueryString = uri.GetLeftPart(UriPartial.Path);

            return newQueryString.Count > 0
                ? String.Format("{0}?{1}", pagePathWithoutQueryString, newQueryString)
                : pagePathWithoutQueryString;
        }

        /// <summary>
        /// Number of matching hits
        /// </summary>
        public int NumberOfHits
        {
            get { return Hits.TotalMatching; }
        }

        /// <summary>
        /// Current active section filter
        /// </summary>
        public string SectionFilter
        {
            get { return HttpContext.Current.Request.QueryString["t"] ?? string.Empty; }
        }

        /// <summary>
        /// Retrieve the paging page from the query string parameter "p".
        /// If no parameter exists, default to the first page.
        /// </summary>
        public int PagingPage
        {
            get
            {
                int pagingPage;
                if (!int.TryParse(HttpContext.Current.Request.QueryString["p"], out pagingPage))
                {
                    pagingPage = 1;
                }

                return pagingPage;
            }
        }

        /// <summary>
        /// Retrieve the paging section from the query string parameter "ps".
        /// If no parameter exists, default to the first paging section.
        /// </summary>
        public int PagingSection
        {
            get
            {
                return 1 + (PagingPage - 1) / PagingSectionSize;
            }
        }

        /// <summary>
        /// Calculate the number of pages required to list results
        /// </summary>
        public int TotalPagingPages
        {
            get
            {
                if (CurrentPage.PageSize > 0)
                {
                    return 1 + (Hits.TotalMatching - 1)/CurrentPage.PageSize;
                }

                return 0;
            }
        }

        public int PagingSectionSize
        {
            get { return 10; }
        }

        /// <summary>
        /// Calculate the number of paging sections required to list page links
        /// </summary>
        public int TotalPagingSections
        {
            get
            {
                return 1 + (TotalPagingPages - 1) / PagingSectionSize;
            }
        }

        /// <summary>
        /// Number of first page in current paging section
        /// </summary>
        public int PagingSectionFirstPage
        {
            get { return 1 + (PagingSection - 1) * PagingSectionSize; }
        }

        /// <summary>
        /// Number of last page in current paging section
        /// </summary>
        public int PagingSectionLastPage
        {
            get { return Math.Min((PagingSection * PagingSectionSize), TotalPagingPages); }
        }

        /// <summary>
        /// Create URL for a specified paging page.
        /// </summary>
        /// <param name="pageNumber">Number of page for which to get a url</param>
        /// <returns>Url for specified page</returns>
        public string GetPagingUrl(int pageNumber)
        {
            return UriSupport.AddQueryString(HttpContext.Current.Request.RawUrl, "p", pageNumber.ToString());
        }

        /// <summary>
        /// Create URL for the next paging section.
        /// </summary>
        /// <returns>Url for next paging section</returns>
        public string GetNextPagingSectionUrl()
        {
            return UriSupport.AddQueryString(HttpContext.Current.Request.RawUrl, "p", ((PagingSection * PagingSectionSize) + 1).ToString());
        }

        /// <summary>
        /// Create URL for the previous paging section.
        /// </summary>
        /// <returns>Url for previous paging section</returns>
        public string GetPreviousPagingSectionUrl()
        {
            return UriSupport.AddQueryString(HttpContext.Current.Request.RawUrl, "p", ((PagingSection - 1) * PagingSectionSize).ToString());
        }

        /// <summary>
        /// User query to search
        /// </summary>
        public string Query
        {
            get { return (HttpContext.Current.Request.QueryString["q"] ?? string.Empty).Trim(); }
        }

        /// <summary>
        /// Search tags like language and site
        /// </summary>
        public string Tags
        {
            get { return string.Join(",", ServiceLocator.Current.GetInstance<IStatisticTagsHelper>().GetTags()); }
        }
		/// <summary>
        /// Length of excerpt
        /// </summary>
        public int ExcerptLength
        {
            get { return CurrentPage.ExcerptLength; }
        }

        /// <summary>
        /// Height of hit images
        /// </summary>
        public int HitImagesHeight 
        { 
        	get { return CurrentPage.HitImagesHeight; } 
    	}

        /// <summary>
        /// Flag retrieved from editor settings to determine if it should 
        /// use AND as the operator for multiple search terms
        /// </summary>
        public bool UseAndForMultipleSearchTerms
        {
            get { return CurrentPage.UseAndForMultipleSearchTerms; }
        }

        public IEnumerable<SelectListItem> Analyzers { get; set; }
    }
}
