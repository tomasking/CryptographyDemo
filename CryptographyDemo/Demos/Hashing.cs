using System;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyDemo.Demos
{
    public class Hashing : IRunnableDemo
    {
        public void Run()
        {
            Console.WriteLine("Running Hashing Demo");
            Console.WriteLine();

            const string originalMessage1 = "Original message to hash";
            const string originalMessage2 = "Original message to hash.";
            Console.WriteLine("Original1: " + originalMessage1);
            Console.WriteLine("Original2: " + originalMessage2);
            Console.WriteLine();

            var md5Hashed1 = ComputeHashMd5(Encoding.UTF8.GetBytes(originalMessage1));
            var md5Hashed2 = ComputeHashMd5(Encoding.UTF8.GetBytes(originalMessage2));
            Console.WriteLine("MD5");
            Console.WriteLine(Convert.ToBase64String(md5Hashed1));
            Console.WriteLine(Convert.ToBase64String(md5Hashed2));
            Console.WriteLine();

            var sha1Hashed1 = ComputeHashSha1(Encoding.UTF8.GetBytes(originalMessage1));
            var sha1Hashed2 = ComputeHashSha1(Encoding.UTF8.GetBytes(originalMessage2));
            Console.WriteLine("SHA-1");
            Console.WriteLine(Convert.ToBase64String(sha1Hashed1));
            Console.WriteLine(Convert.ToBase64String(sha1Hashed2));
            Console.WriteLine();

            var sha256Hashed1 = ComputeHashSha256(Encoding.UTF8.GetBytes(originalMessage1));
            var sha256Hashed2 = ComputeHashSha256(Encoding.UTF8.GetBytes(originalMessage2));
            Console.WriteLine("SHA-256");
            Console.WriteLine(Convert.ToBase64String(sha256Hashed1));
            Console.WriteLine(Convert.ToBase64String(sha256Hashed2));
            Console.WriteLine();

            var sha512Hashed1 = ComputeHashSha512(Encoding.UTF8.GetBytes(originalMessage1));
            var sha512Hashed2 = ComputeHashSha512(Encoding.UTF8.GetBytes(originalMessage2));
            Console.WriteLine("SHA-512");
            Console.WriteLine(Convert.ToBase64String(sha512Hashed1));
            Console.WriteLine(Convert.ToBase64String(sha512Hashed2));
            Console.WriteLine();

            Console.WriteLine("HMAC");
            var random = CreateRandomNumber(32);
            var hmac = CreateHmacSha512(Encoding.UTF8.GetBytes(originalMessage1), random);
            var hmacAgain = CreateHmacSha512(Encoding.UTF8.GetBytes(originalMessage1), random);
            Console.WriteLine(Convert.ToBase64String(hmac));
            Console.WriteLine(Convert.ToBase64String(hmacAgain));
        }

        public static byte[] ComputeHashSha1(byte[] toBeHashed)
        {
            using (var sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(toBeHashed);
            }
        }

        public static byte[] ComputeHashSha256(byte[] toBeHashed)
        {
            using (var sha = SHA256.Create())
            {
                return sha.ComputeHash(toBeHashed);
            }
        }

        public static byte[] ComputeHashSha512(byte[] toBeHashed)
        {
            using (var sha = SHA512.Create())
            {
                return sha.ComputeHash(toBeHashed);
            }
        }

        public static byte[] ComputeHashMd5(byte[] toBeHashed)
        {
            using (var sha = MD5.Create())
            {
                return sha.ComputeHash(toBeHashed);
            }
        }

        public static byte[] CreateHmacSha512(byte[] toBeHashed, byte[] key)
        {
            using (var hmac = new HMACSHA512(key))
            {
                return hmac.ComputeHash(toBeHashed);
            }
        }

        private byte[] CreateRandomNumber(int length)
        {
            using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[length];
                cryptoServiceProvider.GetBytes(randomNumber);
                return randomNumber;
            }
        }
    }
}
