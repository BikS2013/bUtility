using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public static class ExtensionsLocal
    {
        public static MailMessage SetRecipients(this MailMessage mail, IEnumerable<string> mailTo, IEnumerable<string> mailCc, IEnumerable<string> mailBcc)
        {
            if (mail != null)
            {
                mail.To.SetRecipients(mailTo);
                mail.CC.SetRecipients(mailCc);
                mail.Bcc.SetRecipients(mailBcc);
            }
            return mail;
        }

        private static MailAddressCollection SetRecipients(this MailAddressCollection mac, IEnumerable<string> addresses)
        {
            if (mac != null && addresses != null && addresses.Any())
            {
                foreach (var a in addresses)
                {
                    if (a.Clear() != null)
                    {
                        mac.Add(a);
                    }
                }
            }
            return mac;
        }

        public static MailMessage AddAttachments(this MailMessage mail, List<string> attachments, IResourceResolver resourceResolver)
        {
            if (mail != null && attachments != null && attachments.Any())
            {
                if (resourceResolver == null)
                {
                    throw new Exception("ResourceResolver not set");
                }
                attachments.ForEach(a =>
                {
                    MailAttachment att = resourceResolver.GetMailAttachment(a);
                    if (att != null)
                    {
                        MemoryStream ms = new MemoryStream(att.Content);
                        if (att.MediaType.Clear() != null)
                        {
                            mail.Attachments.Add(new Attachment(ms, att.Name, att.MediaType));
                        }
                        else
                        {
                            mail.Attachments.Add(new Attachment(ms, att.Name));
                        }
                    }
                });
            }
            return mail;
        }

        public static MailMessage AddHtmlView(this MailMessage mail, string htmlBody, List<string> resources, IResourceResolver resourceResolver)
        {
            if (mail != null && htmlBody.Clear() != null && resources != null && resources.Any())
            {
                if (resourceResolver == null)
                {
                    throw new Exception("ResourceResolver not set");
                }
                AlternateView av = AlternateView.CreateAlternateViewFromString(htmlBody, null, MediaTypeNames.Text.Html);
                resources.ForEach(r =>
                {
                    HtmlResource hr = resourceResolver.GetHtmlResource(r);
                    if (hr != null)
                    {
                        MemoryStream ms = new MemoryStream(hr.Content);
                        LinkedResource lr;
                        if (hr.MediaType.Clear() != null)
                        {
                            lr = new LinkedResource(ms, hr.MediaType.Clear());
                        }
                        else
                        {
                            lr = new LinkedResource(ms);
                        }
                        lr.ContentId = hr.ContentId;
                        av.LinkedResources.Add(lr);
                    }
                    mail.AlternateViews.Add(av);
                });
            }
            return mail;
        }

        public static MailAddress GetMailAddress(string address, string displayName)
        {
            if (address.Clear() != null)
            {
                if (displayName.Clear() != null)
                {
                    return new MailAddress(address.Clear(), displayName.Clear(), Encoding.UTF8);
                }
                return new MailAddress(address.Clear());
            }
            return null;
        }

        public static byte[] ReadFully(Stream input)
        {
            if (input == null)
            {
                return null;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
