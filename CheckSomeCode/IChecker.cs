using System;
using System.Collections.Generic;
using System.Text;

namespace CheckSomeCode
{
    interface IChecker
    {
        void Check(Action<string> logMessage, object data = null);
    }
}
