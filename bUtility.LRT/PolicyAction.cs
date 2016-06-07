using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT
{
    public interface IPolicyAction
    {
        int Order { get; }
        bool Reverse();
        bool Execute();

        object GetData();
        object GetResult();

        bool IsCompleted();
        bool IsReversed();

        IPolicyAction NextAction();

    }

    public class PolicyAction<R>:IPolicyAction where R :IOperationResult
    {
        public int Order { get; private set; }

        public IAction<R> Action { get; private set; }

        object IPolicyAction.GetData(){
            return Action.GetData();
        }

        public R Result { get; private set; }

        object IPolicyAction.GetResult()
        {
            return Result;
        }
        public R GetResult()
        {
            return Result;
        }

        protected IOperationStore Store { get; set; }
        protected bool Reversed { get; set; }

        public PolicyAction(IOperationStore store, IAction<R> action, int order)
        {
            Order = order; 
            Store = store;
            Action = action;
        }

        public bool IsCompleted()
        {
            return Result?.Completed == true; 
        }

        public bool IsReversed()
        {
            return Reversed;
        }

        //public abstract R Ask();

        public bool Reverse()
        {
            try {
                Reversed = Action.Reverse();
            }
            catch(Exception ex)
            {
                Store.LogReversal(this, IsReversed(), ex);
                return IsReversed();
            }
            if (Reversed && !Store.LogReversal(this, IsReversed(), null))
            {
                return false; 
            }
            return IsReversed(); 
        }

        public bool Execute()
        {
            try {
                Result = Action.Execute();
            }
            catch(Exception ex)
            {
                Store.LogExecution(this, IsCompleted(), ex);
            }
            if ( !Store.LogExecution(this, IsCompleted(), null))
            {
                Reverse();
                return false;
            }
            return IsCompleted();
        }

        public IPolicyAction NextAction()
        {
            return Action.NextPolicyAction(Store, Result, Order+1);
        }

    }
}
