using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SuperAgenda;
using SuperAgenda.Models;

namespace wpfappintermodular.api
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string cokie;

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:8001");
        }

        public async Task<bool> AuthenticateUser(string username, string password)
        {
            try
            {
                var data = new { username, password };
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/admin/auth/login", data);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Incorrect Credentials", "Error");
                    return false;
                }

                string responseAsString = await response.Content.ReadAsStringAsync();
                dynamic result = JObject.Parse(responseAsString);
                string token = result.data.token;
                Settings1.Default.AccessToken = token;

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e}");
                return false;
            }
        }

        public async Task<List<UsuarioModel>> MostrarUsuarios()
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", Settings1.Default.AccessToken);
                var response = await _httpClient.GetAsync("/api/admin/user/show/all");

                if (!response.IsSuccessStatusCode)
                {

                    MessageBox.Show("Error al mostrar los usuarios", "Error");
                    return new List<UsuarioModel>();

                }

                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic result = JObject.Parse(responseBody);
                JArray usuariosArray = result.data;

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new ObjectIdConverter());

                return JsonConvert.DeserializeObject<List<UsuarioModel>>(usuariosArray.ToString(), settings);
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
                return new List<UsuarioModel>();
            }
        }

        // note: here directly shows success
        public async Task CreateEmployee(EmpleadoModel emp)
        {
            // necessary to serialize correctly
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new ObjectIdConverter());

            var corrected = new
            {
                _id = emp.ID,
                username = emp.Name,
                hashed_password = emp.Password
            };

            string jsonPayload = JsonConvert.SerializeObject(corrected, settings);

            // needed for authorization
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", Settings1.Default.AccessToken);

            // process payload again
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("/api/admin/admin/new", content);

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al crear el empleado. Status Code: {response.StatusCode}, Response: {responseContent}", "Error");
                    return;
                }

                MessageBox.Show("El empleado se ha creado correctamente", "Ok");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Error");
            }
        }

        public async Task<List<EmpleadoModel>> GetEmployeeApi()
        {
            try
            {
                List<EmpleadoModel> empleados = new List<EmpleadoModel>();
                _httpClient.DefaultRequestHeaders.Add("Authorization", Settings1.Default.AccessToken);
                var response = await _httpClient.GetAsync("/api/admin/admin/show/all");

                if (!response.IsSuccessStatusCode)
                {
                    return new List<EmpleadoModel>();

                }

                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic result = JObject.Parse(responseBody);
                JArray empleadoArray = result.data;
                return JsonConvert.DeserializeObject<List<EmpleadoModel>>(empleadoArray.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message}");
                return new List<EmpleadoModel>();
            }
        }

        // directly shows success
        public async Task UpdateEmployee(EmpleadoModel emp)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new ObjectIdConverter());

            var corrected = new
            {
                _id = emp.ID,
                username = emp.Name,
                hashed_password = emp.Password
            };

            string jsonPayload = JsonConvert.SerializeObject(corrected, settings);
            _httpClient.DefaultRequestHeaders.Add("Authorization", Settings1.Default.AccessToken);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("/api/admin/admin/update", content);

                var responseCode = response.StatusCode;

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Update Employee Failed", "Error");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
            }

        }
        public async Task<bool> EliminarUsuario(ObjectId id)
        {
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", Settings1.Default.AccessToken);
            }

            try
            {
                var response = await _httpClient.DeleteAsync($"/api/admin/admin/nuke/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}");
                return false;
            }
        }
    }
}
