using BenchmarkDotNet.Running;
using collections_benchmark_playground;

Console.WriteLine(".:: Collections Playground Benchmark ::.");

//BenchmarkRunner.Run(typeof(Program).Assembly);

//BenchmarkRunner.Run<ConcurrentDictionaryString>();
BenchmarkRunner.Run<DictionaryString>();
//BenchmarkRunner.Run<HashSetInteger>();
BenchmarkRunner.Run<HashSetString>();
//BenchmarkRunner.Run<HashTableString>();
//BenchmarkRunner.Run<LinkedListString>();
//BenchmarkRunner.Run<ListString>();
//BenchmarkRunner.Run<SortedListString>();