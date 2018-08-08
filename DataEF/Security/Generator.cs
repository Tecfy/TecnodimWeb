using System;
using System.Security.Cryptography;
using System.Text;

namespace DataEF.Security
{
    public static class Generator
    {
        public static string Password(string strOriginalPassword)
        {
            return GetSHA1(strOriginalPassword);
        }

        public static string GetSHA1(string strPlain)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] HashValue, MessageBytes = UE.GetBytes(strPlain);
            SHA1Managed SHhash = new SHA1Managed();
            string strHex = "";
            HashValue = SHhash.ComputeHash(MessageBytes);
            foreach (byte b in HashValue)
            {
                strHex += String.Format("{0:x2}", b);
            }
            return strHex;
        }

        public static string HashMD5(string original)
        {
            Byte[] encodedBytes = getEncodedBytes(original);

            return BitConverter.ToString(encodedBytes);
        }

        public static string HashMD5Hex(string original)
        {
            Byte[] encodedBytes = getEncodedBytes(original);

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < encodedBytes.Length; i++)
            {
                sBuilder.Append(encodedBytes[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        private static Byte[] getEncodedBytes(string original)
        {
            MD5 md5 = MD5.Create();
            Byte[] originalBytes = ASCIIEncoding.Default.GetBytes(original);
            Byte[] encodedBytes = md5.ComputeHash(originalBytes);

            return encodedBytes;
        }

        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (byte b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex);
            }
            return sb.ToString();
        } 
    }
}
