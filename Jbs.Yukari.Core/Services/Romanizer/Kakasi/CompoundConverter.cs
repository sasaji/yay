using System.IO;

namespace NKakasi
{
    class CompoundConverter : IConverter
    {
        private readonly IConverter front;
        private readonly IConverter back;
        private readonly KakasiReader pipeInput = new KakasiReader();
        private readonly TextWriter pipeOutput;

        public CompoundConverter(IConverter front, IConverter back)
        {
            pipeOutput = pipeInput.CreateConnectedWriter();
            this.front = front;
            this.back = back;
        }

        public bool Convert(KakasiReader input, TextWriter output)
        {
            bool ret = front.Convert(input, pipeOutput);
            if (ret)
            {
                while (pipeInput.Get() >= 0)
                {
                    if (!back.Convert(pipeInput, output))
                    {
                        output.Write((char)pipeInput.Get());
                        pipeInput.Consume(1);
                    }
                }
            }
            return ret;
        }
    }
}
