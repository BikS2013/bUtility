using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public interface IeMailService
    {
        bool EmailSend(SendMailRequest request);

    }
}
