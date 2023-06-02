using System;
using System.IO;

namespace NKakasi
{
    class DelegateConverter : IConverter
    {
        private Func<KakasiReader, TextWriter, bool> _convert;
        public DelegateConverter(Func<KakasiReader, TextWriter, bool> convert)
        {
            _convert = convert;
        }
        public bool Convert(KakasiReader i, TextWriter w) => _convert(i, w);
    }
}
