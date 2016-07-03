using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT
{
    public interface IAction
    {
        IPolicyAction NextPolicyAction(IOperationStore store, object data, int order);
    }

    public interface IAction<out R> : IAction where R : IOperationResult
    {

        object GetData();
        R Execute();

        bool Reverse();
        R Ask();
    }
    public interface IAction<out T, out R>: IAction where R: IOperationResult
    {
        T Data { get; }

        R Execute();

        bool Reverse();
        R Ask();
    }

}
