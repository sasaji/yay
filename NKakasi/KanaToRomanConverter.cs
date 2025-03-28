using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace NKakasi
{
    class KanaToRomanConverter
    {
        private class Table
        {
            readonly Dictionary<char, LinkedList<Entry>> table = new Dictionary<char, LinkedList<Entry>>();

            internal void Add(string kana, string romaji)
            {
                char key = kana[0];
                Entry newEntry = new Entry(kana.Substring(1), romaji);

                if (!table.ContainsKey(key)) {
                    LinkedList<Entry> list = new LinkedList<Entry>();
                    list.AddLast(newEntry);
                    table.Add(key, list);
                } else {
                    LinkedList<Entry> list = table[key];
                    int newLength = newEntry.KanaLength;
                    var currentNode = list.Last;
                    for (LinkedListNode<Entry> node = list.First; node != null; node = node.Next) {
                        if (newLength >= node.Value.KanaLength) {
                            currentNode = node.Previous;
                            break;
                        }
                    }
                    if (currentNode == null) {
                        list.AddFirst(newEntry);
                    } else {
                        list.AddAfter(currentNode, newEntry);
                    }
                }
            }

            internal string Get(KakasiReader input)
            {
                int ch = input.Get();
                if (ch < 0) {
                    return null;
                }
                if (!table.ContainsKey((char)ch))
                    return null;
                LinkedList<Entry> list = table[(char)ch];
                string rest = null;
                int restLength = 0;
                for (LinkedListNode<Entry> node = list.First; node != null; node = node.Next) {
                    Entry entry = node.Value;
                    int length = entry.KanaLength;
                    if (length > 0 && rest == null) {
                        char[] chars = new char[length];
                        restLength = input.More(chars);
                        rest = new string(chars, 0, restLength);
                    }
                    if (length > restLength) {
                        continue;
                    }
                    string romaji = entry.GetRomajiFor(rest);
                    if (romaji != null) {
                        input.Consume(length + 1);
                        return romaji;
                    }
                }
                return null;
            }
        }

        private class Entry
        {
            readonly string kana;
            readonly string romaji;

            public Entry(string kana, string romaji)
            {
                this.kana = kana;
                this.romaji = romaji;
            }

            internal int KanaLength
            {
                get { return kana.Length; }
            }

            internal string GetRomajiFor(string str)
            {
                return
                    KanaLength == 0 ||
                    (str != null && str.StartsWith(kana)) ? romaji : null;
            }
        }

        private static Table hiraganaToHepburn;
        private static Table hiraganaToKunrei;
        private static Table katakanaToHepburn;
        private static Table katakanaToKunrei;

        private static Table GetHiraganaToHepburnTable()
        {
            if (hiraganaToHepburn == null) {
                hiraganaToHepburn = new Table();
                hiraganaToHepburn.Add("\u3041", "a");
                hiraganaToHepburn.Add("\u3042", "a");
                hiraganaToHepburn.Add("\u3043", "i");
                hiraganaToHepburn.Add("\u3044", "i");
                hiraganaToHepburn.Add("\u3045", "u");
                hiraganaToHepburn.Add("\u3046", "u");
                hiraganaToHepburn.Add("\u3046\u309b", "vu");
                hiraganaToHepburn.Add("\u3046\u309b\u3041", "va");
                hiraganaToHepburn.Add("\u3046\u309b\u3043", "vi");
                hiraganaToHepburn.Add("\u3046\u309b\u3047", "ve");
                hiraganaToHepburn.Add("\u3046\u309b\u3049", "vo");
                hiraganaToHepburn.Add("\u3047", "e");
                hiraganaToHepburn.Add("\u3048", "e");
                hiraganaToHepburn.Add("\u3049", "o");
                hiraganaToHepburn.Add("\u304a", "o");
                hiraganaToHepburn.Add("\u304b", "ka");
                hiraganaToHepburn.Add("\u304c", "ga");
                hiraganaToHepburn.Add("\u304d", "ki");
                hiraganaToHepburn.Add("\u304d\u3083", "kya");
                hiraganaToHepburn.Add("\u304d\u3085", "kyu");
                hiraganaToHepburn.Add("\u304d\u3087", "kyo");
                hiraganaToHepburn.Add("\u304e", "gi");
                hiraganaToHepburn.Add("\u304e\u3083", "gya");
                hiraganaToHepburn.Add("\u304e\u3085", "gyu");
                hiraganaToHepburn.Add("\u304e\u3087", "gyo");
                hiraganaToHepburn.Add("\u304f", "ku");
                hiraganaToHepburn.Add("\u3050", "gu");
                hiraganaToHepburn.Add("\u3051", "ke");
                hiraganaToHepburn.Add("\u3052", "ge");
                hiraganaToHepburn.Add("\u3053", "ko");
                hiraganaToHepburn.Add("\u3054", "go");
                hiraganaToHepburn.Add("\u3055", "sa");
                hiraganaToHepburn.Add("\u3056", "za");
                hiraganaToHepburn.Add("\u3057", "shi");
                hiraganaToHepburn.Add("\u3057\u3083", "sha");
                hiraganaToHepburn.Add("\u3057\u3085", "shu");
                hiraganaToHepburn.Add("\u3057\u3087", "sho");
                hiraganaToHepburn.Add("\u3058", "ji");
                hiraganaToHepburn.Add("\u3058\u3083", "ja");
                hiraganaToHepburn.Add("\u3058\u3085", "ju");
                hiraganaToHepburn.Add("\u3058\u3087", "jo");
                hiraganaToHepburn.Add("\u3059", "su");
                hiraganaToHepburn.Add("\u305a", "zu");
                hiraganaToHepburn.Add("\u305b", "se");
                hiraganaToHepburn.Add("\u305c", "ze");
                hiraganaToHepburn.Add("\u305d", "so");
                hiraganaToHepburn.Add("\u305e", "zo");
                hiraganaToHepburn.Add("\u305f", "ta");
                hiraganaToHepburn.Add("\u3060", "da");
                hiraganaToHepburn.Add("\u3061", "chi");
                hiraganaToHepburn.Add("\u3061\u3083", "cha");
                hiraganaToHepburn.Add("\u3061\u3085", "chu");
                hiraganaToHepburn.Add("\u3061\u3087", "cho");
                hiraganaToHepburn.Add("\u3062", "di");
                hiraganaToHepburn.Add("\u3062\u3083", "dya");
                hiraganaToHepburn.Add("\u3062\u3085", "dyu");
                hiraganaToHepburn.Add("\u3062\u3087", "dyo");
                hiraganaToHepburn.Add("\u3063", "tsu");
                hiraganaToHepburn.Add("\u3063\u3046\u309b", "vvu");
                hiraganaToHepburn.Add("\u3063\u3046\u309b\u3041", "vva");
                hiraganaToHepburn.Add("\u3063\u3046\u309b\u3043", "vvi");
                hiraganaToHepburn.Add("\u3063\u3046\u309b\u3047", "vve");
                hiraganaToHepburn.Add("\u3063\u3046\u309b\u3049", "vvo");
                hiraganaToHepburn.Add("\u3063\u304b", "kka");
                hiraganaToHepburn.Add("\u3063\u304c", "gga");
                hiraganaToHepburn.Add("\u3063\u304d", "kki");
                hiraganaToHepburn.Add("\u3063\u304d\u3083", "kkya");
                hiraganaToHepburn.Add("\u3063\u304d\u3085", "kkyu");
                hiraganaToHepburn.Add("\u3063\u304d\u3087", "kkyo");
                hiraganaToHepburn.Add("\u3063\u304e", "ggi");
                hiraganaToHepburn.Add("\u3063\u304e\u3083", "ggya");
                hiraganaToHepburn.Add("\u3063\u304e\u3085", "ggyu");
                hiraganaToHepburn.Add("\u3063\u304e\u3087", "ggyo");
                hiraganaToHepburn.Add("\u3063\u304f", "kku");
                hiraganaToHepburn.Add("\u3063\u3050", "ggu");
                hiraganaToHepburn.Add("\u3063\u3051", "kke");
                hiraganaToHepburn.Add("\u3063\u3052", "gge");
                hiraganaToHepburn.Add("\u3063\u3053", "kko");
                hiraganaToHepburn.Add("\u3063\u3054", "ggo");
                hiraganaToHepburn.Add("\u3063\u3055", "ssa");
                hiraganaToHepburn.Add("\u3063\u3056", "zza");
                hiraganaToHepburn.Add("\u3063\u3057", "sshi");
                hiraganaToHepburn.Add("\u3063\u3057\u3083", "ssha");
                hiraganaToHepburn.Add("\u3063\u3057\u3085", "sshu");
                hiraganaToHepburn.Add("\u3063\u3057\u3087", "ssho");
                hiraganaToHepburn.Add("\u3063\u3058", "jji");
                hiraganaToHepburn.Add("\u3063\u3058\u3083", "jja");
                hiraganaToHepburn.Add("\u3063\u3058\u3085", "jju");
                hiraganaToHepburn.Add("\u3063\u3058\u3087", "jjo");
                hiraganaToHepburn.Add("\u3063\u3059", "ssu");
                hiraganaToHepburn.Add("\u3063\u305a", "zzu");
                hiraganaToHepburn.Add("\u3063\u305b", "sse");
                hiraganaToHepburn.Add("\u3063\u305c", "zze");
                hiraganaToHepburn.Add("\u3063\u305d", "sso");
                hiraganaToHepburn.Add("\u3063\u305e", "zzo");
                hiraganaToHepburn.Add("\u3063\u305f", "tta");
                hiraganaToHepburn.Add("\u3063\u3060", "dda");
                hiraganaToHepburn.Add("\u3063\u3061", "cchi");
                hiraganaToHepburn.Add("\u3063\u3061\u3083", "ccha");
                hiraganaToHepburn.Add("\u3063\u3061\u3085", "cchu");
                hiraganaToHepburn.Add("\u3063\u3061\u3087", "ccho");
                hiraganaToHepburn.Add("\u3063\u3062", "ddi");
                hiraganaToHepburn.Add("\u3063\u3062\u3083", "ddya");
                hiraganaToHepburn.Add("\u3063\u3062\u3085", "ddyu");
                hiraganaToHepburn.Add("\u3063\u3062\u3087", "ddyo");
                hiraganaToHepburn.Add("\u3063\u3064", "ttsu");
                hiraganaToHepburn.Add("\u3063\u3065", "ddu");
                hiraganaToHepburn.Add("\u3063\u3066", "tte");
                hiraganaToHepburn.Add("\u3063\u3067", "dde");
                hiraganaToHepburn.Add("\u3063\u3068", "tto");
                hiraganaToHepburn.Add("\u3063\u3069", "ddo");
                hiraganaToHepburn.Add("\u3063\u306f", "hha");
                hiraganaToHepburn.Add("\u3063\u3070", "bba");
                hiraganaToHepburn.Add("\u3063\u3071", "ppa");
                hiraganaToHepburn.Add("\u3063\u3072", "hhi");
                hiraganaToHepburn.Add("\u3063\u3072\u3083", "hhya");
                hiraganaToHepburn.Add("\u3063\u3072\u3085", "hhyu");
                hiraganaToHepburn.Add("\u3063\u3072\u3087", "hhyo");
                hiraganaToHepburn.Add("\u3063\u3073", "bbi");
                hiraganaToHepburn.Add("\u3063\u3073\u3083", "bbya");
                hiraganaToHepburn.Add("\u3063\u3073\u3085", "bbyu");
                hiraganaToHepburn.Add("\u3063\u3073\u3087", "bbyo");
                hiraganaToHepburn.Add("\u3063\u3074", "ppi");
                hiraganaToHepburn.Add("\u3063\u3074\u3083", "ppya");
                hiraganaToHepburn.Add("\u3063\u3074\u3085", "ppyu");
                hiraganaToHepburn.Add("\u3063\u3074\u3087", "ppyo");
                hiraganaToHepburn.Add("\u3063\u3075", "ffu");
                hiraganaToHepburn.Add("\u3063\u3075\u3041", "ffa");
                hiraganaToHepburn.Add("\u3063\u3075\u3043", "ffi");
                hiraganaToHepburn.Add("\u3063\u3075\u3047", "ffe");
                hiraganaToHepburn.Add("\u3063\u3075\u3049", "ffo");
                hiraganaToHepburn.Add("\u3063\u3076", "bbu");
                hiraganaToHepburn.Add("\u3063\u3077", "ppu");
                hiraganaToHepburn.Add("\u3063\u3078", "hhe");
                hiraganaToHepburn.Add("\u3063\u3079", "bbe");
                hiraganaToHepburn.Add("\u3063\u307a", "ppe");
                hiraganaToHepburn.Add("\u3063\u307b", "hho");
                hiraganaToHepburn.Add("\u3063\u307c", "bbo");
                hiraganaToHepburn.Add("\u3063\u307d", "ppo");
                hiraganaToHepburn.Add("\u3063\u3084", "yya");
                hiraganaToHepburn.Add("\u3063\u3086", "yyu");
                hiraganaToHepburn.Add("\u3063\u3088", "yyo");
                hiraganaToHepburn.Add("\u3063\u3089", "rra");
                hiraganaToHepburn.Add("\u3063\u308a", "rri");
                hiraganaToHepburn.Add("\u3063\u308a\u3083", "rrya");
                hiraganaToHepburn.Add("\u3063\u308a\u3085", "rryu");
                hiraganaToHepburn.Add("\u3063\u308a\u3087", "rryo");
                hiraganaToHepburn.Add("\u3063\u308b", "rru");
                hiraganaToHepburn.Add("\u3063\u308c", "rre");
                hiraganaToHepburn.Add("\u3063\u308d", "rro");
                hiraganaToHepburn.Add("\u3064", "tsu");
                hiraganaToHepburn.Add("\u3065", "du");
                hiraganaToHepburn.Add("\u3066", "te");
                hiraganaToHepburn.Add("\u3067", "de");
                hiraganaToHepburn.Add("\u3068", "to");
                hiraganaToHepburn.Add("\u3069", "do");
                hiraganaToHepburn.Add("\u306a", "na");
                hiraganaToHepburn.Add("\u306b", "ni");
                hiraganaToHepburn.Add("\u306b\u3083", "nya");
                hiraganaToHepburn.Add("\u306b\u3085", "nyu");
                hiraganaToHepburn.Add("\u306b\u3087", "nyo");
                hiraganaToHepburn.Add("\u306c", "nu");
                hiraganaToHepburn.Add("\u306d", "ne");
                hiraganaToHepburn.Add("\u306e", "no");
                hiraganaToHepburn.Add("\u306f", "ha");
                hiraganaToHepburn.Add("\u3070", "ba");
                hiraganaToHepburn.Add("\u3071", "pa");
                hiraganaToHepburn.Add("\u3072", "hi");
                hiraganaToHepburn.Add("\u3072\u3083", "hya");
                hiraganaToHepburn.Add("\u3072\u3085", "hyu");
                hiraganaToHepburn.Add("\u3072\u3087", "hyo");
                hiraganaToHepburn.Add("\u3073", "bi");
                hiraganaToHepburn.Add("\u3073\u3083", "bya");
                hiraganaToHepburn.Add("\u3073\u3085", "byu");
                hiraganaToHepburn.Add("\u3073\u3087", "byo");
                hiraganaToHepburn.Add("\u3074", "pi");
                hiraganaToHepburn.Add("\u3074\u3083", "pya");
                hiraganaToHepburn.Add("\u3074\u3085", "pyu");
                hiraganaToHepburn.Add("\u3074\u3087", "pyo");
                hiraganaToHepburn.Add("\u3075", "fu");
                hiraganaToHepburn.Add("\u3075\u3041", "fa");
                hiraganaToHepburn.Add("\u3075\u3043", "fi");
                hiraganaToHepburn.Add("\u3075\u3047", "fe");
                hiraganaToHepburn.Add("\u3075\u3049", "fo");
                hiraganaToHepburn.Add("\u3076", "bu");
                hiraganaToHepburn.Add("\u3077", "pu");
                hiraganaToHepburn.Add("\u3078", "he");
                hiraganaToHepburn.Add("\u3079", "be");
                hiraganaToHepburn.Add("\u307a", "pe");
                hiraganaToHepburn.Add("\u307b", "ho");
                hiraganaToHepburn.Add("\u307c", "bo");
                hiraganaToHepburn.Add("\u307d", "po");
                hiraganaToHepburn.Add("\u307e", "ma");
                hiraganaToHepburn.Add("\u307f", "mi");
                hiraganaToHepburn.Add("\u307f\u3083", "mya");
                hiraganaToHepburn.Add("\u307f\u3085", "myu");
                hiraganaToHepburn.Add("\u307f\u3087", "myo");
                hiraganaToHepburn.Add("\u3080", "mu");
                hiraganaToHepburn.Add("\u3081", "me");
                hiraganaToHepburn.Add("\u3082", "mo");
                hiraganaToHepburn.Add("\u3083", "ya");
                hiraganaToHepburn.Add("\u3084", "ya");
                hiraganaToHepburn.Add("\u3085", "yu");
                hiraganaToHepburn.Add("\u3086", "yu");
                hiraganaToHepburn.Add("\u3087", "yo");
                hiraganaToHepburn.Add("\u3088", "yo");
                hiraganaToHepburn.Add("\u3089", "ra");
                hiraganaToHepburn.Add("\u308a", "ri");
                hiraganaToHepburn.Add("\u308a\u3083", "rya");
                hiraganaToHepburn.Add("\u308a\u3085", "ryu");
                hiraganaToHepburn.Add("\u308a\u3087", "ryo");
                hiraganaToHepburn.Add("\u308b", "ru");
                hiraganaToHepburn.Add("\u308c", "re");
                hiraganaToHepburn.Add("\u308d", "ro");
                hiraganaToHepburn.Add("\u308e", "wa");
                hiraganaToHepburn.Add("\u308f", "wa");
                hiraganaToHepburn.Add("\u3090", "i");
                hiraganaToHepburn.Add("\u3091", "e");
                hiraganaToHepburn.Add("\u3092", "wo");
                hiraganaToHepburn.Add("\u3093", "n");
                hiraganaToHepburn.Add("\u3093\u3042", "n'a");
                hiraganaToHepburn.Add("\u3093\u3044", "n'i");
                hiraganaToHepburn.Add("\u3093\u3046", "n'u");
                hiraganaToHepburn.Add("\u3093\u3048", "n'e");
                hiraganaToHepburn.Add("\u3093\u304a", "n'o");
            }
            return hiraganaToHepburn;
        }

        private static Table getHiraganaToKunreiTable()
        {
            if (hiraganaToKunrei == null) {
                hiraganaToKunrei = new Table();
                hiraganaToKunrei.Add("\u3041", "a");
                hiraganaToKunrei.Add("\u3042", "a");
                hiraganaToKunrei.Add("\u3043", "i");
                hiraganaToKunrei.Add("\u3044", "i");
                hiraganaToKunrei.Add("\u3045", "u");
                hiraganaToKunrei.Add("\u3046", "u");
                hiraganaToKunrei.Add("\u3046\u309b", "vu");
                hiraganaToKunrei.Add("\u3046\u309b\u3041", "va");
                hiraganaToKunrei.Add("\u3046\u309b\u3043", "vi");
                hiraganaToKunrei.Add("\u3046\u309b\u3047", "ve");
                hiraganaToKunrei.Add("\u3046\u309b\u3049", "vo");
                hiraganaToKunrei.Add("\u3047", "e");
                hiraganaToKunrei.Add("\u3048", "e");
                hiraganaToKunrei.Add("\u3049", "o");
                hiraganaToKunrei.Add("\u304a", "o");
                hiraganaToKunrei.Add("\u304b", "ka");
                hiraganaToKunrei.Add("\u304c", "ga");
                hiraganaToKunrei.Add("\u304d", "ki");
                hiraganaToKunrei.Add("\u304d\u3083", "kya");
                hiraganaToKunrei.Add("\u304d\u3085", "kyu");
                hiraganaToKunrei.Add("\u304d\u3087", "kyo");
                hiraganaToKunrei.Add("\u304e", "gi");
                hiraganaToKunrei.Add("\u304e\u3083", "gya");
                hiraganaToKunrei.Add("\u304e\u3085", "gyu");
                hiraganaToKunrei.Add("\u304e\u3087", "gyo");
                hiraganaToKunrei.Add("\u304f", "ku");
                hiraganaToKunrei.Add("\u3050", "gu");
                hiraganaToKunrei.Add("\u3051", "ke");
                hiraganaToKunrei.Add("\u3052", "ge");
                hiraganaToKunrei.Add("\u3053", "ko");
                hiraganaToKunrei.Add("\u3054", "go");
                hiraganaToKunrei.Add("\u3055", "sa");
                hiraganaToKunrei.Add("\u3056", "za");
                hiraganaToKunrei.Add("\u3057", "si");
                hiraganaToKunrei.Add("\u3057\u3083", "sya");
                hiraganaToKunrei.Add("\u3057\u3085", "syu");
                hiraganaToKunrei.Add("\u3057\u3087", "syo");
                hiraganaToKunrei.Add("\u3058", "zi");
                hiraganaToKunrei.Add("\u3058\u3083", "zya");
                hiraganaToKunrei.Add("\u3058\u3085", "zyu");
                hiraganaToKunrei.Add("\u3058\u3087", "zyo");
                hiraganaToKunrei.Add("\u3059", "su");
                hiraganaToKunrei.Add("\u305a", "zu");
                hiraganaToKunrei.Add("\u305b", "se");
                hiraganaToKunrei.Add("\u305c", "ze");
                hiraganaToKunrei.Add("\u305d", "so");
                hiraganaToKunrei.Add("\u305e", "zo");
                hiraganaToKunrei.Add("\u305f", "ta");
                hiraganaToKunrei.Add("\u3060", "da");
                hiraganaToKunrei.Add("\u3061", "ti");
                hiraganaToKunrei.Add("\u3061\u3083", "tya");
                hiraganaToKunrei.Add("\u3061\u3085", "tyu");
                hiraganaToKunrei.Add("\u3061\u3087", "tyo");
                hiraganaToKunrei.Add("\u3062", "di");
                hiraganaToKunrei.Add("\u3062\u3083", "dya");
                hiraganaToKunrei.Add("\u3062\u3085", "dyu");
                hiraganaToKunrei.Add("\u3062\u3087", "dyo");
                hiraganaToKunrei.Add("\u3063", "tu");
                hiraganaToKunrei.Add("\u3063\u3046\u309b", "vvu");
                hiraganaToKunrei.Add("\u3063\u3046\u309b\u3041", "vva");
                hiraganaToKunrei.Add("\u3063\u3046\u309b\u3043", "vvi");
                hiraganaToKunrei.Add("\u3063\u3046\u309b\u3047", "vve");
                hiraganaToKunrei.Add("\u3063\u3046\u309b\u3049", "vvo");
                hiraganaToKunrei.Add("\u3063\u304b", "kka");
                hiraganaToKunrei.Add("\u3063\u304c", "gga");
                hiraganaToKunrei.Add("\u3063\u304d", "kki");
                hiraganaToKunrei.Add("\u3063\u304d\u3083", "kkya");
                hiraganaToKunrei.Add("\u3063\u304d\u3085", "kkyu");
                hiraganaToKunrei.Add("\u3063\u304d\u3087", "kkyo");
                hiraganaToKunrei.Add("\u3063\u304e", "ggi");
                hiraganaToKunrei.Add("\u3063\u304e\u3083", "ggya");
                hiraganaToKunrei.Add("\u3063\u304e\u3085", "ggyu");
                hiraganaToKunrei.Add("\u3063\u304e\u3087", "ggyo");
                hiraganaToKunrei.Add("\u3063\u304f", "kku");
                hiraganaToKunrei.Add("\u3063\u3050", "ggu");
                hiraganaToKunrei.Add("\u3063\u3051", "kke");
                hiraganaToKunrei.Add("\u3063\u3052", "gge");
                hiraganaToKunrei.Add("\u3063\u3053", "kko");
                hiraganaToKunrei.Add("\u3063\u3054", "ggo");
                hiraganaToKunrei.Add("\u3063\u3055", "ssa");
                hiraganaToKunrei.Add("\u3063\u3056", "zza");
                hiraganaToKunrei.Add("\u3063\u3057", "ssi");
                hiraganaToKunrei.Add("\u3063\u3057\u3083", "ssya");
                hiraganaToKunrei.Add("\u3063\u3057\u3085", "ssyu");
                hiraganaToKunrei.Add("\u3063\u3057\u3087", "ssho");
                hiraganaToKunrei.Add("\u3063\u3058", "zzi");
                hiraganaToKunrei.Add("\u3063\u3058\u3083", "zzya");
                hiraganaToKunrei.Add("\u3063\u3058\u3085", "zzyu");
                hiraganaToKunrei.Add("\u3063\u3058\u3087", "zzyo");
                hiraganaToKunrei.Add("\u3063\u3059", "ssu");
                hiraganaToKunrei.Add("\u3063\u305a", "zzu");
                hiraganaToKunrei.Add("\u3063\u305b", "sse");
                hiraganaToKunrei.Add("\u3063\u305c", "zze");
                hiraganaToKunrei.Add("\u3063\u305d", "sso");
                hiraganaToKunrei.Add("\u3063\u305e", "zzo");
                hiraganaToKunrei.Add("\u3063\u305f", "tta");
                hiraganaToKunrei.Add("\u3063\u3060", "dda");
                hiraganaToKunrei.Add("\u3063\u3061", "tti");
                hiraganaToKunrei.Add("\u3063\u3061\u3083", "ttya");
                hiraganaToKunrei.Add("\u3063\u3061\u3085", "ttyu");
                hiraganaToKunrei.Add("\u3063\u3061\u3087", "ttyo");
                hiraganaToKunrei.Add("\u3063\u3062", "ddi");
                hiraganaToKunrei.Add("\u3063\u3062\u3083", "ddya");
                hiraganaToKunrei.Add("\u3063\u3062\u3085", "ddyu");
                hiraganaToKunrei.Add("\u3063\u3062\u3087", "ddyo");
                hiraganaToKunrei.Add("\u3063\u3064", "ttu");
                hiraganaToKunrei.Add("\u3063\u3065", "ddu");
                hiraganaToKunrei.Add("\u3063\u3066", "tte");
                hiraganaToKunrei.Add("\u3063\u3067", "dde");
                hiraganaToKunrei.Add("\u3063\u3068", "tto");
                hiraganaToKunrei.Add("\u3063\u3069", "ddo");
                hiraganaToKunrei.Add("\u3063\u306f", "hha");
                hiraganaToKunrei.Add("\u3063\u3070", "bba");
                hiraganaToKunrei.Add("\u3063\u3071", "ppa");
                hiraganaToKunrei.Add("\u3063\u3072", "hhi");
                hiraganaToKunrei.Add("\u3063\u3072\u3083", "hhya");
                hiraganaToKunrei.Add("\u3063\u3072\u3085", "hhyu");
                hiraganaToKunrei.Add("\u3063\u3072\u3087", "hhyo");
                hiraganaToKunrei.Add("\u3063\u3073", "bbi");
                hiraganaToKunrei.Add("\u3063\u3073\u3083", "bbya");
                hiraganaToKunrei.Add("\u3063\u3073\u3085", "bbyu");
                hiraganaToKunrei.Add("\u3063\u3073\u3087", "bbyo");
                hiraganaToKunrei.Add("\u3063\u3074", "ppi");
                hiraganaToKunrei.Add("\u3063\u3074\u3083", "ppya");
                hiraganaToKunrei.Add("\u3063\u3074\u3085", "ppyu");
                hiraganaToKunrei.Add("\u3063\u3074\u3087", "ppyo");
                hiraganaToKunrei.Add("\u3063\u3075", "hhu");
                hiraganaToKunrei.Add("\u3063\u3075\u3041", "ffa");
                hiraganaToKunrei.Add("\u3063\u3075\u3043", "ffi");
                hiraganaToKunrei.Add("\u3063\u3075\u3047", "ffe");
                hiraganaToKunrei.Add("\u3063\u3075\u3049", "ffo");
                hiraganaToKunrei.Add("\u3063\u3076", "bbu");
                hiraganaToKunrei.Add("\u3063\u3077", "ppu");
                hiraganaToKunrei.Add("\u3063\u3078", "hhe");
                hiraganaToKunrei.Add("\u3063\u3079", "bbe");
                hiraganaToKunrei.Add("\u3063\u307a", "ppe");
                hiraganaToKunrei.Add("\u3063\u307b", "hho");
                hiraganaToKunrei.Add("\u3063\u307c", "bbo");
                hiraganaToKunrei.Add("\u3063\u307d", "ppo");
                hiraganaToKunrei.Add("\u3063\u3084", "yya");
                hiraganaToKunrei.Add("\u3063\u3086", "yyu");
                hiraganaToKunrei.Add("\u3063\u3088", "yyo");
                hiraganaToKunrei.Add("\u3063\u3089", "rra");
                hiraganaToKunrei.Add("\u3063\u308a", "rri");
                hiraganaToKunrei.Add("\u3063\u308a\u3083", "rrya");
                hiraganaToKunrei.Add("\u3063\u308a\u3085", "rryu");
                hiraganaToKunrei.Add("\u3063\u308a\u3087", "rryo");
                hiraganaToKunrei.Add("\u3063\u308b", "rru");
                hiraganaToKunrei.Add("\u3063\u308c", "rre");
                hiraganaToKunrei.Add("\u3063\u308d", "rro");
                hiraganaToKunrei.Add("\u3064", "tu");
                hiraganaToKunrei.Add("\u3065", "du");
                hiraganaToKunrei.Add("\u3066", "te");
                hiraganaToKunrei.Add("\u3067", "de");
                hiraganaToKunrei.Add("\u3068", "to");
                hiraganaToKunrei.Add("\u3069", "do");
                hiraganaToKunrei.Add("\u306a", "na");
                hiraganaToKunrei.Add("\u306b", "ni");
                hiraganaToKunrei.Add("\u306b\u3083", "nya");
                hiraganaToKunrei.Add("\u306b\u3085", "nyu");
                hiraganaToKunrei.Add("\u306b\u3087", "nyo");
                hiraganaToKunrei.Add("\u306c", "nu");
                hiraganaToKunrei.Add("\u306d", "ne");
                hiraganaToKunrei.Add("\u306e", "no");
                hiraganaToKunrei.Add("\u306f", "ha");
                hiraganaToKunrei.Add("\u3070", "ba");
                hiraganaToKunrei.Add("\u3071", "pa");
                hiraganaToKunrei.Add("\u3072", "hi");
                hiraganaToKunrei.Add("\u3072\u3083", "hya");
                hiraganaToKunrei.Add("\u3072\u3085", "hyu");
                hiraganaToKunrei.Add("\u3072\u3087", "hyo");
                hiraganaToKunrei.Add("\u3073", "bi");
                hiraganaToKunrei.Add("\u3073\u3083", "bya");
                hiraganaToKunrei.Add("\u3073\u3085", "byu");
                hiraganaToKunrei.Add("\u3073\u3087", "byo");
                hiraganaToKunrei.Add("\u3074", "pi");
                hiraganaToKunrei.Add("\u3074\u3083", "pya");
                hiraganaToKunrei.Add("\u3074\u3085", "pyu");
                hiraganaToKunrei.Add("\u3074\u3087", "pyo");
                hiraganaToKunrei.Add("\u3075", "hu");
                hiraganaToKunrei.Add("\u3075\u3041", "fa");
                hiraganaToKunrei.Add("\u3075\u3043", "fi");
                hiraganaToKunrei.Add("\u3075\u3047", "fe");
                hiraganaToKunrei.Add("\u3075\u3049", "fo");
                hiraganaToKunrei.Add("\u3076", "bu");
                hiraganaToKunrei.Add("\u3077", "pu");
                hiraganaToKunrei.Add("\u3078", "he");
                hiraganaToKunrei.Add("\u3079", "be");
                hiraganaToKunrei.Add("\u307a", "pe");
                hiraganaToKunrei.Add("\u307b", "ho");
                hiraganaToKunrei.Add("\u307c", "bo");
                hiraganaToKunrei.Add("\u307d", "po");
                hiraganaToKunrei.Add("\u307e", "ma");
                hiraganaToKunrei.Add("\u307f", "mi");
                hiraganaToKunrei.Add("\u307f\u3083", "mya");
                hiraganaToKunrei.Add("\u307f\u3085", "myu");
                hiraganaToKunrei.Add("\u307f\u3087", "myo");
                hiraganaToKunrei.Add("\u3080", "mu");
                hiraganaToKunrei.Add("\u3081", "me");
                hiraganaToKunrei.Add("\u3082", "mo");
                hiraganaToKunrei.Add("\u3083", "ya");
                hiraganaToKunrei.Add("\u3084", "ya");
                hiraganaToKunrei.Add("\u3085", "yu");
                hiraganaToKunrei.Add("\u3086", "yu");
                hiraganaToKunrei.Add("\u3087", "yo");
                hiraganaToKunrei.Add("\u3088", "yo");
                hiraganaToKunrei.Add("\u3089", "ra");
                hiraganaToKunrei.Add("\u308a", "ri");
                hiraganaToKunrei.Add("\u308a\u3083", "rya");
                hiraganaToKunrei.Add("\u308a\u3085", "ryu");
                hiraganaToKunrei.Add("\u308a\u3087", "ryo");
                hiraganaToKunrei.Add("\u308b", "ru");
                hiraganaToKunrei.Add("\u308c", "re");
                hiraganaToKunrei.Add("\u308d", "ro");
                hiraganaToKunrei.Add("\u308e", "wa");
                hiraganaToKunrei.Add("\u308f", "wa");
                hiraganaToKunrei.Add("\u3090", "i");
                hiraganaToKunrei.Add("\u3091", "e");
                hiraganaToKunrei.Add("\u3092", "wo");
                hiraganaToKunrei.Add("\u3093", "n");
                hiraganaToKunrei.Add("\u3093\u3042", "n'a");
                hiraganaToKunrei.Add("\u3093\u3044", "n'i");
                hiraganaToKunrei.Add("\u3093\u3046", "n'u");
                hiraganaToKunrei.Add("\u3093\u3048", "n'e");
                hiraganaToKunrei.Add("\u3093\u304a", "n'o");
            }
            return hiraganaToKunrei;
        }

        private static Table GetKatakanaToHepburnTable()
        {
            if (katakanaToHepburn == null) {
                katakanaToHepburn = new Table();
                katakanaToHepburn.Add("\u30a1", "a");
                katakanaToHepburn.Add("\u30a2", "a");
                katakanaToHepburn.Add("\u30a3", "i");
                katakanaToHepburn.Add("\u30a4", "i");
                katakanaToHepburn.Add("\u30a5", "u");
                katakanaToHepburn.Add("\u30a6", "u");
                katakanaToHepburn.Add("\u30a7", "e");
                katakanaToHepburn.Add("\u30a8", "e");
                katakanaToHepburn.Add("\u30a9", "o");
                katakanaToHepburn.Add("\u30aa", "o");
                katakanaToHepburn.Add("\u30ab", "ka");
                katakanaToHepburn.Add("\u30ac", "ga");
                katakanaToHepburn.Add("\u30ad", "ki");
                katakanaToHepburn.Add("\u30ad\u30e3", "kya");
                katakanaToHepburn.Add("\u30ad\u30e5", "kyu");
                katakanaToHepburn.Add("\u30ad\u30e7", "kyo");
                katakanaToHepburn.Add("\u30ae", "gi");
                katakanaToHepburn.Add("\u30ae\u30e3", "gya");
                katakanaToHepburn.Add("\u30ae\u30e5", "gyu");
                katakanaToHepburn.Add("\u30ae\u30e7", "gyo");
                katakanaToHepburn.Add("\u30af", "ku");
                katakanaToHepburn.Add("\u30b0", "gu");
                katakanaToHepburn.Add("\u30b1", "ke");
                katakanaToHepburn.Add("\u30b2", "ge");
                katakanaToHepburn.Add("\u30b3", "ko");
                katakanaToHepburn.Add("\u30b4", "go");
                katakanaToHepburn.Add("\u30b5", "sa");
                katakanaToHepburn.Add("\u30b6", "za");
                katakanaToHepburn.Add("\u30b7", "shi");
                katakanaToHepburn.Add("\u30b7\u30e3", "sha");
                katakanaToHepburn.Add("\u30b7\u30e5", "shu");
                katakanaToHepburn.Add("\u30b7\u30e7", "sho");
                katakanaToHepburn.Add("\u30b8", "ji");
                katakanaToHepburn.Add("\u30b8\u30e3", "ja");
                katakanaToHepburn.Add("\u30b8\u30e5", "ju");
                katakanaToHepburn.Add("\u30b8\u30e7", "jo");
                katakanaToHepburn.Add("\u30b9", "su");
                katakanaToHepburn.Add("\u30ba", "zu");
                katakanaToHepburn.Add("\u30bb", "se");
                katakanaToHepburn.Add("\u30bc", "ze");
                katakanaToHepburn.Add("\u30bd", "so");
                katakanaToHepburn.Add("\u30be", "zo");
                katakanaToHepburn.Add("\u30bf", "ta");
                katakanaToHepburn.Add("\u30c0", "da");
                katakanaToHepburn.Add("\u30c1", "chi");
                katakanaToHepburn.Add("\u30c1\u30e3", "cha");
                katakanaToHepburn.Add("\u30c1\u30e5", "chu");
                katakanaToHepburn.Add("\u30c1\u30e7", "cho");
                katakanaToHepburn.Add("\u30c2", "di");
                katakanaToHepburn.Add("\u30c2\u30e3", "dya");
                katakanaToHepburn.Add("\u30c2\u30e5", "dyu");
                katakanaToHepburn.Add("\u30c2\u30e7", "dyo");
                katakanaToHepburn.Add("\u30c3", "tsu");
                katakanaToHepburn.Add("\u30c3\u30ab", "kka");
                katakanaToHepburn.Add("\u30c3\u30ac", "gga");
                katakanaToHepburn.Add("\u30c3\u30ad", "kki");
                katakanaToHepburn.Add("\u30c3\u30ad\u30e3", "kkya");
                katakanaToHepburn.Add("\u30c3\u30ad\u30e5", "kkyu");
                katakanaToHepburn.Add("\u30c3\u30ad\u30e7", "kkyo");
                katakanaToHepburn.Add("\u30c3\u30ae", "ggi");
                katakanaToHepburn.Add("\u30c3\u30ae\u30e3", "ggya");
                katakanaToHepburn.Add("\u30c3\u30ae\u30e5", "ggyu");
                katakanaToHepburn.Add("\u30c3\u30ae\u30e7", "ggyo");
                katakanaToHepburn.Add("\u30c3\u30af", "kku");
                katakanaToHepburn.Add("\u30c3\u30b0", "ggu");
                katakanaToHepburn.Add("\u30c3\u30b1", "kke");
                katakanaToHepburn.Add("\u30c3\u30b2", "gge");
                katakanaToHepburn.Add("\u30c3\u30b3", "kko");
                katakanaToHepburn.Add("\u30c3\u30b4", "ggo");
                katakanaToHepburn.Add("\u30c3\u30b5", "ssa");
                katakanaToHepburn.Add("\u30c3\u30b6", "zza");
                katakanaToHepburn.Add("\u30c3\u30b7", "sshi");
                katakanaToHepburn.Add("\u30c3\u30b7\u30e3", "ssha");
                katakanaToHepburn.Add("\u30c3\u30b7\u30e5", "sshu");
                katakanaToHepburn.Add("\u30c3\u30b7\u30e7", "ssho");
                katakanaToHepburn.Add("\u30c3\u30b8", "jji");
                katakanaToHepburn.Add("\u30c3\u30b8\u30e3", "jja");
                katakanaToHepburn.Add("\u30c3\u30b8\u30e5", "jju");
                katakanaToHepburn.Add("\u30c3\u30b8\u30e7", "jjo");
                katakanaToHepburn.Add("\u30c3\u30b9", "ssu");
                katakanaToHepburn.Add("\u30c3\u30ba", "zzu");
                katakanaToHepburn.Add("\u30c3\u30bb", "sse");
                katakanaToHepburn.Add("\u30c3\u30bc", "zze");
                katakanaToHepburn.Add("\u30c3\u30bd", "sso");
                katakanaToHepburn.Add("\u30c3\u30be", "zzo");
                katakanaToHepburn.Add("\u30c3\u30bf", "tta");
                katakanaToHepburn.Add("\u30c3\u30c0", "dda");
                katakanaToHepburn.Add("\u30c3\u30c1", "cchi");
                katakanaToHepburn.Add("\u30c3\u30c1\u30e3", "ccha");
                katakanaToHepburn.Add("\u30c3\u30c1\u30e5", "cchu");
                katakanaToHepburn.Add("\u30c3\u30c1\u30e7", "ccho");
                katakanaToHepburn.Add("\u30c3\u30c2", "ddi");
                katakanaToHepburn.Add("\u30c3\u30c2\u30e3", "ddya");
                katakanaToHepburn.Add("\u30c3\u30c2\u30e5", "ddyu");
                katakanaToHepburn.Add("\u30c3\u30c2\u30e7", "ddyo");
                katakanaToHepburn.Add("\u30c3\u30c4", "ttsu");
                katakanaToHepburn.Add("\u30c3\u30c5", "ddu");
                katakanaToHepburn.Add("\u30c3\u30c6", "tte");
                katakanaToHepburn.Add("\u30c3\u30c7", "dde");
                katakanaToHepburn.Add("\u30c3\u30c8", "tto");
                katakanaToHepburn.Add("\u30c3\u30c9", "ddo");
                katakanaToHepburn.Add("\u30c3\u30cf", "hha");
                katakanaToHepburn.Add("\u30c3\u30d0", "bba");
                katakanaToHepburn.Add("\u30c3\u30d1", "ppa");
                katakanaToHepburn.Add("\u30c3\u30d2", "hhi");
                katakanaToHepburn.Add("\u30c3\u30d2\u30e3", "hhya");
                katakanaToHepburn.Add("\u30c3\u30d2\u30e5", "hhyu");
                katakanaToHepburn.Add("\u30c3\u30d2\u30e7", "hhyo");
                katakanaToHepburn.Add("\u30c3\u30d3", "bbi");
                katakanaToHepburn.Add("\u30c3\u30d3\u30e3", "bbya");
                katakanaToHepburn.Add("\u30c3\u30d3\u30e5", "bbyu");
                katakanaToHepburn.Add("\u30c3\u30d3\u30e7", "bbyo");
                katakanaToHepburn.Add("\u30c3\u30d4", "ppi");
                katakanaToHepburn.Add("\u30c3\u30d4\u30e3", "ppya");
                katakanaToHepburn.Add("\u30c3\u30d4\u30e5", "ppyu");
                katakanaToHepburn.Add("\u30c3\u30d4\u30e7", "ppyo");
                katakanaToHepburn.Add("\u30c3\u30d5", "ffu");
                katakanaToHepburn.Add("\u30c3\u30d5\u30a1", "ffa");
                katakanaToHepburn.Add("\u30c3\u30d5\u30a3", "ffi");
                katakanaToHepburn.Add("\u30c3\u30d5\u30a7", "ffe");
                katakanaToHepburn.Add("\u30c3\u30d5\u30a9", "ffo");
                katakanaToHepburn.Add("\u30c3\u30d6", "bbu");
                katakanaToHepburn.Add("\u30c3\u30d7", "ppu");
                katakanaToHepburn.Add("\u30c3\u30d8", "hhe");
                katakanaToHepburn.Add("\u30c3\u30d9", "bbe");
                katakanaToHepburn.Add("\u30c3\u30da", "ppe");
                katakanaToHepburn.Add("\u30c3\u30db", "hho");
                katakanaToHepburn.Add("\u30c3\u30dc", "bbo");
                katakanaToHepburn.Add("\u30c3\u30dd", "ppo");
                katakanaToHepburn.Add("\u30c3\u30e4", "yya");
                katakanaToHepburn.Add("\u30c3\u30e6", "yyu");
                katakanaToHepburn.Add("\u30c3\u30e8", "yyo");
                katakanaToHepburn.Add("\u30c3\u30e9", "rra");
                katakanaToHepburn.Add("\u30c3\u30ea", "rri");
                katakanaToHepburn.Add("\u30c3\u30ea\u30e3", "rrya");
                katakanaToHepburn.Add("\u30c3\u30ea\u30e5", "rryu");
                katakanaToHepburn.Add("\u30c3\u30ea\u30e7", "rryo");
                katakanaToHepburn.Add("\u30c3\u30eb", "rru");
                katakanaToHepburn.Add("\u30c3\u30ec", "rre");
                katakanaToHepburn.Add("\u30c3\u30ed", "rro");
                katakanaToHepburn.Add("\u30c3\u30f4", "vvu");
                katakanaToHepburn.Add("\u30c3\u30f4\u30a1", "vva");
                katakanaToHepburn.Add("\u30c3\u30f4\u30a3", "vvi");
                katakanaToHepburn.Add("\u30c3\u30f4\u30a7", "vve");
                katakanaToHepburn.Add("\u30c3\u30f4\u30a9", "vvo");
                katakanaToHepburn.Add("\u30c4", "tsu");
                katakanaToHepburn.Add("\u30c5", "du");
                katakanaToHepburn.Add("\u30c6", "te");
                katakanaToHepburn.Add("\u30c7", "de");
                katakanaToHepburn.Add("\u30c8", "to");
                katakanaToHepburn.Add("\u30c9", "do");
                katakanaToHepburn.Add("\u30ca", "na");
                katakanaToHepburn.Add("\u30cb", "ni");
                katakanaToHepburn.Add("\u30cb\u30e3", "nya");
                katakanaToHepburn.Add("\u30cb\u30e5", "nyu");
                katakanaToHepburn.Add("\u30cb\u30e7", "nyo");
                katakanaToHepburn.Add("\u30cc", "nu");
                katakanaToHepburn.Add("\u30cd", "ne");
                katakanaToHepburn.Add("\u30ce", "no");
                katakanaToHepburn.Add("\u30cf", "ha");
                katakanaToHepburn.Add("\u30d0", "ba");
                katakanaToHepburn.Add("\u30d1", "pa");
                katakanaToHepburn.Add("\u30d2", "hi");
                katakanaToHepburn.Add("\u30d2\u30e3", "hya");
                katakanaToHepburn.Add("\u30d2\u30e5", "hyu");
                katakanaToHepburn.Add("\u30d2\u30e7", "hyo");
                katakanaToHepburn.Add("\u30d3", "bi");
                katakanaToHepburn.Add("\u30d3\u30e3", "bya");
                katakanaToHepburn.Add("\u30d3\u30e5", "byu");
                katakanaToHepburn.Add("\u30d3\u30e7", "byo");
                katakanaToHepburn.Add("\u30d4", "pi");
                katakanaToHepburn.Add("\u30d4\u30e3", "pya");
                katakanaToHepburn.Add("\u30d4\u30e5", "pyu");
                katakanaToHepburn.Add("\u30d4\u30e7", "pyo");
                katakanaToHepburn.Add("\u30d5", "fu");
                katakanaToHepburn.Add("\u30d5\u30a1", "fa");
                katakanaToHepburn.Add("\u30d5\u30a3", "fi");
                katakanaToHepburn.Add("\u30d5\u30a7", "fe");
                katakanaToHepburn.Add("\u30d5\u30a9", "fo");
                katakanaToHepburn.Add("\u30d6", "bu");
                katakanaToHepburn.Add("\u30d7", "pu");
                katakanaToHepburn.Add("\u30d8", "he");
                katakanaToHepburn.Add("\u30d9", "be");
                katakanaToHepburn.Add("\u30da", "pe");
                katakanaToHepburn.Add("\u30db", "ho");
                katakanaToHepburn.Add("\u30dc", "bo");
                katakanaToHepburn.Add("\u30dd", "po");
                katakanaToHepburn.Add("\u30de", "ma");
                katakanaToHepburn.Add("\u30df", "mi");
                katakanaToHepburn.Add("\u30df\u30e3", "mya");
                katakanaToHepburn.Add("\u30df\u30e5", "myu");
                katakanaToHepburn.Add("\u30df\u30e7", "myo");
                katakanaToHepburn.Add("\u30e0", "mu");
                katakanaToHepburn.Add("\u30e1", "me");
                katakanaToHepburn.Add("\u30e2", "mo");
                katakanaToHepburn.Add("\u30e3", "ya");
                katakanaToHepburn.Add("\u30e4", "ya");
                katakanaToHepburn.Add("\u30e5", "yu");
                katakanaToHepburn.Add("\u30e6", "yu");
                katakanaToHepburn.Add("\u30e7", "yo");
                katakanaToHepburn.Add("\u30e8", "yo");
                katakanaToHepburn.Add("\u30e9", "ra");
                katakanaToHepburn.Add("\u30ea", "ri");
                katakanaToHepburn.Add("\u30ea\u30e3", "rya");
                katakanaToHepburn.Add("\u30ea\u30e5", "ryu");
                katakanaToHepburn.Add("\u30ea\u30e7", "ryo");
                katakanaToHepburn.Add("\u30eb", "ru");
                katakanaToHepburn.Add("\u30ec", "re");
                katakanaToHepburn.Add("\u30ed", "ro");
                katakanaToHepburn.Add("\u30ee", "wa");
                katakanaToHepburn.Add("\u30ef", "wa");
                katakanaToHepburn.Add("\u30f0", "i");
                katakanaToHepburn.Add("\u30f1", "e");
                katakanaToHepburn.Add("\u30f2", "wo");
                katakanaToHepburn.Add("\u30f3", "n");
                katakanaToHepburn.Add("\u30f3\u30a2", "n'a");
                katakanaToHepburn.Add("\u30f3\u30a4", "n'i");
                katakanaToHepburn.Add("\u30f3\u30a6", "n'u");
                katakanaToHepburn.Add("\u30f3\u30a8", "n'e");
                katakanaToHepburn.Add("\u30f3\u30aa", "n'o");
                katakanaToHepburn.Add("\u30f4", "vu");
                katakanaToHepburn.Add("\u30f4\u30a1", "va");
                katakanaToHepburn.Add("\u30f4\u30a3", "vi");
                katakanaToHepburn.Add("\u30f4\u30a7", "ve");
                katakanaToHepburn.Add("\u30f4\u30a9", "vo");
                katakanaToHepburn.Add("\u30f5", "ka");
                katakanaToHepburn.Add("\u30f6", "ke");
                katakanaToHepburn.Add("\u30fc", "^");
            }
            return katakanaToHepburn;
        }

        static Table GetKatakanaToKunreiTable()
        {
            if (katakanaToKunrei == null) {
                katakanaToKunrei = new Table();
                katakanaToKunrei.Add("\u30a1", "a");
                katakanaToKunrei.Add("\u30a2", "a");
                katakanaToKunrei.Add("\u30a3", "i");
                katakanaToKunrei.Add("\u30a4", "i");
                katakanaToKunrei.Add("\u30a5", "u");
                katakanaToKunrei.Add("\u30a6", "u");
                katakanaToKunrei.Add("\u30a7", "e");
                katakanaToKunrei.Add("\u30a8", "e");
                katakanaToKunrei.Add("\u30a9", "o");
                katakanaToKunrei.Add("\u30aa", "o");
                katakanaToKunrei.Add("\u30ab", "ka");
                katakanaToKunrei.Add("\u30ac", "ga");
                katakanaToKunrei.Add("\u30ad", "ki");
                katakanaToKunrei.Add("\u30ad\u30e3", "kya");
                katakanaToKunrei.Add("\u30ad\u30e5", "kyu");
                katakanaToKunrei.Add("\u30ad\u30e7", "kyo");
                katakanaToKunrei.Add("\u30ae", "gi");
                katakanaToKunrei.Add("\u30ae\u30e3", "gya");
                katakanaToKunrei.Add("\u30ae\u30e5", "gyu");
                katakanaToKunrei.Add("\u30ae\u30e7", "gyo");
                katakanaToKunrei.Add("\u30af", "ku");
                katakanaToKunrei.Add("\u30b0", "gu");
                katakanaToKunrei.Add("\u30b1", "ke");
                katakanaToKunrei.Add("\u30b2", "ge");
                katakanaToKunrei.Add("\u30b3", "ko");
                katakanaToKunrei.Add("\u30b4", "go");
                katakanaToKunrei.Add("\u30b5", "sa");
                katakanaToKunrei.Add("\u30b6", "za");
                katakanaToKunrei.Add("\u30b7", "si");
                katakanaToKunrei.Add("\u30b7\u30e3", "sya");
                katakanaToKunrei.Add("\u30b7\u30e5", "syu");
                katakanaToKunrei.Add("\u30b7\u30e7", "syo");
                katakanaToKunrei.Add("\u30b8", "zi");
                katakanaToKunrei.Add("\u30b8\u30e3", "zya");
                katakanaToKunrei.Add("\u30b8\u30e5", "zyu");
                katakanaToKunrei.Add("\u30b8\u30e7", "zyo");
                katakanaToKunrei.Add("\u30b9", "su");
                katakanaToKunrei.Add("\u30ba", "zu");
                katakanaToKunrei.Add("\u30bb", "se");
                katakanaToKunrei.Add("\u30bc", "ze");
                katakanaToKunrei.Add("\u30bd", "so");
                katakanaToKunrei.Add("\u30be", "zo");
                katakanaToKunrei.Add("\u30bf", "ta");
                katakanaToKunrei.Add("\u30c0", "da");
                katakanaToKunrei.Add("\u30c1", "ti");
                katakanaToKunrei.Add("\u30c1\u30e3", "tya");
                katakanaToKunrei.Add("\u30c1\u30e5", "tyu");
                katakanaToKunrei.Add("\u30c1\u30e7", "tyo");
                katakanaToKunrei.Add("\u30c2", "di");
                katakanaToKunrei.Add("\u30c2\u30e3", "dya");
                katakanaToKunrei.Add("\u30c2\u30e5", "dyu");
                katakanaToKunrei.Add("\u30c2\u30e7", "dyo");
                katakanaToKunrei.Add("\u30c3", "tu");
                katakanaToKunrei.Add("\u30c3\u30ab", "kka");
                katakanaToKunrei.Add("\u30c3\u30ac", "gga");
                katakanaToKunrei.Add("\u30c3\u30ad", "kki");
                katakanaToKunrei.Add("\u30c3\u30ad\u30e3", "kkya");
                katakanaToKunrei.Add("\u30c3\u30ad\u30e5", "kkyu");
                katakanaToKunrei.Add("\u30c3\u30ad\u30e7", "kkyo");
                katakanaToKunrei.Add("\u30c3\u30ae", "ggi");
                katakanaToKunrei.Add("\u30c3\u30ae\u30e3", "ggya");
                katakanaToKunrei.Add("\u30c3\u30ae\u30e5", "ggyu");
                katakanaToKunrei.Add("\u30c3\u30ae\u30e7", "ggyo");
                katakanaToKunrei.Add("\u30c3\u30af", "kku");
                katakanaToKunrei.Add("\u30c3\u30b0", "ggu");
                katakanaToKunrei.Add("\u30c3\u30b1", "kke");
                katakanaToKunrei.Add("\u30c3\u30b2", "gge");
                katakanaToKunrei.Add("\u30c3\u30b3", "kko");
                katakanaToKunrei.Add("\u30c3\u30b4", "ggo");
                katakanaToKunrei.Add("\u30c3\u30b5", "ssa");
                katakanaToKunrei.Add("\u30c3\u30b6", "zza");
                katakanaToKunrei.Add("\u30c3\u30b7", "ssi");
                katakanaToKunrei.Add("\u30c3\u30b7\u30e3", "ssya");
                katakanaToKunrei.Add("\u30c3\u30b7\u30e5", "ssyu");
                katakanaToKunrei.Add("\u30c3\u30b7\u30e7", "ssho");
                katakanaToKunrei.Add("\u30c3\u30b8", "zzi");
                katakanaToKunrei.Add("\u30c3\u30b8\u30e3", "zzya");
                katakanaToKunrei.Add("\u30c3\u30b8\u30e5", "zzyu");
                katakanaToKunrei.Add("\u30c3\u30b8\u30e7", "zzyo");
                katakanaToKunrei.Add("\u30c3\u30b9", "ssu");
                katakanaToKunrei.Add("\u30c3\u30ba", "zzu");
                katakanaToKunrei.Add("\u30c3\u30bb", "sse");
                katakanaToKunrei.Add("\u30c3\u30bc", "zze");
                katakanaToKunrei.Add("\u30c3\u30bd", "sso");
                katakanaToKunrei.Add("\u30c3\u30be", "zzo");
                katakanaToKunrei.Add("\u30c3\u30bf", "tta");
                katakanaToKunrei.Add("\u30c3\u30c0", "dda");
                katakanaToKunrei.Add("\u30c3\u30c1", "tti");
                katakanaToKunrei.Add("\u30c3\u30c1\u30e3", "ttya");
                katakanaToKunrei.Add("\u30c3\u30c1\u30e5", "ttyu");
                katakanaToKunrei.Add("\u30c3\u30c1\u30e7", "ttyo");
                katakanaToKunrei.Add("\u30c3\u30c2", "ddi");
                katakanaToKunrei.Add("\u30c3\u30c2\u30e3", "ddya");
                katakanaToKunrei.Add("\u30c3\u30c2\u30e5", "ddyu");
                katakanaToKunrei.Add("\u30c3\u30c2\u30e7", "ddyo");
                katakanaToKunrei.Add("\u30c3\u30c4", "ttu");
                katakanaToKunrei.Add("\u30c3\u30c5", "ddu");
                katakanaToKunrei.Add("\u30c3\u30c6", "tte");
                katakanaToKunrei.Add("\u30c3\u30c7", "dde");
                katakanaToKunrei.Add("\u30c3\u30c8", "tto");
                katakanaToKunrei.Add("\u30c3\u30c9", "ddo");
                katakanaToKunrei.Add("\u30c3\u30cf", "hha");
                katakanaToKunrei.Add("\u30c3\u30d0", "bba");
                katakanaToKunrei.Add("\u30c3\u30d1", "ppa");
                katakanaToKunrei.Add("\u30c3\u30d2", "hhi");
                katakanaToKunrei.Add("\u30c3\u30d2\u30e3", "hhya");
                katakanaToKunrei.Add("\u30c3\u30d2\u30e5", "hhyu");
                katakanaToKunrei.Add("\u30c3\u30d2\u30e7", "hhyo");
                katakanaToKunrei.Add("\u30c3\u30d3", "bbi");
                katakanaToKunrei.Add("\u30c3\u30d3\u30e3", "bbya");
                katakanaToKunrei.Add("\u30c3\u30d3\u30e5", "bbyu");
                katakanaToKunrei.Add("\u30c3\u30d3\u30e7", "bbyo");
                katakanaToKunrei.Add("\u30c3\u30d4", "ppi");
                katakanaToKunrei.Add("\u30c3\u30d4\u30e3", "ppya");
                katakanaToKunrei.Add("\u30c3\u30d4\u30e5", "ppyu");
                katakanaToKunrei.Add("\u30c3\u30d4\u30e7", "ppyo");
                katakanaToKunrei.Add("\u30c3\u30d5", "hhu");
                katakanaToKunrei.Add("\u30c3\u30d5\u30a1", "ffa");
                katakanaToKunrei.Add("\u30c3\u30d5\u30a3", "ffi");
                katakanaToKunrei.Add("\u30c3\u30d5\u30a7", "ffe");
                katakanaToKunrei.Add("\u30c3\u30d5\u30a9", "ffo");
                katakanaToKunrei.Add("\u30c3\u30d6", "bbu");
                katakanaToKunrei.Add("\u30c3\u30d7", "ppu");
                katakanaToKunrei.Add("\u30c3\u30d8", "hhe");
                katakanaToKunrei.Add("\u30c3\u30d9", "bbe");
                katakanaToKunrei.Add("\u30c3\u30da", "ppe");
                katakanaToKunrei.Add("\u30c3\u30db", "hho");
                katakanaToKunrei.Add("\u30c3\u30dc", "bbo");
                katakanaToKunrei.Add("\u30c3\u30dd", "ppo");
                katakanaToKunrei.Add("\u30c3\u30e4", "yya");
                katakanaToKunrei.Add("\u30c3\u30e6", "yyu");
                katakanaToKunrei.Add("\u30c3\u30e8", "yyo");
                katakanaToKunrei.Add("\u30c3\u30e9", "rra");
                katakanaToKunrei.Add("\u30c3\u30ea", "rri");
                katakanaToKunrei.Add("\u30c3\u30ea\u30e3", "rrya");
                katakanaToKunrei.Add("\u30c3\u30ea\u30e5", "rryu");
                katakanaToKunrei.Add("\u30c3\u30ea\u30e7", "rryo");
                katakanaToKunrei.Add("\u30c3\u30eb", "rru");
                katakanaToKunrei.Add("\u30c3\u30ec", "rre");
                katakanaToKunrei.Add("\u30c3\u30ed", "rro");
                katakanaToKunrei.Add("\u30c3\u30f4", "vvu");
                katakanaToKunrei.Add("\u30c3\u30f4\u30a1", "vva");
                katakanaToKunrei.Add("\u30c3\u30f4\u30a3", "vvi");
                katakanaToKunrei.Add("\u30c3\u30f4\u30a7", "vve");
                katakanaToKunrei.Add("\u30c3\u30f4\u30a9", "vvo");
                katakanaToKunrei.Add("\u30c4", "tu");
                katakanaToKunrei.Add("\u30c5", "du");
                katakanaToKunrei.Add("\u30c6", "te");
                katakanaToKunrei.Add("\u30c7", "de");
                katakanaToKunrei.Add("\u30c8", "to");
                katakanaToKunrei.Add("\u30c9", "do");
                katakanaToKunrei.Add("\u30ca", "na");
                katakanaToKunrei.Add("\u30cb", "ni");
                katakanaToKunrei.Add("\u30cb\u30e3", "nya");
                katakanaToKunrei.Add("\u30cb\u30e5", "nyu");
                katakanaToKunrei.Add("\u30cb\u30e7", "nyo");
                katakanaToKunrei.Add("\u30cc", "nu");
                katakanaToKunrei.Add("\u30cd", "ne");
                katakanaToKunrei.Add("\u30ce", "no");
                katakanaToKunrei.Add("\u30cf", "ha");
                katakanaToKunrei.Add("\u30d0", "ba");
                katakanaToKunrei.Add("\u30d1", "pa");
                katakanaToKunrei.Add("\u30d2", "hi");
                katakanaToKunrei.Add("\u30d2\u30e3", "hya");
                katakanaToKunrei.Add("\u30d2\u30e5", "hyu");
                katakanaToKunrei.Add("\u30d2\u30e7", "hyo");
                katakanaToKunrei.Add("\u30d3", "bi");
                katakanaToKunrei.Add("\u30d3\u30e3", "bya");
                katakanaToKunrei.Add("\u30d3\u30e5", "byu");
                katakanaToKunrei.Add("\u30d3\u30e7", "byo");
                katakanaToKunrei.Add("\u30d4", "pi");
                katakanaToKunrei.Add("\u30d4\u30e3", "pya");
                katakanaToKunrei.Add("\u30d4\u30e5", "pyu");
                katakanaToKunrei.Add("\u30d4\u30e7", "pyo");
                katakanaToKunrei.Add("\u30d5", "hu");
                katakanaToKunrei.Add("\u30d5\u30a1", "fa");
                katakanaToKunrei.Add("\u30d5\u30a3", "fi");
                katakanaToKunrei.Add("\u30d5\u30a7", "fe");
                katakanaToKunrei.Add("\u30d5\u30a9", "fo");
                katakanaToKunrei.Add("\u30d6", "bu");
                katakanaToKunrei.Add("\u30d7", "pu");
                katakanaToKunrei.Add("\u30d8", "he");
                katakanaToKunrei.Add("\u30d9", "be");
                katakanaToKunrei.Add("\u30da", "pe");
                katakanaToKunrei.Add("\u30db", "ho");
                katakanaToKunrei.Add("\u30dc", "bo");
                katakanaToKunrei.Add("\u30dd", "po");
                katakanaToKunrei.Add("\u30de", "ma");
                katakanaToKunrei.Add("\u30df", "mi");
                katakanaToKunrei.Add("\u30df\u30e3", "mya");
                katakanaToKunrei.Add("\u30df\u30e5", "myu");
                katakanaToKunrei.Add("\u30df\u30e7", "myo");
                katakanaToKunrei.Add("\u30e0", "mu");
                katakanaToKunrei.Add("\u30e1", "me");
                katakanaToKunrei.Add("\u30e2", "mo");
                katakanaToKunrei.Add("\u30e3", "ya");
                katakanaToKunrei.Add("\u30e4", "ya");
                katakanaToKunrei.Add("\u30e5", "yu");
                katakanaToKunrei.Add("\u30e6", "yu");
                katakanaToKunrei.Add("\u30e7", "yo");
                katakanaToKunrei.Add("\u30e8", "yo");
                katakanaToKunrei.Add("\u30e9", "ra");
                katakanaToKunrei.Add("\u30ea", "ri");
                katakanaToKunrei.Add("\u30ea\u30e3", "rya");
                katakanaToKunrei.Add("\u30ea\u30e5", "ryu");
                katakanaToKunrei.Add("\u30ea\u30e7", "ryo");
                katakanaToKunrei.Add("\u30eb", "ru");
                katakanaToKunrei.Add("\u30ec", "re");
                katakanaToKunrei.Add("\u30ed", "ro");
                katakanaToKunrei.Add("\u30ee", "wa");
                katakanaToKunrei.Add("\u30ef", "wa");
                katakanaToKunrei.Add("\u30f0", "i");
                katakanaToKunrei.Add("\u30f1", "e");
                katakanaToKunrei.Add("\u30f2", "wo");
                katakanaToKunrei.Add("\u30f3", "n");
                katakanaToKunrei.Add("\u30f3\u30a2", "n'a");
                katakanaToKunrei.Add("\u30f3\u30a4", "n'i");
                katakanaToKunrei.Add("\u30f3\u30a6", "n'u");
                katakanaToKunrei.Add("\u30f3\u30a8", "n'e");
                katakanaToKunrei.Add("\u30f3\u30aa", "n'o");
                katakanaToKunrei.Add("\u30f4", "vu");
                katakanaToKunrei.Add("\u30f4\u30a1", "va");
                katakanaToKunrei.Add("\u30f4\u30a3", "vi");
                katakanaToKunrei.Add("\u30f4\u30a7", "ve");
                katakanaToKunrei.Add("\u30f4\u30a9", "vo");
                katakanaToKunrei.Add("\u30f5", "ka");
                katakanaToKunrei.Add("\u30f6", "ke");
                katakanaToKunrei.Add("\u30fc", "^");
            }
            return katakanaToKunrei;
        }

        private RomanConversionType romanConversionType;
        private bool capitalizeMode;
        private bool upperCaseMode;

        private Table hiraganaTable;
        private Table katakanaTable;

        internal RomanConversionType RomanConversionType
        {
            get { return romanConversionType; }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                romanConversionType = value;
                hiraganaTable = null;
                katakanaTable = null;
            }
        }

        internal bool CapitalizeMode
        {
            get { return capitalizeMode; }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                capitalizeMode = value;
                hiraganaTable = null;
                katakanaTable = null;
            }
        }

        internal bool UpperCaseMode
        {
            get { return upperCaseMode; }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set
            {
                upperCaseMode = value;
                hiraganaTable = null;
                katakanaTable = null;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal bool ConvertHiragana(KakasiReader input, TextWriter output)
        {
            if (hiraganaTable == null) {
                hiraganaTable = romanConversionType == RomanConversionType.KUNREI ?
                    getHiraganaToKunreiTable() : GetHiraganaToHepburnTable();
            }
            return Convert(input, output, hiraganaTable);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal bool ConvertKatakana(KakasiReader input, TextWriter output)
        {
            if (katakanaTable == null) {
                katakanaTable = romanConversionType == RomanConversionType.KUNREI ?
                    GetKatakanaToKunreiTable() : GetKatakanaToHepburnTable();
            }
            return Convert(input, output, katakanaTable);
        }

        private bool Convert(KakasiReader input, TextWriter output, Table table)
        {
            string romaji = table.Get(input);
            if (romaji == null) {
                return false;
            }
            if (capitalizeMode) {
                output.Write(char.ToUpper(romaji[0]));
                romaji = romaji.Substring(1);
            }
            if (upperCaseMode) {
                romaji = romaji.ToUpper();
            }
            output.Write(romaji);
            while (true) {
                romaji = table.Get(input);
                if (romaji == null) {
                    break;
                }
                if (upperCaseMode) {
                    romaji = romaji.ToUpper();
                }
                output.Write(romaji);
            }
            return true;
        }
    }

    public enum RomanConversionType
    {
        HEPBURN,
        KUNREI
    }
}
