using System;
using System.IO;
using System.Text;
using System.Runtime.CompilerServices;

namespace NKakasi
{
    public class KakasiReader
    {
        private TextReader reader;
        private readonly StringBuilder buffer = new StringBuilder();
        private readonly char[] oneCharacter = new char[1];
        private int nextIndex;
        private bool spaceEatMode;

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetReader(TextReader newReader)
        {
            reader = newReader;
            buffer.Capacity = 0;
        }

        public bool SpaceEatMode
        {
            get { return spaceEatMode; }
            set { spaceEatMode = value; }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal int Get()
        {
            if (reader == null)
            {
                SetReader(Console.In);
            }
            if (buffer.Length == 0)
            {
                int ch = reader.Read();
                if (ch < 0)
                {
                    return -1;
                }
                buffer.Append((char)ch);
            }
            nextIndex = 1;
            return buffer[0];
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal int More()
        {
            return More(oneCharacter) > 0 ? oneCharacter[0] : -1;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal int More(char[] chars)
        {
            int bufferLength = buffer.Length;
            int resultLength = 0;
            for (; resultLength < chars.Length; nextIndex++)
            {
                if (bufferLength <= nextIndex)
                {
                    int chi = reader.Read();
                    if (chi < 0)
                    {
                        break;
                    }
                    buffer.Append((char)chi);
                    ++bufferLength;
                }
                char ch = buffer[nextIndex];
                if (char.IsWhiteSpace(ch))
                {
                    if (!SpaceEatMode)
                    {
                        break;
                    }
                }
                else
                {
                    chars[resultLength++] = ch;
                }
            }
            return resultLength;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void Consume(int length)
        {
            if (SpaceEatMode)
            {
                int start = 0;
                int end = length;
                for (int index = 1; index < end; index++)
                {
                    char ch = buffer[index];
                    if (char.IsWhiteSpace(ch))
                    {
                        buffer[start++] = ch;
                        end++;
                    }
                }
                buffer.Remove(start, end);
            }
            else
            {
                buffer.Remove(0, length);
            }
            nextIndex = 0;
        }

        internal TextWriter CreateConnectedWriter()
        {
            reader = new NullReader();
            return new ConnectedWriter(this);
        }

        private class NullReader : TextReader
        {
            public override int Read(char[] buffer, int index, int count)
            {
                return -1;
            }

            public override void Close()
            {
            }
        }

        private class ConnectedWriter : TextWriter
        {
            private KakasiReader _kanjiInput;

            public ConnectedWriter(KakasiReader kanjiInput)
            {
                _kanjiInput = kanjiInput;
            }

            public override void Write(char[] cbuf, int off, int len)
            {
                _kanjiInput.buffer.Append(cbuf, off, len);
            }

            public override void Write(char c)
            {
                _kanjiInput.buffer.Append(c);
            }

            public override void Flush()
            {
            }

            public override void Close()
            {
            }

            public override Encoding Encoding
            {
                get {
                    return Encoding.Default;
                }
            }
        }
    }
}
