namespace AiToys.Presentation.Contracts;

internal interface IView;

internal interface IView<TViewModel> : IView
    where TViewModel : IViewModel
{
    TViewModel ViewModel { get; set; }
}
