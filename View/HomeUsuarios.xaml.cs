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
        

        HomeUsuariosVM homeUsuariosVM;
        public HomeUsuarios()
        {
            InitializeComponent();
            DataContext = new HomeUsuariosVM();

        }

        private void Habitaciones_Click(object sender, RoutedEventArgs e)
        {
            HomeHabitacion homeHabitacion = new HomeHabitacion();
            homeHabitacion.Show();
            this.Close();
        }

        private void Reserva_Click(object sender, RoutedEventArgs e)
        {
            Home home = new Home();
            home.Show();
            this.Close();
        }

        private void EditarUsuario_Click(object sender, RoutedEventArgs e)
        {
            RegistroUsuario registroUsuario = new RegistroUsuario();
            registroUsuario.ShowDialog();
        }

        private void CrearEmpleado_Click(object sender, RoutedEventArgs e)
        {
            RegistroEmpleado registro = new RegistroEmpleado();
            registro.ShowDialog();
        }
        private void CrearUsuario_Click(object sender, RoutedEventArgs e)
        {
            RegistroUsuario registro = new RegistroUsuario();
            registro.ShowDialog();
        }
        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            PerfilUsuario perfil = new PerfilUsuario();
            perfil.ShowDialog();
        }
        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<UsuarioModel> users = ListBoxCustomers.ItemsSource.Cast<UsuarioModel>().ToList();

            if (!string.IsNullOrEmpty(NameTextBox.Text))
            {
                // At least one field is not empty
                if (!string.IsNullOrEmpty(NameTextBox.Text))
                {
                    // All three fields are not empty, do something
                    ListBoxCustomers.ItemsSource = FilterByName(users, NameTextBox.Text)
                        .ToList();
                }

                else if (!string.IsNullOrEmpty(NameTextBox.Text))
                {
                    // Only Name is filled, do something
                    ListBoxCustomers.ItemsSource = FilterByName(users, NameTextBox.Text);
                }

            }
            else
            {
                MessageBox.Show("At least one of the fields must be filled");
            }

        }

        

        // Filter by password
        public static List<UsuarioModel> FilterByPassword(List<UsuarioModel> usuarios, string password)
        {
            return usuarios.Where(u => u.Password == password).ToList();
        }

        // Filter by name
        public static List<UsuarioModel> FilterByName(List<UsuarioModel> usuarios, string name)
        {
            return usuarios.Where(u => u.Name == name).ToList();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // button editar
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // button eliminar
            UsuarioModel selectedItem = (UsuarioModel) ListBoxCustomers.SelectedItem;

            
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //ListBoxCustomers.ItemsSource = homeUsuariosVM.CustomersList;
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
