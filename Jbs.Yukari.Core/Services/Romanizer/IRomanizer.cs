namespace Jbs.Yukari.Core.Services
{
    public interface IRomanizer
    {
        string Romanize(string kana);
        string Romanize(string kana, string kanji);
    }
}
