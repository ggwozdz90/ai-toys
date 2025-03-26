using System.Windows.Input;
using AiToys.Core.Presentation.Commands;
using AiToys.Core.Presentation.ViewModels;
using AiToys.Translation.Constants;
using TranslationApiClient.Adapter.Adapters;

namespace AiToys.Translation.Presentation.ViewModels;

internal sealed partial class TranslationViewModel(ITranslationAdapter translationAdapter)
    : ViewModelBase,
        IRouteAwareViewModel,
        IDisposable
{
    private bool disposed;

    public string Route => RouteNames.TranslationPage;

    public string? ButtonText { get; set; } = "TranslationViewModel";

    public ICommand TranslateCommand { get; } =
        new AsyncRelayCommand(async _ =>
        {
            await translationAdapter.HealthCheckAsync().ConfigureAwait(false);
            await translationAdapter
                .TranslateAsync("The text of the agreement is very clear.", "en_US", "pl_PL")
                .ConfigureAwait(false);
        });

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        if (TranslateCommand is IDisposable disposable)
        {
            disposable.Dispose();
        }

        disposed = true;
    }
}
