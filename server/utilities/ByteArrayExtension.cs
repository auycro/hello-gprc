using System;

namespace server.utilities
{
    static class ByteArrayExtensions
    {
        //https://stackoverflow.com/questions/240258/removing-trailing-nulls-from-byte-array-in-c-sharp
        public static Byte[] Trim(this Byte[] bytes) {
            if (bytes.Length == 0) return bytes;
            var i = bytes.Length - 1;
            while (bytes[i] == 0) {
                i--;
            }
            Byte[] copy = new Byte[i + 1];
            Array.Copy(bytes, copy, i + 1);
            return copy;
        }
    }
}