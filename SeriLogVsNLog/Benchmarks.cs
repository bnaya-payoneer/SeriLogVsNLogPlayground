#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using BenchmarkDotNet.Attributes;

namespace SeriLogVsNLog;

[MemoryDiagnoser]
//[GcForce(true)] // Forces a full GC before and after each iteration
[GcServer(true)] // Enables server GC
public class Benchmarks
{
    private NLogBenchmarks _nlog = new NLogBenchmarks();
    private SeriLogBenchmarks _seriLog = new SeriLogBenchmarks();

    [GlobalSetup]
    public void Setup()
    {
        _nlog.Setup();
        _seriLog.Setup();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        _nlog.Cleanup();
        _seriLog.Cleanup();
    }

    [Benchmark(Baseline = true)]
    public void UseNLog()
    {
        _nlog.UseNLog();
    }

    [Benchmark]
    public void UseSeriLog()
    {
        _seriLog.UseSeriLog();
    }
}
