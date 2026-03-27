using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using DynamicLocalization.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DynamicLocalization.Demo.Wpf.Resx.ViewModels;

public partial class MainWindowViewModel : ViewModelBase, IDisposable
{
    private readonly ILanguageService _languageService;

    public string Title => _languageService["App.Title"];

    public string Greeting => _languageService["Greeting"];

    public string WelcomeMessage => _languageService["WelcomeMessage"];

    public string SwitchLanguageLabel => _languageService["SwitchLanguage"];

    public ObservableCollection<CultureInfo> AvailableLanguages { get; }

    [ObservableProperty]
    private CultureInfo? _selectedLanguage;

    public string CurrentCultureName => _languageService.CurrentLanguage.Name;

    public string ParentCultureName => _languageService.CurrentLanguage.Parent?.Name ?? "(none)";

    public string AvailableCulturesList => string.Join(", ", _languageService.AvailableLanguages.Select(c => c.Name));

    partial void OnSelectedLanguageChanged(CultureInfo? value)
    {
        if (value != null && _languageService.CurrentLanguage.Name != value.Name)
        {
            _languageService.CurrentLanguage = value;
        }
    }

    public MainWindowViewModel(ILanguageService languageService)
    {
        _languageService = languageService;
        AvailableLanguages = new ObservableCollection<CultureInfo>(languageService.AvailableLanguages);
        SelectedLanguage = languageService.CurrentLanguage;

        _languageService.LanguageChanged += OnLanguageChanged;
    }

    private void OnLanguageChanged(object? sender, LanguageChangedEventArgs e)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Greeting));
            OnPropertyChanged(nameof(WelcomeMessage));
            OnPropertyChanged(nameof(SwitchLanguageLabel));
            OnPropertyChanged(nameof(CurrentCultureName));
            OnPropertyChanged(nameof(ParentCultureName));
        });
    }

    public void Dispose()
    {
        _languageService.LanguageChanged -= OnLanguageChanged;
    }
}
