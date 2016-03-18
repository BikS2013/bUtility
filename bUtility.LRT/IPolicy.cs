using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT
{
    public interface IPolicy<in T, out R>
    {
        bool Execute();
        bool Resume();
        bool Reverse();

    }
}
