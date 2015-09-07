// Decompiled with JetBrains decompiler
// Type: CopyVk.HelperToJson
// Assembly: CopyVk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CF56976C-8F05-4DD8-92E3-E0666A14F803
// Assembly location: D:\Copy VK\CopyVk.exe

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using VkApi.DTO;

namespace VkApi
{
  public static class HelperToJson
  {
    public static string ToJson(IDictionary<string, string> data)
    {
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append("{");
      foreach (KeyValuePair<string, string> keyValuePair in data)
      {
        stringBuilder.Append("\"" + keyValuePair.Key + "\":\"" + keyValuePair.Value + "\"");
        if (!keyValuePair.Equals(data.Last()))
          stringBuilder.Append(",");
      }
      stringBuilder.Append("}");
      return stringBuilder.ToString();
    }

    public static string ToJson(VkUser user)
    {
      if (user == null)
        return string.Empty;
      return new JavaScriptSerializer().Serialize(user);
    }
  }
}
