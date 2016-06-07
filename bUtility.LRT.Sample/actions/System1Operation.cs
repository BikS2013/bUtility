using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT.Sample
{
    public class System1Operation : IAction<System1Result>
    {
        ISystem System { get; set; }

        public ValidationResult Data { get; set; }

        public object GetData()
        {
            return Data;
        }
        public System1Operation(ISystem system, ValidationResult data)
        {
            System = system;
            Data = data;
        }
        public System1Result Ask()
        {
            return System.System1Ask(Data);
        }

        public System1Result Execute()
        {
            return System.System1Execute(Data);
        }

        public bool Reverse()
        {
            return System.System1Reverse(Data);
        }

        public IPolicyAction NextPolicyAction(IOperationStore store, object data, int order)
        {
            return new PolicyAction<System2Result>(store, new System2Operation(System, data as System1Result), order);

        }

    }
}
