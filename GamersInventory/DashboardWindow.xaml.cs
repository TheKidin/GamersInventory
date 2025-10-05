using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace GamersInventory
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            this.InitializeComponent();
            InitializeInventoryFile();
            CargarProductos();
        }

        private string GetDesktopPath()
        {
            // Obtener la ruta del Escritorio
            return Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), "GamersInventory");
        }

        private void InitializeInventoryFile()
        {
            // Crear carpeta en el Escritorio si no existe
            string desktopFolder = GetDesktopPath();
            if (!Directory.Exists(desktopFolder))
            {
                Directory.CreateDirectory(desktopFolder);
            }

            string inventoryFilePath = Path.Combine(desktopFolder, "inventory.json");

            if (!File.Exists(inventoryFilePath))
            {
                var defaultInventory = new
                {
                    productos = new[]
                    {
                        new {
                            id = 1,
                            nombre = "The Last of Us Part II",
                            categoria = "Videojuego",
                            plataforma = "PlayStation",
                            genero = "Acción",
                            precio = 899.00,
                            stock = 5
                        },
                        new {
                            id = 2,
                            nombre = "Xbox Series X",
                            categoria = "Consola",
                            plataforma = "Xbox",
                            genero = "",
                            precio = 10499.00,
                            stock = 2
                        },
                        new {
                            id = 3,
                            nombre = "Audífonos Gamer Pro",
                            categoria = "Accesorio",
                            plataforma = "Multiplataforma",
                            genero = "",
                            precio = 1299.00,
                            stock = 8
                        }
                    }
                };

                string json = JsonSerializer.Serialize(defaultInventory, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(inventoryFilePath, json);
            }
        }

        private void CargarProductos()
        {
            // Por ahora solo muestra los productos de ejemplo
            // Más adelante cargaremos desde el JSON
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e)
        {
            MostrarMensaje("Agregar Producto", "Esta función se implementará pronto");
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            // Volver al login
            var mainWindow = new MainWindow();
            mainWindow.Activate();
            this.Close();
        }

        private async void MostrarMensaje(string titulo, string mensaje)
        {
            var dialog = new ContentDialog
            {
                Title = titulo,
                Content = mensaje,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }
    }
}
