namespace SecurityPassword.Utils
{
	public static class InputHelper
	{
		/// <summary>
		/// Solicita una entrada de texto con un mensaje personalizado.
		/// </summary>
		public static string RequestText(string message, bool requiered = true)
		{
			string input;
			do
			{
				Console.Write(message);
				input = (Console.ReadLine() ?? String.Empty).Trim();

				if (requiered && string.IsNullOrEmpty(input))
				{
					Console.WriteLine("Entrada requerida, por favor intente nuevamente.");
				}
				else
				{
					break;
				}
			} while (requiered);

			return input;
		}

		/// <summary>
		/// Solicita una contraseña de forma segura, ocultando la entrada.
		/// </summary>
		public static string RequestCredential(string message)
		{
			Console.Write(message);
			string credential = string.Empty;

			while (true)
			{
				var key = Console.ReadKey(intercept: true);
				if (key.Key == ConsoleKey.Enter)
				{
					Console.WriteLine();
					break;
				}
				else if (key.Key == ConsoleKey.Backspace && credential.Length > 0)
				{
					credential = credential[0..^1];
					Console.Write("\b \b");
				}
				else if (!char.IsControl(key.KeyChar))
				{
					credential += key.KeyChar;
					Console.Write("*");
				}
			}

			return credential;
		}

		/// <summary>
		/// Solicita una opción de menú asegurando que sea válida.
		/// </summary>
		public static int RequestOption(string message, int minOption, int maxOption)
		{
			int option;
			while (true)
			{
				string input = RequestText(message);
				if (int.TryParse(input, out option) && option >= minOption && option <= maxOption)
				{
					break;
				}
				else
				{
					Console.WriteLine($"Por favor, ingrese un número entre {minOption} y {maxOption}.");
				}
			}
			return option;
		}
	}
}