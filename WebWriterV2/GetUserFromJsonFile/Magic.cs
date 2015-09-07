using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using Dao.Model;
using Newtonsoft.Json.Linq;
using WebWriterV2.Models;

namespace WebWriterV2.GetUserFromJsonFile
{
    public class Magic
    {
        public static List<ExternalUser> DoMagic()
        {
            var result = new List<ExternalUser>();

            var files = Directory.GetFiles(@"c:\Users\Pavel_Lvou@epam.com\Documents\Visual Studio 2013\Projects\WebWriterV2\WebWriterV2\Json\");
            foreach (var filePath in files)
            {
                var json = File.ReadAllText(filePath);
                var user = Deserialize<ExternalUser>(json);
                result.Add(user);
            }

            return result;
        }

        public static string Serialize<T>(T obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, obj);
            string retVal = Encoding.UTF8.GetString(ms.ToArray());
            return retVal;
        }

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }
    }
}