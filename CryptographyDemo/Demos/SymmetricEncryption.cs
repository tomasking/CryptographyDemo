using System;
using System.ComponentModel.Design;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyDemo.Demos
{
    public class SymmetricEncryption : IRunnableDemo
    {
        public void Run()
        {
            Console.WriteLine("Running SymmetricEncryption demo");
            Console.WriteLine();

            var key = GenerateRandomNumber(8);
            var iv = GenerateRandomNumber(8);
            const string original = "text to encrypt";
            Console.WriteLine("Original Text: " + original);
            Console.WriteLine();

            Console.WriteLine("DES");
            var encrypted = EncryptDes(Encoding.UTF8.GetBytes(original), key, iv);
            var decrypted = Encoding.UTF8.GetString(DecryptDes(encrypted, key, iv));
            Console.WriteLine("Encrypted: " + Convert.ToBase64String(encrypted));
            Console.WriteLine("Decrypted: " + decrypted);
            Console.WriteLine();

            Console.WriteLine("TripleDES");
            key = GenerateRandomNumber(24); //8*3 keys
            encrypted = EncryptTripleDes(Encoding.UTF8.GetBytes(original), key, iv);
            decrypted = Encoding.UTF8.GetString(DecryptTripleDes(encrypted, key, iv));
            Console.WriteLine("Encrypted: " + Convert.ToBase64String(encrypted));
            Console.WriteLine("Decrypted: " + decrypted);
            Console.WriteLine();

            Console.WriteLine("AES");
            key = GenerateRandomNumber(32); //256 bits (maximum)
            iv = GenerateRandomNumber(16);
            encrypted = EncryptAes(Encoding.UTF8.GetBytes(original), key, iv);
            decrypted = Encoding.UTF8.GetString(DecryptAes(encrypted, key, iv));
            Console.WriteLine("Encrypted: " + Convert.ToBase64String(encrypted));
            Console.WriteLine("Decrypted: " + decrypted);
            Console.WriteLine();
        }

        public byte[] GenerateRandomNumber(int length)
        {
            using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[length];
                cryptoServiceProvider.GetBytes(randomNumber);
                return randomNumber;
            }
        }

        private byte[] EncryptDes(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC; //default anyway
                des.Padding = PaddingMode.PKCS7; // default anyway
                des.Key = key;
                des.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    return memoryStream.ToArray();
                }
            }
        }

        private byte[] DecryptDes(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var des = new DESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC; //default anyway
                des.Padding = PaddingMode.PKCS7; // default anyway
                des.Key = key;
                des.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    return memoryStream.ToArray();
                }
            }
        }

        private byte[] EncryptTripleDes(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var des = new TripleDESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC; //default anyway
                des.Padding = PaddingMode.PKCS7; // default anyway
                des.Key = key;
                des.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    return memoryStream.ToArray();
                }
            }
        }

        private byte[] DecryptTripleDes(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var des = new TripleDESCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC; //default anyway
                des.Padding = PaddingMode.PKCS7; // default anyway
                des.Key = key;
                des.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    return memoryStream.ToArray();
                }
            }
        }

        public byte[] EncryptAes(byte[] dataToEncrypt, byte[] key, byte[] iv)
        {
            using (var des = new AesCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC; //default anyway
                des.Padding = PaddingMode.PKCS7; // default anyway
                des.Key = key;
                des.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    return memoryStream.ToArray();
                }
            }
        }

        public byte[] DecryptAes(byte[] dataToDecrypt, byte[] key, byte[] iv)
        {
            using (var des = new AesCryptoServiceProvider())
            {
                des.Mode = CipherMode.CBC; //default anyway
                des.Padding = PaddingMode.PKCS7; // default anyway
                des.Key = key;
                des.IV = iv;

                using (var memoryStream = new MemoryStream())
                {
                    var cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                    cryptoStream.FlushFinalBlock();

                    return memoryStream.ToArray();
                }
            }
        }
    }
}
