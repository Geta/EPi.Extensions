using System;
using System.Web.Mvc;

namespace Geta.EPi.Extensions.Attributes
{
    /// <summary>
    ///     Attribute for decorating Episerver content type properties. If a property is decorated with 
    ///     this attribute it will be possible to render a edit button in edit mode for a property that 
    ///     isn't normally rendered in the view for the content type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class EditButtonAttribute : Attribute, IMetadataAware
    {
        /// <summary>
        ///     Creates instance with no icon css class, ShowInGroup=true and TriggerFullRefresh=true.
        /// </summary>
        public EditButtonAttribute() : this(null, true, true)
        {
        }

        /// <summary>
        ///     Creates instance with specified icon css class and ShowInGroup=true and TriggerFullRefresh=true.
        /// </summary>
        /// <param name="iconCssClass"></param>
        public EditButtonAttribute(string iconCssClass) : this(iconCssClass, true, true)
        {
        }

        /// <summary>
        ///     Creates instance with specified icon css class, ShowInGroup and TriggerFullRefresh=true.
        /// </summary>
        /// <param name="iconCssClass"></param>
        /// <param name="showInGroup"></param>
        public EditButtonAttribute(string iconCssClass, bool showInGroup) : this(iconCssClass, showInGroup, true)
        {
        }

        /// <summary>
        ///     Creates instance with specified icon css class, ShowInGroup and TriggerFullRefresh.
        /// </summary>
        /// <param name="iconCssClass"></param>
        /// <param name="showInGroup"></param>
        /// <param name="triggerFullRefresh"></param>
        public EditButtonAttribute(string iconCssClass, bool showInGroup, bool triggerFullRefresh)
        {
            IconCssClass = iconCssClass;
            ShowInGroup = showInGroup;
            TriggerFullRefresh = triggerFullRefresh;
        }

        /// <summary>
        ///     Icon css class for rendered edit button.
        /// </summary>
        public string IconCssClass { get; set; }

        /// <summary>
        ///     Set to true if edit button should be included in button group rendered with 
        ///     <see cref="ContentEditorExtensions">html helper</see> EditButtonsGroup.
        /// </summary>
        public bool ShowInGroup { get; set; }

        /// <summary>
        ///     Set to true if this edit button should trigger a full refresh in edit mode.
        ///     Default is true.
        /// </summary>
        public bool TriggerFullRefresh { get; set; }

        /// <summary>
        ///     Label of the button. If left empty, property display name will be used.
        /// </summary>
        public string ButtonLabel { get; set; }

        /// <summary>
        /// When implemented in a class, provides metadata to the model metadata creation process.
        /// </summary>
        /// <param name="metadata">The model metadata.</param>
        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[MetadataConstants.EditButton.IconCssClassPropertyName] = IconCssClass;
            metadata.AdditionalValues[MetadataConstants.EditButton.ShowInGroupPropertyName] = ShowInGroup;
            metadata.AdditionalValues[MetadataConstants.EditButton.TriggerFullRefreshPropertyName] = TriggerFullRefresh;
            metadata.AdditionalValues[MetadataConstants.EditButton.ButtonLabel] = ButtonLabel;
        }
    }
}