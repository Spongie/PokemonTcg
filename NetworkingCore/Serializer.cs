using Newtonsoft.Json;

namespace NetworkingCore
{
    public static class Serializer
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects
        };

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, JsonSettings);
        }

        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, JsonSettings);
        }
    }
}
