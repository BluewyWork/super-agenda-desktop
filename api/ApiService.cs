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

        public async Task<bool> AutenticarUsuarioAsync(string email, string password)
        {
            string username = email;
            var data = new { username, password };
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/admin/auth/login", data);
            if (response.IsSuccessStatusCode)
            {
                // string jwtCookie = response.Headers.GetValues("Set-Cookie").FirstOrDefault();
                // string[] cookies = jwtCookie.Split(';');
                // Settings1.Default.JWTTokenCookie = cookies[0];
                string responseAsString = await response.Content.ReadAsStringAsync();
                dynamic result = JObject.Parse(responseAsString);
                string token = result.data.token;
                Settings1.Default.AccessToken = token;


                return true;
            }
            else
            {
                MessageBox.Show("Las credenciales no son correctas", "Error");
                return false;
            }
        }

        public async Task<bool> UpdateRoomApi(string description, int number, double pricePerNight, int beds)
        {
                if(description == null)
            {
                description = string.Empty;
            }

            var data = new { number, description, pricePerNight, beds };
            _httpClient.DefaultRequestHeaders.Add("Cookie", Settings1.Default.JWTTokenCookie);
            var response = await _httpClient.PutAsJsonAsync("/api/admin/room/update", data);
            var responseCode = response.EnsureSuccessStatusCode();
            if (responseCode.IsSuccessStatusCode)
            {
                MessageBox.Show("La habitación se ha actualizado correctamente", "Ok");
                return true;
            }
            else
            {
                MessageBox.Show("La habitación no se ha podido actualizar", "Error");
                return false;
            }
        }

        public async Task<List<UsuarioModel>> MostrarUsuarios()
        {
            List<UsuarioModel> usuarios = new List<UsuarioModel>();
            _httpClient.DefaultRequestHeaders.Add("Authorization", Settings1.Default.AccessToken);
            var response = await _httpClient.GetAsync("/api/admin/user/show/all");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic result = JObject.Parse(responseBody);
                JArray usuariosArray = result.data;

                JsonSerializerSettings settings = new JsonSerializerSettings();
                settings.Converters.Add(new ObjectIdConverter());

                //MessageBox.Show($"{usuariosArray}");

                List<UsuarioModel> list = JsonConvert.DeserializeObject<List<UsuarioModel>>(usuariosArray.ToString(), settings);
                return list;
            }
            else
            {
                MessageBox.Show("Error al mostrar los usuarios", "Error");
                return usuarios;
            }
        }

        public async Task<List<HabitacionModel>> MostrarHabitacionesApiAsync()
        {
            List<HabitacionModel> habitaciones = new List<HabitacionModel>();
            _httpClient.DefaultRequestHeaders.Add("Cookie", Settings1.Default.JWTTokenCookie);
            var response = await _httpClient.GetAsync("/api/admin/room/all");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic result = JObject.Parse(responseBody);
                JArray habitacionesArray = result.data;
                habitaciones = JsonConvert.DeserializeObject<List<HabitacionModel>>(habitacionesArray.ToString());
                return habitaciones;
            }
            else
            {
                MessageBox.Show("Error al mostrar las habitaciones ", "Error");
                return habitaciones;
            }
        }

        public async Task<List<ReservasModel>> MostrarReservasApiAsync()
        {
            List<ReservasModel> reservas = new List<ReservasModel>();
            _httpClient.DefaultRequestHeaders.Add("Cookie", Settings1.Default.JWTTokenCookie);
            var response = await _httpClient.GetAsync("/api/admin/reservation/all");
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic result = JObject.Parse(responseBody);
                JArray reservasArray = result.data;
                reservas = JsonConvert.DeserializeObject<List<ReservasModel>>(reservasArray.ToString());
                return reservas;
            }
            else
            {
                MessageBox.Show("Error al mostrar las reservas ", "Error");
                return reservas;
            }
        }

        public async Task<bool> CreateRoomApi(string description, int number,
            double pricePerNight, int beds)
        {
            ReservedDays[] reservedDays = new ReservedDays[0];
            var data = new { number, description, pricePerNight, beds, reservedDays };
            _httpClient.DefaultRequestHeaders.Add("Cookie", Settings1.Default.JWTTokenCookie);
            var response = await _httpClient.PostAsJsonAsync("/api/admin/room/new", data);
            if (!response.StatusCode.Equals("422"))
            {

                MessageBox.Show("La habitacion se ha creado correctamente", "Ok");
                return true;
            }
            else
            {
                MessageBox.Show("Error al crear la habitacion, numero duplicado", "Error");
                return false;
            }
        }

        public async Task CreateEmployee(EmpleadoModel emp)
        {
            // Use the correct settings for serialization
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new ObjectIdConverter());

            // Corrected object for serialization
            var corrected = new
            {
                _id = emp.ID,
                username = emp.Name,
                hashed_password = emp.Password
            };

            // Serialize the corrected object to JSON
            string jsonPayload = JsonConvert.SerializeObject(corrected, settings);

            // Print or log the JSON payload
            Console.WriteLine("Serialized JSON Payload: " + jsonPayload);
            MessageBox.Show("Serialized JSON Payload: " + jsonPayload);

            // Ensure the access token header is correctly set
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Add("Authorization", Settings1.Default.AccessToken);

            // Create StringContent with the serialized JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            try
            {
                // Send the request with the serialized JSON payload
                var response = await _httpClient.PostAsync("/api/admin/admin/new", content);

                if (response.StatusCode == HttpStatusCode.Created)
                {
                    MessageBox.Show("El empleado se ha creado correctamente", "Ok");
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Error al crear el empleado. Status Code: {response.StatusCode}, Response: {responseContent}", "Error");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception: {ex.Message}", "Error");
            }
        }

        public async Task<bool> DeleteRoomApi(int number)
        {
            _httpClient.DefaultRequestHeaders.Add("Cookie", Settings1.Default.JWTTokenCookie);
            var response = await _httpClient.DeleteAsync($"/api/admin/room/delete/{number}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<HabitacionModel>> BuscarHabitaciones(double? precioBuscador, int? camasBuscador, string checkIn, string checkOut)
        {
            HttpClient h = new HttpClient();
            string longurl = _httpClient.BaseAddress + "api/admin/room/search?";
            var uriBuilder = new UriBuilder(longurl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            if (camasBuscador != null) { query["beds"] = camasBuscador.ToString(); }
            if (checkIn != null) { query["checkIn"] = checkIn.ToString(); }
            if (checkOut != null) { query["checkOut"] = checkOut.ToString(); }
            if (precioBuscador != null) { query["pricePerNight"] = precioBuscador.ToString(); }
            uriBuilder.Query = query.ToString();
            longurl = uriBuilder.ToString();
            List<HabitacionModel> habitaciones = new List<HabitacionModel>();
            h.DefaultRequestHeaders.Add("Cookie", Settings1.Default.JWTTokenCookie);
            var response = await h.GetAsync(longurl);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic result = JObject.Parse(responseBody);
                JArray habitacionesArray = result.data;
                habitaciones = JsonConvert.DeserializeObject<List<HabitacionModel>>(habitacionesArray.ToString());
                return habitaciones;
            }
            else
            {
                return habitaciones;
            }

        }

        public async Task<List<EmpleadoModel>> GetEmployeeApi()
        {
            try
            {
                List < EmpleadoModel> empleados = new List<EmpleadoModel>();
                _httpClient.DefaultRequestHeaders.Add("Authorization", Settings1.Default.AccessToken);
                var response = await _httpClient.GetAsync("/api/admin/admin/show/all");

                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    dynamic result = JObject.Parse(responseBody);
                    JArray empleadoArray = result.data;
                    empleados = JsonConvert.DeserializeObject<List<EmpleadoModel>>(empleadoArray.ToString());
                    return empleados;
                }
                else
                {
                    return empleados;
                }    
            }
            catch (Exception e)
            {

                Console.WriteLine($"Error: {e.Message}");
                return null;
            }
        }

        // actualizar este metodo tambien
        public async Task UpdateEmployee(EmpleadoModel emp)
        {
            // Use the correct settings for serialization
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new ObjectIdConverter());

            // Corrected object for serialization
            var corrected = new
            {
                _id = emp.ID,
                username = emp.Name,
                hashed_password = emp.Password
            };

            // Serialize the corrected object to JSON
            string jsonPayload = JsonConvert.SerializeObject(corrected, settings);

            // Print or log the JSON payload
            Console.WriteLine("Serialized JSON Payload: " + jsonPayload);
       
            _httpClient.DefaultRequestHeaders.Add("Authorization", Settings1.Default.AccessToken);

            // Create StringContent with the serialized JSON payload
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Send the request with the serialized JSON payload
            var response = await _httpClient.PostAsync("/api/admin/admin/update", content);

            var responseCode = response.StatusCode;

            if (responseCode == HttpStatusCode.Unauthorized)
            {
                MessageBox.Show("No tienes permisos para realizar esta acción", "Error");
            }
            else if (responseCode == HttpStatusCode.OK)
            {
                MessageBox.Show("El usuario se ha actualizado correctamente", "Ok");
            }
            else
            {
                MessageBox.Show("El usuario no se ha podido actualizar", "Error");
            }
        }
        public async Task<bool> EliminarUsuario(ObjectId id)
        {
            // Ensure the Authorization header is added correctly
            if (!_httpClient.DefaultRequestHeaders.Contains("Authorization"))
            {
                _httpClient.DefaultRequestHeaders.Add("Authorization", Settings1.Default.AccessToken);
            }

            // Create the DELETE request URL with the ObjectId
            var url = $"/api/admin/admin/nuke/{id}";

            // Send the DELETE request
            var response = await _httpClient.DeleteAsync(url);

            // Return whether the request was successful
            return response.IsSuccessStatusCode;
        }

        public async Task GuardarReservaApi(ReservasModel nuevaReserva)
        {
            var data = new { nuevaReserva.RoomNumber, nuevaReserva.GuestName, nuevaReserva.GuestSurname, nuevaReserva.GuestEmail, nuevaReserva.PricePerNight, nuevaReserva.CheckIn, nuevaReserva.CheckOut };
            _httpClient.DefaultRequestHeaders.Add("Cookie", Settings1.Default.JWTTokenCookie);
            var response = await _httpClient.PostAsJsonAsync("/api/admin/reservation/create", data);

            if(response.StatusCode == HttpStatusCode.Conflict)
            {
                MessageBox.Show("La habitacion ya esta reservada", "Error");
                return;
            }

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                MessageBox.Show("Algunos datos son errroneos", "Error");
                return;

            }


            if (response.StatusCode == HttpStatusCode.Created)
            {

                MessageBox.Show("La reserva se ha creado correctamente", "OK", MessageBoxButton.OK);
                return;
            }
            else
            {
                MessageBox.Show("Error al crear la reserva", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;

            }

        }

        public async Task<List<ReservasModel>> BuscarReservasApi(string checkInBuscador, string checkOutBuscador, string correoClienteBuscador)
        {
            HttpClient h = new HttpClient();
            string longurl = _httpClient.BaseAddress + "api/admin/reservation/search?";
            var uriBuilder = new UriBuilder(longurl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            if (checkInBuscador != null) { query["checkIn"] = checkInBuscador.ToString(); }
            if (checkOutBuscador != null) { query["checkOut"] = checkOutBuscador.ToString(); }
            if (correoClienteBuscador != null) { query["email"] = correoClienteBuscador; }
            uriBuilder.Query = query.ToString();
            longurl = uriBuilder.ToString();
            List<ReservasModel> reservas = new List<ReservasModel>();
            h.DefaultRequestHeaders.Add("Cookie", Settings1.Default.JWTTokenCookie);
            var response = await _httpClient.GetAsync(longurl);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                dynamic result = JObject.Parse(responseBody);
                JArray reservasArray = result.data;
                reservas = JsonConvert.DeserializeObject<List<ReservasModel>>(reservasArray.ToString());
                return reservas;
            }
            else if(response.StatusCode == HttpStatusCode.NotFound)
            {
                MessageBox.Show("No existen reservas con esos parametros ", "404", MessageBoxButton.OK, MessageBoxImage.Information);
                return reservas;
            }
            return reservas;
        }
        public async Task<bool> DeleteReservationApi(string? id)
        { 
            _httpClient.DefaultRequestHeaders.Add("Cookie", Settings1.Default.JWTTokenCookie);
            var response = await _httpClient.DeleteAsync($"/api/admin/reservation/delete/{id}");
            if (response.StatusCode==HttpStatusCode.OK)
            {

                MessageBox.Show("La reserva se ha eliminado correctamente", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            else
            {
                MessageBox.Show("Error al eliminar la reserva", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
        
        public async Task<bool> EditarReservaApi(ReservasModel reserva )
        {
            string[] checkInArray = reserva.CheckIn.Split("T");
            string checkIn = checkInArray[0];
            string[] checkOutArray = reserva.CheckOut.Split("T");
            string checkOut = checkOutArray[0];

            var data = new
            {
                checkIn,
                checkOut
            };
            _httpClient.DefaultRequestHeaders.Add("Cookie", Settings1.Default.JWTTokenCookie);
            var response = await _httpClient.PutAsJsonAsync($"api/admin/reservation/update/{reserva._Id}", data);

            MessageBox.Show(response.ToString());
            if (response.StatusCode== HttpStatusCode.OK)
            {

                MessageBox.Show("La reserva se ha acctualizado correctamente", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
                return true;
            }
            else
            {
                MessageBox.Show("Error al actualizar la reserva", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
