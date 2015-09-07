// Decompiled with JetBrains decompiler
// Type: CopyVk.DTO.VkAlbum
// Assembly: CopyVk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CF56976C-8F05-4DD8-92E3-E0666A14F803
// Assembly location: D:\Copy VK\CopyVk.exe

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VkApi.DTO
{
  [DataContract]
  [Serializable]
  public class VkAlbum
  {
    [DataMember(Name = "aid")]
    public long AlbumId { get; set; }

    [DataMember(Name = "thumb_id")]
    public long ThumbId { get; set; }

    [DataMember(Name = "owner_id")]
    public long OwnerId { get; set; }

    [DataMember(Name = "title")]
    public string Title { get; set; }

    [DataMember(Name = "description")]
    public string Description { get; set; }

    [DataMember(Name = "created")]
    public long Created { get; set; }

    [DataMember(Name = "updated")]
    public long Updated { get; set; }

    [DataMember(Name = "size")]
    public int Size { get; set; }

    [DataMember(Name = "thumb_src")]
    public string ThumbSrc { get; set; }

    public List<VkPhoto> Photos { get; set; }
  }
}
