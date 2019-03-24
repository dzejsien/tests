using Common;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Tasks.Dependent
{
    public class AssemblyExecutor : IExecutable
    {
        private ILogger _log = Log.Logger.ForContext<AssemblyExecutor>();

        void IExecutable.ExecuteAsync(Action<string> logMessage)
        {
            ExecuteAsync().GetAwaiter().GetResult();
        }

        public async Task ExecuteAsync()
        {
            var data = InitData();

            var executor = new DependentExecutor();
            var container = await executor.ExecuteAsync(data);

            LogContainer(container);
        }


        public async Task ExecuteAsyncWithPreload()
        {
            var container = new ConcurrentDictionary<Type, IDependent>();
            // ... preloading container
            var data = InitData();

            var executor = new DependentExecutor();
            await executor.ExecuteAsync(data, container);

            LogContainer(container);
        }

        private static IReadOnlyCollection<IDependent> InitData()
        {
            var types = Assembly.GetCallingAssembly().GetTypes()
                            .Where(x => x.IsSubclassOf(typeof(MockedProcessor)));

            var data = new List<IDependent>();

            foreach (var type in types)
            {
                data.Add(Activator.CreateInstance(type) as IDependent);
            }

            return data;
        }


        private void LogContainer(IDictionary<Type, IDependent> container)
        {
            var i = 0;
            foreach (var item in container)
            {
                i++;
                _log.Information($"{i}. Container has {item.Key.Name}");
            }
        }
    }
}
