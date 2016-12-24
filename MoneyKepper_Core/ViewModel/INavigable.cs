using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace MoneyKepper_Core.ViewModel
{
    public interface INavigable
    {
        void OnNavigatedFrom(NavigationEventArgs e);
        void OnNavigatedTo(NavigationEventArgs e);
        void OnNavigatingFrom(NavigatingCancelEventArgs e);
    }
}
