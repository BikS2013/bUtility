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
            cancelled,
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

        public bool LogExecution(IAction action, Exception ex)
        {
            Order++;
            var info = new ActionInfo
            {
                ID = ID,
                BID = BID,
                Order = Order,
                Data = action.GetData().Serialize(),
                Result = action.GetResult().Serialize(),
                Action = action.GetType().Name,
                Exception = ex.Serialize()
            };
            lock (lockObj)
            {
                actionStore[ID].Actions.Add(info);
            }
            return true;
        }
        public bool LogReversal(IAction action, Exception ex)
        {
            throw new NotImplementedException();
        }

    }
}
