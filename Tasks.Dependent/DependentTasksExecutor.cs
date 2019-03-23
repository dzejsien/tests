using Common;
using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.Dependent
{
    public class DependentTasksExecutor : IExecutable
    {
        private ILogger _log = Log.Logger.ForContext<DependentTasksExecutor>();

        public async Task Execute()
        {
            var container = new ConcurrentDictionary<Type, IDependebale>();
            var data = InitData();

            var waitingForExecution = data;
            var readyToExecute = GetReadyForExecution(waitingForExecution, container)
                .Select(x => ((MockedProcessor)x).ProcessAsync(container)).ToList();

            var inter = 0;

            while (readyToExecute.Any())
            {
                _log.Information($"Iteration: {++inter}");
                var executed = await Task.WhenAny(readyToExecute);
                readyToExecute.Remove(executed);

                var tasksToExecute = GetReadyForExecution(waitingForExecution, container).Select(x => ((MockedProcessor)x).ProcessAsync(container)).ToList();
                _log.Information($"New tasks ready to execute: {tasksToExecute.Count}");
                readyToExecute.AddRange(tasksToExecute);
            }
        }

        private IEnumerable<IDependebale> GetReadyForExecution(
            IList<IDependebale> waitingForExecution,
            IDictionary<Type, IDependebale> container)
        {
            var toMove = waitingForExecution.Where(x => !x.DependsOn.Any() || x.DependsOn.All(y => container.ContainsKey(y))).ToList();

            foreach (var x in toMove)
            {
                _log.Information($"{x.GetType().Name} is ready for execution");
                waitingForExecution.Remove(x);
                yield return x;
            }
        }

        private static IList<IDependebale> InitData()
        {
            var types = Assembly.GetCallingAssembly().GetTypes()
                            .Where(x => x.IsSubclassOf(typeof(MockedProcessor)));

            var data = new List<IDependebale>();

            foreach (var type in types)
            {
                data.Add(Activator.CreateInstance(type) as IDependebale);
            }

            return data;
        }

        public void Execute(Action<string> logMessage)
        {
            Execute().GetAwaiter().GetResult();
        }
    }

    public interface IDependebale
    {
        IEnumerable<Type> DependsOn { get; }
    }

    public abstract class MockedProcessor : IDependebale
    {
        private ILogger _log = Log.Logger.ForContext<MockedProcessor>();

        public abstract IEnumerable<Type> DependsOn { get; }

        public async Task ProcessAsync(ConcurrentDictionary<Type, IDependebale> container)
        {
            var random = new Random().Next(1000, 10000);
            _log.Information($"{this.GetType().Name}: start wih {random}");
            await Task.Delay(random);

            if (!container.TryAdd(GetType(), this))
                throw new Exception("The same already exists");

            _log.Information($"{this.GetType().Name}: ends.");
        }
    }

    public class DataA : MockedProcessor, IDependebale
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield break;
            }
        }
    }

    public class DataB : MockedProcessor, IDependebale
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield break;
            }
        }
    }

    public class DataC : MockedProcessor, IDependebale
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield break;
            }
        }
    }

    public class DataD : MockedProcessor, IDependebale
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield break;
            }
        }
    }

    public class DataA2 : MockedProcessor, IDependebale
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataA);
            }
        }
    }

    public class DataB2 : MockedProcessor, IDependebale
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataB);
            }
        }
    }

    public class DataC2 : MockedProcessor, IDependebale
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataC);
            }
        }
    }

    public class DataD2 : MockedProcessor, IDependebale
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataD);
            }
        }
    }

    public class DataA22 : MockedProcessor, IDependebale
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataA);
            }
        }
    }

    public class DataB3 : MockedProcessor, IDependebale
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataB2);
            }
        }
    }

    public class DataC22 : MockedProcessor, IDependebale
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataC);
            }
        }
    }

    public class DataD3 : MockedProcessor, IDependebale
    {
        public override IEnumerable<Type> DependsOn
        {
            get
            {
                yield return typeof(DataD2);
            }
        }
    }
}
