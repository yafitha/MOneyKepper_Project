using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MoneyKepperCore.Service
{
    public interface ICoreNavigationService : GalaSoft.MvvmLight.Views.INavigationService
    {
        event NavigatingCancelEventHandler Navigating;
        event NavigatedEventHandler Navigated;

        event EventHandler BackStackCleared;
        event EventHandler FrameClosed;

        bool CanGoBack { get; }

        void Initialize(Frame frame, IFramePresenter framePresenter = null);
        void Configure(string key, Type pageType);
        void ClearLastBackStackEntry();
    }
}
