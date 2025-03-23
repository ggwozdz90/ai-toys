using AiToys.Home.Presentation.ViewModels;

namespace AiToys.Home.Presentation.Components.Organisms;

internal sealed partial class FeatureTile
{
    public FeatureTile() => InitializeComponent();

    public FeatureTileViewModel ViewModel { get; set; } = null!;
}
