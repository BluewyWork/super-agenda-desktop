﻿using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

public class ObjectIdConverter : JsonConverter<ObjectId>
{
    public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
    {
        JObject jo = new JObject
        {
            { "$oid", value.ToString() }
        };
        jo.WriteTo(writer);
    }

    public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var jo = JObject.Load(reader);
        var oidToken = jo["$oid"];
        return oidToken != null ? new ObjectId(oidToken.ToString()) : ObjectId.Empty;
    }
}
