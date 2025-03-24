using System.Numerics;
using Microsoft.UI.Composition;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Hosting;
using Microsoft.UI.Xaml.Media;

namespace AiToys.Core.Presentation.Components.Organisms;

/// <summary>
/// A user control that displays an image with a gradient mask that fades out the bottom of the image.
/// </summary>
public sealed partial class PageTitleBackgroundImage : IDisposable
{
    /// <summary>
    /// Identifies the <see cref="Source"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
        nameof(Title),
        typeof(string),
        typeof(PageTitleBackgroundImage),
        new PropertyMetadata(defaultValue: null)
    );

    /// <summary>
    /// Identifies the <see cref="Source"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
        nameof(Source),
        typeof(string),
        typeof(PageTitleBackgroundImage),
        new PropertyMetadata(defaultValue: null, OnSourceChanged)
    );

    /// <summary>
    /// Identifies the <see cref="FadeStart"/> dependency property.
    /// </summary>
    public static readonly DependencyProperty FadeStartProperty = DependencyProperty.Register(
        nameof(FadeStart),
        typeof(double),
        typeof(PageTitleBackgroundImage),
        new PropertyMetadata(0.5, OnFadeStartChanged)
    );

    private LoadedImageSurface? imageSurface;
    private SpriteVisual? spriteVisual;
    private CompositionLinearGradientBrush? gradientBrush;
    private CompositionSurfaceBrush? imageBrush;
    private CompositionMaskBrush? maskBrush;
#pragma warning disable CA2213
    // ElementVisual is owned by the framework and should not be manually disposed
    private Visual? elementVisual;
#pragma warning restore CA2213
    private bool isDisposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="PageTitleBackgroundImage"/> class.
    /// </summary>
    public PageTitleBackgroundImage()
    {
        InitializeComponent();

        Loaded += OnLoaded;
        Unloaded += OnUnloaded;
    }

    /// <summary>
    /// Gets or sets the title to display.
    /// </summary>
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// Gets or sets the source of the image to display.
    /// </summary>
    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    /// <summary>
    /// Gets or sets the position at which the image starts to fade out.
    /// </summary>
    public double FadeStart
    {
        get => (double)GetValue(FadeStartProperty);
        set => SetValue(FadeStartProperty, value);
    }

    /// <summary>
    /// Releases all resources used by the control.
    /// </summary>
    public void Dispose()
    {
        if (isDisposed)
        {
            return;
        }

        CleanupResources();
        isDisposed = true;
    }

    private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (PageTitleBackgroundImage)d;
        control.ApplyOpacityMask();
    }

    private static void OnFadeStartChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var control = (PageTitleBackgroundImage)d;
        control.ApplyOpacityMask();
    }

    private static CompositionSurfaceBrush CreateImageBrush(Compositor compositor, LoadedImageSurface imageSurface)
    {
        var brush = compositor.CreateSurfaceBrush(imageSurface);

        brush.Stretch = CompositionStretch.None;

        return brush;
    }

    private static CompositionMaskBrush CreateMaskBrush(
        Compositor compositor,
        CompositionSurfaceBrush source,
        CompositionBrush mask
    )
    {
        var brush = compositor.CreateMaskBrush();

        brush.Source = source;
        brush.Mask = mask;

        return brush;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        ApplyOpacityMask();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        CleanupResources();
    }

    private void CleanupResources()
    {
        if (spriteVisual != null)
        {
            ElementCompositionPreview.SetElementChildVisual(ImageHost, visual: null);
            spriteVisual.Dispose();
            spriteVisual = null;
        }

        if (maskBrush != null)
        {
            maskBrush.Dispose();
            maskBrush = null;
        }

        if (imageBrush != null)
        {
            imageBrush.Dispose();
            imageBrush = null;
        }

        if (gradientBrush != null)
        {
            gradientBrush.Dispose();
            gradientBrush = null;
        }

        if (imageSurface != null)
        {
            imageSurface.LoadCompleted -= OnImageLoadCompleted;
            imageSurface.Dispose();
            imageSurface = null;
        }

        // ElementVisual is owned by the framework and should not be manually disposed
        // Just removing the reference
        elementVisual = null;
    }

    private void ApplyOpacityMask()
    {
        if (isDisposed)
        {
            return;
        }

        CleanupResources();

        if (string.IsNullOrEmpty(Source) || !IsLoaded)
        {
            return;
        }

        try
        {
            elementVisual = ElementCompositionPreview.GetElementVisual(this);
            var compositor = elementVisual.Compositor;

            gradientBrush = CreateGradientBrush(compositor);
            imageSurface = LoadedImageSurface.StartLoadFromUri(new Uri($"ms-appx://{Source}"));

            imageBrush = CreateImageBrush(compositor, imageSurface);
            maskBrush = CreateMaskBrush(compositor, imageBrush, gradientBrush);

            spriteVisual = CreateSpriteVisual(compositor, maskBrush);
            ElementCompositionPreview.SetElementChildVisual(ImageHost, spriteVisual);

            imageSurface.LoadCompleted += OnImageLoadCompleted;
        }
#pragma warning disable CA1031
        catch (Exception)
#pragma warning restore CA1031
        {
            CleanupResources();
        }
    }

    private void OnImageLoadCompleted(LoadedImageSurface sender, object args)
    {
        if (isDisposed || spriteVisual == null)
        {
            return;
        }

        var naturalSize = new Vector2((float)sender.DecodedSize.Width, (float)sender.DecodedSize.Height);

        spriteVisual.Size = naturalSize;
    }

    private CompositionLinearGradientBrush CreateGradientBrush(Compositor compositor)
    {
        var brush = compositor.CreateLinearGradientBrush();

        brush.StartPoint = new Vector2(0.5f, 0);
        brush.EndPoint = new Vector2(0.5f, 1);
        brush.ColorStops.Add(compositor.CreateColorGradientStop(0.0f, Microsoft.UI.Colors.Black));
        brush.ColorStops.Add(compositor.CreateColorGradientStop((float)FadeStart, Microsoft.UI.Colors.Transparent));

        return brush;
    }

    private SpriteVisual CreateSpriteVisual(Compositor compositor, CompositionBrush brush)
    {
        var localSpriteVisual = compositor.CreateSpriteVisual();

        localSpriteVisual.Size = ImageHost.RenderSize.ToVector2();
        localSpriteVisual.Brush = brush;

        return localSpriteVisual;
    }
}
