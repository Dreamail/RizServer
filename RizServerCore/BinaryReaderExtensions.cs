using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RizServerCoreSharp
{
    public static class BinaryReaderExtensions
    {
        // A method that reads an ASN.1 length from a binary reader
        public static int ReadLength(this BinaryReader br)
        {
            // Read the first byte
            var b = br.ReadByte();

            // If the first bit is 0, then the length is encoded in one byte
            if ((b & 0x80) == 0)
            {
                return b;
            }

            // If the first bit is 1, then the length is encoded in multiple bytes
            else
            {
                // The remaining bits indicate how many bytes are used to encode the length
                var count = b & 0x7f;

                // Read the next count bytes and convert them to an integer
                var bytes = br.ReadBytes(count);

                // Reverse the bytes if the system is little endian
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(bytes);
                }

                // Convert the bytes to an integer using big endian order
                var length = 0;
                for (int i = 0; i < bytes.Length; i++)
                {
                    length = (length << 8) + bytes[i];
                }

                return length;
            }
        }


        // A method that reads an ASN.1 integer from a binary reader
        public static byte[] ReadInteger(this BinaryReader br)
        {
            // Read the integer header
            br.ReadByte();

            // Read the integer length
            var length = br.ReadLength();

            // Check if the length is non-negative
            if (length >= 0)
            {
                // Read the integer bytes and return them
                return br.ReadBytes(length);
            }
            else
            {
                // Handle the negative length case
                //throw new ArgumentException("Invalid length for integer");
                return new byte[0];
            }
        }

    }
}
