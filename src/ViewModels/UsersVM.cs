﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfappintermodular.api;
using SuperAgenda.Models;
using SuperAgenda.View;

namespace SuperAgenda.ViewModels
{
   
    public class UsersVM: INotifyPropertyChanged
    {
        private ApiService apiService;
        private ObservableCollection<UsuarioModel> _usuarios;
        public ObservableCollection<UsuarioModel> Usuarios
        {
            get { return _usuarios; }
            set
            {
                _usuarios = value;
                OnPropertyChanged(nameof(Usuarios));
            }
        }

        public UsersVM()
        {
            MostrarUsuarios();
        }
        private async void MostrarUsuarios()
        {
            try
            {
                apiService = new ApiService();
                List<UsuarioModel> usuarios = await apiService.MostrarUsuarios();
                Usuarios = new ObservableCollection<UsuarioModel>(usuarios);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al buscar habitaciones: {ex.Message}");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
