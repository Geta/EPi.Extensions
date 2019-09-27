using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Localization;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;
using Foundation.Core.Extensions;

namespace Geta.EPi.Extensions
{
    /// <summary>
    ///     <see cref="HtmlHelper"/> extension methods for improved content editor user experience.
    /// </summary>
    public static class ContentEditorExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="expr"></param>
        /// <param name="helpText"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlString EditorHelp<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, IContentData>> expr, string helpText)
        {
            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expr, helper.ViewData);
            var content = modelMetadata.Model as IContentData;
            return EditorHelp(helper, content, helpText);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="helpText"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlString EditorHelp<TModel>(this HtmlHelper<TModel> helper, string helpText) where TModel : IContentData
        {
            var content = helper.ViewData.Model as IContentData;
            return EditorHelp(helper, content, helpText);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="content"></param>
        /// <param name="helpText"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlString EditorHelp<TModel>(this HtmlHelper<TModel> helper, IContentData content, string helpText)
        {
            if (!PageIsInEditMode(helper))
            {
                return null;
            }

            if (IsBlockAndNotInPreview(helper, content.GetOriginalType()))
            {
                return null;
            }

            return MvcHtmlString.Create(GetHintsTag(helpText).ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="expr"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <returns></returns>
        public static IHtmlString EditorHelpFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expr)
        {
            if (PageIsInEditMode(helper) == false)
            {
                return null;
            }

            var modelMetaData = ModelMetadata.FromLambdaExpression(expr, helper.ViewData);

            if (IsBlock(modelMetaData.ContainerType) && IsBlockPreviewTemplate(helper) == false)
            {
                return null;
            }

            if (modelMetaData.AdditionalValues.ContainsKey(MetadataConstants.EditorHelp.HelpTextPropertyName))
            {
                var hint = modelMetaData.AdditionalValues[MetadataConstants.EditorHelp.HelpTextPropertyName] as string;

                if (string.IsNullOrWhiteSpace(hint) == false)
                {
                    var tag = GetHintsTag(hint);
                    return new MvcHtmlString(tag.ToString());
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlString EditorHelpSummary<TModel>(this HtmlHelper<TModel> helper) where TModel : IContentData
        {
            return EditorHelpSummary(helper, helper.ViewData.Model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="expr"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlString EditorHelpSummary<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, IContentData>> expr)
        {
            if (PageIsInEditMode(helper) == false)
            {
                return null;
            }

            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expr, helper.ViewData);
            return EditorHelpSummary(helper, modelMetadata.Model as IContentData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="content"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlString EditorHelpSummary<TModel>(this HtmlHelper<TModel> helper, IContentData content)
        {
            if (PageIsInEditMode(helper) == false)
            {
                return null;
            }

            var contentType = content.GetOriginalType();

            if (IsBlockAndNotInPreview(helper, contentType))
            {
                return null;
            }

            IEnumerable<ModelMetadata> propertiesMeta = ModelMetadataProviders.Current.GetMetadataForType(() => content, contentType).Properties;
            IList<string> hints = new List<string>();

            foreach (var propertyMetadata in propertiesMeta)
            {
                object showInSummaryObj;

                if (propertyMetadata.AdditionalValues.TryGetValue(MetadataConstants.EditorHelp.ShowInSummaryPropertyName, out showInSummaryObj) == false)
                {
                    continue;
                }

                var showInSummary = (bool?)showInSummaryObj;

                if (showInSummary.GetValueOrDefault(true) == false)
                {
                    continue;
                }

                object hintObj;

                if (propertyMetadata.AdditionalValues.TryGetValue(MetadataConstants.EditorHelp.HelpTextPropertyName, out hintObj) == false)
                {
                    continue;
                }

                var hint = hintObj as string;

                if (string.IsNullOrWhiteSpace(hint))
                {
                    continue;
                }

                hints.Add(hint);
            }

            if (hints.Count > 0)
            {
                var tag = GetHintsTag(hints);
                return new MvcHtmlString(tag.ToString());
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="expr"></param>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <returns></returns>
        public static IHtmlString EditButtonFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expr)
        {
            if (PageIsInEditMode(helper) == false)
            {
                return null;
            }

            var modelMetaData = ModelMetadata.FromLambdaExpression(expr, helper.ViewData);

            if (IsBlockAndNotInPreview(helper, modelMetaData.ContainerType))
            {
                return null;
            }

            string iconCssClass = null;

            if (modelMetaData.AdditionalValues.ContainsKey(MetadataConstants.EditButton.IconCssClassPropertyName))
            {
                iconCssClass = modelMetaData.AdditionalValues[MetadataConstants.EditButton.IconCssClassPropertyName] as string;
            }

            string tag = GetEditButtonTag(helper, expr, modelMetaData.AdditionalValues[MetadataConstants.EditButton.ButtonLabel] as string ?? modelMetaData.DisplayName ?? modelMetaData.PropertyName, iconCssClass);
            return new MvcHtmlString(tag);
        }

        /// <summary>
        ///     Render edit buttons for current <see cref="IContentData"/> instance.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="includeBuiltInProperties">If true, also renders edit button for built-in Category property.</param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlString EditButtonsGroup<TModel>(this HtmlHelper<TModel> helper, bool includeBuiltInProperties = false) where TModel : IContentData
        {
            return EditButtonsGroup(helper, helper.ViewData.Model, includeBuiltInProperties);
        }

        /// <summary>
        ///     Render edit buttons for <see cref="IContentData"/> instance resolved from lambda expression.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="expr"></param>
        /// <param name="includeBuiltInProperties">If true, also renders edit button for built-in Category property.</param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlString EditButtonsGroup<TModel>(this HtmlHelper<TModel> helper, Expression<Func<TModel, IContentData>> expr, bool includeBuiltInProperties = false)
        {
            if (PageIsInEditMode(helper) == false)
            {
                return null;
            }

            ModelMetadata modelMetadata = ModelMetadata.FromLambdaExpression(expr, helper.ViewData);
            return EditButtonsGroup(helper, modelMetadata.Model as IContentData, includeBuiltInProperties);
        }

        /// <summary>
        ///     Render edit buttons for <see cref="IContentData"/> instance.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="content"></param>
        /// <param name="includeBuiltInProperties">If true, also renders edit button for built-in Category property.</param>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static IHtmlString EditButtonsGroup<TModel>(this HtmlHelper<TModel> helper, IContentData content, bool includeBuiltInProperties = false)
        {
            if (PageIsInEditMode(helper) == false)
            {
                return null;
            }

            var contentType = content.GetOriginalType();

            if (IsBlockAndNotInPreview(helper, contentType))
            {
                return null;
            }

            IEnumerable<ModelMetadata> propertiesMeta = ModelMetadataProviders.Current.GetMetadataForType(() => content, contentType).Properties;
            IList<string> iconCssDivs = new List<string>();
            IList<string> fullRefreshPropertyNames = new List<string>();

            foreach (var propertyMetadata in propertiesMeta)
            {
                object showInGroupObj;

                if (propertyMetadata.ShowForEdit == false || propertyMetadata.AdditionalValues.TryGetValue(MetadataConstants.EditButton.ShowInGroupPropertyName, out showInGroupObj) == false)
                {
                    continue;
                }

                var showInGroup = (bool?)showInGroupObj;

                if (showInGroup.GetValueOrDefault(true) == false)
                {
                    continue;
                }

                object iconCssClassObj;

                propertyMetadata.AdditionalValues.TryGetValue(MetadataConstants.EditButton.IconCssClassPropertyName, out iconCssClassObj);
                var iconCssClass = iconCssClassObj as string;

                bool triggerFullRefresh;

                if (propertyMetadata.TryGetAdditionalValue(MetadataConstants.EditButton.TriggerFullRefreshPropertyName, out triggerFullRefresh) == false)
                {
                    triggerFullRefresh = true;
                }

                if (triggerFullRefresh)
                {
                    fullRefreshPropertyNames.Add(propertyMetadata.PropertyName);
                }

                var editButtonHtml = GetEditButtonTag(helper, propertyMetadata.PropertyName, propertyMetadata.AdditionalValues[MetadataConstants.EditButton.ButtonLabel] as string ?? propertyMetadata.DisplayName ?? propertyMetadata.PropertyName, iconCssClass);

                iconCssDivs.Add(editButtonHtml);
            }

            if (includeBuiltInProperties)
            {
                var categorizableContent = content as ICategorizable;

                if (categorizableContent != null)
                {
                    iconCssDivs.Add(GetSpecialEditButtonTag("Category", LocalizationService.Current.GetString("/contenttypes/icontentdata/properties/icategorizable_category/caption"), null));
                    fullRefreshPropertyNames.Add("icategorizable_category");
                }

                var pageContent = content as PageData;

                if (pageContent != null)
                {
                    iconCssDivs.Add(GetSpecialEditButtonTag("PageExternalURL", LocalizationService.Current.GetString("/contenttypes/icontentdata/properties/pageexternalurl/caption"), null));
                }

                var routable = content as IRoutable;

                if (routable != null)
                {
                    iconCssDivs.Add(GetSpecialEditButtonTag("iroutable_routesegment", LocalizationService.Current.GetString("/contenttypes/icontentdata/properties/pageurlsegment/caption"), null));
                }

                if (pageContent != null)
                {
                    iconCssDivs.Add(GetSpecialEditButtonTag("PageVisibleInMenu", LocalizationService.Current.GetString("/contenttypes/icontentdata/properties/pagevisibleinmenu/caption"), null));
                }

                var versionableContent = content as IVersionable;

                if (versionableContent != null)
                {
                    iconCssDivs.Add(GetSpecialEditButtonTag("iversionable_startpublish", LocalizationService.Current.GetString("/contenttypes/icontentdata/properties/iversionable_startpublish/caption"), null));
                }

                var changeTrackableContent = content as IChangeTrackable;

                if (changeTrackableContent != null)
                {
                    iconCssDivs.Add(GetSpecialEditButtonTag("ichangetrackable_setchangedonpublish", LocalizationService.Current.GetString("/contenttypes/icontentdata/properties/ichangetrackable_setchangedonpublish/caption"), null));
                }
            }

            if (iconCssDivs.Count > 0)
            {
                var container = new TagBuilder("div");
                container.AddCssClass("editor-buttons");

                foreach (var iconCssDiv in iconCssDivs)
                {
                    container.InnerHtml += iconCssDiv;
                }

                return MvcHtmlString.Create(container.ToString() + helper.FullRefreshPropertiesMetaData(fullRefreshPropertyNames.ToArray()));
            }

            return null;
        }

        #region Private methods
        private static string GetEditButtonTag<TModel, TProperty>(HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expr, string displayName, string iconCssClass)
        {
            string withIconCssClass = string.IsNullOrWhiteSpace(iconCssClass) == false
                ? "with-icon "
                : null;

            var html = new StringBuilder();
            html.Append("<div ");
            html.AppendFormat("class=\"editor-button-wrapper {1}{0}\" ", iconCssClass, withIconCssClass);
            html.AppendFormat(">{0}", displayName);
            html.Append("<span class=\"editor-button\"");
            html.Append(helper.EditAttributes(expr));
            html.Append("></span>");
            html.Append("</div>");
            return html.ToString();
        }

        private static string GetEditButtonTag<TModel>(HtmlHelper<TModel> helper, string propertyName, string displayName, string iconCssClass)
        {
            string withIconCssClass = string.IsNullOrWhiteSpace(iconCssClass) == false
                ? "with-icon "
                : null;

            var html = new StringBuilder();
            html.Append("<div ");
            html.AppendFormat("class=\"editor-button-wrapper {1}{0}\" ", iconCssClass, withIconCssClass);
            html.AppendFormat(">{0}", displayName);
            html.Append("<span class=\"editor-button\"");
            html.Append(helper.EditAttributes(propertyName));
            html.Append("></span>");
            html.Append("</div>");
            return html.ToString();
        }

        private static string GetSpecialEditButtonTag(string propertyName, string displayName, string iconCssClass)
        {
            string withIconCssClass = string.IsNullOrWhiteSpace(iconCssClass) == false
                ? "with-icon "
                : null;

            var html = new StringBuilder();
            html.Append("<div ");
            html.AppendFormat("class=\"editor-button-wrapper {1}{0}\" ", iconCssClass, withIconCssClass);
            html.AppendFormat(">{0}", displayName);
            html.Append("<span class=\"editor-button\"");
            html.AppendFormat(" data-epi-property-name=\"{0}\" data-epi-use-mvc=\"True\"", propertyName);
            html.Append("></span>");
            html.Append("</div>");
            return html.ToString();
        }

        private static TagBuilder GetHintsTag(string hint)
        {
            return GetHintsTag(new[] { hint });
        }

        private static TagBuilder GetHintsTag(IEnumerable<string> hints)
        {
            var tag = new TagBuilder("div");
            tag.AddCssClass("editor-help");
            var ul = new TagBuilder("ul");

            foreach (var hint in hints)
            {
                var li = new TagBuilder("li")
                {
                    InnerHtml = hint
                };

                ul.InnerHtml += li.ToString();
            }

            tag.InnerHtml = ul.ToString();
            return tag;
        }

        private static bool IsBlock(Type contentType)
        {
            if (contentType == null)
            {
                return false;
            }

            return typeof(BlockData).IsAssignableFrom(contentType);
        }

        private static bool IsBlockPreviewTemplate<TModel>(HtmlHelper<TModel> helper)
        {
            return helper.ViewContext.IsBlockPreview();
        }

        private static bool IsBlockAndNotInPreview<TModel>(this HtmlHelper<TModel> helper, Type contentType)
        {
            return IsBlock(contentType) && IsBlockPreviewTemplate(helper) == false;
        }

        private static bool PageIsInEditMode<TModel>(HtmlHelper<TModel> helper)
        {
            return helper.ViewContext.RequestContext.IsInEditMode();
        }
        #endregion
    }
}