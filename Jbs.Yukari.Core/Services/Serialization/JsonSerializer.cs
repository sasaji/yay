//using Newtonsoft.Json;
//using Newtonsoft.Json.Serialization;

using System.Text.Json;

namespace Jbs.Yukari.Core.Services.Serialization
{
    public class JsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public string Serialize(object o)
        {
            return System.Text.Json.JsonSerializer.Serialize(o, options);
        }

        public T Deserialize<T>(string s)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(s, options);
        }
    }
}
