using System;
using System.Linq;
using Common;

namespace CheckSomeCode
{
    public class CheckSomeCodeExecutor : IExecutable
    {
        public void ExecuteAsync(Action<string> logMessage)
        {
            //CheckerExecutor<CheckForeachIfYield>(logMessage, new CheckForeachIfYield.Config(5));
            CheckerExecutor<CheckBoxing>(logMessage);
        }

        private void CheckerExecutor<T>(Action<string> logMessage, object data = null) where T : IChecker, new()
        {
            var checker = new T();
            checker.Check(logMessage, data);
        }
    }
}