using DocumentFormat.OpenXml.Wordprocessing;
using HRMNS.Data.EF.Extensions;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig) => _emailConfig = emailConfig;

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        public void SendEmailWithAttack(Message message, string attachmentPath)
        {
            var emailMessage = CreateEmailMessage(message, attachmentPath);
            Send(emailMessage);
        }


        public void SendEmailHtml(Message message)
        {
            var emailMessage = CreateEmailMessageHtml(message);
            Send(emailMessage);
        }

        public void SendEmailHtmlWithAttack(Message message, string attachmentPath)
        {
            var emailMessage = CreateEmailMessageHtml(message, attachmentPath);
            Send(emailMessage);
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }

        private MimeMessage CreateEmailMessage(Message message, string attachmentPath = "")
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            if (attachmentPath == "")
            {
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            }
            else
            {
                var builder = new BodyBuilder { TextBody = message.Content };
                // Đính kèm tệp
                builder.Attachments.Add(attachmentPath);

                emailMessage.Body = builder.ToMessageBody();
            }
            return emailMessage;
        }

        private MimeMessage CreateEmailMessageHtml(Message message, string attachmentPath = "")
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.Cc.AddRange(new List<MailboxAddress> { new MailboxAddress("email", _emailConfig.From) });
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            if (attachmentPath == "")
            {
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message.Content };
            }
            else
            {
                var builder = new BodyBuilder { HtmlBody = message.Content };
                // Đính kèm tệp
                builder.Attachments.Add(attachmentPath);

                emailMessage.Body = builder.ToMessageBody();
            }

            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, MailKit.Security.SecureSocketOptions.None);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                //client.Authenticate(_emailConfig.UserName, _emailConfig.Password);

                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                //log an error message or throw an exception or both.
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
