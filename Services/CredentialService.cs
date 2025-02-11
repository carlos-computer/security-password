using Newtonsoft.Json;
using SecurityPassword.Models;
using SecurityPassword.Utils;

namespace SecurityPassword.Services
{
	public class CredentialService
	{
        private readonly string _filePath = "credentials.enc";
        private List<CredentialRecord> _credentials;

        public CredentialService()
        {
            _credentials = LoadCredentials();
        }

        private List<CredentialRecord> LoadCredentials()
        {
            if (!File.Exists(_filePath)) return [];

            string encryptedData = File.ReadAllText(_filePath);
            try
            {
                string decryptedData = EncryptionService.Decrypt(encryptedData);
                var credentials = JsonConvert.DeserializeObject<List<CredentialRecord>>(decryptedData) ?? [];
                return [.. credentials.OrderBy(c => c.Service ?? string.Empty)];
            }
            catch
            {
                return [];
            }
        }

        private void SaveCredentials()
        {
            string jsonData = JsonConvert.SerializeObject(_credentials);
            string encryptedData = EncryptionService.Encrypt(jsonData);
            File.WriteAllText(_filePath, encryptedData);
        }

        public void AddCredential(string service, string username, string email, string password)
		{
            _credentials.Add(new CredentialRecord(service, username, email, password));
            SaveCredentials();
            Console.WriteLine($"Contraseña para el servicio '{service}' agregada exitosamente.");
		}

		public void ShowCredentials()
		{
			if (_credentials.Count == 0)
			{
				Console.WriteLine("No hay contraseñas almacenadas.");
				return;
			}

			foreach (var record in _credentials)
			{
				Console.WriteLine($"Servicio: {record.Service}, Usuario: {record.Username}, Email: {record.Email}, Contraseña: {record.Password}");
			}
		}

		public void ShowCredentialByService(string service)
		{
			var record = _credentials.Find(c => c.Service.Equals(service, StringComparison.OrdinalIgnoreCase));

			if (record != null)
			{
				Console.WriteLine($"Servicio: {record.Service}, Usuario: {record.Username}, Email: {record.Email}, Contraseña: {record.Password}");
			}
			else
			{
				Console.WriteLine($"No se encontró el servicio '{service}'.");
			}
		}

		public void DeleteCredential(string service)
		{
            var record = _credentials.Find(c => c.Service.Equals(service, StringComparison.OrdinalIgnoreCase));

            if (record == null)
            {
                Console.WriteLine($"No se encontró el servicio '{service}'.");
                return;
            }

            // Mostrar la información del servicio que se va a borrar
            Console.WriteLine("\nSe eliminará la siguiente credencial:");
            Console.WriteLine($"Servicio: {record.Service}");
            Console.WriteLine($"Usuario: {record.Username}");
            Console.WriteLine($"Email: {record.Email}");

            // Pedir confirmación
            Console.Write("\n¿Está seguro que desea eliminar esta credencial? (s/n): ");
            var response = Console.ReadLine()?.ToLower();

            if (response == "s")
            {
                _credentials.Remove(record);
                SaveCredentials(); // Guardar los cambios en el archivo encriptado
                Console.WriteLine($"\nCredencial para el servicio '{service}' eliminada exitosamente.");
            }
            else
            {
                Console.WriteLine("\nOperación cancelada.");
            }
        }

		public void ExportAllCredentials()
		{
            if (_credentials.Count == 0)
            {
                Console.WriteLine("No hay contraseñas almacenadas.");
                return;
            }

            using (StreamWriter writer = new ("credentials_export.txt"))
            {
                writer.WriteLine("\n// Service // Username // Email // Password //\n");
                foreach (var credential in _credentials)
                {
                    writer.WriteLine($"\n{credential.Service} - {credential.Username} - {credential.Email} - {credential.Password}");
                }
            }

            Console.WriteLine("Contraseñas exportadas exitosamente.");
        }

        public void EditCredential(string service)
        {
            var record = _credentials.Find(c => c.Service.Equals(service, StringComparison.OrdinalIgnoreCase));
            if (record == null)
            {
                Console.WriteLine($"No se encontró el servicio '{service}'.");
                return;
            }

            Console.WriteLine("Deje en blanco para mantener el valor actual.");

            string username = InputHelper.RequestText($"Usuario ({record.Username}): ", false);
            string email = InputHelper.RequestText($"Email ({record.Email}): ", false);
            string password = InputHelper.RequestCredential($"Nueva contraseña (dejar vacío para mantener): ");

            record.Username = string.IsNullOrEmpty(username) ? record.Username : username;
            record.Email = string.IsNullOrEmpty(email) ? record.Email : email;
            record.Password = string.IsNullOrEmpty(password) ? record.Password : password;

            SaveCredentials();
            Console.WriteLine($"Credencial para '{service}' actualizada exitosamente.");
        }

    }
}