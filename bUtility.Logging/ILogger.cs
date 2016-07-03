using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bUtility.Logging
{
    public interface ILogger
    {
        void Info(string message);
        void Info(Exception ex, string message = null);
        void Warn(string message);
        void Warn(Exception ex, string message = null);
        void Error(string message);
        void Error(Exception ex, string message = null);

    }
}
