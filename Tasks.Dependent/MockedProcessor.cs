using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tasks.Dependent
{
    public abstract class MockedProcessor : IDependant
    {
        private ILogger _log = Log.Logger.ForContext<MockedProcessor>();

        public abstract IEnumerable<Type> DependsOn { get; }

        public async Task<IDependant> ProcessAsync()
        {
            var random = new Random().Next(1000, 10000);

            _log.Information($"{this.GetType().Name}: start wih {random}");

            await Task.Delay(random);

            _log.Information($"{this.GetType().Name}: ends.");

            return this;
        }
    }
}
