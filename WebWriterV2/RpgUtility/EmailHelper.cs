using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dal.Model;
using WebWriterV2.FrontModels;
using System.Net.Mail;
using System.Net;
using System.Text;
using WebWriterV2.FrontModel.Email;

namespace WebWriterV2.RpgUtility
{
    public static class EmailHelper
    {
        private static string NoReplayEmail = Properties.Settings.Default.NoReplayEmailName;
        public static void SendConfirmRegistrationEmail(string relativeUrl, string userEmail)
        {
            var url = ToAbsoluteUrl(relativeUrl);
            var title = "Интерактивная книга. Регистрация";
            var body = $"Пожалуйста подтвердите регистрацию. Для этого достаточно перейти по ссылке {url}";
            Send(userEmail, title, body);
        }

        public static void SendQuestionnaireResults(string userEmail, QuestionnaireResultEmail questionnaireResult)
        {
            var mailMessage = new MailMessage();
            mailMessage.Subject = $"{Properties.Settings.Default.QuestionnaireResultTitle} '{questionnaireResult.QuestionnaireName}'";
            mailMessage.To.Add(userEmail);
            mailMessage.From = new MailAddress(NoReplayEmail);

            var body = new StringBuilder();
            body.AppendLine($"<h3>Опросник: {questionnaireResult.QuestionnaireName}</h3>");
            body.AppendLine($"<h4>Пользователь: {questionnaireResult.UserName}</h4>");
            foreach (var questionAnswerPair in questionnaireResult.QuestionAnswerPairs) {
                var otherAnswer = questionAnswerPair.OtherAnswerText;
                if (string.IsNullOrWhiteSpace(otherAnswer) && !questionAnswerPair.AnswersText.Any()) {
                    // No answer, no need to write a question
                    continue;
                }
                body.AppendLine($"<b>{questionAnswerPair.QuestionText}</b>");
                body.AppendLine("<ul>");
                foreach (var answer in questionAnswerPair.AnswersText) {
                    body.AppendLine($"<li>{answer}</li>");
                }
                body.AppendLine("</ul>");
                if (!string.IsNullOrWhiteSpace(otherAnswer)) {
                    body.AppendLine($"<p><i>{otherAnswer}</i></p>");
                }
            }

            mailMessage.IsBodyHtml = true;
            mailMessage.Body = body.ToString();

            Send(mailMessage);
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
            to.Split(';').ToList().ForEach(emailTo =>
                Send(new MailMessage(NoReplayEmail, to, title, body)));
        }

        public static void Send(MailMessage mailMessage)
        {
            var smtp = new SmtpClient();
            smtp.Host = Properties.Settings.Default.EmailHost;
            smtp.Port = Properties.Settings.Default.EmailPort;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(
                NoReplayEmail,
                Properties.Settings.Default.NoReplayEmailPassword);

            #if !DEBUG
                smtp.Send(mailMessage);
            #endif
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

        public static void SendCoAuthorNotification(Book book, string email)
        {
            var title = string.Format(WebWriterV2.Localization.MainRu.Email_MultiAuthorNotificationTitle, book.Name);
            var body = $"Пользователь {book.Owner.Name} поделился с вами возможностью работать над произведением \"{book.Name}\"";

            Send(email, title, body);
        }

        public static void SendRecoverPassword(string email, string url)
        {
            var title = string.Format(WebWriterV2.Localization.MainRu.Email_RecoverPasswordTitle);
            var body = $"Вы можете перейти по ссылке {url}, что бы попасть на сайт под своим аккаунтам и увидеть старый пароль";

            Send(email, title, body);
        }

        public static bool IsValidEmail(string email)
        {
            try {
                var addr = new MailAddress(email);
                return addr.Address == email;
            } catch {
                return false;
            }
        }
    }
}