using System;
using System.Collections.Generic;
using System.Text;

namespace CustomDIContainer.Services
{
    public class AzureEmailSender : IEmailSender
    {
        public string Send()
        {
            return "Email Sent via Azure";
        }
    }
}
