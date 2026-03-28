using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace DynamicLocalization.Demo.Avalonia.Json.Converters;

public class FontSizeConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string strValue && double.TryParse(strValue, out double fontSize))
        {
            if (parameter is string multiplierStr && double.TryParse(multiplierStr, out double multiplier))
            {
                return fontSize * multiplier;
            }
            return fontSize;
        }
        return 12;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
