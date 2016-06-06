using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT.InMemoryStore
{
    public class Store : IOperationStore
    {
        enum opStatus
        {
            inProgress,
            completed,
            reversed,
            pendingFailed
        }
        class OperationInfo
        {
            public Guid BID { get; set; }
            public Guid ID { get; set; }

            public opStatus Status { get; set; }

            public List<ActionInfo> Actions { get; set; }
        }
        class ActionInfo
        {
            public Guid BID { get; set; }
            public Guid ID { get; set; }
            public string Action { get; set; }
            public string Data { get; set; }
            public string Result { get; set; }
            public string Exception { get; set; }
            public int Order { get; set; }
            public bool Completed { get; set; }
            public bool Reversed { get; set; }
            public DateTime VersionTime { get; set; }
            public bool IsLast { get; set; }
        }

        static object lockObj = new object();
        static Dictionary<Guid, OperationInfo> actionStore = new Dictionary<Guid, OperationInfo>();

        Guid ID { get; set; }
        Guid BID { get; set; }

        int Order { get; set; }

        public Store(Guid id, Guid bid)
        {
            ID = id;
            BID = bid;
            Order = 0;

            lock (lockObj)
            {
                actionStore.Add(id, new OperationInfo
                {
                    ID = id,
                    BID = bid,
                    Status = opStatus.inProgress,
                    Actions = new List<ActionInfo>()
                });
            }
        }

        ActionInfo get(IAction action)
        {
            lock (lockObj)
            {
                return actionStore[ID].Actions.FirstOrDefault( a => a.Order == action.Order && a.IsLast);
            }
        }

        bool log(IAction action, bool completed, bool reversed, Exception ex)
        {
            var info = new ActionInfo
            {
                ID = ID,
                BID = BID,
                Order = action.Order,
                Data = action.GetData().Serialize(),
                Result = action.GetResult().Serialize(),
                Action = action.GetType().Name,
                Exception = ex.Serialize(),
                Completed = (ex== null) && completed,
                Reversed = (ex == null) && reversed,
                VersionTime = DateTime.Now,
                IsLast = true
            };
            lock (lockObj)
            {
                actionStore[ID].Actions.Add(info);
            }
            return true;
        }

        public bool LogExecution(IAction action, bool completed, Exception ex)
        {
            return log(action, completed, false, ex);
        }
        public bool LogReversal(IAction action, bool reversed, Exception ex)
        {
            var info = get(action);
            var result = log(action, false, reversed, ex);
            if (result && info != null)
            {
                info.IsLast = false; 
            }
            return result;
        }

        public bool LogCompleted()
        {
            lock (lockObj)
            {
                actionStore[ID].Status = opStatus.completed;
            }
            return true;
        }

        public bool LogReversed()
        {
            lock (lockObj)
            {
                actionStore[ID].Status = opStatus.reversed;
            }
            return true;
        }
    }
}
