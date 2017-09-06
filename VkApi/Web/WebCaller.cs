// Decompiled with JetBrains decompiler
// Type: CopyVk.Web.WebCaller
// Assembly: CopyVk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CF56976C-8F05-4DD8-92E3-E0666A14F803
// Assembly location: D:\Copy VK\CopyVk.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using CopyVk;

namespace VkApi.Web
{
  public class WebCaller
  {
    public string Post(Uri uri, IDictionary<string, string> data)
    {
      return Post(uri, data, null);
    }

    public string Post(Uri uri, IDictionary<string, string> data, IDictionary<string, string> header)
    {
      HttpWebRequest httpWebRequest = (HttpWebRequest) WebRequest.Create(uri);
      httpWebRequest.Method = "POST";
      httpWebRequest.ContentType = "application/json; charset=utf-8";
      foreach (KeyValuePair<string, string> keyValuePair in header)
        httpWebRequest.Headers.Add(keyValuePair.Key, keyValuePair.Value);
      byte[] bytes = Encoding.UTF8.GetBytes(HelperToJson.ToJson(data));
      httpWebRequest.ContentLength = bytes.Length;
      httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
      try
      {
        string str;
        using (Stream responseStream = httpWebRequest.GetResponse().GetResponseStream())
        {
          using (StreamReader streamReader = new StreamReader(responseStream))
            str = streamReader.ReadToEnd();
        }
        return str;
      }
      catch (Exception)
      {
        return null;
      }
    }

    public string Get(Uri uri)
    {
      return Get(uri, new Dictionary<string, string>());
    }

    public string Get(Uri uri, IDictionary<string, string> header)
    {
      HttpWebRequest httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
      foreach (KeyValuePair<string, string> keyValuePair in header)
        httpWebRequest.Headers.Add(keyValuePair.Key, keyValuePair.Value);
      httpWebRequest.Timeout = 5000;
      httpWebRequest.KeepAlive = false;
      httpWebRequest.Method = "GET";
      try
      {
        using (HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse)
        {
          Encoding utF8 = Encoding.UTF8;
          using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), utF8))
            return streamReader.ReadToEnd();
        }
      }
      catch (Exception)
      {
        return null;
      }
    }
  }
}
