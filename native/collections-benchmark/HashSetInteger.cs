using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

//[DryJob(RuntimeMoniker.Net80)]
[ShortRunJob(RuntimeMoniker.Net80)]
[Orderer(SummaryOrderPolicy.FastestToSlowest), AllStatisticsColumn, RPlotExporter]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
[WarmupCount(3), IterationCount(1)]
[MemoryDiagnoser]
public class HashSetInteger
{
    const int Items = 1_000_000;

    HashSet<int> collection = new();

    [GlobalSetup]
    public void HashSetSetup()
    {
        for (var i = 1; i <= Items; i++)
            collection.Add(i);
    }

    [Params(1, 568_452, Items)]
    public int FindValue;

    [Benchmark]
    public void TryGetValue() => collection.TryGetValue(FindValue, out _);

    [Benchmark]
    public void Contains() => collection.Contains(FindValue);

    [Benchmark]
    public void FirstOrDefault() => collection.FirstOrDefault(x => x == FindValue);
}
