using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Common;
using Serilog;

namespace ForTesting
{
    class Program
    {
        static class Assemblies
        {
            public const string GenerateRoutes = "GenerateRoutes";
            public const string CheckSomeCode = "CheckSomeCode";
            public const string Tasks_Dependent = "Tasks.Dependent";
        }

        private const string ResultFilePath = "result.txt";

        static void Main(string[] args)
        {
            var log = new LoggerConfiguration().WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}").CreateLogger();
            Log.Logger = log;

            var assembly = Assemblies.Tasks_Dependent;

            var executorType = Assembly.Load(assembly).GetTypes()
                .FirstOrDefault(x => typeof(IExecutable).IsAssignableFrom(x));

            if (executorType != null)
            {
                var executor = Activator.CreateInstance(executorType) as IExecutable ?? throw new ArgumentNullException();
                executor.ExecuteAsync(Console.WriteLine);
                //File.WriteAllText(ResultFilePath, string.Empty);
                //executor.Execute(WriteToFile);
            }

            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        static void WriteToFile(string input)
        {
            File.AppendAllText(ResultFilePath, input + Environment.NewLine);
        }
    }
}
