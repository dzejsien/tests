using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tasks.Dependent
{
    public interface IDependebleExecutor
    {
        Task<IDictionary<Type, IDependant>> ExecuteAsync(IReadOnlyCollection<IDependant> toExecute);
        Task<IDictionary<Type, IDependant>> ExecuteAsync(IReadOnlyCollection<IDependant> toExecute, ConcurrentDictionary<Type, IDependant> container);
    }
}
