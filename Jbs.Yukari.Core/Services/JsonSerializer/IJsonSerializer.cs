namespace Jbs.Yukari.Core.Services
{
    public interface IJsonSerializer
    {
        string Serialize(object o);
        T Deserialize<T>(string s);
    }
}
