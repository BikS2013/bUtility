using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT
{
    public abstract class PolicyAction<T, R> : IAction<T, R> where R :IOperationResult
    {
        public int Order { get; private set; }

        protected IOperationStore Store { get; set; }
        protected T Data { get; set; }
        protected R Result { get; set; }
        protected bool Reversed { get; set; }

        public PolicyAction(IOperationStore store, T data, int order)
        {
            Order = order; 
            Store = store;
            Data = data;
        }

        public bool IsCompleted()
        {
            return Result?.Completed == true; 
        }

        public bool IsReversed()
        {
            return Reversed;
        }

        object IAction.GetData()
        {
            return Data;
        }

        object IAction.GetResult()
        {
            return Result;
        }
        public T GetData()
        {
            return Data;
        }

        public R GetResult()
        {
            return Result;
        }
        public abstract bool Ask();

        protected abstract bool ReverseInternal();
        public bool Reverse()
        {
            try {
                Reversed = ReverseInternal();
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

        protected abstract R ExecuteInternal();
        public bool Execute()
        {
            try {
                Result = ExecuteInternal();
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

        public abstract IAction NextAction();

    }
}
