using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT.Sample
{
    public class Operation : Policy<Request, System3Result>
    {
        ISystem System { get; set; }
        public Operation(IOperationStore store, ISystem system, Request request) : base(store, request)
        {
            System = system;
        }
        public override bool Execute()
        {
            return Execute(new Validation(Store, System, Request, 0));
        }
    }
}
