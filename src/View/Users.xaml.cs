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
using SuperAgenda.ViewModels;

namespace SuperAgenda.View
{
    /// <summary>
    /// Interaction logic for Usuarios.xaml
    /// </summary>
    public partial class Usuarios : Window
    {
        public Usuarios()
        {
            InitializeComponent();
            DataContext = new  UsersVM();
        }
        private void Usuarios_Click(object sender, RoutedEventArgs e)
        {
            HomeUsuarios homeUsuarios = new HomeUsuarios();
            homeUsuarios.Show();
            this.Close();
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
    }
}
