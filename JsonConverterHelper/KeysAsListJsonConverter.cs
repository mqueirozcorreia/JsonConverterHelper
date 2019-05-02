using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonConverterHelper
{
    /// <summary>
    /// Converte um lista Json que o primeiro field é um identificador (que sera ignorado) e o valor é o objeto em si
    /// </summary>
    /// <typeparam name="T">Objeto</typeparam>
    /// <typeparam name="K">Lista do Objeto</typeparam>
    public class KeysAsListJsonConverter<T, K> : JsonConverter
        where T : new()
        where K : List<T>, new()
    {
        public override object ReadJson(JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            K result = new K();

            if (reader.TokenType == JsonToken.StartObject)
            {
                JObject jItem = reader.LoadPropertyAsJObject();
                JEnumerable<JToken> jTokenList = jItem.Children<JToken>();
                foreach (JToken jToken in jTokenList)
                {
                    T item = new T();

                    LoadItem(result, serializer, jToken, item);
                }
            }

            return result;
        }

        protected virtual void LoadItem(K result,
            JsonSerializer serializer,
            JToken jToken,
            T item)
        {
            serializer.Populate(jToken.First.CreateReader(), item);
            ReadItem(jToken, item);
            result.Add(item);
        }

        public virtual void ReadItem(JToken jToken, T item)
        {
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var list = value as K;

            if (list != null)
            {
                writer.WriteStartObject();
                foreach (var item in list)
                {
                    var property = item.GetType()
                        .GetProperty("Id", 
                        BindingFlags.IgnoreCase | 
                        BindingFlags.DeclaredOnly | 
                        BindingFlags.Public | 
                        BindingFlags.Instance);

                    if (property != null)
                    {
                        var id = property.GetValue(item);
                        writer.WritePropertyName(Convert.ToString(id));
                    }
                    else
                    {
                        writer.WritePropertyName("IdNotFound");
                    }
                    JToken t = JToken.FromObject(item);
                    t.WriteTo(writer);
                }
                writer.WriteEndObject();
            }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType is K;
        }
    }
}