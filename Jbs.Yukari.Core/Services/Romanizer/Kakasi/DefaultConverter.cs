using System.IO;

namespace NKakasi
{
    public class DefaultConverter : IConverter
    {
        public bool Convert(KakasiReader input, TextWriter output)
        {
            int ch = input.Get();
            if (ch < 0)
            {
                return false;
            }
            UnicodeBlock pblock = UnicodeBlock.Of((char)ch);
            while (true)
            {
                input.Consume(1);
                output.Write((char)ch);
                ch = input.Get();
                if (ch < 0)
                {
                    break;
                }
                UnicodeBlock block;
                switch (ch)
                {
                    case '\u3005':  // kurikaesi
                    case '\u3006':  // shime
                    case '\u30f5':  // katakana small ka
                    case '\u30f6':  // katakana small ke
                        block = UnicodeBlock.CJKUnifiedIdeographs;
                        break;
                    default:
                        block = UnicodeBlock.Of((char)ch);
                        break;
                }
                if (!block.Equals(pblock))
                    break;
                //if (IsJapanese(block) != IsJapanese(pblock)) {
                //    break;
                //}
            }
            return true;
        }

        private bool IsJapanese(UnicodeBlock block)
        {
            return block.Equals(UnicodeBlock.CJKUnifiedIdeographs) || block.Equals(UnicodeBlock.Hiragana) || block.Equals(UnicodeBlock.Katakana);
        }
    }
}
