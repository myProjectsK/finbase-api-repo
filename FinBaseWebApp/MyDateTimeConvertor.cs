using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace FinBaseWebApp
{
    public class MyDateTimeConvertor : DateTimeConverterBase    
    {

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return DateTime.Parse(reader.Value.ToString());    
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(((DateTime)value).ToString("dd-MMM-yyyy"));       
        }
    }
}

