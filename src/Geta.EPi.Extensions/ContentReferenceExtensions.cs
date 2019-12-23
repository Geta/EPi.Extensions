using System;
using System.Collections.Generic;
using System.Linq;
using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     ContentReference extensions.
    /// </summary>
    public static class ContentReferenceExtensions
    {
        /// <summary>
        ///     Returns enumeration of child contents of PageData type for provided content reference.
        /// </summary>
        /// <param name="contentReference">Content reference for which child contents to get.</param>
        /// <returns>Enumeration of PageData child content.</returns>
        public static IEnumerable<PageData> GetChildren(this ContentReference contentReference)
        {
            return contentReference.GetChildren<PageData>();
        }

        /// <summary>
        ///     Returns enumeration of child contents of concrete type for provided content reference with fallback to master language enabled.
        /// </summary>
        /// <typeparam name="T">Type of child content (IContentData).</typeparam>
        /// <param name="contentReference">Content reference for which child contents to get.</param>
        /// <returns>Enumeration of <typeparamref name="T" /> child content.</returns>
        /// <remarks>Use <see cref="GetChildren{T}(EPiServer.Core.ContentReference,EPiServer.Core.LoaderOptions)"/> overload to customize loader options.</remarks>
        public static IEnumerable<T> GetChildren<T>(this ContentReference contentReference) where T : IContentData
        {
            return contentReference.GetChildren<T>(LanguageSelector.AutoDetect(true));
        }

        /// <summary>
        ///     Returns enumeration of child contents of concrete type for provided content reference with possible loader options.
        /// </summary>
        /// <typeparam name="T">Type of child content (IContentData).</typeparam>
        /// <param name="contentReference">Content reference for which child contents to get.</param>
        /// <param name="options">The loader options to be used while loading the content.</param>
        /// <returns>Enumeration of <typeparamref name="T" /> child content.</returns>
        public static IEnumerable<T> GetChildren<T>(this ContentReference contentReference, LoaderOptions options) where T : IContentData
        {
            if (contentReference.IsNullOrEmpty())
            {
                return Enumerable.Empty<T>();
            }

            var repository = ServiceLocator.Current.GetInstance<IContentLoader>();
            return repository.GetChildren<T>(contentReference);
        }

        /// <summary>
        ///     Returns page of PageData type for provided content reference.
        /// </summary>
        /// <param name="contentReference">Content reference for which to get page.</param>
        /// <returns>Page of PageData type that match content reference.</returns>
        public static PageData GetPage(this ContentReference contentReference)
        {
            return contentReference.GetPage<PageData>();
        }

        /// <summary>
        ///     Returns page of concrete type for provided content reference.
        /// </summary>
        /// <typeparam name="T">Type of page (PageData).</typeparam>
        /// <param name="contentReference">Content reference for which to get page.</param>
        /// <returns>Page of <typeparamref name="T" /> that match content reference.</returns>
        public static T GetPage<T>(this ContentReference contentReference) where T : PageData
        {
            if (contentReference.IsNullOrEmpty())
            {
                return null;
            }

            var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
            return loader.Get<PageData>(contentReference) as T;
        }

        /// <summary>
        ///     Returns a concrete type for provided content reference.
        /// </summary>
        /// <typeparam name="T">Type of content data</typeparam>
        /// <param name="contentReference">Content reference for the content data</param>
        /// <returns>Content of <typeparamref name="T" /> that match content reference.</returns>
        public static T Get<T>(this ContentReference contentReference) where T : IContentData
        {
            if (contentReference.IsNullOrEmpty())
            {
                return default(T);
            }

            var loader = ServiceLocator.Current.GetInstance<IContentLoader>();
            return loader.Get<T>(contentReference);
        }

        /// <summary>
        ///     Returns friendly URL for provided content reference.
        /// </summary>
        /// <param name="contentReference">Content reference for which to create friendly url.</param>
        /// <param name="includeHost">Mark if include host name in the url, unless it is external url then it still will contain absolute url.</param>
        /// <param name="ignoreContextMode"></param>
        /// <param name="urlResolver">Optional UrlResolver instance.</param>
        /// <param name="contentLoader">Optional ContentLoader instance.</param>
        /// <returns>String representation of URL for provided content reference.</returns>
        public static string GetFriendlyUrl(
            this ContentReference contentReference,
            bool includeHost = false,
            bool ignoreContextMode = false,
            UrlResolver urlResolver = null,
            IContentLoader contentLoader = null)
        {
            return GetFriendlyUrl(contentReference, null, includeHost, ignoreContextMode, urlResolver, contentLoader);
        }

        /// <summary>
        ///     Returns friendly URL for provided content reference.
        /// </summary>
        /// <param name="contentReference">Content reference for which to create friendly url.</param>
        /// <param name="language">Language of content.</param>
        /// <param name="includeHost">Mark if include host name in the url, unless it is external url then it still will contain absolute url.</param>
        /// <param name="ignoreContextMode"></param>
        /// <param name="urlResolver">Optional UrlResolver instance.</param>
        /// <param name="contentLoader">Optional ContentLoader instance.</param>
        /// <returns>String representation of URL for provided content reference.</returns>
        public static string GetFriendlyUrl(
            this ContentReference contentReference,
            string language,
            bool includeHost = false,
            bool ignoreContextMode = false,
            UrlResolver urlResolver = null,
            IContentLoader contentLoader = null)
        {
            if (contentReference.IsNullOrEmpty())
            {
                return string.Empty;
            }

            if (urlResolver == null)
            {
                urlResolver = ServiceLocator.Current.GetInstance<UrlResolver>();
            }

            var url = ignoreContextMode
                ? urlResolver.GetUrl(contentReference, language, new VirtualPathArguments {ContextMode = ContextMode.Default})
                : urlResolver.GetUrl(contentReference, language);

            if (!string.IsNullOrWhiteSpace(url) &&
                url.StartsWith("/link/", StringComparison.InvariantCultureIgnoreCase) &&
                url.EndsWith(".aspx", StringComparison.InvariantCultureIgnoreCase))
            {
                // caller asked for friendly url, but got ugly one
                if (Guid.TryParse(url.ToLower().Replace("/link/", string.Empty).Replace(".aspx", string.Empty),
                    out var targetContentGuid))
                {
                    if (contentLoader == null)
                    {
                        contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
                    }

                    if(contentLoader.TryGet<IContent>(targetContentGuid, out var destinationContent) &&
                       destinationContent.ContentLink != contentReference)
                    {
                        // in case this is shortcut from one IContent instance to another IContent instance, will try to get target content friendly url instead
                        url = ignoreContextMode
                            ? urlResolver.GetUrl(destinationContent.ContentLink, language, new VirtualPathArguments {ContextMode = ContextMode.Default})
                            : urlResolver.GetUrl(destinationContent.ContentLink, language);
                    }
                }
            }

            if (includeHost)
            {
                return url.AddHost();
            }

            var content = Get<IContent>(contentReference);
            var pageData = content as PageData;

            return pageData?.LinkType == PageShortcutType.External ? url : url.RemoveHost();
        }

        /// <summary>
        ///     Indicates whether the specified content reference is null or an EmptyReference.
        /// </summary>
        /// <param name="contentReference">Content reference to test.</param>
        /// <returns>true if content reference is null or EmptyReference else false</returns>
        public static bool IsNullOrEmpty(this ContentReference contentReference)
        {
            return ContentReference.IsNullOrEmpty(contentReference);
        }

        /// <summary>
        ///     Gets IContentData from ContentReference.
        /// </summary>
        /// <param name="contentLink">The ContentReference</param>
        /// <returns>IContentData if found, otherwise null.</returns>
        public static IContentData GetContentData(this ContentReference contentLink)
        {
            if (ContentReference.IsNullOrEmpty(contentLink))
            {
                return null;
            }

            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            IContentData contentData;
            contentLoader.TryGet(contentLink, out contentData);
            return contentData;
        }

        /// <summary>
        ///     Get IContent from ContentReference.
        /// </summary>
        /// <param name="contentLink">The ContentReference</param>
        /// <returns>IContent if found by content loader, otherwise null.</returns>
        public static IContent GetContent(this ContentReference contentLink)
        {
            if (ContentReference.IsNullOrEmpty(contentLink))
            {
                return null;
            }

            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            IContent content;
            contentLoader.TryGet(contentLink, out content);
            return content;
        }

        /// <summary>
        /// Opens download dialog for documents and media files
        /// </summary>
        /// <param name="contentReferenceToMediaFile">Content reference to media file</param>
        /// <returns></returns>
        public static string GetDownloadLink(this ContentReference contentReferenceToMediaFile)
        {
            var url = UrlResolver.Current.GetUrl(contentReferenceToMediaFile);

            return url + "/download";
        }
    }
}