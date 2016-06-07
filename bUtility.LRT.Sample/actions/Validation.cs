using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT.Sample
{
    public class Validation : IAction<ValidationResult>
    {
        ISystem System { get; set; }

        public Request Data { get; set; }

        public object GetData()
        {
            return Data;
        }
        public Validation(ISystem system, Request data) 
        {
            System = system;
            Data = data;
        }
        public ValidationResult Ask()
        {
            return System.Validate(Data);
        }

        public ValidationResult Execute()
        {
            return System.Validate(Data);
        }

        public bool Reverse()
        {
            return true;
        }

        public IPolicyAction NextPolicyAction(IOperationStore store, object data, int order)
        {
            return new PolicyAction<System1Result>( store, new System1Operation(System, data as ValidationResult), order);
        }
    }
}
