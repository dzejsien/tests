using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using BenchmarkDotNet.Attributes;

namespace CheckSomeCode
{
    /*
     * size: 24
     *
     * 

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.437 (1809/October2018Update/Redstone5)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100-preview5-011568
  [Host]     : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT


|     Method |     Mean |     Error |    StdDev |   Median | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|----------- |---------:|----------:|----------:|---------:|-----:|-------:|------:|------:|----------:|
|     AggSum | 2.949 us | 0.1064 us | 0.3069 us | 2.853 us |    2 | 0.0877 |     - |     - |     384 B |
| AggForeach | 1.846 us | 0.0365 us | 0.0802 us | 1.825 us |    1 | 0.0191 |     - |     - |      96 B |
     *
     *
     * size: 100
     *
     * * Summary *
     *                                                                                              
     *BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.437 (1809/October2018Update/Redstone5)         
     *Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores          
     *.NET Core SDK=3.0.100-preview5-011568                                                         
     *  [Host]     : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT      
     *  DefaultJob : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT      
     *                                                                                              
     *                                                                                              
     *|     Method |      Mean |     Error |    StdDev | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
     *|----------- |----------:|----------:|----------:|-----:|-------:|------:|------:|----------:|
     *|     AggSum | 11.302 us | 0.2251 us | 0.4547 us |    2 | 0.0763 |     - |     - |     384 B |
     *| AggForeach |  7.298 us | 0.1660 us | 0.1705 us |    1 | 0.0153 |     - |     - |      96 B |
     *
     *
     *
     *size: 1000
     *
     * BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.437 (1809/October2018Update/Redstone5)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100-preview5-011568
  [Host]     : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT


|          Method |      Mean |    Error |   StdDev |    Median | Rank | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------- |----------:|---------:|---------:|----------:|-----:|------:|------:|------:|----------:|
|          AggSum | 103.71 us | 2.192 us | 3.722 us | 102.86 us |    3 |     - |     - |     - |     384 B |
|      AggForeach |  71.27 us | 1.383 us | 1.750 us |  70.98 us |    1 |     - |     - |     - |      96 B |
|     AggSumClass | 127.20 us | 3.277 us | 9.455 us | 124.55 us |    4 |     - |     - |     - |     240 B |
| AggForeachClass |  74.80 us | 1.496 us | 2.659 us |  74.27 us |    2 |     - |     - |     - |     120 B |
     *
     *
     *size = 24
     *
     *BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.437 (1809/October2018Update/Redstone5)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100-preview5-011568
  [Host]     : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT


|       Method |       Mean |     Error |   StdDev |     Median | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------- |-----------:|----------:|---------:|-----------:|-----:|-------:|------:|------:|----------:|
| ArraysSelect | 1,258.1 ns | 32.470 ns | 94.20 ns | 1,217.4 ns |    2 | 0.6351 |     - |     - |   2.61 KB |
|    ArraysFor |   416.7 ns |  8.345 ns | 18.32 ns |   411.9 ns |    1 | 0.4573 |     - |     - |   1.88 KB |
     *
     *
     *
     *
     *size = 24 - no Add
     *
     *BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.437 (1809/October2018Update/Redstone5)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100-preview5-011568
  [Host]     : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT


|       Method |       Mean |    Error |    StdDev |     Median | Ratio | RatioSD | Rank |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------- |-----------:|---------:|----------:|-----------:|------:|--------:|-----:|-------:|------:|------:|----------:|
| ArraysSelect | 1,275.2 ns | 39.21 ns | 109.30 ns | 1,241.2 ns |  3.02 |    0.35 |    2 | 0.6351 |     - |     - |   2.61 KB |
|    ArraysFor |   429.6 ns | 15.19 ns |  43.35 ns |   415.5 ns |  1.00 |    0.00 |    1 | 0.4478 |     - |     - |   1.84 KB |
     *
     *
     *
     *size = 10000
     *
     * BenchmarkDotNet=v0.11.5, OS=Windows 10.0.17763.437 (1809/October2018Update/Redstone5)
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.0.100-preview5-011568
  [Host]     : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT
  DefaultJob : .NET Core 2.2.5 (CoreCLR 4.6.27617.05, CoreFX 4.6.27618.01), 64bit RyuJIT


|       Method |     Mean |    Error |   StdDev | Ratio | RatioSD | Rank |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|------------- |---------:|---------:|---------:|------:|--------:|-----:|---------:|---------:|---------:|----------:|
| ArraysSelect | 429.0 us | 8.563 us | 22.11 us |  1.99 |    0.14 |    2 | 180.6641 | 180.1758 | 180.1758 | 627.27 KB |
|    ArraysFor | 216.2 us | 4.277 us | 10.96 us |  1.00 |    0.00 |    1 | 157.4707 | 157.2266 | 157.2266 | 626.05 KB |
     *
     *
     */

