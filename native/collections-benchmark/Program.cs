using BenchmarkDotNet.Running;

Console.WriteLine(".:: Collections Playground Benchmark ::.");

//BenchmarkRunner.Run(typeof(Program).Assembly);

//BenchmarkRunner.Run<ConcurrentDictionaryString>();
//BenchmarkRunner.Run<DictionaryString>();
//BenchmarkRunner.Run<HashSetInteger>();
BenchmarkRunner.Run<HashSetString>();
//BenchmarkRunner.Run<LinkedListString>();
//BenchmarkRunner.Run<ListString>();
//BenchmarkRunner.Run<SortedListString>();