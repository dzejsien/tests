using System;
using System.Linq;
using Common;

namespace CheckSomeCode
{
    public class CheckSomeCodeExecutor : IExecutable
    {
        public void Execute(Action<string> logMessage)
        {
            CheckForeachOfEnumerableIfItWillBeYielded(logMessage);
        }

        private static void CheckForeachOfEnumerableIfItWillBeYielded(Action<string> logMessage)
        {
            var list = CheckForeachIfYield.GetInt(5, logMessage).ToList();
        }
    }
}