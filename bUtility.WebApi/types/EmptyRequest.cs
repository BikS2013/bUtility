using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility
{
    public class EmptyRequest: IUserIDProvider
    {
        public string UserID { get; set; }
    }
}
