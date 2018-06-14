using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Services.Thirdparty
{
    public class AESHelper
    {
        private static string Key
        {
            get
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings["key"].ConnectionString;
            }
        }
        private static byte[] _key1 = { 151, 23, 1, 162, 134, 112, 206, 117, 223, 105, 198, 208, 25, 233, 178, 106 };
        public static string AESEncrypt(string plainText)
        {
            //分组加密算法
            SymmetricAlgorithm des = Rijndael.Create();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);
            List<byte> list = new List<byte>();
            foreach(var b in _key1)
            {
                list.Add(b);
            }
            foreach(var b2  in inputByteArray)
            {
                list.Add(b2);
            }
            des.Key = Encoding.UTF8.GetBytes(Key);
            des.Mode = CipherMode.CBC;
            des.IV = _key1;
            byte[] cipherBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(list.ToArray(), 0, list.Count);
                    cs.FlushFinalBlock();
                    cipherBytes = ms.ToArray();
                    cs.Close();
                    ms.Close();
                }
            }
            return Convert.ToBase64String(cipherBytes);
        }
        public static string AESDecrypt(string showText)
        {
            byte[] cipherText = Convert.FromBase64String(showText);
            SymmetricAlgorithm des = Rijndael.Create();
            des.Key = Encoding.UTF8.GetBytes(Key);
            des.Mode = CipherMode.CBC;
            des.IV = cipherText.Take(16).ToArray();
            //des.IV = _key1;
            byte[] decryptBytes = new byte[cipherText.Length];
            using (MemoryStream ms = new MemoryStream(cipherText.Skip(16).ToArray()))
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    cs.Read(decryptBytes, 0, decryptBytes.Length);
                    cs.Close();
                    ms.Close();
                }
            }
            return Encoding.UTF8.GetString(decryptBytes).Replace("\0", "");
        }
    }
}