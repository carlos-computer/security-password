using SecurityPassword.Services;
using SecurityPassword.Utils;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Gestor de Contraseñas ===");

        if (!AuthService.MasterKeyExist())
        {
            Console.WriteLine("Debe configurar una clave maestra.");
            string key = InputHelper.RequestCredential("Ingrese una nueva clave maestra: ");
            AuthService.SetMasterKey(key);
            Console.WriteLine("Clave maestra configurada exitosamente.");
        }

        bool auth = false;
        for (int trys = 3; trys > 0; trys--)
        {
            string key = InputHelper.RequestCredential("Ingrese su clave maestra: ");
            if (AuthService.ValidateMasterKey(key))
            {
                auth = true;
                Console.WriteLine("Acceso concedido.");
                break;
            }
            else
            {
                Console.WriteLine($"Clave incorrecta. Intentos restantes: {trys - 1}");
            }
        }

        if (!auth)
        {
            Console.WriteLine("Se ha excedido el número de intentos. Cerrando el programa.");
            return;
        }

        var credentialService = new CredentialService();

        while (true)
        {
            Console.WriteLine("\n=== Gestor de Contraseñas ===");
            Console.WriteLine("1) Agregar");
            Console.WriteLine("2) Editar");
            Console.WriteLine("3) Borrar");
            Console.WriteLine("4) Buscar");
            Console.WriteLine("5) Mostrar todos");
            Console.WriteLine("6) Exportar");
            Console.WriteLine("7) Salir");

            int option = InputHelper.RequestOption("\nSeleccione una opción: ", 1, 7);

            switch (option)
            {
                case 1:
                    Console.WriteLine("\n");
                    string service = InputHelper.RequestText("Servicio: ");
                    string username = InputHelper.RequestText("Usuario: ");
                    string email = InputHelper.RequestText("Email: ");
                    string password = InputHelper.RequestCredential("Contraseña: ");
                    credentialService.AddCredential(service, username, email, password);
                    break;
                case 2:
                    Console.WriteLine("\n");
                    string editService = InputHelper.RequestText("¿Qué serivcio quieres editar? ");
                    credentialService.EditCredential(editService);
                    break;
                case 3:
                    Console.WriteLine("\n");
                    string deleteService = InputHelper.RequestText("Ingrese el servicio para borrar: ");
                    credentialService.DeleteCredential(deleteService);
                    break;
                case 4:
                    Console.WriteLine("\n");
                    string findService = InputHelper.RequestText("Ingrese el servicio para consultar la contraseña: ");
                    credentialService.ShowCredentialByService(findService);
                    break;
                case 5:
                    Console.WriteLine("\n");
                    credentialService.ShowCredentials();
                    break;
                case 6:
                    Console.WriteLine("\n");
                    Console.WriteLine("Exportando credenciales...");
                    credentialService.ExportAllCredentials();
                    break;
                case 7:
                    Console.WriteLine("\n");
                    Console.WriteLine("Saliendo del programa...");
                    return;
                default:
                    Console.WriteLine("\n");
                    Console.WriteLine("Opción inválida.");
                    break;
            }
        }
    }
}
