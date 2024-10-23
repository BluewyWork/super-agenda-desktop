using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using SuperAgenda.Models;
using wpfappintermodular.api;
using SuperAgenda.rsc;

namespace SuperAgenda.ViewModels
{
    class AdminsVM : INotifyPropertyChanged
    {
        private ApiService apiService = new ApiService();
        private ObservableCollection<EmpleadoModel> _empleados;
        private ObservableCollection<EmpleadoModel> _filterEmp;
        private string _searchName;
        private string _searchSurname;
        private string _searchEmail;
        private EmpleadoModel _empleadoSelecionado;

        public ICommand EditUserCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand FilterListCommand { get; }
        public ICommand LimpiarCommand { get; }

        public AdminsVM()
        {
            ShowEmployee();
            FilterListCommand = new RelayCommand(FilterEmpleados);
            LimpiarCommand = new RelayCommand(limparCampos);
            EditUserCommand = new RelayCommand(EditUser, () => EmpleadoSelecionado != null);
            DeleteUserCommand = new RelayCommand(DeleteUser, () => EmpleadoSelecionado != null);
        }

        private async void limparCampos()
        {
            SearchName = "";
            SearchSurname = "";
            SearchEmail = "";
        }

        public void EditUser()
        {
            if (EmpleadoSelecionado == null)
               return;

            PerfilUsuario p = new PerfilUsuario(EmpleadoSelecionado);

            p.Show();
        }

        public async void DeleteUser()
        {
            if (EmpleadoSelecionado == null) return;

            if (EmpleadoSelecionado.Name == null) return;

            await apiService.EliminarUsuario(EmpleadoSelecionado.ID);
        }

        public ObservableCollection<EmpleadoModel> Empleados
        {
            get { return _empleados; }
            set
            {
                if (_empleados != value)
                {
                    _empleados = value;
                    OnPropertyChanged(nameof(Empleados));
                }
            }
        }

        public void FilterEmpleados()
        {

            IEnumerable<EmpleadoModel> filteredEmpleados = Empleados;

            if (!string.IsNullOrEmpty(SearchName))
            {
                filteredEmpleados = filteredEmpleados.Where(e => e.Name.Contains(SearchName));
                Empleados = new ObservableCollection<EmpleadoModel>(filteredEmpleados);
            }
        }

        private async void ShowEmployee()
        {
            try
            {
                List<EmpleadoModel> empleados = await apiService.GetEmployeeApi();
                Empleados = new ObservableCollection<EmpleadoModel>(empleados);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar empleados: {ex.Message}");
            }
        }

        public string SearchName
        {
            get { return _searchName; }
            set
            {
                if (_searchName != value)
                {
                    _searchName = value;
                    OnPropertyChanged(nameof(SearchName));
                    FilterEmpleados();
                }
            }
        }

        public string SearchSurname
        {
            get { return _searchSurname; }
            set
            {
                if (_searchSurname != value)
                {
                    _searchSurname = value;
                    OnPropertyChanged(nameof(SearchSurname));
                    FilterEmpleados();
                }
            }
        }

        public string SearchEmail
        {
            get { return _searchEmail; }
            set
            {
                if (_searchEmail != value)
                {
                    _searchEmail = value;
                    OnPropertyChanged(nameof(SearchEmail));
                    FilterEmpleados();
                }
            }
        }

        public EmpleadoModel EmpleadoSelecionado
        {
            get { return _empleadoSelecionado; }
            set
            {
                if (_empleadoSelecionado != value)
                {
                    _empleadoSelecionado = value;
                    OnPropertyChanged(nameof(EmpleadoSelecionado));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
