using CarnetSante.Core.Enums;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CarnetSante.WPF.Converters;

/// <summary>
/// Convertit null/empty en Visibility.Collapsed.
/// </summary>
[ValueConversion(typeof(string), typeof(Visibility))]
public class NullToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => string.IsNullOrEmpty(value?.ToString()) ? Visibility.Collapsed : Visibility.Visible;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

/// <summary>
/// Convertit bool en Visibility.
/// </summary>
[ValueConversion(typeof(bool), typeof(Visibility))]
public class BoolToVisibilityConverter : IValueConverter
{
    public bool Invert { get; set; } = false;

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool v = value is bool b && b;
        if (Invert) v = !v;
        return v ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is Visibility vis && vis == Visibility.Visible;
}

/// <summary>
/// Convertit un bool IsLoading en texte de bouton.
/// ConverterParameter="Texte chargement|Texte normal"
/// </summary>
[ValueConversion(typeof(bool), typeof(string))]
public class LoadingToTextConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool loading = value is bool b && b;
        string param = parameter?.ToString() ?? "Chargement...|Valider";
        var parts = param.Split('|');
        return loading ? parts[0] : (parts.Length > 1 ? parts[1] : param);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

/// <summary>
/// Calcule l'âge à partir de la date de naissance.
/// </summary>
[ValueConversion(typeof(DateTime), typeof(string))]
public class DateToAgeConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is DateTime dob)
        {
            var today = DateTime.Today;
            int age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age)) age--;
            return $"{age} ans";
        }
        return "-";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

/// <summary>
/// Inversion de booléen pour IsEnabled/IsReadOnly.
/// </summary>
[ValueConversion(typeof(bool), typeof(bool))]
public class InverseBoolConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && !b;

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is bool b && !b;
}

/// <summary>
/// Traduit un enum TypeDecisionReforme en libellé lisible.
/// </summary>
[ValueConversion(typeof(TypeDecisionReforme), typeof(string))]
public class DecisionReformeConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is TypeDecisionReforme d)
            return d switch
            {
                TypeDecisionReforme.Maintien          => "Maintien en service",
                TypeDecisionReforme.ReformeDefinitive => "Réforme définitive",
                TypeDecisionReforme.ReformeTemporaire => "Réforme temporaire",
                TypeDecisionReforme.Reengagement      => "Réengagement",
                TypeDecisionReforme.Reclassement      => "Reclassement",
                _                                     => d.ToString()
            };
        return value?.ToString() ?? "-";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}

/// <summary>
/// Traduit l'enum AptitudeMedicale en libellé lisible.
/// </summary>
[ValueConversion(typeof(AptitudeMedicale), typeof(string))]
public class AptitudeMedicaleConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is AptitudeMedicale a)
            return a switch
            {
                AptitudeMedicale.Apte        => "Apte",
                AptitudeMedicale.Inapte      => "Inapte",
                AptitudeMedicale.ASurveiller => "À surveiller",
                AptitudeMedicale.ApteLimite  => "Apte limité",
                _                            => a.ToString()
            };
        return value?.ToString() ?? "-";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
