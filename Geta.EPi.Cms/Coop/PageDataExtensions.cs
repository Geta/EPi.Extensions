using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using EPiServer;
using EPiServer.BaseLibrary;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using ContentReferenceExtensions = Geta.EPi.Cms.Extensions.ContentReferenceExtensions;

namespace Geta.EPi.Cms.Coop
{
    public static class PageDataExtensions
    {
        /// <summary>
        ///     CurrentPage.FormatHtml("MainIntro", "<p>", "</p>");
        /// </summary>
        /// <param name="page"></param>
        /// <param name="propertyName">Name of property</param>
        /// <param name="startTag">The start tag</param>
        /// <param name="endTag">The closing end tag</param>
        /// <returns>Formatted string with the property value between the start and end tag</returns>
        public static string FormatHtml(this PageData page, string propertyName, string startTag, string endTag)
        {
            var formattedString = string.Empty;

            if (page.IsEPiServerPage() && page.IsValue(propertyName))
            {
                formattedString = string.Format("{0}{1}{2}", startTag, page[propertyName], endTag);
            }

            return formattedString;
        }

        /// <summary>
        ///     CurrentPage.FormatHtml("MainIntro", "<strong>{0}</strong>");
        /// </summary>
        /// <param name="page"></param>
        /// <param name="propertyName">Name of property</param>
        /// <param name="format">The format of the output. Must have {0} inside</param>
        /// <returns>Formatted string with the property value instead of {0}</returns>
        public static string FormatHtml(this PageData page, string propertyName, string format)
        {
            var formattedString = string.Empty;

            if (!page.IsEPiServerPage())
            {
                return formattedString;
            }

            PropertyData prop = page.GetProperty(propertyName);
            if (prop != null && !prop.IsNull)
            {
                formattedString = string.Format(format, prop);
            }

            return formattedString;
        }

        public static T GetChildPageOfType<T>(this PageData currentPage) where T : PageData
        {
            var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
            return loader.GetChildren<T>(currentPage.ContentLink).FirstOrDefault();
        }

        public static PageDataCollection GetChildren(this PageData page)
        {
            var children = new PageDataCollection();

            if (page.IsEPiServerPage())
            {
                children = DataFactory.Instance.GetChildren(page.PageLink);
            }

            return children;
        }

        public static IEnumerable<T> GetChildren<T>(this PageData parentPage) where T : PageData
        {
            if (parentPage != null)
            {
                var repository = ServiceLocator.Current.GetInstance<IContentLoader>();
                return repository.GetChildren<T>(parentPage.ContentLink);
            }

            return Enumerable.Empty<T>();
        }

        public static IEnumerable<PageData> GetDescendants(this PageData pageData, int levels)
        {
            return pageData.GetDescendants<PageData>(levels);
        }

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

        public static string GetExternalUrl(this PageData page)
        {
            string result = string.Empty;

            if (page.IsEPiServerPage())
            {
                UrlBuilder url;

                string linkUrl = page.LinkURL;

                if (page.LinkType != PageShortcutType.External)
                {
                    if (Thread.CurrentThread.CurrentCulture.Name != page.LanguageBranch)
                    {
                        linkUrl = UriSupport.AddLanguageSelection(page.LinkURL, page.LanguageBranch);
                    }

                    url =
                            new UrlBuilder(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                                           HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + linkUrl);

                    if (UrlRewriteProvider.IsFurlEnabled)
                    {
                        Global.UrlRewriteProvider.ConvertToExternal(url, page.PageLink, Encoding.UTF8);
                        result = url.ToString();
                    }
                    else
                    {
                        result = url.ToString();
                    }
                }
                else
                {
                    result = new UrlBuilder(linkUrl).ToString();
                }
            }

            return result;
        }

        public static string GetFriendlyUrl(this PageData page, bool includeHost = false)
        {
            return page != null ? Extensions.ContentReferenceExtensions.GetFriendlyUrl(page.ContentLink, includeHost) : string.Empty;
        }

        public static PageData GetParent(this PageData page)
        {
            if (page != null && !PageReference.IsNullOrEmpty(page.ParentLink) && !DataFactory.Instance.IsWastebasket(page.PageLink))
            {
                return Extensions.ContentReferenceExtensions.GetPage(page.ParentLink);
            }

            return null;
        }

