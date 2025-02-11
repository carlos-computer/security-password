using Newtonsoft.Json;

using SecurityPassword.Models;

namespace SecurityPassword.Data
{
	public class FileStorage(string archivePath)
	{
		private readonly string _path = archivePath;

		public void SaveCredential(CredentialRecord entry)
		{	
			var credentials = LoadCredentials();
			credentials.Add(entry);
			File.WriteAllText(_path, JsonConvert.SerializeObject(credentials));
		}

		public List<CredentialRecord> LoadCredentials()
		{
			if (!File.Exists(_path)) return [];
			string json = File.ReadAllText(_path);
			return JsonConvert.DeserializeObject<List<CredentialRecord>>(json) ?? [];
		}
	}
}