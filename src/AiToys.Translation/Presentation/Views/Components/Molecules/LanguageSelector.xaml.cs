using AiToys.Translation.Domain.Models;
using Microsoft.UI.Xaml;

namespace AiToys.Translation.Presentation.Views.Components.Molecules;

internal sealed partial class LanguageSelector
{
    public static readonly DependencyProperty LabelTextProperty = DependencyProperty.Register(
        nameof(LabelText),
        typeof(string),
        typeof(LanguageSelector),
        new PropertyMetadata(defaultValue: string.Empty)
    );

    public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
        nameof(ItemsSource),
        typeof(IEnumerable<LanguageModel>),
        typeof(LanguageSelector),
        new PropertyMetadata(defaultValue: null)
    );

    public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
        nameof(SelectedItem),
        typeof(LanguageModel),
        typeof(LanguageSelector),
        new PropertyMetadata(defaultValue: null)
    );

    public LanguageSelector()
    {
        InitializeComponent();
    }

    public string LabelText
    {
        get => (string)GetValue(LabelTextProperty);
        set => SetValue(LabelTextProperty, value);
    }

    public IEnumerable<LanguageModel> ItemsSource
    {
        get => (IEnumerable<LanguageModel>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public LanguageModel SelectedItem
    {
        get => (LanguageModel)GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
}
