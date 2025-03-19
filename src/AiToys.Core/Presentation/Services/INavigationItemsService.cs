using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Core.Presentation.Services;

public interface INavigationItemsService
{
    IReadOnlyList<INavigationItemViewModel> GetNavigationItems();
}
