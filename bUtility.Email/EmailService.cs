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
        bool UseAuthentication;
        string UserName;
        string Password;
        string Domain;
        readonly EmailServiceParams ServiceParams;
        readonly IResourceResolver ResourceResolver;

        public EmailService(EmailServiceParams serviceParams, IResourceResolver resourceResolver)
        {
            if (serviceParams.FromAddress.Clear() == null)
            {
                throw new Exception("From address cannot be null or empty");
            }
            if (serviceParams.SmtpAddress.Clear() == null || serviceParams.SmtpPort == 0)
            {
                throw new Exception("Invalid SMTP server configuration");
            }
            ServiceParams = serviceParams;
            From = ExtensionsLocal.GetMailAddress(serviceParams.FromAddress, serviceParams.FromDisplayName);
            Sender = ExtensionsLocal.GetMailAddress(serviceParams.SenderAddress, serviceParams.SenderDisplayName) ?? From;
            UseAuthentication = false;
            ResourceResolver = resourceResolver;
        }

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
                using (SmtpClient client = new SmtpClient(ServiceParams.SmtpAddress, ServiceParams.SmtpPort))
                {
                    client.EnableSsl = ServiceParams.Ssl;
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
