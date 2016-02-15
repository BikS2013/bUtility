using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace bUtility
{
    public class EmailService : IEmailService
    {
        readonly MailAddress Sender;
        readonly MailAddress From;
        readonly string SMTP;
        readonly int Port;
        readonly bool EnableSSL;
        bool UseAuthentication;
        string UserName;
        string Password;
        string Domain;
        readonly IResourceResolver ResourceResolver;

        public EmailService(string from, string fromDisplayName, string sender, string senderDisplayName, string smtp, int port, bool enableSSL, IResourceResolver resourceResolver)
        {
            if (from.Clear() == null)
            {
                throw new Exception("From address cannot be null or empty");
            }
            if (smtp.Clear() == null || port == 0)
            {
                throw new Exception("Invalid SMTP server configuration");
            }
            From = ExtensionsLocal.GetMailAddress(from, fromDisplayName);
            Sender = ExtensionsLocal.GetMailAddress(sender, senderDisplayName) ?? From;
            SMTP = smtp.Clear();
            Port = port;
            EnableSSL = enableSSL;
            UseAuthentication = false;
            ResourceResolver = resourceResolver;
        }

        public EmailService(string from, string fromDisplayName, string sender, string senderDisplayName, string smtp, int port, bool enableSSL) : this(from, fromDisplayName, sender, senderDisplayName, smtp, port, enableSSL, null)
        { }

        public EmailService(string from, string fromDisplayName, string smtp, int port, bool enableSSL, IResourceResolver resourceResolver) : this(from, fromDisplayName, null, null, smtp, port, enableSSL, resourceResolver)
        { }

        public EmailService(string from, string fromDisplayName, string smtp, int port, bool enableSSL) : this(from, fromDisplayName, null, null, smtp, port, enableSSL)
        { }

        public EmailService(string from, string smtp, int port, bool enableSSL, IResourceResolver resourceResolver) : this(from, null, smtp, port, enableSSL, resourceResolver)
        { }
        public EmailService(string from, string smtp, int port, bool enableSSL) : this(from, null, smtp, port, enableSSL)
        { }

        public void SetCredentials(string userName, string password, string domain)
        {
            UserName = userName.Clear();
            Password = password.Clear();
            Domain = domain.Clear();
            UseAuthentication = true;
        }

        public bool Send(SendMailRequest request)
        {
            if (request != null)
            {
                MailMessage mail = new MailMessage();

                mail.SetRecipients(request.MailTo, request.MailCC, request.MailBCC);

                mail.From = From;
                mail.Sender = Sender;

                mail.Subject = request.Subject;
                mail.SubjectEncoding = Encoding.UTF8;

                mail.IsBodyHtml = request.IsBodyHtml;
                mail.BodyEncoding = Encoding.Unicode;
                mail.Body = request.Body;

                mail.AddAttachments(request.Attachments, ResourceResolver);

                mail.AddHtmlView(request.Body, request.HtmlResources, ResourceResolver);

                return Send(mail);
            }
            throw new Exception("Request is null");
        }

        public bool Send(MailMessage mail)
        {
            if (mail != null)
            {
                using (SmtpClient client = new SmtpClient(SMTP, Port))
                {
                    client.EnableSsl = EnableSSL;
                    if (UseAuthentication)
                    {
                        client.Credentials = new System.Net.NetworkCredential(UserName, Password, Domain);
                    }
                    client.Send(mail);
                }
                return true;
            }
            throw new Exception("Mail is null");
        }
    }
}
