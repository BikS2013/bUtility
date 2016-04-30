using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.WebApi
{
    public class ThrottlingException: Exception
    {
        public ThrottlingException( string message): base(message)
        {

        }

        public ThrottlingException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
