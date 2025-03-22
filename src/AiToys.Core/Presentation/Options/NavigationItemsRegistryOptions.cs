namespace AiToys.Core.Presentation.Options;

/// <summary>
/// Options for configuring navigation items registrations.
/// </summary>
internal sealed class NavigationItemsRegistryOptions
{
    /// <summary>
    /// Gets the collection of navigation item types to be registered.
    /// </summary>
    public List<Type> NavigationItemTypes { get; } = [];
}
