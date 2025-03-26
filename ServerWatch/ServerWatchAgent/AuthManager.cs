using Microsoft.Win32;
using System;
using System.Security.Cryptography;

namespace ServerWatchAgent
{
    public class AuthManager
    {
        public string Guid { get; private set; }
        public string PublicKey { get; private set; }
        public string PrivateKey { get; private set; }

        private readonly string RegistryPath = @"SOFTWARE\Cosys\ServerWatch";

        public AuthManager()
        {
            using (var key = Registry.LocalMachine.OpenSubKey(RegistryPath))
            {
                if (key != null)
                {
                    Guid = key.GetValue("Guid") as string;
                    PublicKey = key.GetValue("PublicKey") as string;
                    PrivateKey = key.GetValue("PrivateKey") as string;

                    if (string.IsNullOrEmpty(Guid) || string.IsNullOrEmpty(PublicKey) || string.IsNullOrEmpty(PrivateKey))
                    {
                        throw new InvalidOperationException("Registry values are incomplete or corrupted.");
                    }
                }
                else
                {
                    GenerateAndStoreKeys();
                }
            }
        }

        private void GenerateAndStoreKeys()
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                try
                {
                    Guid = System.Guid.NewGuid().ToString();
                    PublicKey = rsa.ToXmlString(false);
                    PrivateKey = rsa.ToXmlString(true);

                    using (var key = Registry.LocalMachine.CreateSubKey(RegistryPath))
                    {
                        key.SetValue("Guid", Guid);
                        key.SetValue("PublicKey", PublicKey);
                        key.SetValue("PrivateKey", PrivateKey);
                    }
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
    }
}
