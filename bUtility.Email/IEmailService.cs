using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public interface IEmailService
    {
        bool Send(SendMailRequest request);
    }
}
