// Decompiled with JetBrains decompiler
// Type: CopyVk.WebVk
// Assembly: CopyVk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CF56976C-8F05-4DD8-92E3-E0666A14F803
// Assembly location: D:\Copy VK\CopyVk.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VkApi
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
  internal sealed class OldWebVk : ApplicationSettingsBase
  {
    private static OldWebVk defaultInstance = (OldWebVk) SettingsBase.Synchronized((SettingsBase) new OldWebVk());

    public static OldWebVk Default
    {
      get
      {
        OldWebVk webVk = OldWebVk.defaultInstance;
        return webVk;
      }
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("https://api.vk.com/method")]
    public string BaseApiUri
    {
      get
      {
        return (string) this["BaseApiUri"];
      }
    }

    [DefaultSettingValue("users.get")]
    [DebuggerNonUserCode]
    [ApplicationScopedSetting]
    public string GetUserMethod
    {
      get
      {
        return (string) this["GetUserMethod"];
      }
    }

    [DebuggerNonUserCode]
    [DefaultSettingValue("uid,first_name,last_name,nickname,screen_name,sex,bdate,city,country,timezone,photo,photo_medium,photo_big,has_mobile,rate,contacts,education,online,counters,home_phone,mobile_phone,university,university_name,faculty,faculty_name,graduation")]
    [ApplicationScopedSetting]
    public string AllUsersField
    {
      get
      {
        return (string) this["AllUsersField"];
      }
    }

    [DefaultSettingValue("friends.get")]
    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    public string GetFriendsMethod
    {
      get
      {
        return (string) this["GetFriendsMethod"];
      }
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("photos.getAlbums")]
    public string GetAlbumsMethod
    {
      get
      {
        return (string) this["GetAlbumsMethod"];
      }
    }

    [DefaultSettingValue("photos.get")]
    [DebuggerNonUserCode]
    [ApplicationScopedSetting]
    public string GetPhotosMethod
    {
      get
      {
        return (string) this["GetPhotosMethod"];
      }
    }

    [DefaultSettingValue("6174887")]
    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    public long StartId
    {
      get
      {
        return (long) this["StartId"];
      }
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("1")]
    public int DepthSearch
    {
      get
      {
        return (int) this["DepthSearch"];
      }
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("False")]
    public bool CreatePhoto
    {
      get
      {
        return (bool) this["CreatePhoto"];
      }
    }
  }
}
