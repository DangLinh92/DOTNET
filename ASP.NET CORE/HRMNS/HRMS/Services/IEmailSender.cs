using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        void SendEmail(Message message);
    }
}
