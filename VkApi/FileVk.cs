// Decompiled with JetBrains decompiler
// Type: CopyVk.FileVk
// Assembly: CopyVk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CF56976C-8F05-4DD8-92E3-E0666A14F803
// Assembly location: D:\Copy VK\CopyVk.exe

using System.CodeDom.Compiler;
using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace CopyVk
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
  internal sealed class FileVk : ApplicationSettingsBase
  {
    private static FileVk defaultInstance = (FileVk) SettingsBase.Synchronized((SettingsBase) new FileVk());

    public static FileVk Default
    {
      get
      {
        FileVk fileVk = FileVk.defaultInstance;
        return fileVk;
      }
    }

    [ApplicationScopedSetting]
    [DebuggerNonUserCode]
    [DefaultSettingValue("d:\\Copy VK\\DB\\")]
    public string PathToSave
    {
      get
      {
        return (string) this["PathToSave"];
      }
    }
  }
}
