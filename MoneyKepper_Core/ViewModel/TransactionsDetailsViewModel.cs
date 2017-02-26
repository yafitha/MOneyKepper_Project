using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.Models;
using Models;
using MoneyKepper_Core.Services;
using MoneyKepper2.Service;
using MoneyKepperCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;
using MoneyKepper_Core.BL;

namespace MoneyKepper_Core.ViewModel
{
    public class TransactionsDetailsViewModel : NavigableViewModel, ITransactionsDetailsViewModel
    {
        #region Members
        private IDialogService DialogService { get; set; }
        private IDataService DataService { get; set; }
        private IList<Category> Categories { get; set; }

        #endregion

        #region Bindable Properties

        public ObservableCollection<TransactionItem> IncomeItems { get; set; }
        public ObservableCollection<TransactionItem> ExpensesItems { get; set; }

        #endregion

        #region Commands
        public RelayCommand<string> AddTransactionCommand { get; private set; }
        public RelayCommand<TransactionItem> RemoveCommand { get; set; }

        public Action<Tuple<TransactionsViewModel.Types, double>> AddCallBack { get; private set; }
        public Action<Tuple<TransactionsViewModel.Types, double>> RemoveCallBack { get; private set; }

        #endregion

        #region Cotr's
        public TransactionsDetailsViewModel(IDialogService dialogService, IDataService dataService)
        {
            this.DialogService = dialogService;
            this.DataService = dataService;
            this.SetCommands();
        }

        #endregion

        #region Private Methods
        private void SetCommands()
        {
            this.AddTransactionCommand = new RelayCommand<string>(OnAddTransactionCommand);
            this.RemoveCommand = new RelayCommand<TransactionItem>(OnRemoveCommand);
        }

        private void OnAddTransactionCommand(string transactionType)
        {
            TransactionsViewModel.Types type;
            if (!Enum.TryParse(transactionType, out type))
                return;

            Action<TransactionItem> AddNewTransactionCallback = transactionItem =>
            {
                if (type == Types.Expenses)
                {
                    var result = TransactionBL.CreateNewTransaction(transactionItem.Transaction);
                    if (result)
                    {
                        this.ExpensesItems.Add(transactionItem);
                    }
                }
                else
                {
                    var result = TransactionBL.CreateNewTransaction(transactionItem.Transaction);
                    if (result)
                    {
                        this.IncomeItems.Add(transactionItem);
                    }
                }
                this.AddCallBack(Tuple.Create(type, transactionItem.Transaction.Amount));
            };

            var dialogArgs = new Dictionary<string, object>()
                {
                    { "Callback", AddNewTransactionCallback },
                    {"Categories", this.Categories.Where(cat=>cat.TypeID == (int)type).ToList()},
                    {"TransactionType",transactionType }
                };

            this.DialogService.ShowDialog(DialogKeys.ADD_TRANSACTION, dialogArgs);
        }
        private void OnRemoveCommand(TransactionItem transactionItem)
        {
            if (transactionItem.Category.TypeID == (int)Types.Expenses)
            {
                var result = TransactionBL.DeleteTransaction(transactionItem.Transaction.ID);
                if (result)
                {
                    this.ExpensesItems.Remove(transactionItem);
                    this.RemoveCallBack(Tuple.Create(Types.Expenses, transactionItem.Transaction.Amount));
                }
            }
            else
            {
                var result = TransactionBL.DeleteTransaction(transactionItem.Transaction.ID);
                if (result)
                {
                    this.IncomeItems.Remove(transactionItem);
                    this.RemoveCallBack(Tuple.Create(Types.Income, transactionItem.Transaction.Amount));
                }
            }
        }

        private void SetIncomeItemsAndExpensesItems()
        {
            this.Categories = this.DataService.GetAllCategories();
            var expensesTransactions = this.DataService.GetTransactionsByType(Types.Expenses);
            var incomeTransactions = this.DataService.GetTransactionsByType(Types.Income);
            this.IncomeItems = new ObservableCollection<TransactionItem>(incomeTransactions.Select(tran => new TransactionItem(tran, Categories.FirstOrDefault(c => c.ID == tran.CategoryID))).ToList());
            this.ExpensesItems = new ObservableCollection<TransactionItem>(expensesTransactions.Select(tran => new TransactionItem(tran, Categories.FirstOrDefault(c => c.ID == tran.CategoryID))).ToList());
        }

        #endregion

        #region INavigable Methods

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                var args = e.Parameter as Dictionary<string, object>;
                this.AddCallBack = args["AddCallBack"] as Action<Tuple<TransactionsViewModel.Types, double>>;
                this.RemoveCallBack = args["RemoveCallBack"] as Action<Tuple<TransactionsViewModel.Types, double>>;
                this.SetIncomeItemsAndExpensesItems();
            }
        }

        #endregion
    }
}
