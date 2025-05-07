using Microsoft.UI.Dispatching;

namespace Extensions.Hosting.WinUi;

/// <summary>
/// Interface for a service that provides access to the UI thread's dispatcher queue.
/// </summary>
public interface IDispatcherService
{
    /// <summary>
    /// Gets the UI thread's dispatcher queue.
    /// </summary>
    /// <returns>The UI thread's dispatcher queue.</returns>
    DispatcherQueue GetUIDispatcherQueue();

    /// <summary>
    /// Executes an action on the UI thread.
    /// </summary>
    /// <param name="action">The action to execute.</param>
    /// <returns>True if the action was successfully enqueued or executed; otherwise, false.</returns>
    bool ExecuteOnUIThread(Action action);
}

internal sealed class DispatcherService(WinUiContext winUiContext) : IDispatcherService
{
    public DispatcherQueue GetUIDispatcherQueue()
    {
        if (winUiContext.DispatcherQueue is null)
        {
            throw new InvalidOperationException(
                "UI dispatcher queue is not available. Ensure this is called after the UI thread has been initialized."
            );
        }

        return winUiContext.DispatcherQueue;
    }

    public bool ExecuteOnUIThread(Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var dispatcher = GetUIDispatcherQueue();

        if (dispatcher.HasThreadAccess)
        {
            action();
            return true;
        }

        return dispatcher.TryEnqueue(DispatcherQueuePriority.Normal, () => action());
    }
}
