using System;
using System.IO;
using System.IO.Ports;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json;

namespace WebWriterV2.Utility
{
    public static class SerializeHelper
    {
        public static string Serialize<T>(T obj)
        {
            //GlobalConfiguration.Configuration.Formatters
            //    .JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;

            var serializer = new DataContractJsonSerializer(obj.GetType());
            var ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            var retVal = Encoding.UTF8.GetString(ms.ToArray());
            return retVal;
        }

        public static T Deserialize<T>(string json)
        {
            var obj = Activator.CreateInstance<T>();
            var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            var serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }
    }
}