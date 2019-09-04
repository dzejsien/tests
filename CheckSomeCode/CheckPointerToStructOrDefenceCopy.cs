using BenchmarkDotNet.Attributes;

namespace CheckSomeCode
{
    /*
    // * Summary *

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.437 (1809/October2018Update/Redstone5)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100-preview5-011568
  [Host]     : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT


|        Method |      Mean |     Error |    StdDev |    Median |
|-------------- |----------:|----------:|----------:|----------:|
|     ByPointer | 0.0118 ns | 0.0202 ns | 0.0270 ns | 0.0000 ns |
| ByDefenceCopy | 0.0437 ns | 0.0652 ns | 0.0640 ns | 0.0218 ns |


    // * Summary *     10000                                                                     
                                                                                            
    BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.437 (1809/October2018Update/Redstone5)   
    Intel Core i7-8650U CPU 1.90GHz(Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
    .NET Core SDK = 3.0.100-preview5-011568                                                   

[Host]     : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
DefaultJob : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
                                                                                            
                                                                                            
|        Method |     Mean |     Error |    StdDev |                                    
|-------------- |---------:|----------:|----------:|                                    
|     ByPointer | 2.783 us | 0.0330 us | 0.0276 us |                                    
| ByDefenceCopy | 2.753 us | 0.0360 us | 0.0281 us |    */

    public class CheckPointerToStructOrDefenceCopy
    {
        [Benchmark]
        public int ByPointer()
        {
            var tester = new Tester();
            var str = new TestStruct(2, 4);

            int result = 0;

            for (int i = 0; i < 10000; i++)
            {
                result += tester.ByPointer(in str);
            }

            return result;
        }

        [Benchmark]
        public int ByDefenceCopy()
        {
            var tester = new Tester();
            var str = new TestStruct(2, 4);
            int result = 0;

            for (int i = 0; i < 10000; i++)
            {
                result += tester.ByDefenceCopy(str);
            }

            return result;
        }
    }

    public readonly struct TestStruct
    {
        // should have 8 bytes
        public int First { get; }
        public int Second { get; }
        public int Third { get; }

        public TestStruct(int first, int second)
        {
            First = first;
            Second = second;
            Third = 3;
        }
    }

    public class Tester
    {
        public int ByPointer(in TestStruct test)
        {
            return test.First + test.Second;
        }

        public int ByDefenceCopy(TestStruct test)
        {
            return test.First + test.Second;
        }
    }
}