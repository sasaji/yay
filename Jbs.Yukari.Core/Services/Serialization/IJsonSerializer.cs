namespace Jbs.Yukari.Core.Services.Serialization
{
    public interface IJsonSerializer
    {
        string Serialize(object o);
        T Deserialize<T>(string s);
    }
}
