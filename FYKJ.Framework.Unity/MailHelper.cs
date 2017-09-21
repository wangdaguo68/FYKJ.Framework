namespace FYKJ.Framework.Utility
{
    using System;
    using System.Net;
    using System.Net.Mail;

    public static class MailHelper
    {
        public static void SendEmail(string subject, string content)
        {
        }

        private static void SendEmail(string clientHost, string emailAddress, string receiveAddress, string userName, string password, string subject, string body)
        {
            MailMessage message = new MailMessage {
                From = new MailAddress(emailAddress)
            };
            message.To.Add(new MailAddress(receiveAddress));
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;
            message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
            SmtpClient client = new SmtpClient {
                Host = clientHost,
                Credentials = new NetworkCredential(userName, password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            try
            {
                client.SendAsync(message, null);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}

