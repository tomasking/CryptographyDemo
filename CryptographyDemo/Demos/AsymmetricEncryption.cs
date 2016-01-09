using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyDemo.Demos
{
    public class AsymmetricEncryption : IRunnableDemo
    {
        private RSAParameters publicKey;
        private RSAParameters privateKey;

        public void Run()
        {
            Console.WriteLine("Running AsymmetricEncryption demo");
            Console.WriteLine();

            AssignNewKey();

            const string data = "Message to hide";

            var encrypted = EncryptData(Encoding.UTF8.GetBytes(data));
            var decrypted = DecryptData(encrypted);

            Console.WriteLine("Original message: " + data);
            Console.WriteLine("Encrypted: " + Convert.ToBase64String(encrypted));
            Console.WriteLine("Decrpted: " + Encoding.UTF8.GetString(decrypted));
        }

        public void AssignNewKey()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                //in memory
                publicKey = rsa.ExportParameters(false);
                privateKey = rsa.ExportParameters(true);
                return;

                //to file
                File.WriteAllText(@"C:\git\CryptographyDemo\CryptographyDemo\bin\Debug\public.txt", rsa.ToXmlString(false));
                File.WriteAllText(@"C:\git\CryptographyDemo\CryptographyDemo\bin\Debug\private.txt", rsa.ToXmlString(true));
            }

            //To key container, stored for windows user
            const int providerRsaFull = 1;
            CspParameters cspParams = new CspParameters(providerRsaFull);
            cspParams.KeyContainerName = "TomsContainer";
            cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            cspParams.ProviderName = "Microsoft Strong Cryptographic Provider";
            var rsa2 = new RSACryptoServiceProvider(cspParams);
            rsa2.PersistKeyInCsp = true;

            // SHOULD THEN DELETE KEY
        }

        public byte[] EncryptData(byte[] dataToEncrypt)
        {
            byte[] cipherBytes;

            //var cspParams = new CspParameters();
            //cspParams.KeyContainerName = "TomsContainer";

            using (var rsa = new RSACryptoServiceProvider(2048)) //, cspParams)) 
            {
                rsa.ImportParameters(publicKey);
                cipherBytes = rsa.Encrypt(dataToEncrypt, true);
            }
            return cipherBytes;
        }

        public byte[] DecryptData(byte[] dataToDecrypt)
        {
            byte[] plain;
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.PersistKeyInCsp = false;
                rsa.ImportParameters(privateKey);
                plain = rsa.Decrypt(dataToDecrypt, true);
            }
            return plain;
        }
    }
}
