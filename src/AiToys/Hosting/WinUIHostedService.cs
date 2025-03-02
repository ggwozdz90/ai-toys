using Microsoft.Extensions.Hosting;

namespace AiToys.Hosting;

internal sealed class WinUiHostedService(WinUiThread winUIThread, WinUIContext winUIContext) : IHostedService
{
    private readonly WinUiThread winUIThread = winUIThread;
    private readonly WinUIContext winUIContext = winUIContext;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return Task.CompletedTask;
        }

        winUIThread.Start();

        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (winUIContext.IsRunning)
        {
            TaskCompletionSource completion = new();

            winUIContext.DispatcherQueue?.TryEnqueue(() =>
            {
                winUIContext.Application?.Exit();
                completion.SetResult();
            });

            await completion.Task.ConfigureAwait(false);
        }
    }
}
