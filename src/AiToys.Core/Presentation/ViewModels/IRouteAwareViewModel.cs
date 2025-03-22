namespace AiToys.Core.Presentation.ViewModels;

/// <summary>
/// Represents a view model that is aware of its route.
/// </summary>
public interface IRouteAwareViewModel : IViewModel
{
    /// <summary>
    /// Gets the route associated with this view model.
    /// </summary>
    string Route { get; }
}
