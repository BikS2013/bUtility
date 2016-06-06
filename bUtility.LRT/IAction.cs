using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT
{
    public interface IAction
    {
        int Order { get; }

        bool IsCompleted();
        bool IsReversed();

        bool Execute();
        //bool Ask();
        bool Reverse();

        IAction NextAction();

        object GetData();

        object GetResult();
    }

    public interface IAction<out T, out R>: IAction where R: IOperationResult
    {
        new T GetData();
        new R GetResult();
    }

}
