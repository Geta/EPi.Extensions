using System;
using EPiServer.Framework.Localization;

namespace Geta.EPi.Cms.MarineHarvest
{
    public static class EnumExtensions
    {
        public static string GetLocalizedEnumValue(this Enum value)
        {
            return GetLocalizedEnumValue((object)value);
        }

        public static string GetLocalizedEnumValue(object value)
        {
            var type = value.GetType();

            var staticName = Enum.GetName(type, value);

            if (staticName != null)
            {

                string localizationPath = string.Format(
                    "/property/enum/{0}/{1}",
                    type.Name.GenerateSlug(),
                    staticName.GenerateSlug());

                return LocalizationService.Current.GetString(localizationPath);
            }

            return null;
        }
    }
}