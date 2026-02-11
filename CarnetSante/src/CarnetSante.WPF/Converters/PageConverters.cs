using CarnetSante.WPF.ViewModels;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CarnetSante.WPF.Converters;

public class PageToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is AppPage page && parameter is string param)
            return Enum.TryParse<AppPage>(param, out var target) && page == target
                ? Visibility.Visible : Visibility.Collapsed;
        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

public class BoolToSidebarColorConverter : IValueConverter
{
    private static readonly Color Active = Color.FromRgb(0x25, 0x35, 0x45);
    private static readonly Color Inactive = Color.FromRgb(0x1B, 0x2A, 0x3B);

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        => value is bool b && b ? Active : Inactive;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
