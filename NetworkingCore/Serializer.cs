using Newtonsoft.Json;

namespace NetworkingCore
{
    public static class Serializer
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.None,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            SerializationBinder = new NetCoreSerializationBinder()
        };

        private static readonly JsonSerializerSettings FormattedJsonSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto,
            Formatting = Formatting.Indented,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ObjectCreationHandling = ObjectCreationHandling.Replace,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            SerializationBinder = new NetCoreSerializationBinder()
        };

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, JsonSettings);
        }

        public static string SerializeFormatted(object data)
        {
            return JsonConvert.SerializeObject(data, FormattedJsonSettings);
        }

        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, JsonSettings);
        }
    }
}
