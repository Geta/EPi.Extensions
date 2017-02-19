using System.Web;

namespace Geta.EPi.Extensions
{
    /// <summary>
    /// Extensions for <see cref="HttpContextBase"/>
    /// </summary>
    public static class HttpContextBaseExtensions
    {
        /// <summary>
        ///     Checks HttpContextBase.Items for a key named IsBlockPreviewTemplate and if it's set to true. 
        ///     This key is set through the TemplateResolver.TemplateResolved event in the <see cref="ExtensionsInitializationModule">initialization module</see>
        /// </summary>
        /// <param name="httpContext">The current HttpContextBase instance</param>
        /// <returns>True/false</returns>
        public static bool IsBlockPreview(this HttpContextBase httpContext)
        {
            object isBlockPreview = httpContext?.Items["IsBlockPreviewTemplate"];

            if (isBlockPreview != null && (bool)isBlockPreview)
            {
                return true;
            }

            return false;
        }
    }
}