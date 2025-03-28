namespace NKakasi
{
    class KanwaEntry
    {
        private readonly int offset;
        private readonly int numberOfWords;

        internal KanwaEntry(int offset, int numberOfWords)
        {
            this.offset = offset;
            this.numberOfWords = numberOfWords;
        }

        internal int GetOffset()
        {
            return offset;
        }

        internal int GetNumberOfWords()
        {
            return numberOfWords;
        }
    }
}
