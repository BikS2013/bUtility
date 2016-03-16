using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT
{
    public abstract class PolicyAction<T, R> : IAction<T, R> where R :IOperationResult
    {
        protected IOperationStore Store { get; set; }
        protected T Data { get; set; }
        protected R Result { get; set; }

        public PolicyAction(IOperationStore store, T data)
        {
            Store = store;
            Data = data;
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

        public abstract bool Cancel();

        protected abstract R ExecuteInternal();
        public bool Execute()
        {
            Result = ExecuteInternal();
            return Result?.Completed == true; 
        }

        public abstract IAction NextAction();
    }
}
