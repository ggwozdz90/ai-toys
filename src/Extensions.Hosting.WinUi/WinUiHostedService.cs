using Microsoft.Extensions.Hosting;

namespace Extensions.Hosting.WinUi;

internal sealed class WinUiHostedService(WinUiThread winUiThread, WinUiContext winUiContext) : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.CompletedTask;
        }

        winUiThread.Start();

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (winUiContext.IsRunning)
        {
            TaskCompletionSource completion = new();

            winUiContext.DispatcherQueue?.TryEnqueue(() =>
            {
                winUiContext.Application?.Exit();
                completion.SetResult();
            });

            await completion.Task.ConfigureAwait(false);
        }
    }
}
