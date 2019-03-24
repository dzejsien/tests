using System;

namespace Common
{
    public interface IExecutable
    {
        void ExecuteAsync(Action<string> logMessage);
    }
}