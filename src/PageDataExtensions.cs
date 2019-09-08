using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     Extension methods for PageData
    /// </summary>
    public static class PageDataExtensions
    {
        /// <summary>
        ///     Returns all ancestor pages for provided page.
        /// </summary>
        /// <param name="page">Page of PageData type for which to return ancestors.</param>
        /// <param name="includeRootPage">True will include root page</param>
        /// <returns>Returns IEnumerable of ancestor pages.</returns>
        public static IEnumerable<PageData> GetAncestors(this PageData page, bool includeRootPage = false)
        {
            while ((page = page.GetParent()) != null)
            {
                if (!includeRootPage && page.ContentLink.Equals(ContentReference.RootPage))
                {
                    yield break;
                }

                yield return page;
            }
        }

        /// <summary>
        ///     Returns all child pages of PageData type for provided parent page.
        /// </summary>
        /// <param name="parentPage">Parent page of PageData type for which to return children.</param>
        /// <returns>Returns PageDataCollection of child pages.</returns>
        public static PageDataCollection GetChildren(this PageData parentPage)
        {
            return parentPage.GetChildren<PageData>().ToPageDataCollection();
        }

        /// <summary>
        ///     Returns all child pages of <typeparamref name="T" /> for provided parent page.
        /// </summary>
        /// <typeparam name="T">The type of child pages to return.</typeparam>
        /// <param name="parentPage">Parent page of PageData type for which to return children.</param>
        /// <returns>Returns sequence of child pages of <typeparamref name="T" /> for provided parent page.</returns>
        public static IEnumerable<T> GetChildren<T>(this PageData parentPage)
            where T : PageData
        {
            if (parentPage == null)
            {
                return Enumerable.Empty<T>();
            }
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            return contentLoader.GetChildren<T>(parentPage.ContentLink);
        }

        /// <summary>
        ///     Returns parent page of provided page.
        /// </summary>
        /// <param name="page">The page for which to find parent page.</param>
        /// <returns>Returns instance of parent page of PageData type or null if parent page not found./></returns>
        public static PageData GetParent(this PageData page)
        {
            if (page == null
                || PageReference.IsNullOrEmpty(page.ParentLink)
                || DataFactory.Instance.IsWastebasket(page.PageLink))
                // TODO: Might be obsolete in EPi 7 - should verify
            {
                return null;
            }
            return page.ParentLink.GetPage();
        }

        /// <summary>
        ///     Returns all siblings of PageData type of provided page.
        /// </summary>
        /// <param name="currentPage">Page for which to find siblings.</param>
        /// <param name="excludeSelf">Mark if exclude itself from sibling sequence.</param>
        /// <returns>Sequence of siblings of provided page.</returns>
        public static IEnumerable<PageData> GetSiblings(this PageData currentPage, bool excludeSelf = true)
        {
            return currentPage.GetSiblings<PageData>(excludeSelf);
        }

        /// <summary>
        ///     Returns all siblings of <typeparamref name="T" /> type of provided page.
        /// </summary>
        /// <typeparam name="T">Type of siblings to return.</typeparam>
        /// <param name="currentPage">Page for which to find siblings.</param>
        /// <param name="excludeSelf">Mark if exclude itself from sibling sequence.</param>
        /// <returns>Sequence of siblings of provided page.</returns>
        public static IEnumerable<T> GetSiblings<T>(this PageData currentPage, bool excludeSelf = true)
            where T : PageData
        {
            if (currentPage == null)
            {
                return Enumerable.Empty<T>();
            }

            var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var siblings = loader.GetChildren<T>(currentPage.ParentLink);

            if (excludeSelf)
            {
                siblings = siblings.Where(p => !p.PageGuid.Equals(currentPage.PageGuid));
            }

            return siblings;
        }

        /// <summary>
        ///     Returns all descendants of PageData type for provided page and level deep.
        /// </summary>
        /// <param name="pageData">The page for which to find descendants.</param>
        /// <param name="levels">Level of how deep to look for descendants in page hiararchy.</param>
        /// <returns>Returns sequence of PageData of descendants for provided page.</returns>
        public static IEnumerable<PageData> GetDescendants(this PageData pageData, int levels)
        {
            return pageData.GetDescendants<PageData>(levels);
        }

        /// <summary>
        ///     Returns all descendants of <typeparamref name="T" /> type for provided page and level deep.
        /// </summary>
        /// <typeparam name="T">Type of pages to look for.</typeparam>
        /// <param name="pageData">The page for which to find descendants.</param>
        /// <param name="levels">Level of how deep to look for descendants in page hierarchy.</param>
        /// <returns>Returns sequence of PageData of descendants for provided page.</returns>
        public static IEnumerable<T> GetDescendants<T>(this PageData pageData, int levels) where T : PageData
        {
            if (pageData == null || levels <= 0)
            {
                yield break;
            }

            foreach (var child in pageData.GetChildren<T>())
            {
                yield return child;

                if (levels <= 1)
                {
                    continue;
                }

                foreach (var grandChild in child.GetDescendants<T>(levels - 1))
                {
                    yield return grandChild;
                }
            }
        }

        /// <summary>
        ///     Returns friendly URL for provided page.
        /// </summary>
        /// <param name="page">Page for which to create friendly url.</param>
        /// <param name="includeHost">Mark if include host name in the url, unless it is external url then it still will contain absolute url.</param>
        /// <param name="ignoreContextMode">Mark if Url should be generating ignoring context mode. Settings this parameter to <c>true</c> friendly Url will be generated, even in EPiServer EditMode.</param>
        /// <returns>String representation of URL for provided page.</returns>
        public static string GetFriendlyUrl(this PageData page, bool includeHost = false, bool ignoreContextMode = false)
        {
            return page != null ? page.ContentLink.GetFriendlyUrl(includeHost, ignoreContextMode) : string.Empty;
        }

        /// <summary>
        ///     Returns friendly URL for provided page.
        /// </summary>
        /// <param name="page">Page for which to create friendly url.</param>
        /// <param name="language">Language of content</param>
        /// <param name="includeHost">Mark if include host name in the url.</param>
        /// <param name="ignoreContextMode">Mark if Url should be generating ignoring context mode. Settings this parameter to <c>true</c> friendly Url will be generated, even in EPiServer EditMode.</param>
        /// <returns>String representation of URL for provided page.</returns>
        public static string GetFriendlyUrl(this PageData page, string language, bool includeHost = false, bool ignoreContextMode = false)
        {
            return page != null ? page.ContentLink.GetFriendlyUrl(language, includeHost, ignoreContextMode) : string.Empty;
        }

        /// <summary>
        ///     Compares two pages by reference or by WorkID.
        /// </summary>
        /// <param name="page1">First page to compare.</param>
        /// <param name="page2">Second page to compare.</param>
        /// <returns>
        ///     true if first page and second page references are equals or page content link WorkID equals otherwise returns
        ///     false.
        /// </returns>
        public static bool IsEqualTo(this PageData page1, PageData page2)
        {
            if (page1 == page2)
            {
                return true;
            }

            return page1 != null && page2 != null && page1.ContentLink.CompareToIgnoreWorkID(page2.ContentLink);
        }

        /// <summary>
        ///     Converts sequence of PageData to PageDataCollection.
        /// </summary>
        /// <param name="pages">Source sequence of PageData to convert.</param>
        /// <returns>Instance of PageDataCollection from source sequence.</returns>
        public static PageDataCollection ToPageDataCollection(this IEnumerable<PageData> pages)
        {
            return new PageDataCollection(pages);
        }

        /// <summary>
        /// Gets the canonical link for a page.
        /// </summary>
        /// <param name="page">Page to get canonical url for</param>
        /// <param name="considerFetchDataFrom">Consider fetch data from setting in EPiServer.</param>
        /// <param name="considerSimpleAddress">Use simple address of page if it is set.</param>
        /// <param name="urlResolver">Optional UrlResolver instance.</param>
        /// <returns>The complete link to the page. If LinkType is FetchData then the original page URL will be returned.</returns>
        public static string GetCanonicalUrl(this PageData page, bool considerFetchDataFrom = true, bool considerSimpleAddress = false, UrlResolver urlResolver = null)
        {
            if (page == null)
            {
                return null;
            }

            var canonicalPage = page;
            var pageLink = page.PageLink;

            if (considerFetchDataFrom && page.LinkType == PageShortcutType.FetchData)
            {
                var shortcutLink = page["PageShortcutLink"] as PageReference;

                if (!PageReference.IsNullOrEmpty(shortcutLink))
                {
                    pageLink = shortcutLink;
                }
            }

            if (!page.PageLink.CompareToIgnoreWorkID(canonicalPage.PageLink))
            {
                canonicalPage = pageLink.GetPage();
            }

            if (considerSimpleAddress)
            {
                var simpleAddress = canonicalPage["PageExternalUrl"] as string;

                if (!string.IsNullOrWhiteSpace(simpleAddress))
                {
                    return simpleAddress.GetExternalUrl();
                }
            }

            return canonicalPage.PageLink.GetFriendlyUrl(true, urlResolver: urlResolver);
        }
    }
}
