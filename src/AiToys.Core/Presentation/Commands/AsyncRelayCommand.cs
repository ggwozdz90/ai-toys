using System.Windows.Input;

namespace AiToys.Core.Presentation.Commands;

/// <summary>
/// Implementation of IAsyncRelayCommand that handles asynchronous operations.
/// </summary>
public sealed partial class AsyncRelayCommand(
    Func<CancellationToken, Task> executeAsync,
    Func<bool>? canExecute = null,
    bool canBeCanceled = false
) : IAsyncRelayCommand, IDisposable
{
    private readonly Func<CancellationToken, Task> executeAsync =
        executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
    private readonly Func<bool>? canExecute = canExecute;
    private readonly CancellationTokenSource? cancellationTokenSource = canBeCanceled
        ? new CancellationTokenSource()
        : null;

    private bool isDisposed;

    private Task? executionTask;

    /// <summary>
    /// Event raised when the ability to execute the command changes.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Gets a value indicating whether the command is currently executing.
    /// </summary>
    public bool IsExecuting => executionTask?.IsCompleted == false;

    /// <summary>
    /// Gets a value indicating whether the command can be canceled.
    /// </summary>
    public bool CanBeCanceled => cancellationTokenSource != null;

    /// <summary>
    /// Gets a value indicating whether the cancellation has been requested.
    /// </summary>
    public bool IsCancellationRequested => cancellationTokenSource?.IsCancellationRequested == true;

    /// <summary>
    /// Gets the task that represents the asynchronous operation being performed.
    /// </summary>
    public Task? ExecutionTask => executionTask;

    /// <summary>
    /// Manually raises the CanExecuteChanged event.
    /// </summary>
    public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// Determines whether the command can be executed.
    /// </summary>
    /// <returns>True if the command can execute; otherwise, false.</returns>
    public bool CanExecute() => !IsExecuting && (canExecute?.Invoke() ?? true);

    /// <summary>
    /// Determines whether the command can be executed.
    /// </summary>
    /// <param name="parameter">Parameter that is ignored.</param>
    /// <returns>True if the command can execute; otherwise, false.</returns>
    bool ICommand.CanExecute(object? parameter) => CanExecute();

    /// <summary>
    /// Executes the command asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ExecuteAsync()
    {
        if (!CanExecute())
        {
            return;
        }

        try
        {
            executionTask = executeAsync(cancellationTokenSource?.Token ?? CancellationToken.None);
            NotifyCanExecuteChanged();

            await executionTask.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Command was canceled, no action needed
        }
        finally
        {
            NotifyCanExecuteChanged();
        }
    }

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="parameter">Parameter that is ignored.</param>
    void ICommand.Execute(object? parameter) => _ = ExecuteAsync();

    /// <summary>
    /// Cancels the current command execution if possible.
    /// </summary>
    public void Cancel()
    {
        if (!CanBeCanceled || !IsExecuting || IsCancellationRequested)
        {
            return;
        }

        cancellationTokenSource?.Cancel();
        NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Disposes the resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        if (!isDisposed)
        {
            cancellationTokenSource?.Dispose();
            isDisposed = true;
        }
    }
}

/// <summary>
/// Implementation of IAsyncRelayCommand{T} that handles asynchronous operations with a parameter.
/// </summary>
/// <typeparam name="T">The type of parameter passed to the command.</typeparam>
public sealed partial class AsyncRelayCommand<T>(
    Func<T?, CancellationToken, Task> executeAsync,
    Predicate<T?>? canExecute = null,
    bool canBeCanceled = false
) : IAsyncRelayCommand<T>, IDisposable
{
    private readonly Func<T?, CancellationToken, Task> executeAsync =
        executeAsync ?? throw new ArgumentNullException(nameof(executeAsync));
    private readonly Predicate<T?>? canExecute = canExecute;
    private readonly CancellationTokenSource? cancellationTokenSource = canBeCanceled
        ? new CancellationTokenSource()
        : null;
    private bool isDisposed;

    private Task? executionTask;

    /// <summary>
    /// Event raised when the ability to execute the command changes.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Gets a value indicating whether the command is currently executing.
    /// </summary>
    public bool IsExecuting => executionTask?.IsCompleted == false;

    /// <summary>
    /// Gets a value indicating whether the command can be canceled.
    /// </summary>
    public bool CanBeCanceled => cancellationTokenSource != null;

    /// <summary>
    /// Gets a value indicating whether the cancellation has been requested.
    /// </summary>
    public bool IsCancellationRequested => cancellationTokenSource?.IsCancellationRequested == true;

    /// <summary>
    /// Gets the task that represents the asynchronous operation being performed.
    /// </summary>
    public Task? ExecutionTask => executionTask;

    /// <summary>
    /// Manually raises the CanExecuteChanged event.
    /// </summary>
    public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    /// <summary>
    /// Determines whether the command can be executed with the given parameter.
    /// </summary>
    /// <param name="parameter">Data used to determine if the command can be executed.</param>
    /// <returns>True if the command can be executed; otherwise, false.</returns>
    public bool CanExecute(T? parameter) => !IsExecuting && (canExecute?.Invoke(parameter) ?? true);

    /// <summary>
    /// Determines whether the command can be executed with the given parameter.
    /// </summary>
    /// <param name="parameter">Data used to determine if the command can be executed.</param>
    /// <returns>True if the command can execute; otherwise, false.</returns>
    bool ICommand.CanExecute(object? parameter) => parameter is T or null && CanExecute((T?)parameter);

    /// <summary>
    /// Executes the command asynchronously with the given parameter.
    /// </summary>
    /// <param name="parameter">Data used by the command.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ExecuteAsync(T? parameter)
    {
        if (!CanExecute(parameter))
        {
            return;
        }

        try
        {
            executionTask = executeAsync(parameter, cancellationTokenSource?.Token ?? CancellationToken.None);
            NotifyCanExecuteChanged();

            await executionTask.ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Command was canceled, no action needed
        }
        finally
        {
            NotifyCanExecuteChanged();
        }
    }

    /// <summary>
    /// Executes the command with the given parameter.
    /// </summary>
    /// <param name="parameter">Data used by the command.</param>
    void ICommand.Execute(object? parameter)
    {
        if (parameter is T or null)
        {
            _ = ExecuteAsync((T?)parameter);
        }
    }

    /// <summary>
    /// Provides a public method that matches the implicit interface implementation.
    /// </summary>
    /// <param name="parameter">Data used by the command.</param>
    public void Execute(T? parameter) => _ = ExecuteAsync(parameter);

    /// <summary>
    /// Cancels the current command execution if possible.
    /// </summary>
    public void Cancel()
    {
        if (!CanBeCanceled || !IsExecuting || IsCancellationRequested)
        {
            return;
        }

        cancellationTokenSource?.Cancel();
        NotifyCanExecuteChanged();
    }

    /// <summary>
    /// Disposes the resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        if (!isDisposed)
        {
            cancellationTokenSource?.Dispose();
            isDisposed = true;
        }
    }
}
