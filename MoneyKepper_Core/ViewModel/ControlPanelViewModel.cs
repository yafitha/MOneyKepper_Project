using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.Models;
using MoneyKepper_Core.ViewModel;
using MoneyKepperCore.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepperCore.ViewModel
{
    public class ControlPanelViewModel : ViewModelCore, IControlPanelViewModel
    {
        #region Consts

        private const string IVR_HEADER = "בקשת אימות טלפוני";
        private const string IVR_SERVICE_MESSAGE = "אינך מנוי לשירות אימות טלפוני. על מנת להירשם לשירות יש להתקשר למוקד התמיכה במספר 03-9700735";

        #endregion

        #region Members

        public IActionsService ActionsService { get; set; }
        private INavigationService NavigationService { get; set; }

        #endregion

        #region Bindable Members

        private bool _isChecked;
        public bool IsChecked
        {
            get { return _isChecked; }
            set { this.Set(() => IsChecked, ref _isChecked, value); }
        }

        #endregion

        #region Commands

        public RelayCommand BackNavigationCommand { get; private set; }
        public RelayCommand<string> MenuItemCommand { get; private set; }

        #endregion

        #region Constructors

        public ControlPanelViewModel(
            INavigationService navigationService)
        {
            this.NavigationService = navigationService;
            //  this.PrimaryPanelNavigationService. += NavigationService_PrimaryPanelSelectionChanged;
            // this.SetControlsPanelItems();
            this.SetCommands();
            this.IsChecked = true;
        }

        #endregion

        #region Private Methods

        private void SetCommands()
        {
            this.MenuItemCommand = new RelayCommand<string>(OnMenuItemCommand);
            this.BackNavigationCommand = this.OnBackNavigationCommand();
        }

        private RelayCommand OnBackNavigationCommand()
        {
            return new RelayCommand(() =>
            {
                this.NavigationService.PrimaryFrame.GoBack();
            });
        }

        #endregion

        #region Commands Handlers

        private void OnMenuItemCommand(string menuItem)
        {
            PrimaryPanel item;
            if (Enum.TryParse(menuItem, out item))
            {
                var args = new Dictionary<string, object>();
                args.Add("NavigationService", this.NavigationService.DetailsFrame);
                switch (item)
                {
                    case PrimaryPanel.HomePage:
                        this.NavigationService.PrimaryFrame.NavigateTo(NavigationPageKeys.HOME_PAGE, args);
                        break;
                    case PrimaryPanel.History:
                        this.NavigationService.PrimaryFrame.NavigateTo(NavigationPageKeys.HISTORY, args);
                        break;
                    case PrimaryPanel.GRAPHS:
                        this.NavigationService.PrimaryFrame.NavigateTo(NavigationPageKeys.GRAPHS, args);
                        break;
                    case PrimaryPanel.Category:
                        this.NavigationService.PrimaryFrame.NavigateTo(NavigationPageKeys.CATEGORY, args);
                        break;

                    case PrimaryPanel.Report:
                        this.NavigationService.PrimaryFrame.NavigateTo(NavigationPageKeys.REPORT, args);
                        break;

                    case PrimaryPanel.Buget:
                        this.NavigationService.PrimaryFrame.NavigateTo(NavigationPageKeys.BUGET, args);
                        break;
                }
            }
        }

        #endregion
        public enum PrimaryPanel
        {
            HomePage,
            History,
            GRAPHS,
            Category,
            Notifications,
            Report,
            Buget
        }
    }
}
