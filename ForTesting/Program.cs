using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Common;

namespace ForTesting
{
    class Program
    {
        private const string ResultFilePath = "result.txt";

        static void Main(string[] args)
        {
            //var assemblyString = "GenerateRoutes";
            var assemblyString = "CheckSomeCode";

            var executorType = Assembly.Load(assemblyString).GetTypes().FirstOrDefault(x => typeof(IExecutable).IsAssignableFrom(x));

            if (executorType != null)
            {
                var executor = Activator.CreateInstance(executorType) as IExecutable ?? throw new ArgumentNullException();
                executor.Execute(Console.WriteLine);
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
