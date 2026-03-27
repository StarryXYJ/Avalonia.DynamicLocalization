using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using DynamicLocalization.Avalonia.Providers;

namespace DynamicLocalization.Avalonia.Core;

public class LanguageService : ILanguageService, INotifyPropertyChanged
{
    private readonly List<ILocalizationProvider> _providers = new();
    private CultureInfo _currentLanguage;
    private List<CultureInfo>? _availableLanguages;
    private CultureInfo _currentLanguageField;

    public event PropertyChangedEventHandler? PropertyChanged;
    
    public event EventHandler<LanguageChangedEventArgs>? LanguageChanged;

    public CultureInfo CurrentLanguage
    {
        get => _currentLanguageField;
        set
        {
            if (_currentLanguageField != value)
            {
                var oldLanguage = _currentLanguage;
                _currentLanguage = value;
                CultureInfo.CurrentUICulture = value;
                CultureInfo.DefaultThreadCurrentUICulture = value;
                _currentLanguageField = value;
                OnPropertyChanged(nameof(CurrentLanguage));
                OnLanguageChanged(value, oldLanguage);
            }
        }
    }

    public IReadOnlyList<CultureInfo> AvailableLanguages
    {
        get
        {
            if (_availableLanguages == null)
            {
                _availableLanguages = _providers
                    .SelectMany(p => p.GetAvailableCultures())
                    .Distinct()
                    .ToList();
            }
            return _availableLanguages;
        }
    }

    public string this[string key] => GetString(key);

    public LanguageService()
    {
        _currentLanguage = CultureInfo.CurrentUICulture;
        _currentLanguageField = _currentLanguage;
    }

    public string GetString(string key)
    {
        return GetString(key, _currentLanguage);
    }

    public string GetString(string key, CultureInfo? culture)
    {
        culture ??= _currentLanguage;

        foreach (var provider in _providers)
        {
            if (provider.TryGetString(key, culture, out var value) && value != null)
            {
                return value;
            }
        }

        if (AvailableLanguages.Count > 0 && !AvailableLanguages.Contains(culture))
        {
            var fallbackCulture = AvailableLanguages[0];
            foreach (var provider in _providers)
            {
                if (provider.TryGetString(key, fallbackCulture, out var value) && value != null)
                {
                    return value;
                }
            }
        }

        return $"#{key}#";
    }

    public string Format(string key, params object[] args)
    {
        var format = GetString(key);
        return string.Format(_currentLanguage, format, args);
    }

    public void RegisterProvider(ILocalizationProvider provider)
    {
        _providers.Add(provider);
        _availableLanguages = null;
    }

    public void UnregisterProvider(string providerName)
    {
        _providers.RemoveAll(p => p.Name == providerName);
        _availableLanguages = null;
    }

    protected virtual void OnLanguageChanged(CultureInfo newLanguage, CultureInfo oldLanguage)
    {
        LanguageChanged?.Invoke(this, new LanguageChangedEventArgs(newLanguage, oldLanguage));
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
