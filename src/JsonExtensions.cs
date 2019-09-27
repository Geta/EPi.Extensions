using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Geta.EPi.Extensions
{
    /// <summary>
    /// Extension methods for working with JSON data
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Convert object to JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">Object to convert</param>
        /// <param name="includeNull">Include null property values</param>
        /// <returns></returns>
        public static string ToJson<T>(this T obj, bool includeNull = true)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new JsonConverter[] { new StringEnumConverter() },
                NullValueHandling = includeNull ? NullValueHandling.Include : NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}