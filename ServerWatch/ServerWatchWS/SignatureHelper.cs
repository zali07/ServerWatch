using System.Security.Cryptography;
using System.Text;

namespace ServerWatchAPI
{
    public static class SignatureHelper
    {
        public static bool VerifyPayload(string payload, string signatureBase64, string publicKey)
        {
            byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
            byte[] signatureBytes = Convert.FromBase64String(signatureBase64);

            string[] keyParts = publicKey.Split('|');

            if (keyParts.Length != 2)
            {
                throw new InvalidOperationException("Invalid public key format.");
            }

            RSAParameters rsaParams = new RSAParameters
            {
                Modulus = Convert.FromBase64String(keyParts[0]),
                Exponent = Convert.FromBase64String(keyParts[1]),
            };

            using var rsa = RSA.Create();
            rsa.ImportParameters(rsaParams);

            return rsa.VerifyData(payloadBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
