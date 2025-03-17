using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AiToys.Core.Presentation.ViewModels;

/// <summary>
/// Base class for all view models.
/// Implements <see cref="INotifyPropertyChanged"/> and <see cref="IViewModel"/>.
/// </summary>
public partial class ViewModelBase : IViewModel, INotifyPropertyChanged
{
    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Sets the property value and raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <typeparam name="T">The type of the property.</typeparam>
    /// <param name="field">The backing field of the property.</param>
    /// <param name="value">The new value of the property.</param>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>True if the value was changed; otherwise, false.</returns>
    protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (Equals(field, value))
        {
            return false;
        }

        field = value;

        OnPropertyChanged(propertyName);

        return true;
    }
}
