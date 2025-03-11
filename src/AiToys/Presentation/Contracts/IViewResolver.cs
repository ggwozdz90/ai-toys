namespace AiToys.Presentation.Contracts;

internal interface IViewResolver
{
    void RegisterView<TView, TViewModel>()
        where TView : IView<TViewModel>
        where TViewModel : IViewModel;

    Type ResolveViewType<TViewModel>()
        where TViewModel : IViewModel;
}
