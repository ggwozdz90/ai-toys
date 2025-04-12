using Microsoft.UI.Xaml;

namespace AiToys.Translation.Presentation.Views.Components.Organisms;

internal sealed partial class TranslationPanel
{
    public static readonly DependencyProperty SourceTextProperty = DependencyProperty.Register(
        nameof(SourceText),
        typeof(string),
        typeof(TranslationPanel),
        new PropertyMetadata(string.Empty)
    );

    public static readonly DependencyProperty TranslatedTextProperty = DependencyProperty.Register(
        nameof(TranslatedText),
        typeof(string),
        typeof(TranslationPanel),
        new PropertyMetadata(string.Empty)
    );

    public static readonly DependencyProperty IsTranslatingProperty = DependencyProperty.Register(
        nameof(IsTranslating),
        typeof(bool),
        typeof(TranslationPanel),
        new PropertyMetadata(false)
    );

    public TranslationPanel()
    {
        InitializeComponent();
    }

    public string SourceText
    {
        get => (string)GetValue(SourceTextProperty);
        set => SetValue(SourceTextProperty, value);
    }

    public string TranslatedText
    {
        get => (string)GetValue(TranslatedTextProperty);
        set => SetValue(TranslatedTextProperty, value);
    }

    public bool IsTranslating
    {
        get => (bool)GetValue(IsTranslatingProperty);
        set => SetValue(IsTranslatingProperty, value);
    }
}
