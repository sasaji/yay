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
        private BlockType blockType;

        internal static UnicodeBlock Of(char c)
        {
            UnicodeBlock block = new UnicodeBlock();
            if (c >= '\u4E00' && c <= '\u9FFF')
            {
                block.blockType = BlockType.CJKUnifiedIdeographs;
            }
            else if (c >= '\u3040' && c <= '\u309F')
            {
                block.blockType = BlockType.Hiragana;
            }
            else if (c >= '\u30A0' && c <= '\u30FF')
            {
                block.blockType = BlockType.Katakana;
            }
            else if (c >= '\uFF00' && c <= '\uFFEF')
            {
                block.blockType = BlockType.HalfKana;
            }
            else
            {
                block.blockType = BlockType.Unknown;
            }
            return block;
        }

        internal static UnicodeBlock CJKUnifiedIdeographs
        {
            get {
                UnicodeBlock block = new UnicodeBlock
                {
                    blockType = BlockType.CJKUnifiedIdeographs
                };
                return block;
            }
        }

        internal static UnicodeBlock Hiragana
        {
            get {
                UnicodeBlock block = new UnicodeBlock
                {
                    blockType = BlockType.Hiragana
                };
                return block;
            }
        }

        internal static UnicodeBlock Katakana
        {
            get {
                UnicodeBlock block = new UnicodeBlock
                {
                    blockType = BlockType.Katakana
                };
                return block;
            }
        }

        internal static UnicodeBlock HalfKana
        {
            get {
                UnicodeBlock block = new UnicodeBlock
                {
                    blockType = BlockType.HalfKana
                };
                return block;
            }
        }

        internal bool Equals(UnicodeBlock block)
        {
            return blockType == block.blockType;
        }
    }
}