        /// <summary>
        ///     Find the first not null and not IsNull property from a list seperated by |
        /// </summary>
        /// <param name="page"></param>
        /// <param name="propertyNames">The names of the page property to check. seperated by |. Like MainIntro|MainBody</param>
        /// <returns>First PropertyData with value.</returns>
        public static PropertyData GetProperty(this PageData page, string propertyNames)
        {
            if (!page.IsEPiServerPage())
            {
                return null;
            }

            string[] propNames = propertyNames.Split('|');

            foreach (string propSubName in propNames)
            {
                var propertyData = page.Property[propSubName];
                if (propertyData != null && !propertyData.IsNull)
                {
                    return propertyData;
                }
            }

            return null;
        }

        public static IEnumerable<PageData> GetSiblings(this PageData currentPage, bool excludeSelf = true)
        {
            return currentPage.GetSiblings<PageData>();
        }

        public static IEnumerable<T> GetSiblings<T>(this PageData currentPage, bool excludeSelf = true) where T : PageData
        {
            if (currentPage != null)
            {
                var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
                var siblings = loader.GetChildren<T>(currentPage.ParentLink);

                if (excludeSelf)
                {
                    siblings = siblings.Where(p => !p.PageGuid.Equals(currentPage.PageGuid));
                }

                return siblings;
            }

            return Enumerable.Empty<T>();
        }

        public static bool IsEPiServerPage(this PageData page)
        {
            return page != null && page.PageLink != null && page.PageLink.ID > 0;
        }

        /// <summary>
        ///     Determine if the page is published
        /// </summary>
        /// <param name="page"></param>
        /// <returns>True if the page is published</returns>
        public static bool IsPublished(this PageData page)
        {
            if (!page.IsEPiServerPage())
            {
                return false;
            }

            return CheckPublishedStatus(page, PagePublishedStatus.Published);
        }

        /// <summary>
        ///     See http://www.frederikvig.com/2009/11/the-selectedtemplate-and-duplicate-code/ for more information
        /// </summary>
        /// <param name="page">Page to check</param>
        /// <param name="currentPage">CurrentPage or page to check from</param>
        /// <returns>Returns true if page equals currentPage or any of its parents, otherwise false</returns>
        public static bool IsSelected(this PageData page, PageData currentPage)
        {
            if (!page.IsEPiServerPage() || !currentPage.IsEPiServerPage())
            {
                return false;
            }

            var currentLevel = currentPage.PageLink;
            while (!PageReference.IsNullOrEmpty(currentLevel) && currentLevel != PageReference.StartPage && currentLevel != PageReference.RootPage
                   && currentLevel != PageReference.WasteBasket)
            {
                if (page.PageLink.CompareToIgnoreWorkID(currentLevel))
                {
                    return true;
                }

                currentLevel = DataFactory.Instance.GetPage(currentLevel).ParentLink;
            }

            return false;
        }

        /// <summary>
        ///     Determine if the named property exists and has a value.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="propertyName">The name of the page property to check.</param>
        /// <returns>True if the property exists with a valid (i e non-null) value.</returns>
        public static bool IsValue(this PageData page, string propertyName)
        {
            if (!page.IsEPiServerPage())
            {
                return false;
            }

            var propertyData = page.Property[propertyName];

            return propertyData != null && !propertyData.IsNull;
        }

        /// <summary>
        ///     CurrentPage.PropertyValue<DateTime>("PageStartPublish"));
        /// </summary>
        /// <typeparam name="T">The return value data type</typeparam>
        /// <param name="page"></param>
        /// <param name="propertyName">Name of property to check</param>
        /// <returns>The property value or default value</returns>
        public static T PropertyValue<T>(this PageData page, string propertyName)
        {
            return PropertyValueWithDefault<T>(page, propertyName, default(T));
        }

        public static T PropertyValue<T>(this PageData page, string propertyName, string defaultValue) where T : PropertyData, new()
        {
            T prop = new T();
            prop.ParseToSelf(defaultValue);
            return PropertyValueWithDefault<T>(page, propertyName, prop);
        }

        public static T PropertyValueWithDefault<T>(this PageData page, string propertyName, T defaultValue)
        {
            if (page.IsValue(propertyName))
            {
                return (T)page.Property[propertyName].Value;
            }

            return defaultValue;
        }

        private static bool CheckPublishedStatus(this PageData page, PagePublishedStatus status)
        {
            if (!page.IsEPiServerPage())
            {
                return false;
            }

            if (status != PagePublishedStatus.Ignore)
            {
                if (page.PendingPublish)
                {
                    return false;
                }
                if (page.Status != VersionStatus.Published)
                {
                    return false;
                }
                if ((status >= PagePublishedStatus.PublishedIgnoreStopPublish) && (page.StartPublish > Context.Current.RequestTime))
                {
                    return false;
                }
                if ((status >= PagePublishedStatus.Published) && (page.StopPublish < Context.Current.RequestTime))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
