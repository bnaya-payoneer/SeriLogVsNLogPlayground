#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using BenchmarkDotNet.Attributes;
using Cocona;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System.Security.Principal;
using msLog = Microsoft.Extensions.Logging;

namespace SeriLogVsNLog;

public class NLogBenchmarks
{
    private msLog.ILogger _logger;
    private IIdentity _identity;

    [GlobalSetup]
    public void Setup()
    {
        // Setup NLog
        var logger = LogManager.Setup()
            .LoadConfigurationFromFile("nlog.config")
            .GetCurrentClassLogger();

        logger.Info("Service Started");

        var builder = CoconaApp.CreateBuilder();
        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.ClearProviders();
            loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            loggingBuilder.AddNLog();
        });

        var app = builder.Build();
        _logger = app.Services.GetRequiredService<msLog.ILogger<SeriLogBenchmarks>>();
        _identity = new GenericIdentity("John Doe", "CustomAuth");
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        LogManager.Shutdown();
    }

    [Benchmark(Baseline = true)]
    public void UseNLog()
    {
        for (int i = 0; i < 100; i++)
        {
            using var scp = _logger.BeginScope(KeyValuePair.Create("Rate", 10));
            _logger.Comment(1, _identity);
        }
    }
}
