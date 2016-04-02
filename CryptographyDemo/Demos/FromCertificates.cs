namespace CryptographyDemo.Demos
{
    using System;
    using System.Security.Cryptography;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    public class FromCertificates : IRunnableDemo
    {
        public void Run()
        {
            //get certificate
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var certs = store.Certificates.Find(X509FindType.FindByThumbprint, "453990941c1cf508fb02196b474059c3c80a3678", false);
            store.Close();
            var cert = certs[0];

            //encrypt with certificates public key
            var rsa = (RSACryptoServiceProvider)cert.PublicKey.Key;
            byte[] valueBytes = Encoding.UTF8.GetBytes("quick brown fox jumps over the lazy dog");
            byte[] encBytes = rsa.Encrypt(valueBytes, true);

            //decrypt with certificates private key
            var rsa2 = (RSACryptoServiceProvider)cert.PrivateKey;
            var unencrypted = rsa2.Decrypt(encBytes, true);
            //Console.WriteLine(Encoding.UTF8.GetString(unencrypted));


            var originalEncryptedKey =
                "KjTapgZ3NMSy8Yu5+f3guhUFmtSnPjMyYbltZ9xaFQurygU/JvX50Kty3Jkgpv0nkl9SRKlivUuai3RKVGj8DLh4Mm4ntswEB2soJ/ikWIQQXeVMABKWou8vFa+OLBFwBmkD1jURV9BzlG354Zn8qqUurw5Vh2SQP3aYLK9823ZAZXjtYOOYDB6pebiexZ9UpinmVKmgVEywxMR90272DZpPTeYYDWmMRNbiiU1C8WEHa+NNGPwjuWv5sWwLL1OHkVqoi3KSI73LIx/zw9lDDkaCkne7LcSUPN0m2tsdSrQCS9uQs5B4ti0KozJfFb+OSz6Vy3013KH+vmkXsbWL8g==";
            var decryptedOriginalKey = rsa2.Decrypt(Encoding.UTF8.GetBytes(originalEncryptedKey), true);
            Console.WriteLine("Original decrypted key: " + Convert.ToBase64String(decryptedOriginalKey));
            // This decryptedOriginalKey can then be encrypted with the new certificate to get a new Encrypted key, IV remains the same as the old one
            
            // Create new IV and Key from the certificate
            var aes = new AesManaged();
            var iv = Convert.ToBase64String(aes.IV);
            Console.WriteLine("IV: " + iv);
            var key = Convert.ToBase64String(aes.Key);
            var encryptedKey = rsa.Encrypt(Encoding.UTF8.GetBytes(key), true);
            Console.WriteLine("Encrypted Key: " + Convert.ToBase64String(encryptedKey));
        }
    }
}
