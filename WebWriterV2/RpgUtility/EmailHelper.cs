using System;
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
        }
    }
}