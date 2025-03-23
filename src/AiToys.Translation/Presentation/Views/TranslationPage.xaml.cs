using AiToys.Core.Presentation.Views;
using AiToys.Translation.Presentation.ViewModels;

namespace AiToys.Translation.Presentation.Views;

internal sealed partial class TranslationPage : IView<TranslationViewModel>
{
    public TranslationPage() => InitializeComponent();

    public TranslationViewModel ViewModel { get; set; } = null!;
}
