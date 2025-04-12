using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.UI.Dispatching;

namespace AiToys.Core.Presentation.ViewModels;

/// <summary>
/// Base class for all view models.
/// Implements <see cref="INotifyPropertyChanged"/> and <see cref="IViewModel"/>.
/// </summary>
public partial class ViewModelBase : IViewModel, INotifyPropertyChanged
{
    private readonly DispatcherQueue? dispatcherQueue;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
    /// </summary>
    public ViewModelBase()
    {
        dispatcherQueue = DispatcherQueue.GetForCurrentThread();
    }

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
    /// Automatically marshals the property change notification to the UI thread if needed.
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

        if (dispatcherQueue != null && !dispatcherQueue.HasThreadAccess)
        {
            dispatcherQueue.TryEnqueue(() => OnPropertyChanged(propertyName));
        }
        else
        {
            OnPropertyChanged(propertyName);
        }

        return true;
    }
}
