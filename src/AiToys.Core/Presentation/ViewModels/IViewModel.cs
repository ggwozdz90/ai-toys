using System.Windows.Input;

namespace AiToys.Core.Presentation.ViewModels;

/// <summary>
/// Contract for view model.
/// </summary>
#pragma warning disable CA1040
public interface IViewModel;
#pragma warning restore CA1040

/// <summary>
/// Contract for view models that need initialization when view is loaded.
/// </summary>
public interface IInitializableViewModel : IViewModel
{
    /// <summary>
    /// Gets the command that initializes the view model.
    /// </summary>
    ICommand InitializeCommand { get; }
}
