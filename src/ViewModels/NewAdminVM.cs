﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using wpfappintermodular.api;
using SuperAgenda.Models;
using SuperAgenda.rsc;
using MongoDB.Bson;


namespace SuperAgenda.ViewModels
{
    public class NewAdminVM : INotifyPropertyChanged
    {
        private ApiService apiService;
           
        private string _nameemployee;
        private string _surnameemployee;
        private bool _adminemployee;
        private string _passwordemployee;
        private string _emailemployee;
        private string _imageemployee;
           
        public ICommand CrearEmpleado { get; }


        public string NameEmployee
        {
            get { return _nameemployee; }
            set
            {
                _nameemployee = value;
                OnPropertyChanged(NameEmployee);
            }
        }

        public string SurnameEmployee
        {
            get { return _surnameemployee; }
            set
            {
                _surnameemployee = value;
                OnPropertyChanged(SurnameEmployee);
            }
        }

        public bool AdminEmployee
        {
            get { return _adminemployee; }
            set
            {
                _adminemployee = value;
                OnPropertyChanged(nameof(AdminEmployee));
            }
        }

      

        public string PasswordEmployee
        {
            get { return _passwordemployee; }
            set
            {
                _passwordemployee = value;
                OnPropertyChanged(PasswordEmployee);
            }
        }

        public string EmailEmployee
        {
            get { return _emailemployee; }
            set
            {
                _emailemployee = value;
                OnPropertyChanged(EmailEmployee);
            }
        }

        public string ImageEmployee
        {
            get { return _imageemployee; }
            set
            {
                _imageemployee = value;
                OnPropertyChanged(ImageEmployee);
            }
        }



        public NewAdminVM()
        {
            CrearEmpleado = new RelayCommand(OnCrearEmpleado);
        }

        

        private async  void OnCrearEmpleado()
        {
            EmpleadoModel empleadoModel = new EmpleadoModel();
            empleadoModel.Name = NameEmployee;
            empleadoModel.Password = PasswordEmployee;
            empleadoModel.ID = ObjectId.GenerateNewId();

            apiService = new ApiService();
           await apiService.CreateEmployee(empleadoModel);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
