// Decompiled with JetBrains decompiler
// Type: CopyVk.DTO.VkUser
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
  public class VkUser
  {
    [DataMember(Name = "uid")]
    public long UserId { get; set; }

    [DataMember(Name = "first_name")]
    public string FirstName { get; set; }

    [DataMember(Name = "last_name")]
    public string LastName { get; set; }

    [DataMember(Name = "sex")]
    public Sex Sex { get; set; }

    [DataMember(Name = "nickname")]
    public string Nickname { get; set; }

    [DataMember(Name = "screen_name")]
    public string ScreenName { get; set; }

    [DataMember(Name = "bdate")]
    public string Birthday { get; set; }

    [DataMember(Name = "city")]
    public long CityId { get; set; }

    [DataMember(Name = "country")]
    public long CountryId { get; set; }

    [DataMember(Name = "timezone")]
    public float Timezone { get; set; }

    [DataMember(Name = "photo")]
    public string Photo { get; set; }

    [DataMember(Name = "photo_medium")]
    public string PhotoMedium { get; set; }

    [DataMember(Name = "photo_big")]
    public string PhotoBig { get; set; }

    [DataMember(Name = "has_mobile")]
    public bool HasMobile { get; set; }

    [DataMember(Name = "university")]
    public long UniversityId { get; set; }

    [DataMember(Name = "university_name")]
    public string UniversityName { get; set; }

    [DataMember(Name = "faculty")]
    public long FacultyId { get; set; }

    [DataMember(Name = "faculty_name")]
    public string FacultyName { get; set; }

    [DataMember(Name = "graduation")]
    public long Graduation { get; set; }

    [DataMember(Name = "education_form")]
    public string EducationForm { get; set; }

    [DataMember(Name = "education_status")]
    public string EducationStatus { get; set; }

    public List<long> FriendIds { get; set; }

    public List<VkAlbum> Albums { get; set; }
  }
}
