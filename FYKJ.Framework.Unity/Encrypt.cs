namespace FYKJ.Framework.Utility
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class Encrypt
    {
        private static readonly byte[] keys = { 0x12, 0x34, 0x56, 120, 0x90, 0xab, 0xcd, 0xef };

        public static string Decode(string decryptString, string decryptKey = "www.cnsaas.com")
        {
            try
            {
                decryptKey = decryptKey.Substring(0, 8);
                decryptKey = decryptKey.PadRight(8, ' ');
                var bytes = Encoding.UTF8.GetBytes(decryptKey);
                var keys = Encrypt.keys;
                var buffer = Convert.FromBase64String(decryptString);
                var provider = new DESCryptoServiceProvider();
                var stream = new MemoryStream();
                var stream2 = new CryptoStream(stream, provider.CreateDecryptor(bytes, keys), CryptoStreamMode.Write);
                stream2.Write(buffer, 0, buffer.Length);
                stream2.FlushFinalBlock();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
            catch
            {
                return "";
            }
        }

        public static string Decode2(string decryptString, string decryptToken = "www.cnsaas.com", string decryptKey = "www.cnsaas.com")
        {
            var str = Decode(decryptKey, decryptToken);
            return Decode(decryptString, str);
        }

        public static string Encode(string encryptString, string encryptKey = "www.cnsaas.com")
        {
            encryptKey = encryptKey.Substring(0, 8);
            encryptKey = encryptKey.PadRight(8, ' ');
            var bytes = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            var keys = Encrypt.keys;
            var buffer = Encoding.UTF8.GetBytes(encryptString);
            var provider = new DESCryptoServiceProvider();
            var stream = new MemoryStream();
            var stream2 = new CryptoStream(stream, provider.CreateEncryptor(bytes, keys), CryptoStreamMode.Write);
            stream2.Write(buffer, 0, buffer.Length);
            stream2.FlushFinalBlock();
            return Convert.ToBase64String(stream.ToArray());
        }

        public static string Encode2(string encryptString, string encryptToken = "www.cnsaas.com", string encryptKey = "www.cnsaas.com")
        {
            var str = Encode(encryptKey, encryptToken);
            return Encode(encryptString, str);
        }

        public static string MD5(string s)
        {
            var provider = new MD5CryptoServiceProvider();
            return BitConverter.ToString(provider.ComputeHash(Encoding.UTF8.GetBytes(s.Trim())));
        }
    }
}

