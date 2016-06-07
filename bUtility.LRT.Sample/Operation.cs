using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT.Sample
{
    public class Operation : Policy<Request, Result>
    {
        ISystem System { get; set; }
        public Operation(IOperationStore store, ISystem system, Request request) : base(store, request)
        {
            System = system;
        }
        public override bool Execute()
        {
            return Execute(new PolicyAction<ValidationResult>(Store, new Validation(System, Request), 0));
        }
    }
}
