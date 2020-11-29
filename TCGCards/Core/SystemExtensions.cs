using NetworkingCore;
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
        
        public static T Copy<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source, JsonSettings);
            T entity = JsonConvert.DeserializeObject<T>(serialized, JsonSettings);

            return entity;
        }

        public static T Clone<T>(this T source) where T : IEntity
        {
            var serialized = JsonConvert.SerializeObject(source, JsonSettings);
            T entity = JsonConvert.DeserializeObject<T>(serialized, JsonSettings);
            entity.Id = NetworkId.Generate();

            return entity;
        }
    }
}
