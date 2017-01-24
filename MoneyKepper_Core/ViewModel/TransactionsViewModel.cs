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

        private string _currentMonth;
        public string CurrentMonth
        {
            get { return _currentMonth; }
            set { this.Set(ref _currentMonth, value); }
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

        #endregion

        #region Commands

        public RelayCommand ShowExtraInfoCommand { get; private set; }

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
            this.ShowExtraInfoCommand = new RelayCommand(OnShowExtraInfoCommand);
        }

        private void OnShowExtraInfoCommand()
        {
            Action<Tuple<Types, double>> AddCallBack = transaction =>
            {
                if (transaction.Item1 == Types.Expenses)
                {
                    this.Expenses += transaction.Item2;
                }
                else
                {
                    this.Income += transaction.Item2;
                }
                this.Balance = this.Income - this.Expenses;
            };

            Action<Tuple<Types, double>> removeCallBack = transaction =>
            {
                if (transaction.Item1 == Types.Expenses)
                {
                    this.Expenses -= transaction.Item2;
                }
                else
                {
                    this.Income -= transaction.Item2;
                }
                this.Balance = this.Income - this.Expenses;
            };

            this.ActionsService.ShowTransactionsDetails(AddCallBack, removeCallBack);
        }

        private void SetIncomeItemsAndExpensesItems()
        {
            var expensesTransactions = this.DataService.GetTransactionsByType(Types.Expenses);
            var incomeTransactions = this.DataService.GetTransactionsByType(Types.Income);
            this.Income = incomeTransactions.Sum(t => t.Amount);
            this.Expenses = expensesTransactions.Sum(t => t.Amount);
        }

        #endregion

        #region INavigable Methods

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                var args = e.Parameter as Dictionary<string, object>;
                this.SetIncomeItemsAndExpensesItems();
                this.CurrentMonth = DateTime.Now.ToString("MMMM");
                this.Balance = Income - Expenses;
                var list = new List<int>();
                list.Add((int)Types.Expenses);
                var categories = CategoryBL.GetCategoriesByTypes(list);
                //var categories = CategoryBL.GetAllCategories();
                var result = CategoryBL.CreateNewCategory(new Category(5, "חופשה", 2, "5", false));
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
