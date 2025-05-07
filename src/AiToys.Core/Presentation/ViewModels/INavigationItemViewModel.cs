namespace AiToys.Core.Presentation.ViewModels;

/// <summary>
/// Represents a navigation item which is displayed in the main NavigationView.
/// </summary>
public interface INavigationItemViewModel : IViewModel
{
    /// <summary>
    /// Gets the label of the navigation item.
    /// </summary>
    string Label { get; }

    /// <summary>
    /// Gets the route of the navigation item.
    /// </summary>
    string Route { get; }

    /// <summary>
    /// Gets the order of the navigation item.
    /// </summary>
    int Order { get; }

    /// <summary>
    /// Gets the icon key of the navigation item.
    /// The icon key should be a key of an icon in the Segoe Fluent Icons font.
    /// See https://learn.microsoft.com/en-us/windows/apps/design/style/segoe-fluent-icons-font for more information.
    /// </summary>
    string IconKey { get; }

    /// <summary>
    /// Gets the description of the navigation item.
    /// </summary>
    string Description { get; }
}
