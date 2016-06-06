using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT.Sample.Test
{
    class SystemNoErrors : ISystem
    {
        public System1Result System1Ask(ValidationResult data)
        {
            return new System1Result { Completed = true };
        }

        public System1Result System1Execute(ValidationResult data)
        {
            return new System1Result { Completed = true };
        }

        public bool System1Reverse(ValidationResult data)
        {
            return true;
        }

        public System2Result System2Ask(System1Result data)
        {
            return new System2Result { Completed = true };
        }

        public System2Result System2Execute(System1Result data)
        {
            return new System2Result { Completed = true };
        }

        public bool System2Reverse(System1Result data)
        {
            return true; 
        }

        public Result System3Ask(System2Result data)
        {
            return new Result { Completed = true };
        }

        public Result System3Execute(System2Result data)
        {
            return new Result { Completed = true };
        }

        public bool System3Reverse(System2Result data)
        {
            return true;
        }

        public ValidationResult Validate(Request data)
        {
            return new ValidationResult { Completed = true };
        }
    }
}
