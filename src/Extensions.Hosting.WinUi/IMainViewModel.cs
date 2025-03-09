namespace Extensions.Hosting.WinUi;

/// <summary>
/// Defines the contract for the main view model that serves as the root for the application's view model hierarchy.
/// </summary>
public interface IMainViewModel
{
    /// <summary>
    /// Gets the shell view model that handles navigation and overall UI structure.
    /// </summary>
    object ShellViewModel { get; }
}
