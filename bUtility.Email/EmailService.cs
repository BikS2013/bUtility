using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public class EmailService : IeMailService
    {
        private readonly string from;
        private readonly string smtp;
        private readonly int port;
        private readonly bool enableSSL;
        public EmailService(string sender, string SMTP, int SMTPPort, bool MailSSL)
        {
            from = sender;
            smtp = SMTP;
            port = SMTPPort;
            enableSSL = MailSSL;
        }
        public EmailService(string SMTP, int SMTPPort, bool MailSSL)
        {
            smtp = SMTP;
            port = SMTPPort;
            enableSSL = MailSSL;
        }

        public bool EmailSend(SendMailRequest request, string sender)
        {
            MailMessage mail = new MailMessage();
            request.MailTo?.ForEach(to => mail.To.Add(to));
            request.MailCC?.ForEach(cc => mail.CC.Add(cc));
            request.MailBCC?.ForEach(bcc => mail.Bcc.Add(bcc));
            mail.Sender = new MailAddress(sender);
            mail.From = new MailAddress(sender);
            mail.Subject = request.Subject;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.BodyEncoding = System.Text.Encoding.UTF8;

            request.Attachments?.ForEach(data =>
            {
                if (data != null)
                {
                    mail.Attachments.Add(new Attachment(data, MediaTypeNames.Application.Octet));
                }
            });

            mail.IsBodyHtml = true;
            mail.BodyEncoding = System.Text.Encoding.Unicode;
            mail.Body = request.Body;

            if (smtp.Clear() != null && port != 0)
            {
                SmtpClient smtpClient = new SmtpClient(smtp);
                smtpClient.Port = port;
                smtpClient.EnableSsl = enableSSL;

                smtpClient.Send(mail);
                return true;

            }
            else
            {
                throw new Exception("Invalid Configuration.");
            }
        }

        public bool EmailSend(SendMailRequest request)
        {
            return EmailSend(request, from);
        }
    }
}
