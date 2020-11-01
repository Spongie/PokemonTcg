using Newtonsoft.Json;

namespace TCGCards.Core
{
    public static class SystemExtensions
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.None,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize
        };

        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source, JsonSettings);
            return JsonConvert.DeserializeObject<T>(serialized, JsonSettings);
        }
    }
}
