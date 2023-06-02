namespace NKakasi
{
    class UnicodeBlock
    {
        private enum BlockType
        {
            CJKUnifiedIdeographs,
            Hiragana,
            Katakana,
            HalfKana,
            Unknown
        }
        private BlockType _block;

        internal static UnicodeBlock Of(char c)
        {
            UnicodeBlock block = new UnicodeBlock();
            if (c >= '\u4E00' && c <= '\u9FFF')
            {
                block._block = BlockType.CJKUnifiedIdeographs;
            }
            else if (c >= '\u3040' && c <= '\u309F')
            {
                block._block = BlockType.Hiragana;
            }
            else if (c >= '\u30A0' && c <= '\u30FF')
            {
                block._block = BlockType.Katakana;
            }
            else if (c >= '\uFF00' && c <= '\uFFEF')
            {
                block._block = BlockType.HalfKana;
            }
            else
            {
                block._block = BlockType.Unknown;
            }
            return block;
        }

        internal static UnicodeBlock CJKUnifiedIdeographs
        {
            get {
                UnicodeBlock block = new UnicodeBlock();
                block._block = BlockType.CJKUnifiedIdeographs;
                return block;
            }
        }

        internal static UnicodeBlock Hiragana
        {
            get {
                UnicodeBlock block = new UnicodeBlock();
                block._block = BlockType.Hiragana;
                return block;
            }
        }

        internal static UnicodeBlock Katakana
        {
            get {
                UnicodeBlock block = new UnicodeBlock();
                block._block = BlockType.Katakana;
                return block;
            }
        }

        internal static UnicodeBlock HalfKana
        {
            get {
                UnicodeBlock block = new UnicodeBlock();
                block._block = BlockType.HalfKana;
                return block;
            }
        }

        internal bool Equals(UnicodeBlock block)
        {
            return _block == block._block;
        }
    }
}
