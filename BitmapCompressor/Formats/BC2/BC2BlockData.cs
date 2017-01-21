using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitmapCompressor.Formats
{
    public class BC2BlockData
    {
        private readonly byte[] _data = new byte[16];

        public BC2BlockData()
        {
            //blockData[0] = alphas[0, 0];
            //blockData[1] = alphas[0, 1];
            //blockData[2] = alphas[1, 0];
            //blockData[3] = alphas[1, 1];
            //blockData[4] = alphas[2, 0];
            //blockData[5] = alphas[2, 1];
            //blockData[6] = alphas[3, 0];
            //blockData[7] = alphas[3, 1];
            //blockData[8] = c0Low;
            //blockData[9] = c0Hi;
            //blockData[10] = c1Low;
            //blockData[11] = c1Hi;
            //blockData[12] = codes[0];
            //blockData[13] = codes[1];
            //blockData[14] = codes[2];
            //blockData[15] = codes[3];
        }

        public byte this[byte index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        public byte Color0Low
        {
            get { return this[8]; }
            set { this[8] = value; }
        }

        public byte Color0High
        {
            get { return this[9]; }
            set { this[9] = value; }
        }

        public byte Color1Low
        {
            get { return this[10]; }
            set { this[10] = value; }
        }

        public byte Color1High
        {
            get { return this[11]; }
            set { this[11] = value; }
        }

        public byte Codes0
        {
            get { return this[12]; }
            set { this[12] = value; }
        }

        public byte Codes1
        {
            get { return this[13]; }
            set { this[13] = value; }
        }

        public byte Codes2
        {
            get { return this[14]; }
            set { this[14] = value; }
        }

        public byte Codes3
        {
            get { return this[15]; }
            set { this[15] = value; }
        }

        public byte[,] Alphas;
    }
}
