using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tasks.Dependent
{
    public interface IDependent
    {
        IEnumerable<Type> DependsOn { get; }
        Task<IDependent> ProcessAsync();
    }
}
