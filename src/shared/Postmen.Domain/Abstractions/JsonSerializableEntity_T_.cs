using Newtonsoft.Json;

namespace Postmen.Domain.Abstractions
{
    public abstract class JsonSerializableEntity<T>
    {
        public string ToJson() => JsonConvert.SerializeObject(this);

        public static T FromJson(string json) => JsonConvert.DeserializeObject<T>(json);
    }
}
