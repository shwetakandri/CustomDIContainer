using System;
using System.Collections.Generic;
using System.Text;

namespace CustomDIContainer.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailSender _emailSender;

        public NotificationService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public string Notify()
        {
            return _emailSender.Send();
        }
    }
}
