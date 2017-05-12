using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.SimpleInjector.Test
{
    public class Service1a: IService1
    {
        public Service1a(IDependency1 dep1, IDependency2 dep2)
        {

        }
        public Service1a( IDependency1 dep1)
        {

        }
        public Service1a()
        {

        }
    }
}
