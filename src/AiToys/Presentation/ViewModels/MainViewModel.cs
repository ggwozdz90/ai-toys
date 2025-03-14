using AiToys.Core.Presentation.Contracts;
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI.Xaml.Media;

namespace AiToys.Presentation.ViewModels;

internal sealed class MainViewModel : IViewModel
{
    public bool ExtendsContentIntoTitleBar { get; set; } = true;

    public SystemBackdrop SystemBackdrop { get; set; } = new MicaBackdrop() { Kind = MicaKind.Base };
}
