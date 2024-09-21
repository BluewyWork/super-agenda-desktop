using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperAgenda.Models
{
    class AdminData
    {
        [JsonProperty("_id")]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId id;

        [JsonProperty("username")]
        public string? _name;
        [JsonProperty("hashed_password")]
        public string? _password;
    }
}
