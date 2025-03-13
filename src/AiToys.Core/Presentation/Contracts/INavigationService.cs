using Microsoft.UI.Xaml.Controls;

namespace AiToys.Core.Presentation.Contracts;

/// <summary>
/// Contract for the navigation service.
/// This service is responsible for switching content in the main frame.
/// </summary>
public interface INavigationService
{
    /// <summary>
    /// Sets the main frame that will be used for navigation.
    /// </summary>
    /// <param name="frame">The main frame.</param>
    void SetNavigationFrame(Frame frame);

    /// <summary>
    /// Navigates to the view represented by the specified view model.
    /// View and view model must be registered in the dependency injection container and must be configured in the <see cref="IViewResolver"/>.
    /// </summary>
    /// <typeparam name="TViewModel">The view model to navigate to.</typeparam>
    void NavigateTo<TViewModel>()
        where TViewModel : IViewModel;
}
