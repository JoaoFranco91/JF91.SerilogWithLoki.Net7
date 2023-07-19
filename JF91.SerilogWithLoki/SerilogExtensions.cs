using Microsoft.Extensions.Configuration;
using Serilog.Exceptions.Core;

namespace JF91.SerilogWithLoki;

using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.Grafana.Loki;
using Serilog.Sinks.SystemConsole.Themes;
using Microsoft.Extensions.Hosting;

public static class SerilogExtensions
{
    private static SerilogSettings serilogSettings = 
        new SerilogSettings();
    
    public static void CreateLogger(IConfiguration config)
    {
        config.GetSection(nameof(SerilogSettings)).Bind(serilogSettings);
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .Enrich.FromLogContext()
            .Enrich.FromGlobalLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithCorrelationId()
            .Enrich.WithExceptionDetails(new DestructuringOptionsBuilder().WithDefaultDestructurers())
            .Enrich.WithProperty("Application", Environment.GetEnvironmentVariable("APPLICATION_NAME"))
            .Enrich.WithProperty("Environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
            .WriteTo.Async
            (
                wt =>
                {
                    if (serilogSettings.SerilogSinks.Console)
                    {
                        wt.Console
                        (
                            outputTemplate: SerilogConstants.OutputTemplate,
                            theme: AnsiConsoleTheme.Code
                        );
                    }
                }
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
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName);

                if (serilogSettings.SerilogSinks.Console)
                {
                    configuration.WriteTo.Async
                    (
                        wt =>
                        {
                            if (serilogSettings.SerilogSinks.Console)
                            {
                                wt.Console
                                (
                                    outputTemplate: SerilogConstants.OutputTemplate,
                                    theme: AnsiConsoleTheme.Code
                                );
                            }
                        }
                    );
                }

                if (serilogSettings.SerilogSinks.Loki)
                {
                    var customLabels = serilogSettings.LokiSettings.CustomLabels != null
                        ? serilogSettings.LokiSettings.CustomLabels.ToList()
                        : new List<string>();
                    
                    configuration.WriteTo.GrafanaLoki
                    (
                        uri: serilogSettings.LokiSettings.Url,
                        propertiesAsLabels: SerilogLabelProvider.PropertiesAsLabels.Concat(customLabels)
                    );
                }
                    
                configuration.ReadFrom.Configuration(context.Configuration);
            }
        );

        return host;
    }
}