    [RankColumn]
    [MemoryDiagnoser]
    public class ArrayOfObjectToArraysOfValues
    {
        private Tester2 tester;

        public ArrayOfObjectToArraysOfValues()
        {
            tester = new Tester2(10000);
        }

        /* test 1
        [Benchmark]
        public TestStruct2 AggSum()
        {
            return tester.AggregateSum();
        }

        [Benchmark]
        public TestStruct2 AggForeach()
        {
            return tester.AggregateForeach();
        }

        [Benchmark]
        public TestStruct2Class AggSumClass()
        {
            return tester.AggregateSumClass();
        }

        [Benchmark]
        public TestStruct2Class AggForeachClass()
        {
            return tester.AggregateForeachClass();
        }
        */

        // test 2
        [Benchmark]
        public IReadOnlyCollection<RowSubValueInfo> ArraysSelect()
        {
            return tester.GetRowSubValueInfosSelect().ToList();
        }

        [Benchmark(Baseline = true)]
        public IReadOnlyCollection<RowSubValueInfo> ArraysFor()
        {
            return tester.GetRowSubValueInfosFor();
        }

    }

    public static class Extension
    {
        public static decimal NextDecimal(this Random rng)
        {
            var decimals = new[] { 0.01m, 0.002m, 0.003m, 0.00004m, 1.23m };
            return decimals[rng.Next(0, 4)];
        }
    }

    public readonly struct TestStruct2
    {
        public decimal Prop1 { get; }
        public decimal Prop2 { get; }
        public decimal Prop3 { get; }
        public decimal Prop4 { get; }

        public TestStruct2(decimal p1, decimal p2, decimal p3, decimal p4)
        {
            Prop1 = p1;
            Prop2 = p2;
            Prop3 = p3;
            Prop4 = p4;
        }
    }

    public class TestStruct2Class
    {
        public decimal Prop1 { get; }
        public decimal Prop2 { get; }
        public decimal Prop3 { get; }
        public decimal Prop4 { get; }

        public TestStruct2Class(decimal p1, decimal p2, decimal p3, decimal p4)
        {
            Prop1 = p1;
            Prop2 = p2;
            Prop3 = p3;
            Prop4 = p4;
        }
    }

    public class Tester2
    {
        private readonly IReadOnlyList<TestStruct2> _testColl;
        private readonly IReadOnlyList<TestStruct2Class> _testCollClass;

        public Tester2(int size)
        {
            var list = new List<TestStruct2>();
            var list2 = new List<TestStruct2Class>();
            var rand = new Random();

            for (int i = 0; i < size; i++)
            {
                list.Add(new TestStruct2(rand.NextDecimal(), rand.NextDecimal(), rand.NextDecimal(), rand.NextDecimal()));
                list2.Add(new TestStruct2Class(rand.NextDecimal(), rand.NextDecimal(), rand.NextDecimal(), rand.NextDecimal()));
            }

            _testColl = list;
            _testCollClass = list2;
        }

        public TestStruct2 AggregateSum()
        {
            var prop1 = _testColl.Sum(x => x.Prop1);
            var prop2 = _testColl.Sum(x => x.Prop2);
            var prop3 = _testColl.Sum(x => x.Prop3);
            var prop4 = _testColl.Sum(x => x.Prop4);

            return new TestStruct2(prop1, prop2, prop3, prop4);
        }

        public TestStruct2 AggregateForeach()
        {
            var prop1 = 0m;
            var prop2 = 0m;
            var prop3 = 0m;
            var prop4 = 0m;

            foreach (var item in _testColl)
            {
                prop1 += item.Prop1;
                prop2 += item.Prop2;
                prop3 += item.Prop3;
                prop4 += item.Prop4;
            }

            return new TestStruct2(prop1, prop2, prop3, prop4);
        }

