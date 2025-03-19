using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Core.Presentation.Services;

public interface INavigationItemsProvider
{
    IEnumerable<INavigationItemViewModel> GetNavigationItems();
}
