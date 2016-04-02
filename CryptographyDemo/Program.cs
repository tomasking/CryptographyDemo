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
<<<<<<< HEAD
            IRunnableDemo demo = new HybridEncryptionDemo();
            
=======
            IRunnableDemo demo = new FromCertificates();

>>>>>>> 8bfa060fbb3aa30a0371166f2a4eb7f833ef22e7
            demo.Run();

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }
    }
}
