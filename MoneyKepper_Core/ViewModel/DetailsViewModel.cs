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
    public class DetailsViewModel: MoneyKepperCore.ViewModel.ViewModelCore, IDetailsViewModel, IFramePresenter
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

        public DetailsViewModel(
            INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            this.SetCommands();
        }

        #endregion

        #region Private Commands


        private void SetCommands()
        {
            this.ViewLoadedCommand = this.OnViewLoadedCommand();
        }

        #endregion

        #region Commands Handlers

        private RelayCommand<Frame> OnViewLoadedCommand()
        {
            return new RelayCommand<Frame>(frame =>
            {
                this.NavigationService.DetailsFrame.Initialize(frame, this);
            });
        }

        #endregion
    }
}
