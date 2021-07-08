using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace ADServer.BLL
{
    public partial class RSAHelper
    {
        /// <summary>
        /// 不要直接使用伯约扔过来的公钥
        /// </summary>
        private static string xmlPublicKey = @"<RSAKeyValue><Modulus>1OiUqSjHOoEHlcvYhDoE3JFbhUUHM9gq60Hmm9qbxLd8P8nbXCgBVYb55xpjygHgST3JBWUXWA1ucB7vlu9YYvUESBs0+Q5H7jmf2rOwJrZ01Ve/XObVVvBn7D/lsEYNcmvbNlOOmvIq6jCbDAtS+gioC7PnsumCtjrS02soraM=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        public static void Motify_RSA_PublicKey(string key)
        {
            xmlPublicKey = key;
        }

        /// <summary>
        /// .net RSA 标准加密
        /// </summary>
        /// <param name="xmlPublicKey"></param>
        /// <param name="encryptString"></param>
        /// <returns></returns>
        public static string RSAEncrypt(string encryptString)
        {
            try
            {
                byte[] PlainTextBArray;
                byte[] CypherTextBArray;
                string Result;
                System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(xmlPublicKey);
                PlainTextBArray = (new UTF8Encoding()).GetBytes(encryptString);
                //PlainTextBArray = Encoding.UTF8.GetBytes(encryptString);
                CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
                Result = Convert.ToBase64String(CypherTextBArray);
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
