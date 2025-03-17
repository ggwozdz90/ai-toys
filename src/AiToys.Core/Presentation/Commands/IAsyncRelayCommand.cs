using System.Windows.Input;

namespace AiToys.Core.Presentation.Commands;

/// <summary>
/// Base interface for asynchronous commands with common functionality.
/// </summary>
public interface IAsyncRelayCommandBase : ICommand
{
    /// <summary>
    /// Gets a value indicating whether the command is currently executing.
    /// </summary>
    bool IsExecuting { get; }

    /// <summary>
    /// Gets a value indicating whether the command can be canceled.
    /// </summary>
    bool CanBeCanceled { get; }

    /// <summary>
    /// Gets a value indicating whether the cancellation has been requested.
    /// </summary>
    bool IsCancellationRequested { get; }

    /// <summary>
    /// Gets the task that represents the asynchronous operation being performed.
    /// </summary>
    Task? ExecutionTask { get; }

    /// <summary>
    /// Cancels the current command execution if possible.
    /// </summary>
    void Cancel();
}

/// <summary>
/// Defines a command that supports asynchronous operations.
/// </summary>
public interface IAsyncRelayCommand : IAsyncRelayCommandBase
{
    /// <summary>
    /// Executes the command asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ExecuteAsync();

    /// <summary>
    /// Determines whether the command can execute.
    /// </summary>
    /// <returns>True if the command can execute; otherwise, false.</returns>
    bool CanExecute();
}

/// <summary>
/// Defines a command that supports asynchronous operations with a parameter.
/// </summary>
/// <typeparam name="T">The type of parameter passed to the command.</typeparam>
public interface IAsyncRelayCommand<in T> : IAsyncRelayCommandBase
{
    /// <summary>
    /// Executes the command asynchronously with the provided parameter.
    /// </summary>
    /// <param name="parameter">Data used by the command.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ExecuteAsync(T? parameter);

    /// <summary>
    /// Determines whether the command can execute with the provided parameter.
    /// </summary>
    /// <param name="parameter">Data used to determine if command can execute.</param>
    /// <returns>True if the command can execute; otherwise, false.</returns>
    bool CanExecute(T? parameter);
}
