using AiToys.Core.Presentation.Contracts;

namespace AiToys.Presentation.ViewModels;

internal sealed class HomeViewModel : IViewModel
{
    public string? ButtonText { get; set; } = "Click Me";
}
