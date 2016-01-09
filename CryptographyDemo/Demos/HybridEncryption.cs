using System;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyDemo.Demos
{
    public class HybridEncryption : IRunnableDemo
    {
        SymmetricEncryption aesEncryption = new SymmetricEncryption();

        AsymmetricEncryption asymmetricEncryption = new AsymmetricEncryption();
        
        public void Run()
        {
            Console.WriteLine("Running HybridEncryption demo");
            Console.WriteLine();

            const string original = "some message I want to hide";

            asymmetricEncryption.AssignNewKey();
            var encryptedBlock = EncryptData(Encoding.UTF8.GetBytes(original));
            var decrypted = DecryptData(encryptedBlock);

            Console.WriteLine("Original: " + original);
            Console.WriteLine("Encrypted: " + Convert.ToBase64String(encryptedBlock.EncryptedData));
            Console.WriteLine("Decrypted: " + Encoding.UTF8.GetString(decrypted));
        }

        public EncryptedPacket EncryptData(byte[] original)
        {
            var sessionKey = aesEncryption.GenerateRandomNumber(32);
            var encryptedPacket = new EncryptedPacket() { Iv = aesEncryption.GenerateRandomNumber(16) };
            encryptedPacket.EncryptedData = aesEncryption.EncryptAes(original, sessionKey, encryptedPacket.Iv);
            encryptedPacket.EncryptedSessionKey = asymmetricEncryption.EncryptData(sessionKey);
            using (var hmac = new HMACSHA256(sessionKey))
            {
                encryptedPacket.Hmac = hmac.ComputeHash(encryptedPacket.EncryptedData);
            }

            return encryptedPacket;
        }

        public byte[] DecryptData(EncryptedPacket encryptedPacket)
        {
            var decryptedSessionKey = asymmetricEncryption.DecryptData(encryptedPacket.EncryptedSessionKey);

            using (var hmac = new HMACSHA256(decryptedSessionKey))
            {
                var hmacToCheck = hmac.ComputeHash(encryptedPacket.EncryptedData);
                if (!Compare(encryptedPacket.Hmac, hmacToCheck))
                {
                    throw new CryptographicException("HMAC comparison failed");
                }
            }

            var decryptedData = aesEncryption.DecryptAes(encryptedPacket.EncryptedData, decryptedSessionKey,
                encryptedPacket.Iv);
            return decryptedData;
        }

        // important not to fail on first mismatch as a hacker can use that info
        private static bool Compare(byte[] array1, byte[] array2)
        {
            var result = array1.Length == array2.Length;
            for (var i = 0; i < array1.Length && i < array2.Length; i++)
            {
                result &= array1[i] == array2[i];
            }
            return result;
        }
    }


    public class EncryptedPacket
    {
        public byte[] EncryptedSessionKey { get; set; }
        public byte[] EncryptedData { get; set; }
        public byte[] Iv { get; set; }
        public byte[] Hmac { get; set; }
    }
}
