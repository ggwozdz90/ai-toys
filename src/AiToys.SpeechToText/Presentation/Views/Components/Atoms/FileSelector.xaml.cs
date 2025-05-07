using System.Windows.Input;
using Microsoft.UI.Xaml;

namespace AiToys.SpeechToText.Presentation.Views.Components.Atoms;

internal sealed partial class FileSelector
{
    public static readonly DependencyProperty BrowseCommandProperty = DependencyProperty.Register(
        nameof(BrowseCommand),
        typeof(ICommand),
        typeof(FileSelector),
        new PropertyMetadata(defaultValue: null)
    );

    public FileSelector() => InitializeComponent();

    public ICommand BrowseCommand
    {
        get => (ICommand)GetValue(BrowseCommandProperty);
        set => SetValue(BrowseCommandProperty, value);
    }
}
