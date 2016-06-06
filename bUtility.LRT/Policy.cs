using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT
{
    public abstract class Policy<T,R>: IPolicy<T, R> where T: class where R : class
    {
        protected IOperationStore Store { get; set; }
        protected T Request { get; set; }
        protected R Result { get; set; }

        public List<IAction> Actions { get; set; }

        public Policy(IOperationStore store, T request)
        {
            Store = store;
            Request = request;
            Actions = new List<IAction>();
        }

        public bool Reverse()
        {
            IAction action = Actions.LastOrDefault();
            while (action != null)
            {
                if (action.Reverse())
                {
                    Actions.Remove(action);
                    action = Actions.LastOrDefault();
                }
                else
                {
                    return false;
                }
            }
            return Store.LogReversed();
        }

        public bool Execute( IAction initialAction )
        {
            IAction action = initialAction;
            while (action != null)
            {
                if (action.Execute())
                {
                    Actions.Add(action);
                    action = action.NextAction();
                }
                else
                {
                    if (action.IsReversed())
                    {
                        Reverse();

                        return false;
                    }
                }
            }
            Result = action.GetResult() as R; 
            return Store.LogCompleted();
        }

        public abstract bool Execute();

        public bool Resume()
        {
            throw new NotImplementedException();
        }
    }
}
