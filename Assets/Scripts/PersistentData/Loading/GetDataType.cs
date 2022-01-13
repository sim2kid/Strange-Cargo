using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PersistentData.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Loading
{
    public class GetDataType : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ISaveData);
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new InvalidOperationException("Use default serialization.");
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var DataType = default(ISaveData);
            // This switch is where we decode all possible types of data storage
            switch (jsonObject["DataType"].Value<string>()) 
            {
                case "Location":
                    DataType = new LocationData();
                    break;
                case "RandomColor":
                    DataType = new ColorData();
                    break;
                case "Creature":
                    DataType = new CreatureData();
                    break;
                case "Prefab":
                    DataType = new PrefabData();
                    break;
                case "Rigidbody":
                    DataType = new RigidbodyData();
                    break;
                case "Persistent":
                    DataType = new ReusedData();
                    break;

            }
            serializer.Populate(jsonObject.CreateReader(), DataType);
            return DataType;
        }
    }
}