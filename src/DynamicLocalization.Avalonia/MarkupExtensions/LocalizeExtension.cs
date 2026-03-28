using System;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using DynamicLocalization.Core;

namespace DynamicLocalization.Avalonia.MarkupExtensions;

/// <summary>
/// XAML markup extension for binding localized strings in XAML.
/// </summary>
/// <remarks>
/// <para>
/// This extension retrieves the culture service through <see cref="LocalizationService"/>
/// and returns a binding object bound to <see cref="LocalizedString"/>,
/// enabling automatic UI updates when culture changes.
/// </para>
/// </remarks>
/// <example>
/// Basic usage:
/// <code>
/// &lt;TextBlock Text="{loc:Localize Greeting}"/&gt;
/// </code>
/// 
/// With string format:
/// <code>
/// &lt;TextBlock Text="{loc:Localize WelcomeMessage, StringFormat='Hello, {0}!'}"/&gt;
/// </code>
/// 
/// With converter:
/// <code>
/// &lt;TextBlock Text="{loc:Localize Status, Converter={StaticResource MyConverter}}"/&gt;
/// </code>
/// 
/// With converter and parameter:
/// <code>
/// &lt;TextBlock Text="{loc:Localize Status, Converter={StaticResource MyConverter}, ConverterParameter='Active'}"/&gt;
/// </code>
/// </example>
public class LocalizeExtension(string key) : MarkupExtension
{
    /// <summary>
    /// Gets the localization key.
    /// </summary>
    public string Key { get; } = key;

    /// <summary>
    /// Gets or sets the string format template.
    /// </summary>
    public string? StringFormat { get; set; }

    /// <summary>
    /// Gets or sets the value converter.
    /// </summary>
    public IValueConverter? Converter { get; set; }

    /// <summary>
    /// Gets or sets the parameter passed to the converter.
    /// </summary>
    public object? ConverterParameter { get; set; }

    /// <summary>
    /// Provides the binding value.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <returns>A binding object bound to the localized string.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var cultureService = LocalizationService.CultureService;
        if (cultureService == null)
        {
            return $"#{Key}#";
        }

        var localizedString = new LocalizedString(cultureService, Key);

        var binding = new Binding(nameof(LocalizedString.Value))
        {
            Source = localizedString,
            Mode = BindingMode.OneWay,
            StringFormat = StringFormat,
            Converter = Converter,
            ConverterParameter = ConverterParameter
        };

        return binding;
    }
}
