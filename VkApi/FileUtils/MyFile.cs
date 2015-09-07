// Decompiled with JetBrains decompiler
// Type: CopyVk.FileUtils.MyFile
// Assembly: CopyVk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CF56976C-8F05-4DD8-92E3-E0666A14F803
// Assembly location: D:\Copy VK\CopyVk.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using CopyVk;
using VkApi.DTO;

namespace VkApi.FileUtils
{
  public static class MyFile
  {
    public static void InitFolder()
    {
      string path = FileVk.Default.PathToSave + Constant.UserFolder;
      if (Directory.Exists(path))
        return;
      Directory.CreateDirectory(path);
    }

    public static void Save(VkUser user)
    {
      if (user == null || user.UserId < 1L)
        return;
      string path = FileVk.Default.PathToSave + (object) Constant.UserFolder + "\\" + (string) (object) user.UserId + Constant.Extension;
      if (File.Exists(path))
        return;
      string str = HelperToJson.ToJson(user);
      using (StreamWriter streamWriter = new StreamWriter((Stream) File.Create(path)))
        streamWriter.Write(str);
    }

    public static VkUser Get(long id)
    {
      return new JavaScriptSerializer().Deserialize<VkUser>(File.ReadAllText(FileVk.Default.PathToSave + (object) Constant.UserFolder + "\\" + (string) (object) id + Constant.Extension));
    }

    public static List<long> AlreadyExist()
    {
      string[] files = Directory.GetFiles(FileVk.Default.PathToSave + Constant.UserFolder);
      long id = 0L;
      return Enumerable.ToList<long>(Enumerable.Select<string, long>(Enumerable.Where<string>(Enumerable.Select<string, string>((IEnumerable<string>) files, (Func<string, string>) (file => Path.GetFileNameWithoutExtension(file))), (Func<string, bool>) (name => long.TryParse(name, out id))), (Func<string, long>) (name => id)));
    }

    public static void SaveHtmlPhoto(VkUser user)
    {
      string path = string.Format(FileVk.Default.PathToSave + Constant.UserFolder + "\\{0}-{1}-photo.html", (object) user.UserId, (object) user.LastName);
      if (File.Exists(path))
        File.Delete(path);
      using (StreamWriter streamWriter = new StreamWriter((Stream) File.Create(path)))
      {
        if (user.Albums == null)
          return;
        foreach (VkAlbum vkAlbum in user.Albums)
        {
          if (vkAlbum.Photos == null)
            break;
          foreach (VkPhoto vkPhoto in vkAlbum.Photos)
          {
            if (vkPhoto != null && !string.IsNullOrEmpty(vkPhoto.SrcBig))
              streamWriter.WriteLine("<img src={0} />", (object) vkPhoto.SrcBig);
          }
        }
      }
    }
  }
}
