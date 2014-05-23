using System;
using System.Collections.Generic;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Shell.ObjectEditing.EditorDescriptors;

namespace Geta.EPi.Extensions.EditorDescriptors
{
    /// <summary>
    /// Obsolete.
    /// EPiServer Editor descriptor for creating custom properties with enum types as options
    /// Source: http://joelabrahamsson.com/enum-properties-with-episerver/
    /// </summary>
    /// <typeparam name="TEnum">Enum type with options</typeparam>
    [Obsolete("Replaced by EnumAttribute in same namespace")]
    public class EnumEditorDescriptor<TEnum> : EditorDescriptor
    {
        public override void ModifyMetadata(ExtendedMetadata metadata, IEnumerable<Attribute> attributes)
        {
            SelectionFactoryType = typeof(EnumSelectionFactory<TEnum>);

            ClientEditingClass = "epi.cms.contentediting.editors.SelectionEditor";

            base.ModifyMetadata(metadata, attributes);
        }
    }
}