using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT
{
    public interface IOperationStore
    {
        bool LogExecution(IPolicyAction action, bool completed, Exception ex);
        bool LogReversal(IPolicyAction action, bool reversed, Exception ex);

        bool LogCompleted();
        bool LogReversed();

    }
}
