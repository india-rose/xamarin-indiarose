using System;
using IndiaRose.Interfaces;
using Lotz.Xam.Messaging;

namespace IndiaRose.Services
{
    public class EmailService : IEmailService
    {
        public void Send(string title, string address, string body)
        {
            var emailTask = MessagingPlugin.EmailMessenger;
            if (emailTask.CanSendEmail)
            {
                var message = new EmailMessageBuilder().To(address).Subject(title).Body(body).Build();
                emailTask.SendEmail(message);
            }
            else
            {
                throw new Exception("Client is not allowed to send e-mails");
            }
        }
    }
}
