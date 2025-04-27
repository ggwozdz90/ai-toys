using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace AiToys.Core.Presentation.Converters;

/// <summary>
/// Converts a boolean value to a Visibility value.
/// </summary>
public sealed partial class BooleanToVisibilityConverter : IValueConverter
{
    /// <summary>
    /// Gets or sets a value indicating whether the conversion logic is inverted.
    /// When true, false values convert to Visible and true values convert to Collapsed.
    /// </summary>
    public bool IsInverted { get; set; }

    /// <summary>
    /// Converts a boolean value to a Visibility value.
    /// </summary>
    /// <param name="value">The boolean value to convert.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">Optional parameter (not used).</param>
    /// <param name="language">The language (not used).</param>
    /// <returns>Visibility.Visible if value is true (or false if inverted), Visibility.Collapsed otherwise.</returns>
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is bool boolValue)
        {
            var result = IsInverted ? !boolValue : boolValue;
            return result ? Visibility.Visible : Visibility.Collapsed;
        }

        return IsInverted ? Visibility.Visible : Visibility.Collapsed;
    }

    /// <summary>
    /// Converts a Visibility value back to a boolean value.
    /// </summary>
    /// <param name="value">The Visibility value to convert back.</param>
    /// <param name="targetType">The type of the target property.</param>
    /// <param name="parameter">Optional parameter (not used).</param>
    /// <param name="language">The language (not used).</param>
    /// <returns>true if value is Visibility.Visible (or Visibility.Collapsed if inverted), false otherwise.</returns>
    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        if (value is Visibility visibility)
        {
            var result = visibility == Visibility.Visible;
            return IsInverted ? !result : result;
        }

        return !IsInverted;
    }
}
