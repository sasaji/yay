using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace NKakasi
{
    public class KanwaDictionary
    {
        private readonly Dictionary<char, SortedSet<KanjiYomi>> contentsTable = new Dictionary<char, SortedSet<KanjiYomi>>(8192);
        private Dictionary<char, KanwaEntry> entryTable;
        private SortedSet<char> loadedKeys;
        private RandomAccessFile file;

        public void Load(string filename)
        {
            Load(filename, Encoding.Default);
        }

        public void Load(string filename, Encoding encoding)
        {
            FileStream inp = new FileStream(filename, FileMode.Open, FileAccess.Read);
            try
            {
                StreamReader reader = new StreamReader(inp, encoding);
                try
                {
                    Load(reader);
                }
                finally
                {
                    try
                    {
                        reader.Close();
                        inp = null;
                    }
                    catch (IOException exception)
                    {
                        Console.Error.WriteLine(exception.StackTrace);
                    }
                }
            }
            finally
            {
                if (inp != null)
                {
                    try
                    {
                        inp.Close();
                    }
                    catch (IOException exception)
                    {
                        Console.Error.WriteLine(exception.StackTrace);
                    }
                }
            }
        }

        public void Load(StreamReader reader)
        {
            while (true)
            {
                string line = reader.ReadLine();
                if (line == null)
                {
                    break;
                }
                int length = line.Length;
                if (length == 0)
                {
                    continue;
                }
                UnicodeBlock yomiBlock = UnicodeBlock.Of(line[0]);
                if (!yomiBlock.Equals(UnicodeBlock.Hiragana) && !yomiBlock.Equals(UnicodeBlock.Katakana))
                {
                    continue;
                }
                StringBuilder yomiBuffer = new StringBuilder();
                yomiBuffer.Append(line[0]);
                int index = 1;
                for (; index < length; index++)
                {
                    char ch = line[index];
                    if (" ,\t".IndexOf(ch) >= 0)
                    {
                        break;
                    }
                    yomiBuffer.Append(ch);
                }
                if (index >= length)
                {
                    Console.Error.WriteLine("KanwaDictionary: Ignored line: " + line);
                    continue;
                }
                char okurigana = '0';
                char yomiLast = yomiBuffer[index - 1];
                if (yomiLast >= 'a' && yomiLast <= 'z')
                {
                    okurigana = yomiLast;
                    yomiBuffer.Length = index - 1;
                }
                string yomi = yomiBuffer.ToString();
                for (++index; index < length; index++)
                {
                    char ch = line[index];
                    if (" ,\t".IndexOf(ch) < 0)
                    {
                        break;
                    }
                }
                if (index >= length)
                {
                    Console.Error.WriteLine("KanwaDictionary: Ignored line: " + line);
                    continue;
                }
                if (line[index] == '/')
                {
                SKK_LOOP:
                    while (true)
                    {
                        StringBuilder kanji = new StringBuilder();
                        for (++index; index < length; index++)
                        {
                            char ch = line[index];
                            if (ch == '/')
                            {
                                break;
                            }
                            if (ch == ';')
                            {
                                index = length - 1;
                                break;
                            }
                            if (ch == '[')
                            {
                                goto SKK_LOOP;
                            }
                            kanji.Append(ch);
                        }
                        if (index >= length)
                        {
                            break;
                        }
                        AddItem(kanji.ToString(), yomi, okurigana);
                    }
                }
                else
                {
                    StringBuilder kanji = new StringBuilder();
                    kanji.Append(line[index]);
                    for (++index; index < length; index++)
                    {
                        char ch = line[index];
                        if (" ,\t".IndexOf(ch) >= 0)
                        {
                            break;
                        }
                        kanji.Append(ch);
                    }
                    AddItem(kanji.ToString(), yomi, okurigana);
                }
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddItem(string kanji, string yomi, char okurigana)
        {
            UnicodeBlock kanjiBlock = UnicodeBlock.Of(kanji[0]);
            if (!kanjiBlock.Equals(UnicodeBlock.CJKUnifiedIdeographs))
            {
                //System.err.println("KanwaDictionary: Ignored item:" +
                //                   " kanji=" + kanji + " yomi=" + yomi);
                return;
            }
            int kanjiLength = kanji.Length;
            StringBuilder kanjiBuffer = new StringBuilder(kanjiLength);
            for (int index = 0; index < kanjiLength; index++)
            {
                char ch = kanji[index];
                //if (ch < '\u0100') {
                //    System.err.println("KanwaDictionary: Ignored item:" +
                //                       " kanji=" + kanji + " yomi=" + yomi);
                //    return;
                //}
                kanjiBuffer.Append(ItaijiDictionary.GetInstance().Get(ch));
            }
            char key = kanjiBuffer[0];
            kanji = kanjiBuffer.ToString().Substring(1);

            int yomiLength = yomi.Length;
            StringBuilder yomiBuffer = new StringBuilder(yomiLength);
            for (int index = 0; index < yomiLength; index++)
            {
                char ch = yomi[index];
                UnicodeBlock block = UnicodeBlock.Of(ch);
                if (!block.Equals(UnicodeBlock.Hiragana) && !block.Equals(UnicodeBlock.Katakana))
                {
                    Console.Error.WriteLine("KanwaDictionary: Ignored item:" +
                                       " kanji=" + kanjiBuffer + " yomi=" + yomi);
                    return;
                }
                if ((ch >= '\u30a1' && ch <= '\u30f3') ||
                    ch == '\u30fd' || ch == '\u30fe')
                {
                    yomiBuffer.Append((char)(ch - 0x60));
                }
                else if (ch == '\u30f4')
                {    // 'vu'
                    yomiBuffer.Append('\u3046');
                    yomiBuffer.Append('\u309b');
                }
                else
                {
                    yomiBuffer.Append(ch);
                }
            }
            yomi = yomiBuffer.ToString();

            KanjiYomi kanjiYomi = new KanjiYomi(kanji, yomi, okurigana);
            if (!contentsTable.ContainsKey(key))
            {
                contentsTable.Add(key, new SortedSet<KanjiYomi>());
            }
            SortedSet<KanjiYomi> list = contentsTable[key];
            list.Add(kanjiYomi);
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal SortedSet<KanjiYomi>.Enumerator Lookup(char k)
        {
            if (entryTable == null)
            {
                Initialize();
            }
            char key = k;

            if (!contentsTable.ContainsKey(key))
            {
                contentsTable.Add(key, new SortedSet<KanjiYomi>());
            }
            SortedSet<KanjiYomi> list = contentsTable[key];
            if (file != null && !loadedKeys.Contains(key))
            {
                if (entryTable.ContainsKey(key))
                {
                    KanwaEntry entry = entryTable[key];
                    if (entry != null)
                    {
                        file.Seek(entry.GetOffset());
                        int numWords = entry.GetNumberOfWords();
                        for (int index = 0; index < numWords; index++)
                        {
                            string kanji = file.ReadUTF();
                            string yomi = file.ReadUTF();
                            char okurigana = (char)file.ReadByte();
                            list.Add(new KanjiYomi(kanji, yomi, okurigana));
                        }
                    }
                }
                loadedKeys.Add(key);
            }
            return list.GetEnumerator();
        }

        private void Initialize()
        {
            string path = Path.Combine(new string[] { Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Services", "Romanizer", "Kakasi", "kanwadict" });
            if (!File.Exists(path))
                path = Environment.GetEnvironmentVariable("KANWADICTPATH");
            file = new RandomAccessFile(path, "r");
            int numKanji = file.ReadInt();
            entryTable = new Dictionary<char, KanwaEntry>(numKanji);
            loadedKeys = new SortedSet<char>();
            for (int index = 0; index < numKanji; index++)
            {
                char key = file.ReadChar();
                int offset = file.ReadInt();
                int numWords = file.ReadShort();
                entryTable.Add(key, new KanwaEntry(offset, numWords));
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Close()
        {
            if (file != null)
            {
                file.Close();
                file = null;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Save(RandomAccessFile file)
        {
        }
    }
}
