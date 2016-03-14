using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.LRT
{
    public interface IAction
    {
    }

    public interface IAction<in T, out R>: IAction
    {
        R Execute(T data);
        R Ask(T data);
        void Cancel(T data);
    }

}
