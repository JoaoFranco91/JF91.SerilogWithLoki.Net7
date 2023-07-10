namespace JF91.SerilogWithLoki;

public class SerilogConstants
{
    public const string OutputTemplate = 
        @"[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}";
}