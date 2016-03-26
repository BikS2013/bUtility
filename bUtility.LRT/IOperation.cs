using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT
{
    public interface IOperation
    {
        Guid? ID { get; }
        Guid? BID { get; }
    }
}
