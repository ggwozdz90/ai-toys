using Microsoft.UI.Xaml;

namespace AiToys.Translation.Presentation.Views.Components.Molecules;

internal sealed partial class TextPanel
{
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(TextPanel),
        new PropertyMetadata(defaultValue: string.Empty)
    );

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text),
        typeof(string),
        typeof(TextPanel),
        new PropertyMetadata(defaultValue: string.Empty)
    );

    public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
        nameof(PlaceholderText),
        typeof(string),
        typeof(TextPanel),
        new PropertyMetadata(defaultValue: string.Empty)
    );

    public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register(
        nameof(IsReadOnly),
        typeof(bool),
        typeof(TextPanel),
        new PropertyMetadata(defaultValue: false)
    );

    public TextPanel()
    {
        InitializeComponent();
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string PlaceholderText
    {
        get => (string)GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }
}
