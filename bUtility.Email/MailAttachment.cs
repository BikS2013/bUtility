using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public class MailAttachment
    {
        public string Name { get; private set; }

        public byte[] Content { get; private set; }

        public string MediaType { get; private set; }

        public MailAttachment(string name, byte[] content, string mediaType)
        {
            if (name.Clear() == null || content == null || !content.Any())
            {
                throw new Exception("Invalid input");
            }
            Name = name;
            Content = content;
            MediaType = mediaType;
        }

        public MailAttachment(string name, byte[] content) : this(name, content, null)
        { }

        public MailAttachment(string name, string filePath, string mediaType) : this(name, File.ReadAllBytes(filePath), mediaType)
        { }

        public MailAttachment(string name, string filePath) : this(name, filePath, null)
        { }

        public MailAttachment(string filePath) : this(filePath, filePath)
        { }

        public MailAttachment(string name, Stream fileStream, string mediaType) : this(name, ExtensionsLocal.ReadFully(fileStream), mediaType)
        { }

        public MailAttachment(string name, Stream fileStream) : this(name, fileStream, null)
        { }
    }
}
