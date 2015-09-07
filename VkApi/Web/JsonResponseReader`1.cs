// Decompiled with JetBrains decompiler
// Type: CopyVk.Web.JsonResponseReader`1
// Assembly: CopyVk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CF56976C-8F05-4DD8-92E3-E0666A14F803
// Assembly location: D:\Copy VK\CopyVk.exe

using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace VkApi.Web
{
    public class JsonResponseReader<T> where T : class
    {
        public T ReadResponse(string response)
        {
            if (string.IsNullOrEmpty(response))
                return default(T);
            DataContractJsonSerializer contractJsonSerializer = new DataContractJsonSerializer(typeof (T));
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(response)))
                    return (T) contractJsonSerializer.ReadObject((Stream) memoryStream);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
    }
}
