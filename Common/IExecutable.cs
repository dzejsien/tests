using System;

namespace Common
{
    public interface IExecutable
    {
        void Execute(Action<string> logMessage);
    }
}