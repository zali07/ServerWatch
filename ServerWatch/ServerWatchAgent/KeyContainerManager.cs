using Microsoft.Win32;
using System;
using System.Security.Cryptography;
using System.Text;

namespace ServerWatchAgent
{
    /// <summary>
    /// Manages credentials for authentication.
    /// </summary>
    public static class KeyContainerManager
    {
        private const string RegistryBasePath = @"SOFTWARE\Cosys\ServerWatchAgent";
        private const string GuidValueName = "KeyContainerGuid";
        private static string _guid;

        /// <summary>
        /// This GUID is used to authenticate the agent to the server.
        /// </summary>
        public static string Guid => _guid;

        /// <summary>
        /// Static constructor to initialize the GUID on startup.
        /// </summary>
        static KeyContainerManager()
        {
            _guid = LoadOrCreateGuid();
        }

        /// <summary>
        /// Get or create the key in the machine key store.
        /// </summary>
        private static RSACryptoServiceProvider GetOrCreateKey()
        {
            var cspParams = new CspParameters
            {
                KeyContainerName = _guid,
                Flags = CspProviderFlags.UseMachineKeyStore
            };

            var rsa = new RSACryptoServiceProvider(2048, cspParams)
            {
                PersistKeyInCsp = true
            };

            return rsa;
        }

        public static string SignData(string message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            byte[] dataBytes = Encoding.UTF8.GetBytes(message);

            using (var rsa = GetOrCreateKey())
            {
                // For .NET Framework 4.x, SignData overload with CryptoConfig
                var signature = rsa.SignData(dataBytes, CryptoConfig.MapNameToOID("SHA256"));
                return Convert.ToBase64String(signature);
            }
        }

        public static string GetPublicKey()
        {
            using (var rsa = GetOrCreateKey())
            {
                RSAParameters pubParams = rsa.ExportParameters(false);
                string modulus = Convert.ToBase64String(pubParams.Modulus);
                string exponent = Convert.ToBase64String(pubParams.Exponent);

                return $"{modulus}|{exponent}";
            }
        }

        /// <summary>
        /// Load or create a GUID in the registry.
        /// </summary>
        private static string LoadOrCreateGuid()
        {
            using (var baseKey = Registry.LocalMachine.CreateSubKey(RegistryBasePath))
            {
                var guidFromRegistry = baseKey.GetValue(GuidValueName) as string;
                if (!string.IsNullOrEmpty(guidFromRegistry))
                {
                    return guidFromRegistry;
                }

                string newGuid = System.Guid.NewGuid().ToString();
                baseKey.SetValue(GuidValueName, newGuid);
                return newGuid;
            }
        }
    }
}
