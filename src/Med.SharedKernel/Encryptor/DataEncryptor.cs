using System.Security.Cryptography;
using System.Text;

namespace Med.SharedKernel.Encryptor
{
    public static class DataEncryptor
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("dD7vC9wR4uT8eQ1KmX6sZjNpLtGaYh2B");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("A9xL3eVmNzQ5TfUc");

        public static string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            var encryptor = aes.CreateEncryptor();
            var bytes = Encoding.UTF8.GetBytes(plainText);
            var encrypted = encryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string encryptedText)
        {
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            var decryptor = aes.CreateDecryptor();
            var bytes = Convert.FromBase64String(encryptedText);
            var decrypted = decryptor.TransformFinalBlock(bytes, 0, bytes.Length);
            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
