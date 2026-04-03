# DynamicLocalization

[![MIT License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![NuGet](https://img.shields.io/nuget/v/DynamicLocalization.Core.svg)](https://www.nuget.org/packages/DynamicLocalization.Core/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/DynamicLocalization.Core.svg)](https://www.nuget.org/packages/DynamicLocalization.Core/)

[English](README.md)

一个轻量级、可扩展、可插拔的国际化库，支持热重载和多种数据源，适用于 Avalonia 和 WPF。

## 特性

- 🌍 **多语言支持** - 支持任意数量的语言
- 🔄 **热重载** - 运行时动态切换语言，无需重启
- 🔌 **可插拔架构** - 支持自定义数据源提供者
- 🧩 **插件支持** - 动态注册/注销提供者，支持插件场景
- 📦 **JSON 支持** - 内置 JSON 本地化文件支持（扁平格式和嵌套格式）
- 📄 **RESX 支持** - 内置 RESX 资源文件支持
- 🎯 **XAML 友好** - 提供简洁的 XAML 标记扩展
- 💉 **DI 集成** - 完整的依赖注入支持
- 🖥️ **多平台** - 支持 Avalonia 和 WPF

## 包

| 包 | 描述 | 平台 |
|---------|-------------|----------|
| [![NuGet](https://img.shields.io/nuget/v/DynamicLocalization.Core.svg)](https://www.nuget.org/packages/DynamicLocalization.Core/) [DynamicLocalization.Core](https://www.nuget.org/packages/DynamicLocalization.Core/) | 核心库，包含平台无关逻辑 | .NET 6+ |
| [![NuGet](https://img.shields.io/nuget/v/DynamicLocalization.Avalonia.svg)](https://www.nuget.org/packages/DynamicLocalization.Avalonia/) [DynamicLocalization.Avalonia](https://www.nuget.org/packages/DynamicLocalization.Avalonia/) | Avalonia 平台实现 | Avalonia 11+ |
| [![NuGet](https://img.shields.io/nuget/v/DynamicLocalization.WPF.svg)](https://www.nuget.org/packages/DynamicLocalization.WPF/) [DynamicLocalization.WPF](https://www.nuget.org/packages/DynamicLocalization.WPF/) | WPF 平台实现 | WPF (.NET 6+) |

## 安装

### Avalonia

```xml
<PackageReference Include="DynamicLocalization.Avalonia" />
```

### WPF

```xml
<PackageReference Include="DynamicLocalization.WPF" />
```

## 快速开始

### 1. 创建本地化文件

#### 方式 A: JSON 文件

在项目中创建 `Localization` 文件夹并添加 JSON 文件。

**扁平格式（传统）：**

**Localization/en.json**
```json
{
  "App.Title": "My Application",
  "Greeting": "Hello, World!",
  "WelcomeMessage": "Welcome to our application."
}
```

**嵌套格式（推荐，结构更清晰）：**

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

两种格式生成的键相同：`App.Title`、`App.Version`、`Greeting`、`WelcomeMessage`、`Features.Title` 等。

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

#### 方式 B: RESX 文件

在项目中添加 RESX 资源文件：

**Resources/Strings.resx** (默认/英文)
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

**Resources/Strings.zh-CN.resx** (中文)
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

### 2. 配置服务

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
        
        // 方式 A: JSON 文件
        services.AddJsonLocalization(options =>
        {
            options.BasePath = "Localization";
            options.UseEmbeddedResources = true;
            options.Assembly = typeof(App).Assembly;
        });

        // 方式 B: RESX 文件
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
        
        // 方式 A: JSON 文件
        services.AddJsonLocalization(options =>
        {
            options.BasePath = "Localization";
            options.UseEmbeddedResources = true;
            options.Assembly = typeof(App).Assembly;
        });

        // 方式 B: RESX 文件
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

### 2b. 不使用依赖注入（单例模式）

如果您不想使用依赖注入，可以直接使用单例模式：

#### Avalonia (App.axaml.cs)

```csharp
using DynamicLocalization.Avalonia;
using DynamicLocalization.Core;
using DynamicLocalization.Core.Providers;

public partial class App : Application
{
    public override void Initialize()
    {
        // 创建并初始化 JSON 提供者
        var jsonProvider = new JsonLocalizationProvider();
        jsonProvider.Initialize(new JsonLocalizationProviderOptions
        {
            BasePath = "Localization",
            UseEmbeddedResources = true,
            Assembly = typeof(App).Assembly
        });

        // 或创建并初始化 RESX 提供者
        // var resxProvider = new ResxLocalizationProvider();
        // resxProvider.Initialize(new ResxLocalizationProviderOptions
        // {
        //     ResourceType = typeof(Resources.Strings)
        // });

        // 创建文化服务并注册提供者
        var cultureService = new CultureService();
        cultureService.RegisterProvider(jsonProvider);
        
        // 初始化静态服务以供 XAML 标记扩展使用
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
        // 创建并初始化 JSON 提供者
        var jsonProvider = new JsonLocalizationProvider();
        jsonProvider.Initialize(new JsonLocalizationProviderOptions
        {
            BasePath = "Localization",
            UseEmbeddedResources = true,
            Assembly = typeof(App).Assembly
        });

        // 或创建并初始化 RESX 提供者
        // var resxProvider = new ResxLocalizationProvider();
        // resxProvider.Initialize(new ResxLocalizationProviderOptions
        // {
        //     ResourceType = typeof(Properties.Resources)
        // });

        // 创建文化服务并注册提供者
        var cultureService = new CultureService();
        cultureService.RegisterProvider(jsonProvider);
        
        // 初始化静态服务以供 XAML 标记扩展使用
        LocalizationService.Initialize(cultureService);
        
        base.OnStartup(e);
    }
}
```

#### 访问服务

```csharp
// 获取单例实例
var cultureService = LocalizationService.CultureService;

// 获取本地化字符串
var greeting = cultureService["Greeting"];

// 切换文化
cultureService.SetCulture("zh-CN");

// 订阅文化变更事件
cultureService.CultureChanged += (s, e) => 
{
    Console.WriteLine($"文化已从 {e.OldCulture.Name} 切换到 {e.NewCulture.Name}");
};
```

### 3. 在 XAML 中使用

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

### 4. 在 ViewModel 中使用

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
        // 更新绑定属性
    }
}
```

## JSON 格式详解

JSON 提供者支持两种格式：

### 扁平格式
```json
{
  "App.Title": "My App",
  "App.Version": "1.0",
  "Features.HotReload": "Hot reload support"
}
```

### 嵌套格式
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

两种格式生成的键相同：`App.Title`、`App.Version`、`Features.HotReload`。

推荐使用嵌套格式，结构更清晰，特别适合大型项目。

## API 参考

### ICultureService

核心文化服务接口：

| 属性/方法 | 描述 |
|-----------------|-------------|
| `CurrentCulture` | 获取或设置当前文化 |
| `CurrentCultureName` | 获取当前文化名称（如 "en"、"zh-CN"） |
| `AvailableCultures` | 获取所有可用文化列表 |
| `this[string key]` | 获取指定键的本地化字符串 |
| `GetString(string key)` | 获取本地化字符串 |
| `GetString(string key, CultureInfo? culture)` | 获取指定区域性的本地化字符串 |
| `Format(string key, params object[] args)` | 格式化本地化字符串 |
| `SetCulture(string cultureName)` | 通过名称设置当前文化 |
| `SetCulture(string cultureName, bool includeFormatting)` | 设置当前文化，可选择是否包含格式化文化 |
| `RegisterProvider(ILocalizationProvider provider)` | 注册本地化提供者 |
| `UnregisterProvider(string providerName)` | 通过名称注销本地化提供者 |
| `CultureChanged` | 文化更改事件 |
| `ProvidersChanged` | 提供者注册/注销事件 |

### ILocalizationProvider

数据源提供者接口，用于自定义实现：

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

## 提供者选项

### JsonLocalizationProvider

| 选项 | 描述 | 默认值 |
|--------|-------------|---------|
| `BasePath` | JSON 文件所在目录 | `"Localization"` |
| `FilePattern` | 文件匹配模式 | `"*.json"` |
| `UseEmbeddedResources` | 是否从嵌入资源加载 | `false` |
| `Assembly` | 指定程序集（嵌入资源模式） | 调用程序集 |

### ResxLocalizationProvider

| 选项 | 描述 | 默认值 |
|--------|-------------|---------|
| `ResourceType` | RESX 资源文件类型 | 必需 |
| `AutoDetectCultures` | 是否自动检测可用区域性 | `true` |
| `KnownCultures` | 手动指定已知区域性列表 | `null` |

## 自定义提供者

实现 `ILocalizationProvider` 接口来创建自定义数据源：

```csharp
public class DatabaseLocalizationProvider : ILocalizationProvider
{
    public string Name => "Database";

    public IEnumerable<CultureInfo> GetAvailableCultures()
    {
        // 从数据库获取支持的语言
    }

    public string? GetString(string key, CultureInfo culture)
    {
        // 从数据库获取本地化字符串
    }

    public bool TryGetString(string key, CultureInfo culture, out string? value)
    {
        value = GetString(key, culture);
        return value != null;
    }

    public async Task ReloadAsync(CancellationToken cancellationToken = default)
    {
        // 重新加载数据
    }
}
```

## 插件集成

DynamicLocalization 支持动态注册/注销提供者，非常适合插件架构。

### 插件本地化设置

```csharp
public class PluginLocalizationProvider : ILocalizationProvider
{
    private readonly Dictionary<string, Dictionary<string, string>> _cache = new();

    public string Name => "MyPlugin";  // 使用唯一名称避免冲突

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

    // ... 实现其他接口方法
}
```

### 插件生命周期管理

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
        // UI 自动刷新，显示插件翻译
    }

    public void Unload()
    {
        _cultureService.UnregisterProvider(_provider.Name);
        // UI 自动刷新，移除插件翻译
    }
}
```

### 键命名规范

使用前缀避免与主程序或其他插件的键冲突：

| 格式 | 示例 |
|--------|---------|
| `{插件名}.{功能}.{项}` | `MyPlugin.Menu.Open` |
| `{插件名}.{项}` | `MyPlugin.Title` |

## 扩展提供者

`JsonLocalizationProvider` 和 `ResxLocalizationProvider` 都设计为可继承的。关键方法使用 `protected virtual` 修饰，便于自定义。

### 扩展 JsonLocalizationProvider

```csharp
public class CustomJsonProvider : JsonLocalizationProvider
{
    public override string Name => "CustomJson";  // 自定义提供者名称

    protected override string? ExtractCultureName(string resourceName)
    {
        // 自定义资源名称解析逻辑
        return base.ExtractCultureName(resourceName);
    }

    protected override Dictionary<string, string>? ParseJsonToFlatDictionary(string json)
    {
        // 自定义 JSON 解析（如支持 YAML 或其他格式）
        return base.ParseJsonToFlatDictionary(json);
    }
}
```

### 扩展 ResxLocalizationProvider

```csharp
public class CustomResxProvider : ResxLocalizationProvider
{
    public override string Name => "CustomResx";

    protected override void DetectAvailableCultures()
    {
        // 自定义文化检测逻辑
        base.DetectAvailableCultures();
    }
}
```

### 可重写成员

**JsonLocalizationProvider:**
| 成员 | 描述 |
|--------|-------------|
| `Name` | 提供者标识符 |
| `LoadAll()` | 加载所有资源 |
| `LoadFromEmbeddedResources()` | 从嵌入资源加载 |
| `LoadFromFiles()` | 从文件系统加载 |
| `ExtractCultureName()` | 从资源名提取文化 |
| `ParseJsonToFlatDictionary()` | 解析 JSON 为字典 |
| `FlattenJsonObject()` | 扁平化嵌套 JSON |
| `TryGetFromCulture()` | 从指定文化获取字符串 |

**ResxLocalizationProvider:**
| 成员 | 描述 |
|--------|-------------|
| `Name` | 提供者标识符 |
| `DetectAvailableCultures()` | 检测可用文化 |

## 架构

```
┌─────────────────────────────────────────────────────────────────┐
│                      DynamicLocalization                        │
├─────────────────────────────────────────────────────────────────┤
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐    │
│  │                  DynamicLocalization.Core               │    │
│  │  - ICultureService, CultureService                      │    │
│  │  - ILocalizationProvider, Providers                     │    │
│  │  - 平台无关逻辑                                          │    │
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

## 许可证

MIT License
