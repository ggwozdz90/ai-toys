using AiToys.SpeechToText.Domain.Enums;
using Microsoft.UI.Xaml;

namespace AiToys.SpeechToText.Presentation.Views.Components.Atoms;

internal sealed partial class FileItemStatusIcon
{
    public static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
        nameof(Status),
        typeof(FileItemStatus),
        typeof(FileItemStatusIcon),
        new PropertyMetadata(defaultValue: FileItemStatus.Pending)
    );

    public FileItemStatusIcon() => InitializeComponent();

    public FileItemStatus Status
    {
        get => (FileItemStatus)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }
}
