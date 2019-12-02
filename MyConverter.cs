
//using Newtonsoft.Json;
//using SDG.Framework.IO.Serialization;
//using System;
//using System.Collections;
//using System.IO;

//namespace ItemRestrictorAdvanced
//{
//    public class MyConverter : JsonConverter
//    {
//        public override bool CanConvert(Type objectType)
//        {
//            return true;
//        }

//        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
//        {
//            byte[] byteArr = new byte[0];

//            while (reader.Read())
//            {
//                if (reader.TokenType == JsonToken.EndArray)
//                    break;
//                byte[] tempArr = new byte[byteArr.Length];
//                for (byte j = 0; j < tempArr.Length; j++)
//                    tempArr[j] = byteArr[j];
//                byteArr = new byte[tempArr.Length + 1];
//                for (byte n = 0; n < tempArr.Length; n++)
//                    byteArr[n] = tempArr[n];
//                byteArr[byteArr.Length - 1] = byte.Parse(serializer.Deserialize(reader).ToString());
//            }

//            return byteArr;
//        }

//        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
//        {
//            byte[] byteArray = value as byte[];
//            string[] strArray = new string[byteArray.Length];
//            for (byte i = 0; i < byteArray.Length; i++)
//                strArray[i] = byteArray[i].ToString();

//            serializer.Serialize(writer, strArray);
//        }
//    }
//}
