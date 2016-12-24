using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.Models;
using MoneyKepperCore.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace MoneyKepper_Core.ViewModel
{
    public class PrimaryPanelViewModel : MoneyKepperCore.ViewModel.ViewModelCore, IPrimaryPanelViewModel, IFramePresenter
    {
        #region Events

        public event EventHandler Closed;
        public event EventHandler ClearBackStackRequestRaised;

        #endregion

        #region Members

        private INavigationService NavigationService { get; set; }

        #endregion

        #region Commands

        public RelayCommand<Frame> ViewLoadedCommand { get; set; }

        #endregion

        #region Constructors

        public PrimaryPanelViewModel(
            INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.SetCommands();
        }

        #endregion

        #region Private Commands

        private void SetCommands()
        {
            this.ViewLoadedCommand = new RelayCommand<Frame>(OnViewLoadedCommand);
        }

        #endregion

        #region Commands Handlers

        private void OnViewLoadedCommand(Frame frame)
        {
            this.NavigationService.PrimaryFrame.Initialize(frame, this);
            this.NavigationService.PrimaryFrame.NavigateTo(NavigationPageKeys.HOME_PAGE);

        }

        #endregion
    }
}
