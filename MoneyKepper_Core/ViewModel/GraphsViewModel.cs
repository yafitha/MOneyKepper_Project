using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.Managers;
using MoneyKepper_Core.Models;
using MoneyKepperCore.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace MoneyKepper_Core.ViewModel
{
    public class GraphsViewModel : NavigableViewModel, IGraphsViewModel
    {
        #region Members
        private IActionsService ActionsService { get; set; }

        #endregion

        #region Bindable Proerty


        private DateTime _currentMonth;
        public DateTime CurrentMonth
        {
            get { return _currentMonth; }
            set { this.Set(ref _currentMonth, value); }
        }

        private ObservableCollection<DateTime> _allMonths;
        public ObservableCollection<DateTime> AllMonths
        {
            get { return _allMonths; }
            set { this.Set(ref _allMonths, value); }
        }


        #endregion

        #region Commands

        public RelayCommand ShowGraphCommmand { get; private set; }
        #endregion

        #region Ctor's
        public GraphsViewModel(IActionsService actionsService)
        {
            this.ActionsService = actionsService;
            this.SetCommands();
        }

        #endregion

        #region Methods
        private void SetCommands()
        {
            this.ShowGraphCommmand = new RelayCommand(OnShowGraphCommmand);
        }

        private void OnShowGraphCommmand()
        {
          
            this.ActionsService.ShowMonthGraphs(this.CurrentMonth);
        }

        private void InitAllMonths()
        {
            this.AllMonths = new ObservableCollection<DateTime>();
            for (int i = 0; i < 11; i++)
            {
                var month = DateTime.Now.AddMonths(-i);
                this.AllMonths.Add(month);
            }
        }

        #endregion

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                this.InitAllMonths();
                this.CurrentMonth = this.AllMonths[0];
            }
        }

        public override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.ActionsService.ShowEmptyPage();
        }
    }
}
