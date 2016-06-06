using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.WebApi
{
    public class SimpleRequest<T> : Request<T> where T : class
    {
        public override string UserID { get; set; }
    }
}
