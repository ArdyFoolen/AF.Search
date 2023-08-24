using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AF.Search
{
    internal class Bom
    {
        public int Length { get; private set; }
        public Encoding Encoding { get; private set; }

        public Bom(Encoding Encoding, int length)
        {
            this.Encoding = Encoding;
            this.Length = length;
        }
    }
}
