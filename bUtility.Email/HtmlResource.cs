using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public class HtmlResource
    {
        public string ContentId { get; private set; }

        public byte[] Content { get; private set; }

        public string MediaType { get; private set; }

        public HtmlResource(string contentId, byte[] content, string mediaType)
        {
            if (contentId.Clear() == null || content == null || !content.Any())
            {
                throw new Exception("Invalid input");
            }
            ContentId = contentId;
            Content = content;
            MediaType = mediaType;
        }

        public HtmlResource(string contentId, byte[] content) : this(contentId, content, null)
        { }

        public HtmlResource(string contentId, string filePath, string mediaType) : this(contentId, File.ReadAllBytes(filePath), mediaType)
        { }

        public HtmlResource(string contentId, string filePath) : this(contentId, filePath, null)
        { }

        public HtmlResource(string contentId, Stream fileStream, string mediaType) : this(contentId, ExtensionsLocal.ReadFully(fileStream), mediaType)
        { }

        public HtmlResource(string contentId, Stream fileStream) : this(contentId, fileStream, null)
        { }
    }
}
