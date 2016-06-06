using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT.Sample
{
    public class System1Operation : PolicyAction<ValidationResult, System1Result>
    {
        ISystem System { get; set; }
        public System1Operation(IOperationStore store, ISystem system, ValidationResult data, int order) : base(store, data, order)
        {
            System = system;
        }
        public override System1Result Ask()
        {
            return System.System1Ask(Data);
        }

        public override IAction NextAction()
        {
            return new System2Operation(Store, System, Result, Order + 1);
        }

        protected override System1Result ExecuteInternal()
        {
            return System.System1Execute(Data);
        }

        protected override bool ReverseInternal()
        {
            return System.System1Reverse(Data);
        }
    }
}
