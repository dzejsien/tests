using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tasks.Dependent
{
    public interface IDependant
    {
        IEnumerable<Type> DependsOn { get; }
        Task<IDependant> ProcessAsync();
    }
}
