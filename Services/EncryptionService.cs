using System.Security.Cryptography;
using System.Text;

namespace SecurityPassword.Services
{
	public class EncryptionService
	{
        private static readonly byte[] Key = new byte[32]; // Generar una clave segura
        private static readonly byte[] IV = new byte[16];  // Generar un IV seguro

        public static string Encrypt(string text)
		{
			using Aes aes = Aes.Create();
			aes.Key = Key;
            aes.IV = IV;

            using var encryptor = aes.CreateEncryptor();
            byte[] encrypted = encryptor.TransformFinalBlock(
                Encoding.UTF8.GetBytes(text), 0, text.Length);

            return Convert.ToBase64String(encrypted);
        }

		public static string Decrypt(string text)
		{
            using var aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;

            using var decryptor = aes.CreateDecryptor();
            byte[] cipher = Convert.FromBase64String(text);
            byte[] decrypted = decryptor.TransformFinalBlock(cipher, 0, cipher.Length);

            return Encoding.UTF8.GetString(decrypted);
        }
	}
}