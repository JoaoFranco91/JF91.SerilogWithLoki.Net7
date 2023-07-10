using Microsoft.Extensions.Configuration;

namespace JF91.SerilogWithLoki;

using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Grafana.Loki;
using Serilog.Sinks.SystemConsole.Themes;
using Microsoft.Extensions.Hosting;

public static class SerilogExtensions
{
    public static void CreateLogger(IConfiguration config)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithCorrelationId()
            .Enrich.WithExceptionDetails()
            .Enrich.WithProperty("Application", Environment.GetEnvironmentVariable("APPLICATION_NAME"))
            .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
            .WriteTo.Async
            (
                wt =>
                    wt.Console
                    (
                        outputTemplate: SerilogConstants.OutputTemplate,
                        theme: AnsiConsoleTheme.Code
                    )
            )
            .ReadFrom.Configuration(config)
            .CreateLogger();
    }
    
    public static IHostBuilder ConfigureSerilog
    (
        this IHostBuilder host
    )
    {
        host.UseSerilog
        (
            (
                context,
                configuration
            ) =>
            {
                configuration
                    .Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .Enrich.WithCorrelationId()
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithProperty("Application", Environment.GetEnvironmentVariable("APPLICATION_NAME"))
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .WriteTo.Async
                    (
                        wt =>
                            wt.Console
                            (
                                outputTemplate: SerilogConstants.OutputTemplate,
                                theme: AnsiConsoleTheme.Code
                            )
                    )
                    .WriteTo.GrafanaLoki(
                        uri: "http://localhost:3100",
                        propertiesAsLabels: SerilogLabelProvider.PropertiesAsLabels
                    )
                    .ReadFrom.Configuration(context.Configuration);
            }
        );

        return host;
    }
}