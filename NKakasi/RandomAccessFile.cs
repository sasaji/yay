using System.IO;
using System.Text;

namespace NKakasi
{
    public class RandomAccessFile
    {
        private readonly FileStream file;

        public RandomAccessFile(string path, string fileAccess)
        {
            file = new FileStream(path, FileMode.Open, FileAccess.Read);
        }

        public int ReadInt()
        {
            byte[] buf = new byte[4];
            _ = file.Read(buf, 0, 4);
            return (buf[0] << 24) | (buf[1] << 16) + (buf[2] << 8) + buf[3];
        }

        public short ReadShort()
        {
            byte[] buf = new byte[2];
            _ = file.Read(buf, 0, 2);
            return (short)((buf[0] << 8) | buf[1]);
        }

        public int ReadUnsignedShort()
        {
            byte[] buf = new byte[2];
            _ = file.Read(buf, 0, 2);
            return ((buf[0] << 8) | buf[1]);
        }

        public char ReadChar()
        {
            byte[] buf = new byte[2];
            _ = file.Read(buf, 0, 2);
            return (char)((buf[0] << 8) | buf[1]);
        }

        public string ReadUTF()
        {
            int length = ReadUnsignedShort();
            byte[] buf = new byte[length];
            _ = file.Read(buf, 0, length);
            return Encoding.UTF8.GetString(buf);
        }

        public byte ReadByte()
        {
            byte[] buf = new byte[1];
            _ = file.Read(buf, 0, 1);
            return buf[0];
        }

        public void Seek(int offset)
        {
            file.Seek(offset, SeekOrigin.Begin);
        }

        public void Close()
        {
            file.Close();
        }
    }
}
