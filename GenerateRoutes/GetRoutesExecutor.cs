using System;
using System.Linq;
using System.Reflection;
using Common;

namespace GenerateRoutes
{
    public class GetRoutesExecutor : IExecutable
    {
        public void Execute(Action<string> logMessage)
        {
            var props = typeof(TagRoutes).GetFields().Select(x => new { Name = x.Name , Value = x.GetRawConstantValue()});

            foreach (var prop in props)
            {
                logMessage($"public const string {prop.Name} = \"{prop.Value}\";");
            }
        }
    }
}