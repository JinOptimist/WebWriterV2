using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dao.Model;
using WebWriterV2.FrontModels;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace WebWriterV2.RpgUtility
{
    public static class PathHelper
    {
        public static string PathToAvatar(long userId, string extension)
        {
            var serverPath = HttpContext.Current.Server.MapPath("~");
            return Path.Combine(serverPath, "Content", "avatar", $"{userId}.{extension}");
        }

        public static string PathToUrl(string path)
        {
            var serverPath = HttpContext.Current.Server.MapPath("~");
            var localPath = path.Remove(0, serverPath.Length);
            return "/" + localPath.Replace("\\", "/");
        }
    }
}