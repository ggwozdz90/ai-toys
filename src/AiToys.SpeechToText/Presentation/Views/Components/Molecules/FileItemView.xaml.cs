using AiToys.SpeechToText.Presentation.ViewModels;
using Microsoft.UI.Xaml;

namespace AiToys.SpeechToText.Presentation.Views.Components.Molecules;

internal sealed partial class FileItemView
{
    public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
        nameof(ViewModel),
        typeof(FileItemViewModel),
        typeof(FileItemView),
        new PropertyMetadata(defaultValue: null)
    );

    public FileItemView() => InitializeComponent();

    public FileItemViewModel ViewModel
    {
        get => (FileItemViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
}
