using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NKakasi
{
    internal class KanjiConverter
    {
        private readonly ItaijiDictionary itaijiDictionary = ItaijiDictionary.GetInstance();
        private readonly KanwaDictionary kanwaDictionary;

        internal bool HeikiMode { get; set; }
        internal bool FuriganaMode { get; set; }

        internal KanjiConverter(KanwaDictionary kanwaDictionary)
        {
            this.kanwaDictionary = kanwaDictionary;
        }

        internal bool ToHiragana(KakasiReader input, TextWriter output)
        {
            char key = itaijiDictionary.Get((char)input.Get());
            SortedSet<KanjiYomi>.Enumerator iterator = kanwaDictionary.Lookup(key);
            HashSet<string> yomiSet = new HashSet<string>();
            string rest = null;
            int restLength = 0;
            int resultLength = 0;
            while (iterator.MoveNext()) {
                KanjiYomi kanjiYomi = iterator.Current;
                if (rest == null) {
                    char[] chars = new char[kanjiYomi.Length + 1];
                    restLength = input.More(chars);
                    for (int index = 0; index < restLength; index++) {
                        chars[index] = itaijiDictionary.Get(chars[index]);
                    }
                    rest = new string(chars, 0, restLength);
                }
                Logger.Log("kanjiYomi: " + kanjiYomi.Kanji + "," + kanjiYomi.Yomi + "," + kanjiYomi.Okurigana + "," + kanjiYomi.Length + "," + restLength);
                if (kanjiYomi.Length < resultLength) {
                    break;
                }
                if (kanjiYomi.Length > restLength) {
                    continue;
                }
                string yomi = kanjiYomi.GetYomiFor(rest);
                if (yomi == null) {
                    continue;
                }
                yomiSet.Add(yomi);
                resultLength = kanjiYomi.Length;
                if (!HeikiMode) {
                    break;
                }
            }
            if (yomiSet.Count == 0) {
                return false;
            }
            char additionalChar = (char)0;
            if (resultLength > 0 && restLength > resultLength &&
                rest[resultLength - 1] == '\u3063') {
                char nextCh = rest[resultLength];
                UnicodeBlock block = UnicodeBlock.Of(nextCh);
                if (block.Equals(UnicodeBlock.Hiragana)) {
                    ++resultLength;
                    additionalChar = nextCh;
                }
            }
            input.Consume(resultLength + 1);
            if (FuriganaMode) {
                output.Write(key);
                if (resultLength > 0) {
                    output.Write(rest.ToCharArray(), 0, resultLength);
                }
                output.Write('[');
            }
            if (yomiSet.Count == 1) {
                output.Write(yomiSet.FirstOrDefault());
                if (additionalChar > 0) {
                    output.Write(additionalChar);
                }
            } else if (yomiSet.Count > 1) {
                HashSet<string>.Enumerator iter = yomiSet.GetEnumerator();
                output.Write('{');
                bool bar = false;
                while (iter.MoveNext()) {
                    if (bar) {
                        output.Write('|');
                    }
                    output.Write(iter.Current);
                    if (additionalChar > 0) {
                        output.Write(additionalChar);
                    }
                    bar = true;
                }
                output.Write('}');
            }
            if (FuriganaMode) {
                output.Write(']');
            }
            return true;
        }

        public bool ToKanji(KakasiReader input, TextWriter output)
        {
            char key = itaijiDictionary.Get((char)input.Get());
            SortedSet<KanjiYomi>.Enumerator iterator = kanwaDictionary.Lookup(key);
            string rest = null;
            int restLength = 0;
            int resultLength = 0;
            while (iterator.MoveNext()) {
                KanjiYomi kanjiYomi = iterator.Current;
                int length = kanjiYomi.Length;
                if (rest == null) {
                    char[] chars = new char[length + 1];
                    restLength = input.More(chars);
                    for (int index = 0; index < restLength; index++) {
                        chars[index] = itaijiDictionary.Get(chars[index]);
                    }
                    rest = new string(chars, 0, restLength);
                }
                if (length < resultLength) {
                    break;
                }
                if (length > restLength) {
                    continue;
                }
                if (kanjiYomi.GetYomiFor(rest) != null) {
                    resultLength = length;
                    break;
                }
            }
            if (resultLength > 0 && restLength > resultLength &&
                rest[resultLength - 1] == '\u3063') {
                char nextCh = rest[resultLength];
                UnicodeBlock block = UnicodeBlock.Of(nextCh);
                if (block.Equals(UnicodeBlock.Hiragana)) {
                    ++resultLength;
                }
            }
            input.Consume(resultLength + 1);
            output.Write(key);
            if (resultLength > 0) {
                output.Write(rest.ToCharArray(), 0, resultLength);
            }
            return true;
        }
    }
}
