using Newtonsoft.Json;
using System;

namespace Net.Chdk.Json
{
    public sealed class HexStringJsonConverter : JsonConverter
    {
        private string Format { get; }

        public HexStringJsonConverter()
            : this("x")
        {
        }

        public HexStringJsonConverter(string format)
        {
            Format = $"0x{{0:{format}}}";
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(uint).Equals(objectType) || typeof(ulong).Equals(objectType);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var str = string.Format(Format, value);
            writer.WriteValue(str);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var str = reader.Value as string;
            if (str == null)
            {
                if (objectType == typeof(uint?) || objectType == typeof(ulong?))
                    return null;
                throw new JsonSerializationException();
            }
            try
            {
                var fromBase = GetBase(str);
                if (objectType == typeof(uint) || objectType == typeof(uint?))
                    return Convert.ToUInt32(str, fromBase);
                return Convert.ToUInt64(str, fromBase);
            }
            catch (Exception ex)
            {
                throw new JsonSerializationException("Error deserializing", ex);
            }
        }

        private static int GetBase(string str)
        {
            if (str.Length < 2 || str[0] != '0')
                return 10;

            switch (str[1])
            {
                case 'b':
                case 'B':
                    return 2;
                case 'x':
                case 'X':
                    return 16;
                default:
                    return 8;
            }
        }
    }
}
