#pragma warning disable CA1515

using Microsoft.UI.Xaml;

namespace AiToys;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    public App()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched normally by the end user.  Other entry points.
    /// </summary>
    /// <param name="args">Details about the launch request and process. </param>
    protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
    {
        var mWindow = new MainWindow();
        mWindow.Activate();
    }
}

#pragma warning restore CA1515
