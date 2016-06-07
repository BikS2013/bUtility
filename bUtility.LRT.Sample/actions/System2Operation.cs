using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT.Sample
{
    public class System2Operation : IAction<System2Result>
    {
        ISystem System { get; set; }

        public System1Result Data { get; set; }

        public object GetData()
        {
            return Data;
        }
        public System2Operation(ISystem system, System1Result data)
        {
            Data = data;
            System = system;
        }
        public System2Result Ask()
        {
            return System.System2Execute(Data);
        }

        public System2Result Execute()
        {
            return System.System2Execute(Data);
        }

        public bool Reverse()
        {
            return System.System2Reverse(Data);
        }

        public IPolicyAction NextPolicyAction(IOperationStore store, object data, int order)
        {
            return new PolicyAction<Result>(store, new System3Operation(System, data as System2Result), order);
        }
    }
}
