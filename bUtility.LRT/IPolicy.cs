using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT
{
    public interface IPolicy
    {
        void Execute();
        void Resume();
        void Cancel();

    }
}
