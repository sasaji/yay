using System.IO;

namespace NKakasi
{
    public interface IConverter
    {
        bool Convert(KakasiReader input, TextWriter output);
    }
}
