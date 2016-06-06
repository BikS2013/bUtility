using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT.Sample
{
    public class System3Operation : PolicyAction<System2Result, Result>
    {
        ISystem System { get; set; }
        public System3Operation(IOperationStore store, ISystem system, System2Result data, int order) : base(store, data, order)
        {
            System = system;
        }
        public override Result Ask()
        {
            return System.System3Ask(Data);
        }

        public override IAction NextAction()
        {
            return null;
        }

        protected override Result ExecuteInternal()
        {
            return System.System3Execute(Data);
        }

        protected override bool ReverseInternal()
        {
            return System.System3Reverse(Data);
        }
    }
}
