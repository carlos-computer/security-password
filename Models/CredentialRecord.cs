namespace SecurityPassword.Models
{
	public class CredentialRecord(string service, string username, string email, string password)
	{
		public string Service { get; set; } = service;
		public string Username { get; set; } = username;
		public string Email { get; set; } = email;
		public string Password { get; set; } = password;
	}
}