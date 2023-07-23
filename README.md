Use this package to integrate Serilog into your ASP.Net Web API and to logs to Grafana Loki.

Notes:

1 - To view logs in console with color themes you need to run the project with HTTP or HTTPS Profile.

2 - This example is a very simple one straightforward to the point. You can add more settings to Serilog through ```appsettings.json```

3 - Loki Connection String can be defined in appsettings.json or on the configure method as a lambda function
<br>

Follow these steps to get it done:

### 1: Install Nuget Package
```
dotnet add package JF91.SerilogWithLoki
```

<br>

### 2: Add this line in ```program.cs``` after ```var builder = WebApplication.CreateBuilder(args);```
```
SerilogExtensions.CreateLogger(builder.Configuration);
```

<br>

### 3: Add this line in ```program.cs``` before ```var app = builder.Build();```
```
builder.Host.ConfigureSerilog();
```

### 3.2: Customize Loki Connection String:
```
builder.Host.ConfigureSerilog(() => "LokiConnection");

// OR

builder.Host.ConfigureSerilog(() => 
{
    // Your code
    
    return "LokiConnection";
});
```

<br>

### 4: Replace Logger in ```appsettings.json``` with this one:

#### 4.1 Remove this:
```
"Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
```

#### 4.2 Add this and feel free to customize it:
```
"Serilog": {
    "MinimumLevel": {
      "Default": "Debug"
    }
  }
```

<br>

### 5: Add this to your ```appsettings.json``` and modify it your needs:

```
"SerilogSettings": {
    "SerilogSinks": {
      "Console": true,
      "Loki": true
    },
    "LokiSettings": {
      "Url": "http://localhost:3100",
      "CustomLabels": [
        "Label1",
        "Label2"
      ]
    }
}
```
<br>

### 6 - Add this Environment Variable to your ```launchSettings.json```:
```
"APPLICATION_NAME": "MyKickassApi"
```

---

### How to use Serilog anywhere:

#### 1: Inject ILogger in constructor:
```
public MyClass(ILogger<MyClass> logger)
{
    _logger = logger;
}
```

#### 2: Writing Logs:
```
public void MyMethod()
{
    _logger.LogDebug("Debug Log");
    _logger.LogTrace("Trace Log");
    _logger.LogCritical("Critical Log");
    _logger.LogError("Error Log");
    _logger.LogWarning("Warning Log");
    _logger.LogInformation("Information Log");
}
```
