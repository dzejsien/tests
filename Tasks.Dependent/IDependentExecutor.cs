using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tasks.Dependent
{
    public interface IDependentExecutor
    {
        Task<IDictionary<Type, IDependent>> ExecuteAsync(IReadOnlyCollection<IDependent> toExecute);
        Task<IDictionary<Type, IDependent>> ExecuteAsync(IReadOnlyCollection<IDependent> toExecute, ConcurrentDictionary<Type, IDependent> container);
    }
}
