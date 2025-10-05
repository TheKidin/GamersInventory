using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Text.Json;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GamersInventory
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            InitializeUsersFile();
        }

        private string GetDesktopPath()
        {
            // Obtener la ruta del Escritorio
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "GamersInventory");
        }

        private void InitializeUsersFile()
        {
            // Crear carpeta en el Escritorio si no existe
            string desktopFolder = GetDesktopPath();
            if (!Directory.Exists(desktopFolder))
            {
                Directory.CreateDirectory(desktopFolder);
            }

            string usersFilePath = Path.Combine(desktopFolder, "users.json");

            if (!File.Exists(usersFilePath))
            {
                var defaultUsers = new UsersData
                {
                    usuarios = new List<User>
                    {
                        new User { id = 1, usuario = "admin", password = "123456", nombre = "Administrador" },
                        new User { id = 2, usuario = "vendedor", password = "123456", nombre = "Vendedor" }
                    }
                };

                string json = JsonSerializer.Serialize(defaultUsers, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(usersFilePath, json);
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string usuario = txtUsuario.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
            {
                ShowMessage("Error", "Por favor completa todos los campos");
                return;
            }

            if (AuthenticateUser(usuario, password))
            {
                // Abrir dashboard
                var dashboard = new DashboardWindow();
                dashboard.Activate();
                this.Close();
            }
            else
            {
                ShowMessage("Error", "Usuario o contraseña incorrectos");
            }
        }

        private bool AuthenticateUser(string usuario, string password)
        {
            try
            {
                string usersFilePath = Path.Combine(GetDesktopPath(), "users.json");
                if (!File.Exists(usersFilePath)) return false;

                string json = File.ReadAllText(usersFilePath);
                var usersData = JsonSerializer.Deserialize<UsersData>(json);

                if (usersData?.usuarios == null) return false;

                foreach (var user in usersData.usuarios)
                {
                    if (user.usuario == usuario && user.password == password)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        private async void ShowMessage(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }

    public class UsersData
    {
        public List<User> usuarios { get; set; } = new List<User>();
    }

    public class User
    {
        public int id { get; set; }
        public string usuario { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string nombre { get; set; } = string.Empty;
    }
}