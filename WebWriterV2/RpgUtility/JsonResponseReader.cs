using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace WebWriterV2.RpgUtility
{
    public class JsonResponseReader<T> where T : class
    {
        public T ReadResponse(string response)
        {
            if (string.IsNullOrEmpty(response))
                return default(T);
            var contractJsonSerializer = new DataContractJsonSerializer(typeof(T));
            try {
                using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                    return (T)contractJsonSerializer.ReadObject(memoryStream);
            } catch (Exception ex) {
                return default(T);
            }
        }
    }
}