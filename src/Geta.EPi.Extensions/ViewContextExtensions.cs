using System.Web.Mvc;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     ViewContext extension method.
    /// </summary>
    public static class ViewContextExtensions
    {
        /// <summary>
        ///     Checks if currently in Block preview controller.
        /// </summary>
        /// <param name="viewContext"></param>
        /// <returns></returns>
        public static bool IsBlockPreview(this ViewContext viewContext)
        {
            return viewContext.HttpContext.IsBlockPreview();
        }
    }
}