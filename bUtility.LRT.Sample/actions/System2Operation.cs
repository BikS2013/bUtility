using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT.Sample
{
    public class System2Operation : PolicyAction<System1Result, System2Result>
    {
        ISystem System { get; set; }
        public System2Operation(IOperationStore store, ISystem system, System1Result data, int order) : base(store, data, order)
        {
            System = system;
        }
        public override System2Result Ask()
        {
            return System.System2Execute(Data);
        }

        public override IAction NextAction()
        {
            return new System3Operation(Store, System, Result, Order + 1);
        }

        protected override System2Result ExecuteInternal()
        {
            return System.System2Execute(Data);
        }

        protected override bool ReverseInternal()
        {
            return System.System2Reverse(Data);
        }
    }
}
