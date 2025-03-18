using System;
using System.IO;

namespace NKakasi
{
    class DelegateConverter : IConverter
    {
        private readonly Func<KakasiReader, TextWriter, bool> convert;
        public DelegateConverter(Func<KakasiReader, TextWriter, bool> convert)
        {
            this.convert = convert;
        }
        public bool Convert(KakasiReader i, TextWriter w) => convert(i, w);
    }
}
