namespace AiToys.Core.Presentation.Options;

/// <summary>
/// Options for configuring route registrations.
/// </summary>
internal sealed class RouteRegistryOptions
{
    /// <summary>
    /// Gets the route mappings.
    /// </summary>
#pragma warning disable CsWinRT1030
    public IDictionary<string, Type> RouteMappings { get; } =
#pragma warning restore CsWinRT1030
        new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
}
