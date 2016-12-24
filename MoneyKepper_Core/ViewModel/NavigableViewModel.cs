using MoneyKepperCore.Service;
using MoneyKepperCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace MoneyKepper_Core.ViewModel
{
    public class NavigableViewModel : ViewModelCore, INavigable
    {
        #region Members

        private ICoreNavigationService _navigationService;
        public ICoreNavigationService NavigationService
        {
            get { return _navigationService; }
            set { this.Set(() => NavigationService, ref _navigationService, value); }
        }

        #endregion

        #region INavigable Members

        public virtual void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public virtual void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode == NavigationMode.New)
            {
                var args = e.Parameter as Dictionary<string, object>;
                if (args == null)
                    return;

                if (args.ContainsKey("NavigationService"))
                {
                    this.NavigationService = args["NavigationService"] as ICoreNavigationService;
                }
            }
        }

        public virtual void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
        }

        #endregion
    }
}
