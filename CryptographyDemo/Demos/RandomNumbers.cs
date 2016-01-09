using System;
using System.Security.Cryptography;

namespace CryptographyDemo.Demos
{
    public class RandomNumbers : IRunnableDemo
    {
        public void Run()
        {
            Console.WriteLine("Running RandomNumbers Demo");
            Console.WriteLine();

            Console.WriteLine("Pseudo way");
            PseudoRandomWay();
            Console.WriteLine();

            Console.WriteLine("Using RNGCryptoServiceProvider");
            SecureWay(32);
        }

        public void PseudoRandomWay()
        {
            Random random = new Random(DateTime.UtcNow.Millisecond);
            for (var i = 0; i < 10; i++)
                Console.Write(random.Next(0, 10) + " ");
        }

        public void SecureWay(int length)
        {
            for (int i = 0; i < 10; i++)
            {
                var random = CreateRandomNumber(length);
                Console.WriteLine(Convert.ToBase64String(random) + " ");
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
