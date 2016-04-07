using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace TB.Helpers
{
    public static class Extensions
    {
        private static readonly String strDesKey = "luckykey";  //8 digits key for DES
        private static readonly String strAesKey = "www.zuora.com.for.salesforce.com";  //32 digits key for AES

        public static string GetConfigString(string key)
        {
            return ConfigurationManager.AppSettings[key] ?? "";
        }

        public static string FormatWith(this string formatStr, params object[] args)
        {
            return string.Format(formatStr, args);
        }

        /// <summary>
        /// DES encrypt
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String Encrypt_DES(this String str)
        {
            System.Security.Cryptography.DESCryptoServiceProvider des = new System.Security.Cryptography.DESCryptoServiceProvider();
            Byte[] inputByteArray = System.Text.Encoding.Default.GetBytes(str);
            des.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(strDesKey);
            des.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(strDesKey);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, des.CreateEncryptor(), System.Security.Cryptography.CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (Byte b in ms.ToArray())
                sb.AppendFormat("{0:X2}", b);
            return sb.ToString();
        }

        /// <summary>
        /// DES decrypt
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String Decrypt_DES(this String str)
        {
            System.Security.Cryptography.DESCryptoServiceProvider des = new System.Security.Cryptography.DESCryptoServiceProvider();
            Int32 x;
            Byte[] inputByteArray = new Byte[str.Length / 2];
            for (x = 0; x < str.Length / 2; x++)
                inputByteArray[x] = (Byte)(Convert.ToInt32(str.Substring(x * 2, 2), 16));
            des.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(strDesKey);
            des.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(strDesKey);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            System.Security.Cryptography.CryptoStream cs = new System.Security.Cryptography.CryptoStream(ms, des.CreateDecryptor(), System.Security.Cryptography.CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.StringBuilder ret = new System.Text.StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// AES encrypt
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String Encrypt_AES(this String str)
        {
            Byte[] keyArray = System.Text.UTF8Encoding.UTF8.GetBytes(strAesKey);
            Byte[] toEncryptArray = System.Text.UTF8Encoding.UTF8.GetBytes(str);
            System.Security.Cryptography.RijndaelManaged rDel = new System.Security.Cryptography.RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = System.Security.Cryptography.CipherMode.ECB;
            rDel.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            System.Security.Cryptography.ICryptoTransform cTransform = rDel.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        /// <summary>
        /// AES decrypt
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static String Decrypt_AES(this String str)
        {
            Byte[] keyArray = System.Text.UTF8Encoding.UTF8.GetBytes(strAesKey);
            Byte[] toEncryptArray = Convert.FromBase64String(str);
            System.Security.Cryptography.RijndaelManaged rDel = new System.Security.Cryptography.RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = System.Security.Cryptography.CipherMode.ECB;
            rDel.Padding = System.Security.Cryptography.PaddingMode.PKCS7;
            System.Security.Cryptography.ICryptoTransform cTransform = rDel.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return System.Text.UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}