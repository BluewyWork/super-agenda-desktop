
using DocumentFormat.OpenXml.Office2010.Excel;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.ComponentModel;


namespace WpfAppIntermodular.Models
{
    public class EmpleadoModel: INotifyPropertyChanged
    {
        [JsonProperty("_id")]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId id;

        [JsonProperty("username")]
        private string? _name;
        [JsonProperty("hashed_password")]
           private string? _password;
     
        public EmpleadoModel()
        {
        }

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


        public string? Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }
     
        public string? Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
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
