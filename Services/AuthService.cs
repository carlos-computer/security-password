using System.Security.Cryptography;
using System.Text;


namespace SecurityPassword.Services
{
	public static class AuthService
	{
		private static readonly string Path = "claveMaestra.dat";

		/// <summary>
		/// Verifica si ya existe una clave maestra configurada.
		/// </summary>
		public static bool MasterKeyExist()
		{
			return File.Exists(Path);
		}

		/// <summary>
		/// Configura una nueva clave maestra, guardándola de forma segura.
		/// </summary>
		public static void SetMasterKey(string key)
		{
			string hashKey = HashSHA256(key);
			File.WriteAllText(Path, hashKey);
		}

		/// <summary>
		/// Verifica si la clave maestra ingresada es correcta.
		/// </summary>
		public static bool ValidateMasterKey(string key)
		{
			if (!MasterKeyExist()) return false;

			string hash = File.ReadAllText(Path);
			string hashInserted = HashSHA256(key);

			return hash == hashInserted;
		}

		/// <summary>
		/// Genera un hash SHA-256 de la clave para almacenamiento seguro.
		/// </summary>
		private static string HashSHA256(string text)
		{
			byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(text));
			return Convert.ToBase64String(bytes);
		}
	}
}