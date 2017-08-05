﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dao.Model;
using WebWriterV2.FrontModels;
using System.Net.Mail;
using System.Net;

namespace WebWriterV2.RpgUtility
{
    public static class EmailHelper
    {
        public static void Send(string to, string title, string body)
        {
            var smtp = new SmtpClient();
            smtp.Host = Properties.Settings.Default.EmailHost;
            smtp.Port = Properties.Settings.Default.EmailPort;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = new NetworkCredential(
                Properties.Settings.Default.NoReplayEmailName, 
                Properties.Settings.Default.NoReplayEmailPassword);
            smtp.Send(Properties.Settings.Default.NoReplayEmailName, to, title, body);
        }

        public static string ToAbsoluteUrl(this string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return String.Format("{0}://{1}{2}{3}",
                url.Scheme, url.Host, port, VirtualPathUtility.ToAbsolute(relativeUrl));
        }
    }
}