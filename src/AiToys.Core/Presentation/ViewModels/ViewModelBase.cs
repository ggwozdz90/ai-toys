using System.Collections.Concurrent;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AiToys.Core.Presentation.Commands;
using Extensions.Hosting.WinUi;

namespace AiToys.Core.Presentation.ViewModels;

/// <summary>
/// Base class for all view models.
/// Implements <see cref="INotifyPropertyChanged"/> and <see cref="IViewModel"/>.
/// </summary>
public partial class ViewModelBase : IViewModel, INotifyPropertyChanged, IDisposable
{
    private readonly ConcurrentDictionary<string, HashSet<ICommandBase>> propertyObservers = new(
        StringComparer.Ordinal
    );
    private readonly IDispatcherService dispatcherService;
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
    /// </summary>
    /// <param name="dispatcherService">The dispatcher service used to marshal calls to the UI thread.</param>
    protected ViewModelBase(IDispatcherService dispatcherService)
    {
        this.dispatcherService = dispatcherService;
    }

    /// <inheritdoc />
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Disposes of resources used by the view model.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Registers a command to be notified when a property changes.
    /// </summary>
    /// <param name="propertyName">The name of the property to observe.</param>
    /// <param name="command">The command to notify.</param>
    internal void RegisterCommandPropertyObserver(string propertyName, ICommandBase command)
    {
        ArgumentException.ThrowIfNullOrEmpty(propertyName);
        ArgumentNullException.ThrowIfNull(command);

        var commands = propertyObservers.GetOrAdd(propertyName, _ => []);

        commands.Add(command);
    }

    /// <summary>
    /// Executes the specified action on the UI thread.
    /// If already on the UI thread, executes immediately.
    /// </summary>
    /// <param name="action">The action to execute on the UI thread.</param>
    /// <returns>True if the action was successfully enqueued or executed; otherwise, false.</returns>
    protected bool ExecuteOnUIThread(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        return dispatcherService.ExecuteOnUIThread(action);
    }

    /// <summary>
    /// Raises the <see cref="PropertyChanged"/> event.
    /// </summary>
    /// <param name="propertyName">The name of the property.</param>
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        if (propertyName == null || !propertyObservers.TryGetValue(propertyName, out var commands))
        {
            return;
        }

        foreach (var command in commands)
        {
            NotifyCommandCanExecuteChanged(command);
        }
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

        ExecuteOnUIThread(() => OnPropertyChanged(propertyName));

        return true;
    }

    /// <summary>
    /// Disposes of resources used by the view model.
    /// </summary>
    /// <param name="disposing">True if called from Dispose; false if called from the finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!isDisposed)
        {
            if (disposing)
            {
                propertyObservers.Clear();
            }

            isDisposed = true;
        }
    }

    /// <summary>
    /// Notifies a command that it should check its CanExecute status.
    /// Ensures the notification happens on the UI thread.
    /// </summary>
    /// <param name="command">The command to notify.</param>
    private void NotifyCommandCanExecuteChanged(ICommandBase command)
    {
        ExecuteOnUIThread(command.NotifyCanExecuteChanged);
    }
}
