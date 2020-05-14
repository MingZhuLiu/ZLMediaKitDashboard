using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZhiHuReptile.Web.Commons
{
    public class RSAHelper
    {

        private string privateKey;
        private string publicKey;

        private RSAHelper()
        {

            if (!Directory.Exists(Environment.CurrentDirectory + "\\SecretKeys\\"))
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\SecretKeys\\");
            if (File.Exists(Environment.CurrentDirectory + "\\SecretKeys\\publicKey.dat"))
            {
                StreamReader sr = new StreamReader(Environment.CurrentDirectory + "\\SecretKeys\\publicKey.dat");
                publicKey = sr.ReadToEnd();
                sr.Close();

                sr = new StreamReader(Environment.CurrentDirectory + "\\SecretKeys\\privateKey.dat");
                privateKey = sr.ReadToEnd();
                sr.Close();
            }
            else
            {
                var keys = GetKeyPair1();
                StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "\\SecretKeys\\publicKey.dat");
                sw.Write(keys.Key);
                sw.Close();

                sw = new StreamWriter(Environment.CurrentDirectory + "\\SecretKeys\\privateKey.dat");
                sw.Write(keys.Value);
                sw.Close();

                publicKey = keys.Key;
                privateKey = keys.Value;
            }

        }
        private readonly static object _lock = new object();
        private static RSAHelper _instance;
        public static RSAHelper Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new RSAHelper();
                    }
                    return _instance;
                }
            }
        }

        /// <summary>
        /// 生成一对公钥和私钥
        /// </summary>
        /// <returns></returns>
        private KeyValuePair<string, string> GetKeyPair1()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            string public_Key = Convert.ToBase64String(RSA.ExportCspBlob(false));
            string private_Key = Convert.ToBase64String(RSA.ExportCspBlob(true));
            return new KeyValuePair<string, string>(public_Key, private_Key);
        }


        /// <summary>
        /// 生成一对公钥和私钥
        /// </summary>
        /// <returns></returns>
        private KeyValuePair<string, string> GetKeyPair2()
        {
            RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
            string public_Key = RSA.ToXmlString(false);
            string private_Key = RSA.ToXmlString(true);
            return new KeyValuePair<string, string>(public_Key, private_Key);
        }








        /// <summary>
        /// RAS加密
        /// </summary>
        /// <param name="xmlPublicKey">公钥</param>
        /// <param name="EncryptString">明文</param>
        /// <returns>密文</returns>

        public string Encrypt(string EncryptString)
        {
            byte[] PlainTextBArray;
            byte[] CypherTextBArray;
            string Result = String.Empty;
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(Convert.FromBase64String(publicKey));
            int t = (int)(Math.Ceiling((double)EncryptString.Length / (double)50));
            //分割明文
            for (int i = 0; i <= t - 1; i++)
            {

                PlainTextBArray = (new UnicodeEncoding()).GetBytes(EncryptString.Substring(i * 50, EncryptString.Length - (i * 50) > 50 ? 50 : EncryptString.Length - (i * 50)));
                CypherTextBArray = rsa.Encrypt(PlainTextBArray, false);
                Result += Convert.ToBase64String(CypherTextBArray) + "^&*&%$%@N_12d144VB@@4341sfvr13@@@@!";
            }
            return Result;
        }
        /// <summary>
        /// RAS解密
        /// </summary>
        /// <param name="xmlPrivateKey">私钥</param>
        /// <param name="DecryptString">密文</param>
        /// <returns>明文</returns>
        public string Decrypt(string DecryptString)
        {
            byte[] PlainTextBArray;
            byte[] DypherTextBArray;
            string Result = String.Empty;
            System.Security.Cryptography.RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportCspBlob(Convert.FromBase64String(privateKey));
            string[] Split = new string[1];
            Split[0] = "^&*&%$%@N_12d144VB@@4341sfvr13@@@@!";
            //分割密文
            string[] mis = DecryptString.Split(Split, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < mis.Length; i++)
            {
                PlainTextBArray = Convert.FromBase64String(mis[i]);
                DypherTextBArray = rsa.Decrypt(PlainTextBArray,false);
                Result += (new UnicodeEncoding()).GetString(DypherTextBArray);
            }
            return Result;
        }
    }

}
