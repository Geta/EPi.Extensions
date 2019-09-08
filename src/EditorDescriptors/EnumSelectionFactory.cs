using System;
using System.Collections.Generic;
using EPiServer.Framework.Localization;
using EPiServer.Shell.ObjectEditing;

namespace Geta.EPi.Extensions.EditorDescriptors
{
    /// <summary>
    /// Selection factory for enum types used by EPiServer Properties
    /// </summary>
    /// <typeparam name="TEnum">Enum type with options</typeparam>
    public class EnumSelectionFactory<TEnum> : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(
            ExtendedMetadata metadata)
        {
            var values = Enum.GetValues(typeof(TEnum));
            foreach (var value in values)
            {
                yield return new SelectItem
                {
                    Text = GetValueName(value),
                    Value = value
                };
            }
        }

        private string GetValueName(object value)
        {
            var staticName = Enum.GetName(typeof(TEnum), value);

            string localizationPath = string.Format("/property/enum/{0}/{1}", typeof(TEnum).Name.ToLowerInvariant(), staticName.ToLowerInvariant());

            string localizedName;
            if (LocalizationService.Current.TryGetString(localizationPath, out localizedName))
            {
                return localizedName;
            }

            return staticName;
        }
    }
}