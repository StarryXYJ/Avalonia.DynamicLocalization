# DynamicLocalization

[![MIT License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![NuGet](https://img.shields.io/nuget/v/DynamicLocalization.Core.svg)](https://www.nuget.org/packages/DynamicLocalization.Core/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/DynamicLocalization.Core.svg)](https://www.nuget.org/packages/DynamicLocalization.Core/)

[中文文档](README.zh-CN.md)

A lightweight, extensible, and pluggable internationalization library with hot-reload support and multiple data sources for Avalonia and WPF.

## Features

- 🌍 **Multi-language Support** - Support for any number of languages
- 🔄 **Hot Reload** - Dynamically switch languages at runtime without restart
- 🔌 **Pluggable Architecture** - Support for custom data source providers
- 🧩 **Plugin Support** - Dynamic provider registration/unregistration for plugin scenarios
- 📦 **JSON Support** - Built-in JSON localization file support (flat and nested formats)
- 📄 **RESX Support** - Built-in RESX resource file support
- 🎯 **XAML Friendly** - Provides clean XAML markup extensions
- 💉 **DI Integration** - Full dependency injection support
- 🖥️ **Multi-Platform** - Support for Avalonia and WPF

## Packages

| Package | Description | Platform |
|---------|-------------|----------|
| [![NuGet](https://img.shields.io/nuget/v/DynamicLocalization.Core.svg)](https://www.nuget.org/packages/DynamicLocalization.Core/) [DynamicLocalization.Core](https://www.nuget.org/packages/DynamicLocalization.Core/) | Core library with platform-independent logic | .NET 6+ |
| [![NuGet](https://img.shields.io/nuget/v/DynamicLocalization.Avalonia.svg)](https://www.nuget.org/packages/DynamicLocalization.Avalonia/) [DynamicLocalization.Avalonia](https://www.nuget.org/packages/DynamicLocalization.Avalonia/) | Avalonia platform implementation | Avalonia 11+ |
| [![NuGet](https://img.shields.io/nuget/v/DynamicLocalization.WPF.svg)](https://www.nuget.org/packages/DynamicLocalization.WPF/) [DynamicLocalization.WPF](https://www.nuget.org/packages/DynamicLocalization.WPF/) | WPF platform implementation | WPF (.NET 6+) |

## Installation

### Avalonia

```xml
<PackageReference Include="DynamicLocalization.Avalonia" />
```

### WPF

```xml
<PackageReference Include="DynamicLocalization.WPF" />
```

## Quick Start

### 1. Create Localization Files

#### Option A: JSON Files

Create a `Localization` folder in your project and add JSON files.

**Flat Format (Traditional):**

**Localization/en.json**
```json
{
  "App.Title": "My Application",
  "Greeting": "Hello, World!",
  "WelcomeMessage": "Welcome to our application."
}
```

**Nested Format (Recommended for better organization):**

**Localization/en.json**
```json
{
  "App": {
    "Title": "My Application",
    "Version": "1.0.0"
  },
  "Greeting": "Hello, World!",
  "WelcomeMessage": "Welcome to our application.",
  "Features": {
    "Title": "Features:",
    "HotReload": "Hot reload support",
    "Pluggable": "Pluggable provider system"
  }
}
```

Both formats produce the same keys: `App.Title`, `App.Version`, `Greeting`, `WelcomeMessage`, `Features.Title`, etc.

**Localization/zh-CN.json**
```json
{
  "App": {
    "Title": "我的应用",
    "Version": "1.0.0"
  },
  "Greeting": "你好，世界！",
  "WelcomeMessage": "欢迎使用我们的应用程序。",
  "Features": {
    "Title": "特性：",
    "HotReload": "热重载支持",
    "Pluggable": "可插拔的提供程序系统"
  }
}
```

#### Option B: RESX Files

Add RESX resource files to your project:

**Resources/Strings.resx** (Default/English)
```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="App.Title" xml:space="preserve">
    <value>My Application</value>
  </data>
  <data name="Greeting" xml:space="preserve">
    <value>Hello, World!</value>
  </data>
</root>
```

**Resources/Strings.zh-CN.resx** (Chinese)
```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
  <data name="App.Title" xml:space="preserve">
    <value>我的应用</value>
  </data>
  <data name="Greeting" xml:space="preserve">
    <value>你好，世界！</value>
  </data>
</root>
```

### 2. Configure Services

#### Avalonia (App.axaml.cs)

```csharp
using DynamicLocalization.Avalonia.Extensions;
using DynamicLocalization.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

public partial class App : Application
{
    public IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        var services = new ServiceCollection();
        
        // Option A: JSON files
        services.AddJsonLocalization(options =>
        {
            options.BasePath = "Localization";
            options.UseEmbeddedResources = true;
            options.Assembly = typeof(App).Assembly;
        });

        // Option B: RESX files
        // services.AddResxLocalization(options =>
        // {
        //     options.ResourceType = typeof(Resources.Strings);
        // });

        services.AddCultureService();
        Services = services.BuildServiceProvider().InitializeLocalization();
        AvaloniaXamlLoader.Load(this);
    }
}
```

#### WPF (App.xaml.cs)

```csharp
using DynamicLocalization.WPF.Extensions;
using DynamicLocalization.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

public partial class App : Application
{
    public IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();
        
        // Option A: JSON files
        services.AddJsonLocalization(options =>
        {
            options.BasePath = "Localization";
            options.UseEmbeddedResources = true;
            options.Assembly = typeof(App).Assembly;
        });

        // Option B: RESX files
        // services.AddResxLocalization(options =>
        // {
        //     options.ResourceType = typeof(Properties.Resources);
        // });

        services.AddCultureService();
        Services = services.BuildServiceProvider().InitializeLocalization();
        
        base.OnStartup(e);
    }
}
```

### 2b. Without Dependency Injection (Singleton Pattern)

If you prefer not to use dependency injection, you can use the singleton pattern directly:

#### Avalonia (App.axaml.cs)

```csharp
using DynamicLocalization.Avalonia;
using DynamicLocalization.Core;
using DynamicLocalization.Core.Providers;

public partial class App : Application
{
    public override void Initialize()
    {
        // Create and initialize JSON provider
        var jsonProvider = new JsonLocalizationProvider();
        jsonProvider.Initialize(new JsonLocalizationProviderOptions
        {
            BasePath = "Localization",
            UseEmbeddedResources = true,
            Assembly = typeof(App).Assembly
        });

        // Or create and initialize RESX provider
        // var resxProvider = new ResxLocalizationProvider();
        // resxProvider.Initialize(new ResxLocalizationProviderOptions
        // {
        //     ResourceType = typeof(Resources.Strings)
        // });

        // Create culture service and register provider
        var cultureService = new CultureService();
        cultureService.RegisterProvider(jsonProvider);
        
        // Initialize static service for XAML markup extensions
        LocalizationService.Initialize(cultureService);
        
        AvaloniaXamlLoader.Load(this);
    }
}
```

#### WPF (App.xaml.cs)

```csharp
using DynamicLocalization.WPF;
using DynamicLocalization.Core;
using DynamicLocalization.Core.Providers;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Create and initialize JSON provider
        var jsonProvider = new JsonLocalizationProvider();
        jsonProvider.Initialize(new JsonLocalizationProviderOptions
        {
            BasePath = "Localization",
            UseEmbeddedResources = true,
            Assembly = typeof(App).Assembly
        });

        // Or create and initialize RESX provider
        // var resxProvider = new ResxLocalizationProvider();
        // resxProvider.Initialize(new ResxLocalizationProviderOptions
        // {
        //     ResourceType = typeof(Properties.Resources)
        // });

        // Create culture service and register provider
        var cultureService = new CultureService();
        cultureService.RegisterProvider(jsonProvider);
        
        // Initialize static service for XAML markup extensions
        LocalizationService.Initialize(cultureService);
        
        base.OnStartup(e);
    }
}
```

#### Accessing the Service

```csharp
// Get the singleton instance
var cultureService = LocalizationService.CultureService;

// Get localized string
var greeting = cultureService["Greeting"];

// Change culture
cultureService.SetCulture("zh-CN");

// Subscribe to culture changes
cultureService.CultureChanged += (s, e) => 
{
    Console.WriteLine($"Culture changed from {e.OldCulture.Name} to {e.NewCulture.Name}");
};
```

### 3. Using in XAML

#### Avalonia

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:loc="clr-namespace:DynamicLocalization.Avalonia.MarkupExtensions;assembly=DynamicLocalization.Avalonia"
        Title="{loc:Localize App.Title}">

    <StackPanel Margin="20">
        <TextBlock Text="{loc:Localize Greeting}" FontSize="24"/>
        <TextBlock Text="{loc:Localize WelcomeMessage}"/>
        <TextBlock Text="{loc:Localize Features.HotReload}"/>
    </StackPanel>
</Window>
```

#### WPF

```xml
<Window xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:loc="clr-namespace:DynamicLocalization.WPF.MarkupExtensions;assembly=DynamicLocalization.WPF"
        Title="{loc:Localize App.Title}">

    <StackPanel Margin="20">
        <TextBlock Text="{loc:Localize Greeting}" FontSize="24"/>
        <TextBlock Text="{loc:Localize WelcomeMessage}"/>
        <TextBlock Text="{loc:Localize Features.HotReload}"/>
    </StackPanel>
</Window>
```

### LocalizeExtension Properties

The `LocalizeExtension` supports the following optional properties:

| Property | Type | Description |
|----------|------|-------------|
| `Key` | `string` | The localization key (required, constructor parameter) |
| `StringFormat` | `string?` | Format string applied to the localized value |
| `Converter` | `IValueConverter?` | Value converter for transforming the localized string |
| `ConverterParameter` | `object?` | Parameter passed to the converter |

#### Examples

**With StringFormat:**
```xml
<TextBlock Text="{loc:Localize WelcomeMessage, StringFormat='Hello, {0}!'}"/>
```

**With Converter:**
```xml
<Window.Resources>
    <local:FontSizeConverter x:Key="FontSizeConverter"/>
</Window.Resources>

<!-- Using converter to transform string to font size -->
<TextBlock Text="{loc:Localize SampleText}" 
           FontSize="{loc:Localize FontSize.Default, Converter={StaticResource FontSizeConverter}}"/>

<!-- With converter parameter (multiplier) -->
<TextBlock Text="{loc:Localize SampleText}" 
           FontSize="{loc:Localize FontSize.Default, Converter={StaticResource FontSizeConverter}, ConverterParameter=1.5}"/>
```

**Culture-aware Font Size Example:**

JSON resource files:
```json
// en.json
{
  "FontSize": {
    "Default": "16",
    "SampleText": "This text size changes with culture!"
  }
}

// zh-CN.json
{
  "FontSize": {
    "Default": "18",
    "SampleText": "这段文字的大小会随文化变化！"
  }
}
```

XAML:
```xml
<!-- Default type conversion (string to double) -->
<TextBlock Text="{loc:Localize FontSize.SampleText}" 
           FontSize="{loc:Localize FontSize.Default}"/>
```

### 4. Using in ViewModel

```csharp
using DynamicLocalization.Core;
using System.Globalization;

public class MainViewModel
{
    private readonly ICultureService _cultureService;

    public string Greeting => _cultureService["Greeting"];
    
    public IReadOnlyList<CultureInfo> AvailableCultures => _cultureService.AvailableCultures;

    public MainViewModel(ICultureService cultureService)
    {
        _cultureService = cultureService;
        _cultureService.CultureChanged += OnCultureChanged;
    }

    public void ChangeCulture(CultureInfo culture)
    {
        _cultureService.CurrentCulture = culture;
    }

    private void OnCultureChanged(object? sender, CultureChangedEventArgs e)
    {
        // Update bound properties
    }
}
```

## JSON Format Details

The JSON provider supports two formats:

### Flat Format
```json
{
  "App.Title": "My App",
  "App.Version": "1.0",
  "Features.HotReload": "Hot reload support"
}
```

### Nested Format
```json
{
  "App": {
    "Title": "My App",
    "Version": "1.0"
  },
  "Features": {
    "HotReload": "Hot reload support"
  }
}
```

Both formats result in the same keys: `App.Title`, `App.Version`, `Features.HotReload`.

The nested format is recommended for better organization and readability, especially for large projects.

## API Reference

### ICultureService

Core culture service interface:

| Property/Method | Description |
|-----------------|-------------|
| `CurrentCulture` | Gets or sets the current culture |
| `CurrentCultureName` | Gets the current culture name (e.g., "en", "zh-CN") |
| `AvailableCultures` | Gets the list of all available cultures |
| `this[string key]` | Gets the localized string for the specified key |
| `GetString(string key)` | Gets a localized string |
| `GetString(string key, CultureInfo? culture)` | Gets a localized string for the specified culture |
| `Format(string key, params object[] args)` | Formats a localized string |
| `SetCulture(string cultureName)` | Sets the current culture by name |
| `SetCulture(string cultureName, bool includeFormatting)` | Sets the current culture with optional formatting culture |
| `RegisterProvider(ILocalizationProvider provider)` | Registers a localization provider |
| `UnregisterProvider(string providerName)` | Unregisters a localization provider by name |
| `CultureChanged` | Culture changed event |
| `ProvidersChanged` | Provider registered/unregistered event |

### ILocalizationProvider

Data source provider interface for custom implementations:

```csharp
public interface ILocalizationProvider
{
    string Name { get; }
    IEnumerable<CultureInfo> GetAvailableCultures();
    string? GetString(string key, CultureInfo culture);
    bool TryGetString(string key, CultureInfo culture, out string? value);
    Task ReloadAsync(CancellationToken cancellationToken = default);
}
```

## Provider Options

### JsonLocalizationProvider

| Option | Description | Default |
|--------|-------------|---------|
| `BasePath` | Directory containing JSON files | `"Localization"` |
| `FilePattern` | File matching pattern | `"*.json"` |
| `UseEmbeddedResources` | Whether to load from embedded resources | `false` |
| `Assembly` | Specified assembly (embedded resource mode) | Calling assembly |

### ResxLocalizationProvider

| Option | Description | Default |
|--------|-------------|---------|
| `ResourceType` | Type of the RESX resource file | Required |
| `AutoDetectCultures` | Whether to auto-detect available cultures | `true` |
| `KnownCultures` | Manually specify known culture list | `null` |

## Custom Provider

Implement the `ILocalizationProvider` interface to create custom data sources:

```csharp
public class DatabaseLocalizationProvider : ILocalizationProvider
{
    public string Name => "Database";

    public IEnumerable<CultureInfo> GetAvailableCultures()
    {
        // Get supported languages from database
    }

    public string? GetString(string key, CultureInfo culture)
    {
        // Get localized string from database
    }

    public bool TryGetString(string key, CultureInfo culture, out string? value)
    {
        value = GetString(key, culture);
        return value != null;
    }

    public async Task ReloadAsync(CancellationToken cancellationToken = default)
    {
        // Reload data
    }
}
```

## Plugin Integration

DynamicLocalization supports dynamic provider registration/unregistration, making it ideal for plugin architectures.

### Plugin Localization Setup

```csharp
public class PluginLocalizationProvider : ILocalizationProvider
{
    private readonly Dictionary<string, Dictionary<string, string>> _cache = new();

    public string Name => "MyPlugin";  // Use a unique name to avoid conflicts

    public PluginLocalizationProvider()
    {
        LoadFromEmbeddedResources();
    }

    private void LoadFromEmbeddedResources()
    {
        var assembly = typeof(PluginLocalizationProvider).Assembly;
        var resourceNames = assembly.GetManifestResourceNames();

        foreach (var name in resourceNames)
        {
            if (!name.Contains(".Localization.") || !name.EndsWith(".json"))
                continue;

            var cultureName = ExtractCultureName(name);
            if (string.IsNullOrEmpty(cultureName)) continue;

            using var stream = assembly.GetManifestResourceStream(name);
            if (stream == null) continue;

            using var reader = new StreamReader(stream);
            var json = reader.ReadToEnd();
            var dict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
            if (dict != null)
            {
                _cache[cultureName] = dict;
            }
        }
    }

    // ... implement other interface methods
}
```

### Plugin Lifecycle Management

```csharp
public class PluginEntryPoint
{
    private readonly ICultureService _cultureService;
    private readonly PluginLocalizationProvider _provider;

    public PluginEntryPoint(ICultureService cultureService)
    {
        _cultureService = cultureService;
        _provider = new PluginLocalizationProvider();
    }

    public void Initialize()
    {
        _cultureService.RegisterProvider(_provider);
        // UI automatically refreshes to include plugin translations
    }

    public void Unload()
    {
        _cultureService.UnregisterProvider(_provider.Name);
        // UI automatically refreshes to remove plugin translations
    }
}
```

### Key Naming Convention

Use a prefix to avoid key conflicts with the main application or other plugins:

| Format | Example |
|--------|---------|
| `{PluginName}.{Feature}.{Item}` | `MyPlugin.Menu.Open` |
| `{PluginName}.{Item}` | `MyPlugin.Title` |

## Extending Providers

Both `JsonLocalizationProvider` and `ResxLocalizationProvider` are designed for inheritance. Key methods are `protected virtual` for easy customization.

### Extending JsonLocalizationProvider

```csharp
public class CustomJsonProvider : JsonLocalizationProvider
{
    public override string Name => "CustomJson";  // Custom provider name

    protected override string? ExtractCultureName(string resourceName)
    {
        // Custom resource name parsing logic
        return base.ExtractCultureName(resourceName);
    }

    protected override Dictionary<string, string>? ParseJsonToFlatDictionary(string json)
    {
        // Custom JSON parsing (e.g., support YAML or other formats)
        return base.ParseJsonToFlatDictionary(json);
    }
}
```

### Extending ResxLocalizationProvider

```csharp
public class CustomResxProvider : ResxLocalizationProvider
{
    public override string Name => "CustomResx";

    protected override void DetectAvailableCultures()
    {
        // Custom culture detection logic
        base.DetectAvailableCultures();
    }
}
```

### Overridable Members

**JsonLocalizationProvider:**
| Member | Description |
|--------|-------------|
| `Name` | Provider identifier |
| `LoadAll()` | Load all resources |
| `LoadFromEmbeddedResources()` | Load from embedded resources |
| `LoadFromFiles()` | Load from file system |
| `ExtractCultureName()` | Extract culture from resource name |
| `ParseJsonToFlatDictionary()` | Parse JSON to dictionary |
| `FlattenJsonObject()` | Flatten nested JSON |
| `TryGetFromCulture()` | Get string from specific culture |

**ResxLocalizationProvider:**
| Member | Description |
|--------|-------------|
| `Name` | Provider identifier |
| `DetectAvailableCultures()` | Detect available cultures |

## Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                      DynamicLocalization                        │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                  DynamicLocalization.Core               │    │
│  │  - ICultureService, CultureService                      │    │
│  │  - ILocalizationProvider, Providers                     │    │
│  │  - Platform-independent logic                           │    │
│  └─────────────────────────────────────────────────────────┘    │
│                              │                                  │
│              ┌───────────────┴───────────────┐                  │
│              ▼                               ▼                  │
│  ┌─────────────────────┐         ┌─────────────────────┐        │
│  │ DynamicLocalization │         │ DynamicLocalization │        │
│  │      .Avalonia      │         │        .WPF         │        │
│  │  - Avalonia Binding │         │  - WPF Binding      │        │
│  └─────────────────────┘         └─────────────────────┘        │
│                                                                 │
└─────────────────────────────────────────────────────────────────┘
```

## License

MIT License
