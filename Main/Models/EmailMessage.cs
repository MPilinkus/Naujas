using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using Slack.Webhooks.Core;

namespace Main.Models
{
    public class EmailMessage : Worker
    {
        public string Author { get; set; }
        public int RecieverID { get; set; }
        public string Message { get; set; }

        public void MailSend(string Author, string Reciever, string Message, string RecieverEmail)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(Author, "martynas9x@gmail.com"));
            message.To.Add(new MailboxAddress(Reciever, RecieverEmail));
            message.Subject = "Happy Birthday!";

            message.Body = new TextPart("plain")
            {
                Text = Message
            };
            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("mail.google.com", 587, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("cidbirthdaystest@gmail.com", "papartis");

                client.Send(message);
                client.Disconnect(true);
            }
        }

        
    }
}
