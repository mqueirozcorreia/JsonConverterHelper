using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonConverterHelper
{
    public static class JsonReaderExtension
    {
        public static JObject LoadPropertyAsJObject(this JsonReader reader)
        {
            JObject jObject = null;
            while (reader.TokenType != JsonToken.StartObject)
            {
                reader.Read();
            }

            jObject = JObject.Load(reader);

            return jObject;
        }
    }
}
