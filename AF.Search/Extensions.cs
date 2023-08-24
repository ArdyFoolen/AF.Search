using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Search
{
    public static class Extensions
    {
        public static void ForEach(this IEnumerable<char> source, Action<char, int> action)
        {
            int i = 0;
            foreach (char c in source)
                action(c, i++);
        }

        public static char[] ToCharArray(this byte[] buffer, int length, Encoding encoding)
        {
            byte[] text = new byte[length];
            Array.Copy(buffer, text, length);

            return encoding.GetString(text).ToCharArray();
        }

        internal static Bom Bom(this FileStream stream)
        {
            Encoding encoding = Encoding.ASCII;
            int bomLength = 0;
            var bom = new byte[4];
            stream.Read(bom, 0, bom.Length);

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76)
            {
                encoding = Encoding.UTF7;
                bomLength = 3;
            }
            else if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
            {
                encoding = Encoding.UTF8;
                bomLength = 3;
            }
            else if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0)
            {
                encoding = Encoding.UTF32; //UTF-32LE
                bomLength = 4;
            }
            else if (bom[0] == 0xff && bom[1] == 0xfe)
            {
                encoding = Encoding.Unicode; //UTF-16LE
                bomLength = 2;
            }
            else if (bom[0] == 0xfe && bom[1] == 0xff)
            {
                encoding = Encoding.BigEndianUnicode; //UTF-16BE
                bomLength = 2;
            }
            else if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff)
            {
                encoding = new UTF32Encoding(true, true);  //UTF-32BE
                bomLength = 4;
            }

            return new Bom(encoding, bomLength);
        }
    }
}
