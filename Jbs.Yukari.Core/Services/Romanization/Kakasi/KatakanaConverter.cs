using System.IO;

namespace NKakasi
{
    internal class KatakanaConverter
    {
        internal bool ToHiragana(KakasiReader input, TextWriter output)
        {
            if (!IsKatakana(input.Get())) {
                return false;
            }
            while (true) {
                int ch = input.Get();
                if ((ch >= '\u30a1' && ch <= '\u30f3') || ch == '\u30fd' || ch == '\u30fe') {
                    // from small 'a' to 'n' and iteration marks
                    input.Consume(1);
                    output.Write((char)(ch - 0x60));
                } else if (ch == '\u30f4') {    // 'vu'
                    input.Consume(1);
                    output.Write('\u3046');
                    output.Write('\u309b');
                } else if (IsKatakana(ch)) {
                    input.Consume(1);
                    output.Write((char)ch);
                } else {
                    break;
                }
            }
            return true;
        }

        internal bool ToKatakana(KakasiReader input, TextWriter output)
        {
            int ch = input.Get();
            if (!IsKatakana(ch)) {
                return false;
            }
            output.Write((char)ch);
            int length = 1;
            for (; ; length++) {
                ch = input.More();
                if (!IsKatakana(ch)) {
                    break;
                }
                output.Write((char)ch);
            }
            input.Consume(length);
            return true;
        }

        private static bool IsKatakana(int ch)
        {
            if (ch < 0) {
                return false;
            }
            if (ch == '\u309b' || ch == '\u309c') { // voice sound mark
                return true;
            }
            UnicodeBlock block = UnicodeBlock.Of((char)ch);
            return block.Equals(UnicodeBlock.Katakana);
        }
    }
}
