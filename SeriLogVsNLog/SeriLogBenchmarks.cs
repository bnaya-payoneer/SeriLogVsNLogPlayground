#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using BenchmarkDotNet.Attributes;
using Cocona;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Security.Principal;
using msLog = Microsoft.Extensions.Logging;

namespace SeriLogVsNLog;

public class SeriLogBenchmarks
{
    private msLog.ILogger _logger;
    private IIdentity _identity;

    [GlobalSetup]
    public void Setup()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
            .Build();

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            //.MinimumLevel.Debug()
            //.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //.Enrich.FromLogContext()
            //// Add this line:
            //.WriteTo.File(new CompactJsonFormatter(), "logs/data.log",
            //   rollingInterval: RollingInterval.Day,
            //   fileSizeLimitBytes: 10 * 1024 * 1024,
            //   retainedFileCountLimit: 2,
            //   rollOnFileSizeLimit: true,
            //   shared: true,
            //   flushToDiskInterval: TimeSpan.FromSeconds(1))
            .CreateLogger();

        Log.Information("Service Started");

        var builder = CoconaApp.CreateBuilder();
        builder.Logging.ClearProviders();
        builder.Services.AddSerilog();

        var app = builder.Build();
        _logger = app.Services.GetRequiredService<msLog.ILogger<SeriLogBenchmarks>>();
        _identity = new GenericIdentity("John Doe", "CustomAuth");
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        Log.CloseAndFlush();
    }

    [Benchmark]
    public void UseSeriLog()
    {
        for (int i = 0; i < 100; i++)
        {
            using var scp = _logger.BeginScope(KeyValuePair.Create("Rate", 10));
            _logger.Comment(1, _identity);
        }
    }
}
