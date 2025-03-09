using ReactiveUI;

namespace AiToys.Presentation.ViewModels;

internal sealed partial class ShellViewModel : ReactiveObject
{
    private string? buttonText;

    public ShellViewModel()
    {
        buttonText = "Click Me";
    }

    public string? ButtonText
    {
        get => buttonText;
        set => this.RaiseAndSetIfChanged(ref buttonText, value);
    }
}
