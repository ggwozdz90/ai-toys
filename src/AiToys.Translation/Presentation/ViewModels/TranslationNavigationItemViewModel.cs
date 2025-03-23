using AiToys.Core.Presentation.ViewModels;
using AiToys.Translation.Constants;

namespace AiToys.Translation.Presentation.ViewModels;

internal sealed class TranslationNavigationItemViewModel : INavigationItemViewModel
{
    public string Label => PageLabels.TranslationPage;
    public string Route => RouteNames.TranslationPage;
    public int Order => 300;
    public string IconKey => IconKeys.Translation;
}
