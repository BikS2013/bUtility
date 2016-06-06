using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT.Sample
{
    public class Validation : PolicyAction<Request, ValidationResult>
    {
        ISystem System { get; set; }
        public Validation(IOperationStore store, ISystem system, Request data, int order) : base(store, data, order)
        {
            System = system; 
        }
        public override ValidationResult Ask()
        {
            return System.Validate(Data);
        }

        public override IAction NextAction()
        {
            return new System1Operation(Store, System, Result, Order + 1);
        }

        protected override ValidationResult ExecuteInternal()
        {
            return System.Validate(Data);
        }

        protected override bool ReverseInternal()
        {
            return true; 
        }
    }
}
