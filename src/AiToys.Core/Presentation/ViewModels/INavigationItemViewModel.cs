namespace AiToys.Core.Presentation.ViewModels;

public interface INavigationItemViewModel : IViewModel
{
    string Label { get; }
    string Route { get; }
    int Order { get; }
    string IconKey { get; }
}
