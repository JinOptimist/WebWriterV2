// Decompiled with JetBrains decompiler
// Type: CopyVk.Web.VkApiCaller
// Assembly: CopyVk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CF56976C-8F05-4DD8-92E3-E0666A14F803
// Assembly location: D:\Copy VK\CopyVk.exe

using System;
using System.Collections.Generic;
using System.Linq;
using VkApi.DTO;

namespace VkApi.Web
{
    public class VkApiCaller
    {
        private readonly WebCaller caller;
        private static string _getUserDetailsUri;
        private static string _getUserFriendsUri;
        private static string _getUserAlbumsUri;
        private static string _getUserPhotosUri;

        public VkApiCaller()
        {
            caller = new WebCaller();
            _getUserDetailsUri = Properties.Settings.Default.BaseApiUri + "/" +
                                             Properties.Settings.Default.GetUserMethod + "?uids={0}&fields=" +
                                             Properties.Settings.Default.AllUsersField;
            _getUserFriendsUri = Properties.Settings.Default.BaseApiUri + "/" +
                                             Properties.Settings.Default.GetFriendsMethod + "?uid={0}";
            _getUserAlbumsUri = Properties.Settings.Default.BaseApiUri + "/" +
                                            Properties.Settings.Default.GetAlbumsMethod +
                                            "?uid={0}&need_covers=1";
            _getUserPhotosUri = Properties.Settings.Default.BaseApiUri + "/" +
                                            Properties.Settings.Default.GetPhotosMethod + "?uid={0}&aid={1}";
        }

        public VkUser GetVkUser(long userId)
        {
            var response = caller.Get(new Uri(string.Format(_getUserDetailsUri,  userId)));
            if (string.IsNullOrEmpty(response))
                return  null;
            var responseGetUser = new JsonResponseReader<ResponseGetUser>().ReadResponse(response);
            if (responseGetUser == null || !responseGetUser.Users.Any())
                return  null;
            return responseGetUser.Users.First();
        }

        public long[] GetUserFriends(long userId)
        {
            string response = caller.Get(new Uri(string.Format(_getUserFriendsUri,  userId)));
            if (string.IsNullOrEmpty(response))
                return (long[]) null;
            return new JsonResponseReader<ResponseGetUserFriends>().ReadResponse(response).UserIds;
        }

        public VkAlbum[] GetAlbum(long userId)
        {
            string response = caller.Get(new Uri(string.Format(_getUserAlbumsUri,  userId)));
            if (string.IsNullOrEmpty(response))
                return (VkAlbum[]) null;
            ResponseGetUserAlbums responseGetUserAlbums =
                new JsonResponseReader<ResponseGetUserAlbums>().ReadResponse(response);
            if (responseGetUserAlbums == null || responseGetUserAlbums.Albums == null)
                return (VkAlbum[]) null;
            foreach (VkAlbum vkAlbum in responseGetUserAlbums.Albums)
            {
                VkPhoto[] photo = GetPhoto(userId, vkAlbum.AlbumId);
                if (photo != null)
                    vkAlbum.Photos = Enumerable.ToList<VkPhoto>((IEnumerable<VkPhoto>) photo);
            }
            return responseGetUserAlbums.Albums;
        }

        public VkPhoto[] GetPhoto(long userId, long albumId)
        {
            string response =
                caller.Get(new Uri(string.Format(_getUserPhotosUri,  userId,  albumId)));
            if (string.IsNullOrEmpty(response))
                return (VkPhoto[]) null;
            ResponseGetUserPhotos responseGetUserPhotos =
                new JsonResponseReader<ResponseGetUserPhotos>().ReadResponse(response);
            if (responseGetUserPhotos == null || responseGetUserPhotos.Photos == null)
                return (VkPhoto[]) null;
            return responseGetUserPhotos.Photos;
        }
    }
}
