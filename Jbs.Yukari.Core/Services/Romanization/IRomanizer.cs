namespace Jbs.Yukari.Core.Services.Romanization
{
    public interface IRomanizer
    {
        string Romanize(string kana);
        string Romanize(string kana, string kanji);
    }
}
