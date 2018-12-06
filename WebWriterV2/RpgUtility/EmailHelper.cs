using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dal.Model;
using WebWriterV2.FrontModels;
using System.Net.Mail;
using System.Net;

namespace WebWriterV2.RpgUtility
{
    public static class EmailHelper
    {
        public static void SendConfirmRegistrationEmail(string relativeUrl, string userEmail)
        {
            var url = ToAbsoluteUrl(relativeUrl);
            var title = "Интерактивная книга. Регистрация";
            var body = $"Пожалуйста подтвердите регистрацию. Для этого достаточно перейти по ссылке {url}";
            Send(userEmail, title, body);
        }

        public static void SendError(Exception e)
        {
            Send(Properties.Settings.Default.AdminEmail, "На сайте проблемы!", e.ToString());
        }

        public static void SendUnexpectedRequest(Exception e, HttpRequest request)
        {
            var body = $@"Request.Url = {request.Url}
                        QueryString - {request.QueryString}
                        Headers - {request.Headers.ToString()}
                        {e.ToString()}";
            Send(Properties.Settings.Default.AdminEmail, "Опять странный запрос", body);
        }

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