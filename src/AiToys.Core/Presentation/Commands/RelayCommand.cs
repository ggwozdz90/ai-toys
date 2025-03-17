using System.Windows.Input;

namespace AiToys.Core.Presentation.Commands;

/// <summary>
/// Implementation of IRelayCommand that handles synchronous operations.
/// </summary>
public partial class RelayCommand(Action execute, Func<bool>? canExecute = null) : IRelayCommand
{
    private readonly Action execute = execute ?? throw new ArgumentNullException(nameof(execute));
    private readonly Func<bool>? canExecute = canExecute;

    /// <summary>
    /// Event raised when the ability to execute the command changes.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Raises the CanExecuteChanged event.
    /// </summary>
    public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// Determines whether the command can be executed.
    /// </summary>
    /// <returns>True if the command can be executed; otherwise, false.</returns>
    public bool CanExecute() => canExecute?.Invoke() ?? true;

    /// <summary>
    /// Determines whether the command can be executed.
    /// </summary>
    /// <param name="parameter">Parameter that is ignored.</param>
    /// <returns>True if the command can be executed; otherwise, false.</returns>
    bool ICommand.CanExecute(object? parameter) => CanExecute();

    /// <summary>
    /// Executes the command.
    /// </summary>
    public void Execute() => execute();

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter">Parameter that is ignored.</param>
    void ICommand.Execute(object? parameter) => Execute();
}

/// <summary>
/// Implementation of IRelayCommand{T} that handles synchronous operations with a parameter.
/// </summary>
/// <typeparam name="T">The type of parameter passed to the command.</typeparam>
public partial class RelayCommand<T>(Action<T?> execute, Predicate<T?>? canExecute = null) : IRelayCommand<T>
{
    private readonly Action<T?> execute = execute ?? throw new ArgumentNullException(nameof(execute));
    private readonly Predicate<T?>? canExecute = canExecute;

    /// <summary>
    /// Event raised when the ability to execute the command changes.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Raises the CanExecuteChanged event.
    /// </summary>
    public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// Determines whether the command can be executed with the given parameter.
    /// </summary>
    /// <param name="parameter">Data used to determine if the command can be executed.</param>
    /// <returns>True if the command can be executed; otherwise, false.</returns>
    public bool CanExecute(T? parameter) => canExecute?.Invoke(parameter) ?? true;

    /// <summary>
    /// Determines whether the command can be executed with the given parameter.
    /// </summary>
    /// <param name="parameter">Data used to determine if the command can be executed.</param>
    /// <returns>True if the command can be executed; otherwise, false.</returns>
    bool ICommand.CanExecute(object? parameter) => parameter is T or null && CanExecute((T?)parameter);

    /// <summary>
    /// Executes the command with the given parameter.
    /// </summary>
    /// <param name="parameter">Data used by the command.</param>
    public void Execute(T? parameter) => execute(parameter);

    /// <summary>
    /// Executes the command with the given parameter.
    /// </summary>
    /// <param name="parameter">Data used by the command.</param>
    void ICommand.Execute(object? parameter)
    {
        if (parameter is T or null)
        {
            Execute((T?)parameter);
        }
    }
}
