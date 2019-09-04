using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CheckSomeCode;
using Common;
using Serilog;

namespace ForTesting
{
    public class Program
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

            for (int i = 0; i < 1000; i++)
            {
                var a = i;

                File.WriteAllText("test.txt", i.ToString());
                Console.WriteLine(i);
                Task.Delay(1000).GetAwaiter().GetResult();
            }

            //var log = new LoggerConfiguration().WriteTo.Console(outputTemplate:"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}").CreateLogger();
            //Log.Logger = log;

            //var assembly = Assemblies.Tasks_Dependent;

            //var executorType = Assembly.Load(assembly).GetTypes()
            //    .FirstOrDefault(x => typeof(IExecutable).IsAssignableFrom(x));

            //if (executorType != null)
            //{
            //    var executor = Activator.CreateInstance(executorType) as IExecutable ?? throw new ArgumentNullException();
            //    executor.ExecuteAsync(Console.WriteLine);
            //    //File.WriteAllText(ResultFilePath, string.Empty);
            //    //executor.Execute(WriteToFile);
            //}

            //var summary = BenchmarkRunner.Run<ArrayOfObjectToArraysOfValues>();
            //Console.WriteLine(Marshal.SizeOf<TestStruct>());

            // benchmark example
            //var enumerable = Enumerable.Repeat(1, 1000000);
            //var collection = enumerable.ToList();

            //Stopwatch st = Stopwatch.StartNew();
            //List<int> copy1 = enumerable.ToList();
            //Console.WriteLine(st.ElapsedMilliseconds);

            //st = Stopwatch.StartNew();
            //List<int> copy2 = new List<int>(enumerable);
            //Console.WriteLine(st.ElapsedMilliseconds);


            Console.WriteLine("Done!");
            Console.ReadKey();
        }

        static void WriteToFile(string input)
        {
            File.AppendAllText(ResultFilePath, input + Environment.NewLine);
        }
    }

    class Program2
    {
        static void Main2(string[] args)
        {
            Console.WriteLine(FirstExample());
            Console.WriteLine(SecondExample());
            Console.WriteLine(ThirdExample());
        }

        static string FirstExample()
        {
            var span = new ReadOnlySpan<byte>(new byte[] { 1, 2, 3 });

            return span.ToString();
        }

        static string SecondExample()
        {
            var data = new byte[] { 1, 2, 3 };
            var span = new ReadOnlySpan<byte>(data);

            return span.ToString();
        }

        static string ThirdExample()
        {
            var data = new byte[] { 1, 2, 3 };
            return data.ToString();
        }
    }
}
