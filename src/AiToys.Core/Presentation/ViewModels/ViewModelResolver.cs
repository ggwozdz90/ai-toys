using AiToys.Core.Presentation.Views;

namespace AiToys.Core.Presentation.ViewModels;

/// <summary>
/// Resolves view models based on view types.
/// </summary>
internal interface IViewModelResolver
{
    /// <summary>
    /// Resolves a view type to its associated view model type.
    /// </summary>
    /// <param name="viewType">The view type to resolve.</param>
    /// <returns>The view model type associated with the view.</returns>
    Type ResolveViewModelType(Type viewType);
}

internal sealed class ViewModelResolver : IViewModelResolver
{
    public Type ResolveViewModelType(Type viewType)
    {
        ArgumentNullException.ThrowIfNull(viewType);

        foreach (var interfaceType in viewType.GetInterfaces())
        {
            if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == typeof(IView<>))
            {
                return interfaceType.GetGenericArguments()[0];
            }
        }

        throw new InvalidOperationException($"View '{viewType.Name}' does not implement IView<TViewModel>.");
    }
}
