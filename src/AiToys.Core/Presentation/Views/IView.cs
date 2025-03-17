using AiToys.Core.Presentation.ViewModels;

namespace AiToys.Core.Presentation.Views;

/// <summary>
/// Contract for a view.
/// </summary>
#pragma warning disable CA1040 // Avoid empty interfaces
public interface IView;
#pragma warning restore CA1040

/// <summary>
/// Contract for a view with a view model.
/// Used to resolve views via the <see cref="IViewResolver"/> and to set the view model.
/// </summary>
/// <typeparam name="TViewModel">The type of the view model.</typeparam>
public interface IView<TViewModel> : IView
    where TViewModel : IViewModel
{
    /// <summary>
    /// Gets or sets the view model.
    /// View models are automatically resolved while navigating to the view.
    /// </summary>
    TViewModel ViewModel { get; set; }
}
