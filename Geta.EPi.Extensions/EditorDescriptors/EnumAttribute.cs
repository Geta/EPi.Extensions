using System;
using System.Web.Mvc;
using EPiServer.Shell.ObjectEditing;

namespace Geta.EPi.Extensions.EditorDescriptors
{
    /// <summary>
    /// EPiServer Editor descriptor for creating custom properties with enum types as options
    /// Source: http://world.episerver.com/Blogs/Linus-Ekstrom/Dates/2014/5/Enum-properties-for-EPiServer-75/
    /// </summary>    
    public class EnumAttribute : SelectOneAttribute, IMetadataAware
    {
        /// <summary>
        /// Enum type in EnumAttribute
        /// </summary>
        public Type EnumType { get; set; }

        /// <summary>
        /// Enum Attribute
        /// </summary>
        /// <param name="enumType">Type of enum</param>
        public EnumAttribute(Type enumType)
        {
            EnumType = enumType;
        }

        public new void OnMetadataCreated(ModelMetadata metadata)
        {
            SelectionFactoryType = typeof(EnumSelectionFactory<>).MakeGenericType(EnumType);
            base.OnMetadataCreated(metadata);
        }
    }

}