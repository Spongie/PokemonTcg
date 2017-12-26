using Newtonsoft.Json;

namespace NetworkingClient.Common
{
    public static class Serializer
    {
        private static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All };

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
