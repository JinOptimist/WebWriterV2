// Decompiled with JetBrains decompiler
// Type: CopyVk.DTO.VkPhoto
// Assembly: CopyVk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CF56976C-8F05-4DD8-92E3-E0666A14F803
// Assembly location: D:\Copy VK\CopyVk.exe

using System;
using System.Runtime.Serialization;

namespace VkApi.DTO
{
  [DataContract]
  [Serializable]
  public class VkPhoto
  {
    [DataMember(Name = "pid")]
    public long PhotoId { get; set; }

    [DataMember(Name = "aid")]
    public long AlbumId { get; set; }

    [DataMember(Name = "owner_id")]
    public long OwnerId { get; set; }

    [DataMember(Name = "src_small")]
    public string SrcSmall { get; set; }

    [DataMember(Name = "src")]
    public string Src { get; set; }

    [DataMember(Name = "src_big")]
    public string SrcBig { get; set; }

    [DataMember(Name = "text")]
    public string Text { get; set; }

    [DataMember(Name = "created")]
    public long Created { get; set; }
  }
}
