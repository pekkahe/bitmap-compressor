using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitmapCompressor.Formats
{
    public class DataBlockBuilder
    {
        /// <summary>
        /// Returns the byte array containing the 8-byte properly laid out
        /// BC1 block data of this instance.
        /// </summary>
        public byte[] Build()
        {
            //byte[] bytes = BitConverter.GetBytes(_data);

            //// BitConverter operates in LE order on LE machines. To preserve
            //// the original order of our data, reverse the byte array prior 
            //// returning the data.
            //if (BitConverter.IsLittleEndian)
            //    Array.Reverse(bytes);

            //return bytes;

            throw new NotImplementedException();
        }
    }
}
