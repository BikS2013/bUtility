using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT
{
    public interface IAction
    {
        bool Execute();
        bool Ask();
        bool Cancel();

        IAction NextAction();
    }

    public interface IAction<out T, out R>: IAction where R: IOperationResult
    {
        T GetData();
        R GetResult();
    }

}
