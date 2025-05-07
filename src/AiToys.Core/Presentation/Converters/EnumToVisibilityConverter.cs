using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace AiToys.Core.Presentation.Converters;

/// <summary>
/// Converts an enum value to Visibility based on equality with a parameter.
/// </summary>
internal sealed partial class EnumToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Converts an enum value to Visibility.Visible if it equals the parameter, otherwise Visibility.Collapsed.
    /// </summary>
    /// <param name="value">The source enum value.</param>
    /// <param name="targetType">The target type, expected to be Visibility.</param>
    /// <param name="parameter">The enum value to compare against, can be a string or enum value.</param>
    /// <param name="language">The language.</param>
    /// <returns>Visibility.Visible if value equals parameter, otherwise Visibility.Collapsed.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value == null || parameter == null)
        {
            return Visibility.Collapsed;
        }

        if (parameter is string parameterString && value.GetType().IsEnum)
        {
            return string.Equals(value.ToString(), parameterString, StringComparison.Ordinal)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        return value.Equals(parameter) ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// Not implemented as this is a one-way converter.
    /// </summary>
    /// <param name="value">The value produced by the binding target.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="language">The culture to use in the converter.</param>
    /// <exception cref="NotSupportedException">Always thrown as this method is not supported.</exception>
    /// <returns>Throws NotSupportedException.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException("EnumToVisibilityConverter is a one-way converter.");
    }
}
