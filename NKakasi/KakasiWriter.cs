using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace NKakasi
{
    public class KakasiWriter : TextWriter
    {
        private TextWriter writer;
        private bool autoFlushMode = true;
        private bool splitMode;
        private bool lastWasSpace;
        private bool outSeparator;

        internal KakasiWriter()
        {
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetWriter(TextWriter newWriter)
        {
            writer = newWriter;
            lastWasSpace = true;
        }

        public bool AutoFlushMode
        {
            get { return autoFlushMode; }
            set { autoFlushMode = value; }
        }

        public bool SplitMode
        {
            get { return splitMode; }
            [MethodImpl(MethodImplOptions.Synchronized)]
            set {
                splitMode = value;
                outSeparator = false;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void PutSeparator()
        {
            if (splitMode)
            {
                outSeparator = true;
            }
        }

        public override void Write(char[] cbuf, int off, int len)
        {
            int max = off + len;
            for (int index = off; index < max; index++)
            {
                Write(cbuf[index]);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Write(char c)
        {
            if (writer == null)
            {
                SetWriter(Console.Out);
            }
            if (SplitMode)
            {
                if (char.IsWhiteSpace(c))
                {
                    lastWasSpace = true;
                    outSeparator = false;
                }
                else
                {
                    if (outSeparator)
                    {
                        outSeparator = false;
                        if (!lastWasSpace)
                        {
                            writer.Write(' ');
                        }
                    }
                    lastWasSpace = false;
                }
            }
            writer.Write(c);
            if (c == '\n' && AutoFlushMode)
            {
                Flush();
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void Flush()
        {
            if (writer != null)
            {
                writer.Flush();
            }
        }

        public override void Close()
        {
            if (writer != null)
            {
                writer.Close();
            }
        }

        public override Encoding Encoding
        {
            get {
                return writer.Encoding;
            }
        }
    }
}
