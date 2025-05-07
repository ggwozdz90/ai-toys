using System.ComponentModel;
using System.Windows.Input;
using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Core.Presentation.Commands;

/// <summary>
/// Base interface for all commands, defining common functionality.
/// </summary>
public interface ICommandBase : ICommand, IDisposable
{
    /// <summary>
    /// Manually raises the CanExecuteChanged event.
    /// </summary>
    void NotifyCanExecuteChanged();

    /// <summary>
    /// Configures the command to observe a property for changes and automatically
    /// call NotifyCanExecuteChanged when the property changes.
    /// </summary>
    /// <param name="viewModel">The view model containing the property to observe.</param>
    /// <param name="propertyName">The name of the property to observe.</param>
    /// <returns>The command instance for method chaining.</returns>
    ICommandBase ObservesProperty(INotifyPropertyChanged viewModel, string propertyName);

    /// <summary>
    /// Stops observing all property changes.
    /// </summary>
    void StopObservingPropertyChanges();
}

/// <summary>
/// Base class for all commands providing common functionality for property observation.
/// </summary>
public abstract class CommandBase : ICommandBase
{
    private readonly Dictionary<INotifyPropertyChanged, HashSet<string>> observedProperties = [];
    private readonly Dictionary<INotifyPropertyChanged, PropertyChangedEventHandler> propertyChangedHandlers = [];
    private bool isDisposed;

    /// <summary>
    /// Event raised when the ability to execute the command changes.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Determines whether the command can be executed.
    /// </summary>
    /// <param name="parameter">Parameter that is ignored.</param>
    /// <returns>True if the command can execute; otherwise, false.</returns>
    public abstract bool CanExecute(object? parameter);

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter">Parameter that is ignored.</param>
    public abstract void Execute(object? parameter);

    /// <summary>
    /// Manually raises the CanExecuteChanged event.
    /// </summary>
    public void NotifyCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Configures the command to observe a property for changes and automatically
    /// call NotifyCanExecuteChanged when the property changes.
    /// </summary>
    /// <param name="viewModel">The view model containing the property to observe.</param>
    /// <param name="propertyName">The name of the property to observe.</param>
    /// <returns>The command instance for method chaining.</returns>
    public ICommandBase ObservesProperty(INotifyPropertyChanged viewModel, string propertyName)
    {
        ArgumentNullException.ThrowIfNull(viewModel);
        ArgumentException.ThrowIfNullOrEmpty(propertyName);

        if (viewModel is ViewModelBase viewModelBase)
        {
            viewModelBase.RegisterCommandPropertyObserver(propertyName, this);
        }
        else
        {
            if (!observedProperties.TryGetValue(viewModel, out var properties))
            {
                properties = new HashSet<string>(StringComparer.Ordinal);
                observedProperties[viewModel] = properties;

                PropertyChangedEventHandler handler = PropertyChangedHandler;
                propertyChangedHandlers[viewModel] = handler;
                viewModel.PropertyChanged += handler;
            }

            properties.Add(propertyName);
        }

        return this;

        void PropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not INotifyPropertyChanged vm || !observedProperties.TryGetValue(vm, out var properties))
            {
                return;
            }

            if (string.IsNullOrEmpty(e.PropertyName) || properties.Contains(e.PropertyName))
            {
                NotifyCanExecuteChanged();
            }
        }
    }

    /// <summary>
    /// Stops observing all property changes.
    /// </summary>
    public void StopObservingPropertyChanges()
    {
        foreach (var (viewModel, handler) in propertyChangedHandlers)
        {
            viewModel.PropertyChanged -= handler;
        }

        observedProperties.Clear();
        propertyChangedHandlers.Clear();
    }

    /// <summary>
    /// Disposes of resources used by the command.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes of resources used by the command.
    /// </summary>
    /// <param name="disposing">True if called from Dispose; false if called from the finalizer.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (isDisposed)
        {
            return;
        }

        if (disposing)
        {
            StopObservingPropertyChanges();
        }

        isDisposed = true;
    }
}
