using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT.Sample
{
    public class System3Operation : IAction<Result>
    {
        ISystem System { get; set; }

        public System2Result Data { get; set; }

        public object GetData()
        {
            return Data;
        }
        public System3Operation(ISystem system, System2Result data) 
        {
            System = system;
            Data = data;
        }
        public Result Ask()
        {
            return System.System3Ask(Data);
        }

        public Result Execute()
        {
            return System.System3Execute(Data);
        }

        public bool Reverse()
        {
            return System.System3Reverse(Data);
        }

        public IPolicyAction NextPolicyAction(IOperationStore store, object data, int order)
        {
            return null;
        }
    }
}
