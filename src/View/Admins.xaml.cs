using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SuperAgenda.Models;
using SuperAgenda.View;
using SuperAgenda.ViewModels;

namespace SuperAgenda
{
    /// <summary>
    /// Lógica de interacción para HomeUsuarios.xaml
    /// </summary>
    public partial class HomeUsuarios : Window
    {
        AdminsVM homeUsuariosVM;
        public HomeUsuarios()
        {
            InitializeComponent();
            DataContext = new AdminsVM();

        }

        private void CrearEmpleado_Click(object sender, RoutedEventArgs e)
        {
            RegistroEmpleado registro = new RegistroEmpleado();
            registro.ShowDialog();
        }
     
        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Clientes_Click(object sender, RoutedEventArgs e)
        {
            Usuarios usuarios = new Usuarios();
            usuarios.Show();
            this.Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            HomeUsuarios x = new HomeUsuarios();
            x.Show();
            this.Close();
        }
    }
}
