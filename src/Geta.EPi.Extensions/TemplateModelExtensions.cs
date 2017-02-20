using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework.Web;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     <see cref="TemplateModel"/> extension methods.
    /// </summary>
    public static class TemplateModelExtensions
    {
        /// <summary>
        ///     Check if a <see cref="TemplateModel"/> is block preview template.
        /// </summary>
        /// <param name="templateModel">The template model</param>
        /// <returns>true/false</returns>
        public static bool IsBlockPreviewTemplate(this TemplateModel templateModel)
        {
            if (templateModel == null)
            {
                return false;
            }

            var tags = templateModel.Tags;

            bool isBlockPreview =
                templateModel.TemplateTypeCategory == TemplateTypeCategories.MvcController
                && typeof(BlockData).IsAssignableFrom(templateModel.ModelType)
                && templateModel.AvailableWithoutTag == false
                && tags != null
                && tags.Length == 1
                && tags[0] == RenderingTags.Preview;

            return isBlockPreview;
        }
    }
}