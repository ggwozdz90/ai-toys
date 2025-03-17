using System.Windows.Input;

namespace AiToys.Core.Presentation.Commands;

/// <summary>
/// Defines a command that supports synchronous operations.
/// </summary>
public interface IRelayCommand : ICommand
{
    /// <summary>
    /// Executes the command synchronously.
    /// </summary>
    void Execute();

    /// <summary>
    /// Determines whether the command can execute.
    /// </summary>
    /// <returns>True if the command can execute; otherwise, false.</returns>
    bool CanExecute();

    /// <summary>
    /// Manually raises the CanExecuteChanged event.
    /// </summary>
    void NotifyCanExecuteChanged();
}

/// <summary>
/// Defines a command that supports synchronous operations with a parameter.
/// </summary>
/// <typeparam name="T">The type of parameter passed to the command.</typeparam>
public interface IRelayCommand<in T> : ICommand
{
    /// <summary>
    /// Executes the command synchronously with the provided parameter.
    /// </summary>
    /// <param name="parameter">Data used by the command.</param>
    void Execute(T? parameter);

    /// <summary>
    /// Determines whether the command can execute with the provided parameter.
    /// </summary>
    /// <param name="parameter">Data used to determine if command can execute.</param>
    /// <returns>True if the command can execute; otherwise, false.</returns>
    bool CanExecute(T? parameter);

    /// <summary>
    /// Manually raises the CanExecuteChanged event.
    /// </summary>
    void NotifyCanExecuteChanged();
}
