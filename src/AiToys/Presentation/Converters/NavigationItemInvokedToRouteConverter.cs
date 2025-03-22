using AiToys.Core.Presentation.ViewModels;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;

namespace AiToys.Presentation.Converters;

internal sealed partial class NavigationItemInvokedToRouteConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (
            value is NavigationViewItemInvokedEventArgs
            {
                InvokedItemContainer.DataContext: INavigationItemViewModel navigationItemViewModel
            }
        )
        {
            return navigationItemViewModel.Route;
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language)
    {
        throw new NotSupportedException("This method should never be called");
    }
}
