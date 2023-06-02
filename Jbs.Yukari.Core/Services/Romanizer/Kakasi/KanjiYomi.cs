using System;
using System.Collections.Generic;

namespace NKakasi
{
    class KanjiYomi : IComparable
    {
        private readonly Dictionary<char, string> okuriganaTable = new Dictionary<char, string>() {
            { '\u3041', "aiueow" },
            { '\u3042', "aiueow" },
            { '\u3043', "aiueow" },
            { '\u3044', "aiueow" },
            { '\u3045', "aiueow" },
            { '\u3046', "aiueow" },
            { '\u3047', "aiueow" },
            { '\u3048', "aiueow" },
            { '\u3049', "aiueow" },
            { '\u304a', "aiueow" },
            { '\u304b', "k" },
            { '\u304d', "k" },
            { '\u304f', "k" },
            { '\u3051', "k" },
            { '\u3053', "k" },
            { '\u304c', "g" },
            { '\u304e', "g" },
            { '\u3050', "g" },
            { '\u3052', "g" },
            { '\u3054', "g" },
            { '\u3055', "s" },
            { '\u3057', "s" },
            { '\u3059', "s" },
            { '\u305b', "s" },
            { '\u305d', "s" },
            { '\u3056', "zj" },
            { '\u3058', "zj" },
            { '\u305a', "zj" },
            { '\u305c', "zj" },
            { '\u305e', "zj" },
            { '\u305f', "t" },
            { '\u3061', "tc" },
            { '\u3063', "aiueokstchgzjfdbpw" },
            { '\u3064', "t" },
            { '\u3066', "t" },
            { '\u3068', "t" },
            { '\u3060', "d" },
            { '\u3062', "d" },
            { '\u3065', "d" },
            { '\u3067', "d" },
            { '\u3069', "d" },
            { '\u306a', "n" },
            { '\u306b', "n" },
            { '\u306c', "n" },
            { '\u306d', "n" },
            { '\u306e', "n" },
            { '\u306f', "h" },
            { '\u3072', "h" },
            { '\u3075', "hf" },
            { '\u3078', "h" },
            { '\u307b', "h" },
            { '\u3070', "b" },
            { '\u3073', "b" },
            { '\u3076', "b" },
            { '\u3079', "b" },
            { '\u307c', "b" },
            { '\u3071', "p" },
            { '\u3074', "p" },
            { '\u3077', "p" },
            { '\u307a', "p" },
            { '\u307d', "p" },
            { '\u307e', "m" },
            { '\u307f', "m" },
            { '\u3080', "m" },
            { '\u3081', "m" },
            { '\u3082', "m" },
            { '\u3083', "y" },
            { '\u3084', "y" },
            { '\u3085', "y" },
            { '\u3086', "y" },
            { '\u3087', "y" },
            { '\u3088', "y" },
            { '\u3089', "rl" },
            { '\u308a', "rl" },
            { '\u308b', "rl" },
            { '\u308c', "rl" },
            { '\u308d', "rl" },
            { '\u308e', "wiueo" },
            { '\u308f', "wiueo" },
            { '\u3090', "wiueo" },
            { '\u3091', "wiueo" },
            { '\u3092', "w" },
            { '\u3093', "n" },
            { '\u30f5', "k" },
            { '\u30f6', "k" },
        };

        private static readonly object LOCK = new object();
        private static long objectConter;
        private readonly long objectIndex;

        private readonly string kanji;
        private readonly string yomi;
        private readonly char okurigana;
        private readonly int kanjiLength;
        private readonly int hashCode;

        internal KanjiYomi(string kanji, string yomi, char okurigana)
        {
            this.kanji = kanji;
            this.yomi = yomi;
            this.okurigana = okurigana;
            kanjiLength = kanji.Length;
            hashCode = kanji.GetHashCode() ^ yomi.GetHashCode() ^ (int)okurigana;
            lock (LOCK)
            {
                objectIndex = objectConter++;
            }
        }

        internal string Kanji
        {
            get { return kanji; }
        }

        internal string Yomi
        {
            get { return yomi; }
        }

        internal char Okurigana
        {
            get { return okurigana; }
        }

        internal int Length
        {
            get { return kanjiLength + (okurigana > 0 ? 1 : 0); }
        }

        internal string GetYomiFor(string target)
        {
            if (kanjiLength > 0 && !target.StartsWith(kanji))
            {
                return null;
            }
            if (okurigana == 0)
            {
                return yomi;
            }
            try
            {
                char ch = target[kanjiLength];
                string okuriganaList = (string)okuriganaTable[ch];
                return
                    okuriganaList == null || okuriganaList.IndexOf(okurigana) < 0 ?
                    null : yomi + ch;
            }
            catch
            {
                return null;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is KanjiYomi kanjiYomi)
            {
                return
                        hashCode == kanjiYomi.GetHashCode() &&
                        kanji.Equals(kanjiYomi.kanji) &&
                        yomi.Equals(kanjiYomi.yomi) &&
                        okurigana == kanjiYomi.okurigana;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }

        public int CompareTo(object o)
        {
            KanjiYomi other = (KanjiYomi)o;
            if (other.kanjiLength == kanjiLength)
            {
                if (okurigana > 0 && other.okurigana == 0)
                {
                    return -1;
                }
                else if (okurigana == 0 && other.okurigana > 0)
                {
                    return 1;
                }
                else
                {
                    return Equals(other) ? 0 :
                        objectIndex < other.objectIndex ? -1 : 1;
                }
            }
            else
            {
                return other.kanjiLength < kanjiLength ? -1 : 1;
            }
        }
    }
}
