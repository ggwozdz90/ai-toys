using System.Windows.Input;
using AiToys.SpeechToText.Domain.Models;
using Microsoft.UI.Xaml;

namespace AiToys.SpeechToText.Presentation.Views.Components.Organisms;

internal sealed partial class LanguageSelectionPanel
{
    public static readonly DependencyProperty LanguagesProperty = DependencyProperty.Register(
        nameof(Languages),
        typeof(IEnumerable<LanguageModel>),
        typeof(LanguageSelectionPanel),
        new PropertyMetadata(defaultValue: null)
    );

    public static readonly DependencyProperty SourceLanguageProperty = DependencyProperty.Register(
        nameof(SourceLanguage),
        typeof(LanguageModel),
        typeof(LanguageSelectionPanel),
        new PropertyMetadata(defaultValue: null)
    );

    public static readonly DependencyProperty TargetLanguageProperty = DependencyProperty.Register(
        nameof(TargetLanguage),
        typeof(LanguageModel),
        typeof(LanguageSelectionPanel),
        new PropertyMetadata(defaultValue: null)
    );

    public static readonly DependencyProperty SwapLanguagesCommandProperty = DependencyProperty.Register(
        nameof(SwapLanguagesCommand),
        typeof(ICommand),
        typeof(LanguageSelectionPanel),
        new PropertyMetadata(defaultValue: null)
    );

    public LanguageSelectionPanel()
    {
        InitializeComponent();
    }

    public IEnumerable<LanguageModel> Languages
    {
        get => (IEnumerable<LanguageModel>)GetValue(LanguagesProperty);
        set => SetValue(LanguagesProperty, value);
    }

    public LanguageModel SourceLanguage
    {
        get => (LanguageModel)GetValue(SourceLanguageProperty);
        set => SetValue(SourceLanguageProperty, value);
    }

    public LanguageModel TargetLanguage
    {
        get => (LanguageModel)GetValue(TargetLanguageProperty);
        set => SetValue(TargetLanguageProperty, value);
    }

    public ICommand SwapLanguagesCommand
    {
        get => (ICommand)GetValue(SwapLanguagesCommandProperty);
        set => SetValue(SwapLanguagesCommandProperty, value);
    }
}
