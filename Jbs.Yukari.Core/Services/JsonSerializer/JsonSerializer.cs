using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jbs.Yukari.Core.Services
{
    // System.Text.JsonはguidをデシリアライズできないのでNewtonsoft.Jsonを使う。
    public class JsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public string Serialize(object o)
        {
            return JsonConvert.SerializeObject(o, settings);
        }

        public T Deserialize<T>(string s)
        {
            return JsonConvert.DeserializeObject<T>(s);
        }
    }
}
