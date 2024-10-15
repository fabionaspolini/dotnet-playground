using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using System.Collections;

//[DryJob(RuntimeMoniker.Net80)]
[ShortRunJob(RuntimeMoniker.Net80)]
[Orderer(SummaryOrderPolicy.FastestToSlowest), AllStatisticsColumn, RPlotExporter]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
[WarmupCount(3), IterationCount(1)]
[MemoryDiagnoser]
public class HashTableString
{
    const int Items = 1_000_000;

    Hashtable collection = new();

    [GlobalSetup]
    public void HashSetSetup()
    {
        for (var i = 1; i <= Items; i++)
            collection.Add($"Teste {i}", i);
    }

    [Params("Teste 1", "Teste 568452", "Teste 1000000")]
    public string FindValue;

    [Benchmark]
    public void Contains() => collection.ContainsKey(FindValue);

    [Benchmark]
    public void IndexValue() => _ = collection[FindValue];
}