        public TestStruct2Class AggregateSumClass()
        {
            var prop1 = _testCollClass.Sum(x => x.Prop1);
            var prop2 = _testCollClass.Sum(x => x.Prop2);
            var prop3 = _testCollClass.Sum(x => x.Prop3);
            var prop4 = _testCollClass.Sum(x => x.Prop4);

            return new TestStruct2Class(prop1, prop2, prop3, prop4);
        }

        public TestStruct2Class AggregateForeachClass()
        {
            var prop1 = 0m;
            var prop2 = 0m;
            var prop3 = 0m;
            var prop4 = 0m;

            foreach (var item in _testCollClass)
            {
                prop1 += item.Prop1;
                prop2 += item.Prop2;
                prop3 += item.Prop3;
                prop4 += item.Prop4;
            }

            return new TestStruct2Class(prop1, prop2, prop3, prop4);
        }

        public IEnumerable<RowSubValueInfo> GetRowSubValueInfosSelect()
        {
            IReadOnlyCollection<TestStruct2> rangeValues = _testColl;

            yield return new RowSubValueInfo(DataTypeConstants.NetBookValue, 0m, rangeValues.Select(v => v.Prop1).ToList());
            yield return new RowSubValueInfo(DataTypeConstants.Depreciation, 0m, rangeValues.Select(v => v.Prop2).ToList());
            yield return new RowSubValueInfo(DataTypeConstants.Lease, 0m, rangeValues.Select(v => v.Prop3).ToList());
            yield return new RowSubValueInfo(DataTypeConstants.Acquisition, 0m, rangeValues.Select(v => v.Prop4).ToList());
        }

        public IReadOnlyCollection<RowSubValueInfo> GetRowSubValueInfosFor()
        {
            IReadOnlyList<TestStruct2> rangeValues = _testColl;

            var result = new List<RowSubValueInfo>();
            var netBook = new decimal[rangeValues.Count];
            var depr = new decimal[rangeValues.Count];
            var lease = new decimal[rangeValues.Count];
            var acq = new decimal[rangeValues.Count];

            for (int i = 0; i < rangeValues.Count; i++)
            {
                var item = rangeValues[i];
                netBook[i] = item.Prop1;
                depr[i] = item.Prop2;
                lease[i] = item.Prop3;
                acq[i] = item.Prop4;
            }

            result.Add(new RowSubValueInfo(DataTypeConstants.NetBookValue, 0m, netBook));
            result.Add(new RowSubValueInfo(DataTypeConstants.Depreciation, 0m, depr));
            result.Add(new RowSubValueInfo(DataTypeConstants.Lease, 0m, lease));
            result.Add(new RowSubValueInfo(DataTypeConstants.Acquisition, 0m, acq));
            return result;
        }

            //return new[]
            //{
            //    new RowSubValueInfo(DataTypeConstants.NetBookValue, 0m, netBook),
            //    new RowSubValueInfo(DataTypeConstants.Depreciation, 0m, depr),
            //    new RowSubValueInfo(DataTypeConstants.Lease, 0m, lease),
            //    new RowSubValueInfo(DataTypeConstants.Acquisition, 0m, acq)
            //};
        
    }

    public class RowSubValueInfo
    {
        public string Title { get; }
        public decimal Average { get; }
        public IReadOnlyList<decimal> MonthValues { get; }

        public RowSubValueInfo(string title, decimal average, IReadOnlyList<decimal> monthValues)
        {
            Title = title;
            Average = average;
            MonthValues = monthValues;
        }
    }

    public static class DataTypeConstants
    {
        //staffing
        public const string Compensation = "Compensation";
        public const string HoursWorkedByReportingPeriod = "Hours Worked by Reporting Period";
        public const string AvgBlendedRatePerHour = "Avg Blended Rate per Hour";
        public const string PyramidMix = "Pyramid Mix";
        public const string AverageFTE = "Average FTE";
        public const string AnnualizedCompensationFTE = "Annualized Compensation/FTE";

        //hardware
        public const string NetBookValue = "Net Book Value";
        public const string Depreciation = "Depreciation";
        public const string Lease = "Lease";
        public const string Acquisition = "Acquisition";
        public const string UnitsAcquired = "Units Acquired";
        public const string UnitValue = "Unit Value";

        //software
        public const string SubscriptionAndSupport = "Subscription & Support";
        public const string Amortization = "Amortization";
        public const string InitialFeeOtc = "Initial Fee / OTC";
        public const string QuantityAcquired = "Quantity acquired";
        public const string UnitCost = "Unit cost";
    }
}