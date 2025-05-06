using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace AiToys.Translation.Presentation.Views.Components.Atoms;

internal sealed partial class ApiHealthStatusIcon : Button
{
    public static readonly DependencyProperty IsHealthyProperty = DependencyProperty.Register(
        nameof(IsHealthy),
        typeof(bool),
        typeof(ApiHealthStatusIcon),
        new PropertyMetadata(defaultValue: false)
    );

    public ApiHealthStatusIcon() => InitializeComponent();

    public bool IsHealthy
    {
        get => (bool)GetValue(IsHealthyProperty);
        set => SetValue(IsHealthyProperty, value);
    }
}
