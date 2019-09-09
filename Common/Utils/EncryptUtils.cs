using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using ZoneTop.Application.SSO.Common.Grobal;

namespace ZoneTop.Application.SSO.Common.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class EncryptUtils
    {
        private static string encryptKey = GrobalConfig.EncryptToken;

        #region "MD5加密"
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">加密字符</param>
        /// <param name="encryptionLength">加密位数16/32</param>
        /// <returns></returns>
        public static string EncryptMD5(string str, int encryptionLength)
        {
            string strEncrypt = string.Empty;
            if (encryptionLength.Equals(GrobalConfig.Encryption16))
            {
                strEncrypt = FormsAuthentication.HashPasswordForStoringInConfigFile(str, GrobalConfig.MD5).Substring(8, 16);
            }

            if (encryptionLength.Equals(GrobalConfig.Encryption16))
            {
                strEncrypt = FormsAuthentication.HashPasswordForStoringInConfigFile(str, GrobalConfig.MD5);
            }

            return strEncrypt;
        }
        #endregion


        #region 加密接口
        /// <summary>
        /// 供外部调用
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string doEncrypt(string str)
        {
            return doEncrypt(str, encryptKey);
        }
        #endregion

        #region 加密
        /// <summary>
        /// 加密的实现，内部函数，MD5加密
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string doEncrypt(string str, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray;
            inputByteArray = Encoding.Default.GetBytes(str);
            des.Key = ASCIIEncoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(key, GrobalConfig.MD5).Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(key, GrobalConfig.MD5).Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }
        #endregion

        #region 解密接口
        /// <summary>
        /// 供外部带哦与
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string doDecrypt(string str)
        {
            return doDecrypt(str, encryptKey);
        }
        #endregion

        #region 解密
        /// <summary>
        /// 解密的实现，内部函数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string doDecrypt(string str, string key)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = str.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(str.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(key, GrobalConfig.MD5).Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(key, GrobalConfig.MD5).Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }
        #endregion
    }
}
