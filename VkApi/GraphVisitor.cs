// Decompiled with JetBrains decompiler
// Type: CopyVk.GraphVisitor
// Assembly: CopyVk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: CF56976C-8F05-4DD8-92E3-E0666A14F803
// Assembly location: D:\Copy VK\CopyVk.exe

using System;
using System.Collections.Generic;
using System.Linq;
using CopyVk;
using VkApi.DTO;
using VkApi.FileUtils;
using VkApi.Web;

namespace VkApi
{
    public class GraphVisitor
    {
        private int depthSearch;
        private static GraphVisitor _instance;
        private readonly List<long> listAlreadyExist;
        private readonly VkApiCaller api;

        public static GraphVisitor Instance
        {
            get { return _instance ?? (_instance = new GraphVisitor()); }
        }

        private GraphVisitor()
        {
            this.listAlreadyExist = MyFile.AlreadyExist();
            this.api = new VkApiCaller();
        }

        public VkUser SaveAndGet(long userId)
        {
            if (this.listAlreadyExist.IndexOf(userId) > 0)
                return MyFile.Get(userId);
            VkUser vkUser = this.api.GetVkUser(userId);
            long[] userFriends = this.api.GetUserFriends(userId);
            vkUser.FriendIds = Enumerable.ToList<long>(userFriends);
            MyFile.Save(vkUser);
            this.listAlreadyExist.Add(userId);
            return vkUser;
        }

        public void Save(long userId)
        {
            if (this.listAlreadyExist.IndexOf(userId) > 0)
                return;
            VkUser vkUser = this.api.GetVkUser(userId);
            long[] userFriends = this.api.GetUserFriends(userId);
            if (userFriends != null)
                vkUser.FriendIds = Enumerable.ToList<long>(userFriends);
            Console.WriteLine("Save user id:{0}. First Name:{1}", (object) vkUser.UserId, (object) vkUser.FirstName);
            VkAlbum[] album = this.api.GetAlbum(userId);
            if (album != null)
                vkUser.Albums = Enumerable.ToList<VkAlbum>(album);
            MyFile.Save(vkUser);
            if (OldWebVk.Default.CreatePhoto)
                MyFile.SaveHtmlPhoto(vkUser);
            this.listAlreadyExist.Add(userId);
            if (this.depthSearch > OldWebVk.Default.DepthSearch)
                return;
            ++this.depthSearch;
            if (userFriends == null)
                return;
            foreach (long userId1 in userFriends)
                this.Save(userId1);
        }
    }
}