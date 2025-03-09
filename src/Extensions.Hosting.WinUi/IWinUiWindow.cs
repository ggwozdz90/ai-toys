namespace Extensions.Hosting.WinUi;

/// <summary>
/// Defines a contract for WinUI windows to support view models.
/// </summary>
public interface IWinUiWindow
{
    /// <summary>
    /// Gets or sets the view model associated with the window.
    /// </summary>
    IMainViewModel ViewModel { get; set; }
}
