using System;
using System.Configuration;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CryptographyDemo.Demos
{
    public class HybridEncryptionDemo : IRunnableDemo
    {
        public void Run()
        {
            string thumbprint = "fa72c780a5e8465f6efd709147242c4b661acbba";
            X509Certificate2 certificate = GetCertificateByThumbprint(thumbprint);

            GenerateEncryptionParameters(certificate);

            AesManaged aesManaged = CreateAesManaged(certificate);

            string originalText = "Some random thing";
            string encryptedText = AesEncrypt(aesManaged.CreateEncryptor(), originalText);
            string decryptedText = AesDecrypt(aesManaged.CreateDecryptor(), encryptedText);
        }
        
        private X509Certificate2 GetCertificateByThumbprint(string thumbprint)
        {
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            var certs = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false);
            store.Close();
            var cert = certs[0];
            if (cert.PrivateKey == null)
            {
                throw new Exception("Private key is null");
            }
            return cert;
        }

        private void GenerateEncryptionParameters(X509Certificate2 certificate)
        {
            var aesManaged = new AesManaged();
            aesManaged.GenerateIV();
            aesManaged.GenerateKey();

            string ivToStoreInConfig = Convert.ToBase64String(aesManaged.IV);
            string encryptionKeyAsString = Convert.ToBase64String(aesManaged.Key);
            string keyToStoreInConfig = RsaEncrypt(certificate, encryptionKeyAsString);

            ConfigurationManager.AppSettings["EncryptionIV"] = ivToStoreInConfig;
            ConfigurationManager.AppSettings["EncryptionKey"] = keyToStoreInConfig;
        }

        private static AesManaged CreateAesManaged(X509Certificate2 certificate)
        {
            var iv = ConfigurationManager.AppSettings["EncryptionIV"];
            var encryptedKey = ConfigurationManager.AppSettings["EncryptionKey"];
            var decryptedKey = RsaDecrypt(certificate, encryptedKey);

            var aesManaged = new AesManaged()
            {
                Key = Convert.FromBase64String(decryptedKey),
                IV = Convert.FromBase64String(iv)
            };
            return aesManaged;
        }

        private static string RsaEncrypt(X509Certificate2 cert, string textToEncrypt)
        {
            var rsa = (RSACryptoServiceProvider)cert.PublicKey.Key;
            var valueBytes = Encoding.UTF8.GetBytes(textToEncrypt);
            var encBytes = rsa.Encrypt(valueBytes, true);
            return Convert.ToBase64String(encBytes);
        }

        private static string RsaDecrypt(X509Certificate2 cert, string encryptedText)
        {
            var rsa2 = (RSACryptoServiceProvider)cert.PrivateKey;
            var encryptTextBytes = Convert.FromBase64String(encryptedText);
            return Encoding.UTF8.GetString(rsa2.Decrypt(encryptTextBytes, true));
        }

        public string AesEncrypt(ICryptoTransform encryptor, string plainText)
        {
            byte[] inBlock = Encoding.Unicode.GetBytes(plainText);
            byte[] outBlock = encryptor.TransformFinalBlock(inBlock, 0, inBlock.Length);
            return Convert.ToBase64String(outBlock);
        }

        public string AesDecrypt(ICryptoTransform decrypter, string encryptedString)
        {
            var inBytes = Convert.FromBase64String(encryptedString);
            byte[] outBlock = decrypter.TransformFinalBlock(inBytes, 0, inBytes.Length);
            return Encoding.Unicode.GetString(outBlock);
        }
    }
}
