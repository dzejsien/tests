using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasks.Dependent
{
    public class DependentExecutor : IDependentExecutor
    {
        private ILogger _log = Log.Logger.ForContext<DependentExecutor>();

        private IDictionary<Type, IDependent> _container = new ConcurrentDictionary<Type, IDependent>();
        private IList<IDependent> _watingForExecution = new List<IDependent>();

        public async Task<IDictionary<Type, IDependent>> ExecuteAsync(IReadOnlyCollection<IDependent> toExecute, ConcurrentDictionary<Type, IDependent> container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            return await ExecuteAsync(toExecute);
        }

        public async Task<IDictionary<Type, IDependent>> ExecuteAsync(IReadOnlyCollection<IDependent> toExecute)
        {
            _watingForExecution = toExecute.ToList();
            var readyToExecute = GetReadyForExecution().Select(x => x.ProcessAsync()).ToList();
            var inter = 0;

            while (readyToExecute.Any())
            {
                _log.Information($"Iteration: {++inter}");

                var resultTask = await Task.WhenAny(readyToExecute);
                readyToExecute.Remove(resultTask);
                var result = await resultTask;

                if (!_container.TryAdd(result.GetType(), result))
                    throw new InvalidOperationException($"Key {result.GetType()} already exists");

                var tasksToExecute = GetReadyForExecution().Select(x => x.ProcessAsync()).ToList();
                readyToExecute.AddRange(tasksToExecute);

                _log.Information($"New tasks ready to execute: {tasksToExecute.Count}");
            }

            return _container;
        }

        private IEnumerable<IDependent> GetReadyForExecution()
        {
            var toMove = _watingForExecution.Where(x => !x.DependsOn.Any() || x.DependsOn.All(y => _container.ContainsKey(y))).ToList();

            foreach (var x in toMove)
            {
                _log.Information($"{x.GetType().Name} is ready for execution");
                _watingForExecution.Remove(x);
                yield return x;
            }
        }
    }
}
