using System.Collections.Generic;
using EPiServer;
using EPiServer.Core;
using ContentReferenceExtensions = Geta.EPi.Cms.Extensions.ContentReferenceExtensions;

namespace Geta.EPi.Cms.Coop
{
    public static class PageReferenceExtensions
    {
        public static PageDataCollection GetChildren(this PageReference pageReference)
        {
            if (pageReference.IsNullOrEmptyPageReference())
            {
                return null;
            }

            return DataFactory.Instance.GetChildren(pageReference);
        }

        public static IEnumerable<PageData> GetDescendants(this PageReference rootPageReference)
        {
            if (rootPageReference.IsNullOrEmptyPageReference())
            {
                return null;
            }

            return rootPageReference.GetDescendants(int.MaxValue);
        }

        public static IEnumerable<PageData> GetDescendants(this PageReference rootPageReference, int levels)
        {
            if (rootPageReference.IsNullOrEmptyPageReference() || levels == 0)
            {
                yield break;
            }

            foreach (PageData child in rootPageReference.GetChildren())
            {
                if (child.IsEPiServerPage())
                {
                    yield return child;

                    if (levels > 1)
                    {
                        foreach (PageData grandChild in Extensions.PageDataExtensions.GetDescendants(child, levels - 1))
                        {
                            yield return grandChild;
                        }
                    }
                }
            }
        }

        public static string GetExternalUrl(this PageReference pageReference)
        {
            if (pageReference.IsNullOrEmptyPageReference())
            {
                return string.Empty;
            }

            return Extensions.ContentReferenceExtensions.GetPage(pageReference).GetExternalUrl();
        }

        public static string GetLinkUrl(this PageReference pageReference)
        {
            var page = Extensions.ContentReferenceExtensions.GetPage(pageReference);
            return page != null ? page.LinkURL : string.Empty;
        }

        public static bool IsNullOrEmptyPageReference(this PageReference pageReference)
        {
            return PageReference.IsNullOrEmpty(pageReference);
        }
    }
}
