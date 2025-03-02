using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using WinRT;

namespace AiToys.Hosting;

#pragma warning disable CsWinRT1028
internal class WinUiThread : IDisposable
#pragma warning restore CsWinRT1028
{
    private readonly ManualResetEvent manualResetEvent = new(initialState: false);
    private readonly IHostApplicationLifetime hostApplicationLifetime;
    private readonly WinUIContext winUiContext;
    private readonly IServiceProvider serviceProvider;

    internal WinUiThread(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;

        winUiContext = serviceProvider.GetRequiredService<WinUIContext>();
        hostApplicationLifetime = serviceProvider.GetRequiredService<IHostApplicationLifetime>();

        StartUiThread();
    }

    public void Start() => manualResetEvent.Set();

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            manualResetEvent.Dispose();
        }
    }

    private void StartUiThread()
    {
        var uiThread = new Thread(InternalUiThreadStart) { IsBackground = true };
        uiThread.SetApartmentState(ApartmentState.STA);
        uiThread.Start();
    }

    private void InternalUiThreadStart()
    {
        ComWrappersSupport.InitializeComWrappers();
        manualResetEvent.WaitOne();
        winUiContext.IsRunning = true;

        Application.Start(_ =>
        {
            winUiContext.DispatcherQueue = DispatcherQueue.GetForCurrentThread();

            DispatcherQueueSynchronizationContext context = new(winUiContext.DispatcherQueue);
            SynchronizationContext.SetSynchronizationContext(context);

            winUiContext.Application = serviceProvider.GetRequiredService<Application>();

            winUiContext.MainWindow =
                ActivatorUtilities.CreateInstance(serviceProvider, winUiContext.MainWindowType) as Window;
            winUiContext.MainWindow!.Activate();
        });

        HandleApplicationExit();
    }

    private void HandleApplicationExit()
    {
        winUiContext.IsRunning = false;

        if (!winUiContext.IsLifetimeLinked)
        {
            return;
        }

        if (
            hostApplicationLifetime.ApplicationStopped.IsCancellationRequested
            || hostApplicationLifetime.ApplicationStopping.IsCancellationRequested
        )
        {
            return;
        }

        hostApplicationLifetime.StopApplication();
    }
}
