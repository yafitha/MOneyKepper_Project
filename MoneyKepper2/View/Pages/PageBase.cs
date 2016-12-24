using MoneyKepper_Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MoneyKepper2.View.Pages
{
    public class PageBase : Page
    {
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            if (this.DataContext is INavigable)
            {
                (this.DataContext as INavigable).OnNavigatedFrom(e);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (this.DataContext is INavigable)
            {
                (this.DataContext as INavigable).OnNavigatedTo(e);
            }
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (this.DataContext is INavigable)
            {
                (this.DataContext as INavigable).OnNavigatingFrom(e);
            }
        }
    }
}
