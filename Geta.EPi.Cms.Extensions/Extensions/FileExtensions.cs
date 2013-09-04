using System.Web;
using EPiServer;
using EPiServer.Web;

namespace Geta.EPi.Cms.Extensions
{
    public static class FileExtensions
    {
         public static UrlBuilder GetExternalUrl(this string permanentlink)
         {
             if (string.IsNullOrWhiteSpace(permanentlink))
             {
                 return null;
             }

             var url = new UrlBuilder(new UrlBuilder(permanentlink));

             PermanentLinkMapStore.ToMapped(url);

             url.Host = HttpContext.Current.Request.Url.Host;
             url.Scheme = HttpContext.Current.Request.Url.Scheme;
             url.Port = HttpContext.Current.Request.Url.Port;

             return url;
         }
    }
}