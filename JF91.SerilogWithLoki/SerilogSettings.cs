namespace JF91.SerilogWithLoki;

public class SerilogSettings
{
    public SerilogSinks SerilogSinks { get; set; }
    public LokiSettings LokiSettings { get; set; }
}

public class LokiSettings
{
    public string Url { get; set; } = "http://localhost:3100";
    public string[] CustomLabels { get; set; }
}

public class SerilogSinks
{
    public bool Console { get; set; } = true;
    public bool Loki { get; set; } = true;
}