namespace Jbs.Yukari.Core.Services
{
    public enum LongVowelStyle
    {
        Omit, // 長音を構成する ou/oo は無視する
        AsIs, // 何もしない
        ToOH  // 長音を構成する ou/oo は oh に変換する
    }
}
