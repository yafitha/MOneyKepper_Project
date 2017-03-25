using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.BL;
using MoneyKepper_Core.Models;
using MoneyKepper_Core.Services;
using MoneyKepper2.Service;
using MoneyKepperCore.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;

namespace MoneyKepper_Core.ViewModel
{
    public class BugetViewModel : NavigableViewModel, IBugetViewModel
    {
        #region Members
        private IDialogService DialogService { get; set; }
        private IDataService DataService { get; set; }
        private IActionsService ActionsService { get; set; }

        #endregion

        #region Bindable Properties

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

        private double _income;
        public double Income
        {
            get { return _income; }
            set { this.Set(ref _income, value); }
        }
        private double _expenses;
        public double Expenses
        {
            get { return _expenses; }
            set { this.Set(ref _expenses, value); }
        }

        private double _balance;
        public double Balance
        {
            get { return _balance; }
            set { this.Set(ref _balance, value); }
        }

        public RelayCommand ShowBugetCommand { get; private set; }

        #endregion

        #region Commands

        #endregion

        #region Cotr's
        public BugetViewModel(IDialogService dialogService, IDataService dataService, IActionsService actionsService)
        {
            this.DialogService = dialogService;
            this.DataService = dataService;
            this.ActionsService = actionsService;
            this.SetCommands();
        }

        #endregion

        #region Private Methods
        private void SetCommands()
        {
            this.ShowBugetCommand = new RelayCommand(OnShowBugetCommand);
        }

        private void OnShowBugetCommand()
        {
            this.SetIncomeItemsAndExpensesItems();
            this.ShowDetails();
        }

        private void SetIncomeItemsAndExpensesItems()
        {
            var firstDayOfMonth = new DateTime(this.CurrentMonth.Year, CurrentMonth.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var expensesBuget = BugetBL.GetBugetByDatesAndType(firstDayOfMonth, lastDayOfMonth, (int)Types.Expenses);
            var incomeBuget = BugetBL.GetBugetByDatesAndType(firstDayOfMonth, lastDayOfMonth, (int)Types.Income);
            this.Income = incomeBuget.Sum(t => t.Amount);
            this.Expenses = expensesBuget.Sum(t => t.Amount);
            this.Balance = this.Income - this.Expenses;
        }
        private void ShowDetails()
        {
            Action<BugetItem> addCallBack = bugetItem =>
            {
                if (bugetItem.Category.TypeID == (int)Types.Expenses)
                {
                    this.Expenses += bugetItem.Buget.Amount;
                }
                else
                {
                    this.Income += bugetItem.Buget.Amount;
                }
                this.Balance = this.Income - this.Expenses;
            };

            Action<BugetItem> removeCallBack = bugetItem =>
            {
                if (bugetItem.Category.TypeID == (int)Types.Expenses)
                {
                    this.Expenses -= bugetItem.Buget.Amount;
                }
                else
                {
                    this.Income -= bugetItem.Buget.Amount;
                }
                this.Balance = this.Income - this.Expenses;
            };

            this.ActionsService.ShowBugetDetails(addCallBack,removeCallBack , this.CurrentMonth);
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

        #region INavigable Methods

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                var args = e.Parameter as Dictionary<string, object>;
                this.InitAllMonths();
                this.CurrentMonth = this.AllMonths[0];
                this.SetIncomeItemsAndExpensesItems();
                // this.CurrentMonth = DateTime.Now.ToString("MMMM");
                this.Balance = Income - Expenses;
                this.ShowDetails();
            }
        }

        public override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.ActionsService.ShowEmptyPage();
        }

        #endregion
    }
}
