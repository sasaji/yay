using System.IO;
using Microsoft.VisualBasic;

namespace NKakasi
{
    public class HiraganaConverter
    {
        private const char WO = '\u3092';

        /*
        internal bool ToKatakana(KanjiInput input, TextWriter output)
        {
            if (!IsHiragana(input.Get())) {
                return false;
            }
            while (true) {
                int ch = input.Get();
                if (ch == '\u3046') {   // 'u'
                    input.Consume(1);
                    int ch2 = input.Get();
                    if (ch2 == '\u309b') {  // voice sound mark
                        input.Consume(1);
                        output.Write('\u30f4'); // 'vu'
                    } else {
                        output.Write('\u30a6');
                    }
                } else if ((ch >= '\u3041' && ch <= '\u3093') ||
                           ch == '\u309d' || ch == '\u309e') {
                    // from small 'a' to 'n' and iteration marks
                    input.Consume(1);
                    output.Write((char)(ch + 0x60));
                } else if (IsHiragana(ch)) {
                    input.Consume(1);
                    output.Write((char)ch);
                } else {
                    break;
                }
            }
            return true;
        }
        */

        internal bool ToKatakana(KakasiReader input, TextWriter output)
        {
            return ToKatakana(input, output, VbStrConv.Katakana);
        }

        internal bool ToHalfKana(KakasiReader input, TextWriter output)
        {
            return ToKatakana(input, output, VbStrConv.Katakana | VbStrConv.Narrow);
        }

        private static bool ToKatakana(KakasiReader input, TextWriter output, VbStrConv conv)
        {
            if (!IsHiragana(input.Get()))
            {
                return false;
            }
            while (true)
            {
                int ch = input.Get();
                if (IsHiragana(ch))
                {
                    input.Consume(1);
                    foreach (char c in Strings.StrConv(((char)ch).ToString(), conv).ToCharArray())
                    {
                        output.Write(c);
                    }
                }
                else
                {
                    break;
                }
            }
            return true;
        }

        internal static bool ToHiragana(KakasiReader input, TextWriter output)
        {
            int ch = input.Get();
            if (!IsHiragana(ch))
            {
                return false;
            }
            output.Write((char)ch);
            int length = 1;
            if (ch != WO)
            {
                for (; ; length++)
                {
                    ch = input.More();
                    if (ch == WO)
                    {
                        break;
                    }
                    if (!IsHiragana(ch))
                    {
                        break;
                    }
                    output.Write((char)ch);
                }
            }
            input.Consume(length);
            return true;
        }

        private static bool IsHiragana(int ch)
        {
            if (ch < 0)
            {
                return false;
            }
            if (ch == '\u30fc')
            {   // prolonged sound mark
                return true;
            }
            UnicodeBlock block = UnicodeBlock.Of((char)ch);
            return block.Equals(UnicodeBlock.Hiragana);
        }
    }
}
