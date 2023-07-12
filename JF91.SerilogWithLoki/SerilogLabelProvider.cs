namespace JF91.SerilogWithLoki;

public class SerilogLabelProvider
{
    public static IList<string> PropertiesAsLabels { get; set; } = new List<string>
    {
        "level", // Since 3.0.0, you need to explicitly add level if you want it!
        "Admin",
        "Account",
        "ElapsedMilliseconds",
        "StatusCode",
        "ContentType",
        "ContentLength",
        "Protocol",
        "Method",
        "Scheme",
        "Host",
        "PathBase",
        "Path",
        "QueryString",
        "EventId",
        "SourceContext",
        "RequestId",
        "RequestPath",
        "ConnectionId",
        "MachineName",
        "Application",
        "Environment",
        "CorrelationId",
        "Message",
        "MessageTemplate",
        "elapsed",
        "parameters",
        "commandType",
        "commandTimeout",
        "newLine",
        "commandText",
        "ActionId",
        "ActionName",
    };
}