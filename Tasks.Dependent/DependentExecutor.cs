using Serilog;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tasks.Dependent
{
    public class DependebleExecutor : IDependebleExecutor
    {
        private ILogger _log = Log.Logger.ForContext<DependebleExecutor>();

        private IDictionary<Type, IDependant> _container = new ConcurrentDictionary<Type, IDependant>();
        private IList<IDependant> _watingForExecution = new List<IDependant>();

        public async Task<IDictionary<Type, IDependant>> ExecuteAsync(IReadOnlyCollection<IDependant> toExecute, ConcurrentDictionary<Type, IDependant> container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
            return await ExecuteAsync(toExecute);
        }

        public async Task<IDictionary<Type, IDependant>> ExecuteAsync(IReadOnlyCollection<IDependant> toExecute)
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

        private IEnumerable<IDependant> GetReadyForExecution()
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
