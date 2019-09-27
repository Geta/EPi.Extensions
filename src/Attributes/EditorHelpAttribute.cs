using System;
using System.Web.Mvc;

namespace Geta.EPi.Extensions.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EditorHelpAttribute : Attribute, IMetadataAware
    {
        public string Hint { get; set; }
        public bool ShowInSummary { get; set; }

        public EditorHelpAttribute(string hint) : this(hint, true)
        {
        }

        public EditorHelpAttribute(string hint, bool showInSummary)
        {
            Hint = hint;
            ShowInSummary = showInSummary;
        }

        public void OnMetadataCreated(ModelMetadata metadata)
        {
            metadata.AdditionalValues[MetadataConstants.EditorHelp.HelpTextPropertyName] = Hint;
            metadata.AdditionalValues[MetadataConstants.EditorHelp.ShowInSummaryPropertyName] = ShowInSummary;
        }
    }
}