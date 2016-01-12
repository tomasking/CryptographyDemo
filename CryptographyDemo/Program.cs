using System;
using CryptographyDemo.Demos;

namespace CryptographyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //IRunnableDemo demo = new RandomNumbers();
            //IRunnableDemo demo = new Hashing();
            //IRunnableDemo demo = new SecurePasswordStorage();
            //IRunnableDemo demo = new SymmetricEncryption();
            //IRunnableDemo demo = new AsymmetricEncryption();
            //IRunnableDemo demo = new HybridEncryption();
            IRunnableDemo demo = new FromCertificates();

            demo.Run();

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
