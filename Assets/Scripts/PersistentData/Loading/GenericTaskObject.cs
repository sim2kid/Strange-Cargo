using Creature.Brain;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentData.Loading
{
    public class GenericTaskObject : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => false;
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ITaskObject);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var DataType = new UnspecifiedTask();
            serializer.Populate(reader, DataType);
            return DataType;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new InvalidOperationException("Use default serialization.");
        }
    }
}
