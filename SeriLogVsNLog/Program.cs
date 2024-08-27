// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using SeriLogVsNLog;

Console.WriteLine("Start");

var summary = BenchmarkRunner.Run(new[] { typeof(Benchmarks) });
//var summary = BenchmarkRunner.Run(new[] { typeof(NLogBenchmarks), typeof(SeriLogBenchmarks) });

Console.WriteLine(summary);


Console.ReadKey(true);