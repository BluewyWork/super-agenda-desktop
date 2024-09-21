using System;
using System.ComponentModel;
using System.Windows.Input;
using wpfappintermodular.api;
using SuperAgenda.Models;
using SuperAgenda.rsc;

namespace SuperAgenda.ViewModels
{
    class PerfilUsuarioVM : INotifyPropertyChanged
    {
        private ApiService apiService;
        public EmpleadoModel emp;
        private PerfilUsuario view;
        public event PropertyChangedEventHandler PropertyChanged;

        public PerfilUsuarioVM(PerfilUsuario view)
        {
            this.view = view;
            apiService = new ApiService(); 
            GuardarCommand = new RelayCommand(Guardar);
        }

        public ICommand GuardarCommand { get; }
        public ICommand AtrasCommand { get; }

         
        private async void Guardar()
        {
            emp.Name = view.VName.Text;
             await apiService.UpdateEmployee(emp);
        }

   
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}