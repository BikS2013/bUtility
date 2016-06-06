using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT.Sample
{
    public interface ISystem
    {
        ValidationResult Validate(Request data);

        System1Result System1Execute(ValidationResult data);
        bool System1Reverse(ValidationResult data);
        System1Result System1Ask(ValidationResult data);

        System2Result System2Execute(System1Result data);
        bool System2Reverse(System1Result data);
        System2Result System2Ask(System1Result data);

        Result System3Execute(System2Result data);
        bool System3Reverse(System2Result data);
        Result System3Ask(System2Result data);
    }
}
