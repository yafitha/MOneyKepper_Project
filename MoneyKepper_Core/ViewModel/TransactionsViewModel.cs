using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.BL;
using MoneyKepper_Core.Models;
using Models;
using MoneyKepper_Core.Services;
using MoneyKepper2.Service;
using MoneyKepperCore.Service;
using MoneyKepperCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;

namespace MoneyKepper_Core.ViewModel
{
    public class TransactionsViewModel : NavigableViewModel, ITransactionsViewModel
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

        public RelayCommand ShowTransactionsCommand { get; private set; }

        #endregion

        #region Commands

        #endregion

        #region Cotr's
        public TransactionsViewModel(IDialogService dialogService, IDataService dataService, IActionsService actionsService)
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
            this.ShowTransactionsCommand = new RelayCommand(OnShowTransactionsCommand);
        }

        private void OnShowTransactionsCommand()
        {
            this.SetIncomeItemsAndExpensesItems();
            this.ShowExtraInfo();
        }

        private void ShowExtraInfo()
        {
            Action<TransactionItem> AddCallBack = transactionItem =>
            {
                if (transactionItem.Category.TypeID == (int)Types.Expenses)
                {
                    this.Expenses += transactionItem.Transaction.Amount;
                }
                else
                {
                    this.Income += transactionItem.Transaction.Amount;
                }
                this.Balance = this.Income - this.Expenses;
            };

            Action<TransactionItem> removeCallBack = transactionItem =>
            {
                if (transactionItem.Category.TypeID == (int)Types.Expenses)
                {
                    this.Expenses -= transactionItem.Transaction.Amount;
                }
                else
                {
                    this.Income -= transactionItem.Transaction.Amount;
                }
                this.Balance = this.Income - this.Expenses;
            };

            this.ActionsService.ShowTransactionsDetails(AddCallBack, removeCallBack, this.CurrentMonth);
        }

        private void SetIncomeItemsAndExpensesItems()
        {
            var firstDayOfMonth = new DateTime(this.CurrentMonth.Year, CurrentMonth.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var expensesTransactions = this.DataService.GetTransactionsByDateAndType(firstDayOfMonth,lastDayOfMonth, (int)Types.Expenses);
            var incomeTransactions = this.DataService.GetTransactionsByDateAndType(firstDayOfMonth, lastDayOfMonth, (int)Types.Income);
            this.Income = incomeTransactions.Sum(t => t.Amount);
            this.Expenses = expensesTransactions.Sum(t => t.Amount);
            this.Balance = this.Income - this.Expenses;
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
                this.ShowExtraInfo();
            }
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

        public override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.ActionsService.ShowEmptyPage();
        }

        #endregion

        public enum Types
        {
            Income = 1,
            Expenses = 2
        };
    }
}
