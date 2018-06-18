using System;
using System.Collections.Generic;
using System.Text;

namespace Yanyitec.Log
{
    public interface ILogger
    {
        void Log(LogLevels level, string message, object extraData=null);
    }
}
