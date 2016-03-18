using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT
{
    public interface IOperationStore
    {
        bool LogExecution(IAction action, Exception ex);
        bool LogReversal(IAction action, Exception ex);
    }
}
