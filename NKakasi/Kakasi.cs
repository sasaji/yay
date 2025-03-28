using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace NKakasi
{
    public class Kakasi
    {
        private enum CharacterSet
        {
            ASCII,
            KANJI,
            HIRAGANA,
            KATAKANA,
            HALFKANA,
            UNKNOWN
        }

        private readonly IConverter defaultConverter = new DefaultConverter();
        private IConverter kanjiConverter;
        private IConverter hiraganaConverter;
        private IConverter katakanaConverter;
        private IConverter halfKanaConverter;

        private readonly KanjiConverter kanjiConverterImpl;
        private readonly HiraganaConverter hiraganaConverterImpl;
        private readonly KatakanaConverter katakanaConverterImpl;
        private readonly KanaToRomanConverter kanaToRomanConverterImpl;

        private readonly KakasiReader input = new KakasiReader();
        private readonly KakasiWriter output = new KakasiWriter();
        private readonly KanwaDictionary kanwaDictionary;

        private bool wakachigakiMode;

        public Kakasi()
        {
            kanwaDictionary = new KanwaDictionary();
            kanjiConverterImpl = new KanjiConverter(kanwaDictionary);
            hiraganaConverterImpl = new HiraganaConverter();
            katakanaConverterImpl = new KatakanaConverter();
            kanaToRomanConverterImpl = new KanaToRomanConverter();
        }

        public bool SetArguments(string[] args)
        {
            int index = 0;
            for (; index < args.Length; index++)
            {
                if (args[index][0] != '-')
                {
                    break;
                }
                int length = args[index].Length;
                if (length < 2)
                {
                    return false;
                }
                CharacterSet characterSet = CharacterSet.UNKNOWN;
                Encoding encoding = null;
                string typeString = null;
                switch (args[index][1])
                {
                    case 'J':
                        if (length < 3)
                        {
                            return false;
                        }
                        switch (args[index][2])
                        {
                            case 'H':
                                characterSet = CharacterSet.HIRAGANA;
                                break;
                            case 'K':
                                characterSet = CharacterSet.KATAKANA;
                                break;
                            case 'k':
                                characterSet = CharacterSet.HALFKANA;
                                break;
                            case 'a':
                                characterSet = CharacterSet.ASCII;
                                break;
                            default:
                                return false;
                        }
                        SetupKanjiConverter(characterSet);
                        break;
                    case 'H':
                        if (length < 3)
                        {
                            return false;
                        }
                        switch (args[index][2])
                        {
                            case 'K':
                                characterSet = CharacterSet.KATAKANA;
                                break;
                            case 'a':
                                characterSet = CharacterSet.ASCII;
                                break;
                            default:
                                return false;
                        }
                        SetupHiraganaConverter(characterSet);
                        break;
                    case 'K':
                        if (length < 3)
                        {
                            return false;
                        }
                        switch (args[index][2])
                        {
                            case 'H':
                                characterSet = CharacterSet.HIRAGANA;
                                break;
                            case 'a':
                                characterSet = CharacterSet.ASCII;
                                break;
                            default:
                                return false;
                        }
                        SetupKatakanaConverter(characterSet);
                        break;
                    case 'i':
                        if (length > 2)
                        {
                            encoding = Encoding.GetEncoding(args[index].Substring(2));
                        }
                        else if (++index < args.Length)
                        {
                            encoding = Encoding.GetEncoding(args[index]);
                        }
                        else
                        {
                            return false;
                        }
                        input.SetReader(new StreamReader(Console.OpenStandardInput(), encoding));
                        break;
                    case 'o':
                        if (length > 2)
                        {
                            encoding = Encoding.GetEncoding(args[index].Substring(2));
                        }
                        else if (++index < args.Length)
                        {
                            encoding = Encoding.GetEncoding(args[index]);
                        }
                        else
                        {
                            return false;
                        }
                        output.SetWriter(new StreamWriter(Console.OpenStandardOutput(), encoding));
                        break;
                    case 'p':
                        HeikiMode = true;
                        break;
                    case 'f':
                        FuriganaMode = true;
                        break;
                    case 'c':
                        input.SpaceEatMode = true;
                        break;
                    case 's':
                        output.SplitMode = true;
                        break;
                    case 'b':
                        output.AutoFlushMode = false;
                        break;
                    case 'r':
                        if (length > 2)
                        {
                            typeString = args[index].Substring(2);
                        }
                        else if (++index < args.Length)
                        {
                            typeString = args[index];
                        }
                        else
                        {
                            return false;
                        }
                        if ("hepburn".Equals(typeString, StringComparison.CurrentCultureIgnoreCase))
                        {
                            RomajiType = RomanConversionType.HEPBURN;
                        }
                        else if ("kunrei".Equals(typeString, StringComparison.CurrentCultureIgnoreCase))
                        {
                            RomajiType = RomanConversionType.KUNREI;
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case 'C':
                        RomajiCapitalizeMode = true;
                        break;
                    case 'U':
                        RomajiUpperCaseMode = true;
                        break;
                    case 'w':
                        WakachigakiMode = true;
                        break;
                    default:
                        return false;
                }
            }
            for (; index < args.Length; index++)
            {
                kanwaDictionary.Load(args[index]);
            }
            return true;
        }

        private void SetupKanjiConverter(CharacterSet characterSet)
        {
            kanjiConverter = characterSet == CharacterSet.UNKNOWN ? null : CreateKanjiConverter(characterSet);
        }

        private void SetupHiraganaConverter(CharacterSet characterSet)
        {
            hiraganaConverter = characterSet == CharacterSet.UNKNOWN ? null : CreateHiraganaConverter(characterSet);
        }

        private void SetupKatakanaConverter(CharacterSet characterSet)
        {
            katakanaConverter = characterSet == CharacterSet.UNKNOWN ? null : CreateKatakanaConverter(characterSet);
        }

        private IConverter CreateKanjiConverter(CharacterSet characterSet)
        {
            if (characterSet.Equals(CharacterSet.ASCII))
            {
                return new CompoundConverter(CreateKanjiConverter(CharacterSet.HIRAGANA), CreateHiraganaConverter(CharacterSet.ASCII));
            }
            else if (characterSet.Equals(CharacterSet.KANJI))
            {
                return new DelegateConverter(kanjiConverterImpl.ToKanji);
            }
            else if (characterSet.Equals(CharacterSet.HIRAGANA))
            {
                return new DelegateConverter(kanjiConverterImpl.ToHiragana);
            }
            else if (characterSet.Equals(CharacterSet.KATAKANA))
            {
                return new CompoundConverter(CreateKanjiConverter(CharacterSet.HIRAGANA), CreateHiraganaConverter(CharacterSet.KATAKANA));
            }
            else if (characterSet.Equals(CharacterSet.HALFKANA))
            {
                return new CompoundConverter(CreateKanjiConverter(CharacterSet.HIRAGANA), CreateHiraganaConverter(CharacterSet.HALFKANA));
            }
            else
            {
                return null;
            }
        }

        private IConverter CreateHiraganaConverter(CharacterSet characterSet)
        {
            if (characterSet.Equals(CharacterSet.ASCII))
            {
                return new DelegateConverter(kanaToRomanConverterImpl.ConvertHiragana);
            }
            else if (characterSet.Equals(CharacterSet.HIRAGANA))
            {
                return new DelegateConverter(HiraganaConverter.ToHiragana);
            }
            else if (characterSet.Equals(CharacterSet.KATAKANA))
            {
                return new DelegateConverter(hiraganaConverterImpl.ToKatakana);
            }
            else if (characterSet.Equals(CharacterSet.HALFKANA))
            {
                return new DelegateConverter(hiraganaConverterImpl.ToHalfKana);
            }
            else
            {
                return null;
            }
        }

        private IConverter CreateKatakanaConverter(CharacterSet characterSet)
        {
            if (characterSet.Equals(CharacterSet.ASCII))
            {
                return new DelegateConverter(kanaToRomanConverterImpl.ConvertKatakana);
            }
            else if (characterSet.Equals(CharacterSet.HIRAGANA))
            {
                return new DelegateConverter(katakanaConverterImpl.ToHiragana);
            }
            else if (characterSet.Equals(CharacterSet.KATAKANA))
            {
                return new DelegateConverter(katakanaConverterImpl.ToKatakana);
            }
            else
            {
                return null;
            }
        }

        public bool HeikiMode
        {
            get { return kanjiConverterImpl.HeikiMode; }
            set { kanjiConverterImpl.HeikiMode = value; }
        }

        public bool FuriganaMode
        {
            get { return kanjiConverterImpl.FuriganaMode; }
            set { kanjiConverterImpl.FuriganaMode = value; }
        }

        public bool WakachigakiMode
        {
            get { return wakachigakiMode; }
            set
            {
                if (value)
                {
                    output.SplitMode = true;
                    SetupKanjiConverter(CharacterSet.KANJI);
                    SetupHiraganaConverter(CharacterSet.HIRAGANA);
                    SetupKatakanaConverter(CharacterSet.KATAKANA);
                }
                else
                {
                    output.SplitMode = false;
                    SetupKanjiConverter(CharacterSet.UNKNOWN);
                    SetupHiraganaConverter(CharacterSet.UNKNOWN);
                    SetupKatakanaConverter(CharacterSet.UNKNOWN);
                }
                wakachigakiMode = value;
            }
        }

        public RomanConversionType RomajiType
        {
            get { return kanaToRomanConverterImpl.RomanConversionType; }
            set { kanaToRomanConverterImpl.RomanConversionType = value; }
        }

        public bool RomajiCapitalizeMode
        {
            get { return kanaToRomanConverterImpl.CapitalizeMode; }
            set { kanaToRomanConverterImpl.CapitalizeMode = value; }
        }

        public bool RomajiUpperCaseMode
        {
            get { return kanaToRomanConverterImpl.UpperCaseMode; }
            set { kanaToRomanConverterImpl.UpperCaseMode = value; }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Do()
        {
            while (true)
            {
                int ch = input.Get();
                if (ch < 0)
                {
                    break;
                }
                IConverter converter = null;
                UnicodeBlock block = UnicodeBlock.Of((char)ch);
                if (block.Equals(UnicodeBlock.CJKUnifiedIdeographs))
                {
                    converter = kanjiConverter;
                }
                else if (block.Equals(UnicodeBlock.Hiragana))
                {
                    converter = hiraganaConverter;
                }
                else if (block.Equals(UnicodeBlock.Katakana))
                {
                    converter = katakanaConverter;
                }
                else if (block.Equals(UnicodeBlock.HalfKana))
                {
                    converter = halfKanaConverter;
                }
                if (converter == null)
                {
                    converter = defaultConverter;
                }
                output.PutSeparator();
                if (!converter.Convert(input, output))
                {
                    input.Consume(1);
                    if (wakachigakiMode)
                    {
                        output.Write((char)ch);
                    }
                }
            }
            output.Flush();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public string DoString(string source)
        {
            input.SetReader(new StringReader(source));
            var builder = new StringBuilder();
            output.SetWriter(new StringWriter(builder));
            Do();
            input.SetReader(null);
            output.SetWriter(null);
            return builder.ToString();
        }
    }
}
