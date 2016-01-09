using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;

namespace CryptographyDemo.Demos
{
    public class SecurePasswordStorage : IRunnableDemo
    {
        public void Run()
        {
            Console.WriteLine("Running SecurePasswordStorage");

            const string password = "somepassword";
            byte[] salt = GenerateSalt();

            var hashedPassword = HashPasswordWithSalt(Encoding.UTF8.GetBytes(password), salt);

            Console.WriteLine("Password: " + password);
            Console.WriteLine("Hashed Password: " + Convert.ToBase64String(hashedPassword));
            Console.WriteLine();

            Console.WriteLine("Using derivation function");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var derivationPassword = HashPasswordWithDerivationFunction(Encoding.UTF8.GetBytes(password), salt, 150000);
            stopwatch.Stop();
            Console.WriteLine(Convert.ToBase64String(derivationPassword));
            Console.WriteLine("Completed in " + stopwatch.Elapsed);
        }

        public static byte[] GenerateSalt()
        {
            const int saltLength = 32;
            using (var randomGenerator = new RNGCryptoServiceProvider())
            {
                var randomNumber = new byte[saltLength];
                randomGenerator.GetBytes(randomNumber);
                return randomNumber;
            }
        }

        public static byte[] Combine(byte[] first, byte[] second)
        {
            var ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second,0,ret,first.Length, second.Length);
            return ret;
        }

        public static byte[] HashPasswordWithSalt(byte[] toBeHashed, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Combine(toBeHashed, salt));
            }
        }

        public static byte[] HashPasswordWithDerivationFunction(byte[] password, byte[] salt, int rounds)
        {
            using (var rfc2898 = new Rfc2898DeriveBytes(password, salt, rounds))
            {
                return rfc2898.GetBytes(32);
            }
        }
    }
}
