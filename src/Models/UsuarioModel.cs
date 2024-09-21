using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SuperAgenda.Models
{
    public class UsuarioModel : INotifyPropertyChanged
    {
        [JsonProperty("_id")]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId id;
        

        public string? hashed_password;
[JsonProperty("username")]
        public string? username;


        public ObjectId ID
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(ID));

                }
            }
        }

        public string? Password
        {
            get { return hashed_password; }
            set
            {
                if (hashed_password != value)
                {
                    hashed_password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public string? Name 
        {
            get { return username; }
            set
            {
                if(username != value)
                {
                    username = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
