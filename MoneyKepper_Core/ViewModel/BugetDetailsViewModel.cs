using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using MoneyKepper_Core.BL;
using MoneyKepper_Core.Services;
using GalaSoft.MvvmLight.Command;
using MoneyKepper2.Service;
using MoneyKepper_Core.Models;
using System;
using Models;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;
using Windows.UI.Xaml.Controls;

namespace MoneyKepper_Core.ViewModel
{
    class BugetDetailsViewModel : NavigableViewModel, IBugetDetailsViewModel
    {
        #region Members
        private IDialogService DialogService { get; set; }
        private IDataService DataService { get; set; }
        private List<Category> Categories { get; set; }

        #endregion

        #region Bindable Properties

        private DateTime _currentMonth;
        public DateTime CurrentMonth
        {
            get { return _currentMonth; }
            set { this.Set(ref _currentMonth, value); }
        }

        private bool _isShowTransactions;
        public bool IsShowTransactions
        {
            get { return _isShowTransactions; }
            set { this.Set(ref _isShowTransactions, value); }
        }

        private ObservableCollection<BugetItem> _incomeItems;
        public ObservableCollection<BugetItem> IncomeItems
        {
            get { return _incomeItems; }
            set
            {
                this.Set(ref _incomeItems, value);
                SetCategories();
            }
        }

        private ObservableCollection<BugetItem> _expensesItems;
        public ObservableCollection<BugetItem> ExpensesItems
        {
            get { return _expensesItems; }
            set
            {
                this.Set(ref _expensesItems, value);
                SetCategories();
            }
        }

        public ObservableCollection<TransactionItem> AllTransactionsItems { get; set; }

        private ObservableCollection<TransactionItem> _categoryTransactions;
        public ObservableCollection<TransactionItem> CategoryTransactions
        {
            get { return _categoryTransactions; }
            set { this.Set(ref _categoryTransactions, value); }
        }

        string _transactionsTitle;
        public string TransactionsTitle
        {
            get { return _transactionsTitle; }
            set
            {
                this.Set(ref _transactionsTitle, value);
            }
        }
        public string Title { get; private set; }

        #endregion

        #region Commands
        public RelayCommand<string> AddTransactionCommand { get; private set; }
        public RelayCommand<BugetItem> ShowBugetCommand { get; private set; }
        public RelayCommand<BugetItem> UpdateBugetCommand { get; private set; }
        public RelayCommand<BugetItem> RemoveBugetCommand { get; private set; }

        public Action<BugetItem> AddCallBack { get; private set; }
        public Action<BugetItem> RemoveCallBack { get; private set; }
        public RelayCommand AddBugetCommand { get; private set; }
        public Action<Tuple<BugetItem, double>> UpdateCallBack { get; private set; }

        #endregion

        #region Cotr's
        public BugetDetailsViewModel(IDialogService dialogService, IDataService dataService)
        {
            this.DialogService = dialogService;
            this.DataService = dataService;
            this.SetCommands();
        }

        #endregion

        #region Private Methods
        private void SetCommands()
        {
            this.UpdateBugetCommand = new RelayCommand<BugetItem>(OnUpdateBugetCommand);
            this.RemoveBugetCommand = new RelayCommand<BugetItem>(OnRemoveBugetCommand);
            this.ShowBugetCommand = new RelayCommand<BugetItem>(OnShowBugetCommand);
            this.AddBugetCommand = new RelayCommand(OnAddBugetCommand);
        }

        private void OnAddBugetCommand()
        {
            Action<BugetItem> addNewBugetCallback = bugetItem =>
            {
                var result = BugetBL.CreateNewBuget(bugetItem.Buget);
                bugetItem.Transactions = this.GetTrnsactionsByBuget(bugetItem.Buget);
                bugetItem.UseMoney = bugetItem.Transactions.Sum(t => t.Transaction.Amount);
                bugetItem.LeftMoney = bugetItem.Buget.Amount - bugetItem.UseMoney;
                if (result)
                {
                    if (bugetItem.Category.TypeID == (int)Types.Expenses)
                    {
                        this.ExpensesItems.Add(bugetItem);
                    }
                    else
                    {
                        this.IncomeItems.Add(bugetItem);
                    }
                    var cat = this.Categories.FirstOrDefault(c => c.ID == bugetItem.Category.ID);
                    if (cat != null)
                    {
                        this.Categories.Remove(cat);
                    }
                }

                this.AddCallBack(bugetItem);
            };

            var dialogArgs = new Dictionary<string, object>()
                {
                    { "Callback", addNewBugetCallback },
                   {"CurrentMonth", this.CurrentMonth },
                    {"Categories", this.Categories},
                };
            this.DialogService.ShowDialog(DialogKeys.ADD_BUGET, dialogArgs);
        }

        private void OnUpdateBugetCommand(BugetItem item)
        {
            Action<BugetItem> callback = bugetItem =>
            {
                BugetBL.UpdateBuget(bugetItem.Buget);
                this.UpdateCallBack(new Tuple<BugetItem, double>(bugetItem, item.Amount));
                bugetItem.Amount = bugetItem.Buget.Amount;
                bugetItem.Note = bugetItem.Buget.Note;
                bugetItem.LeftMoney = bugetItem.Buget.Amount - bugetItem.UseMoney;
              
            };

            var dialogArgs = new Dictionary<string, object>()
                {
                    { "Callback", callback },
                   {"CurrentMonth", this.CurrentMonth },
                    {"Categories", this.Categories},
                {"BugetItem ",item }
                };
            this.DialogService.ShowDialog(DialogKeys.ADD_BUGET, dialogArgs);
        }

        private void OnShowBugetCommand(BugetItem item)
        {
            if (this.AllTransactionsItems == null || this.AllTransactionsItems.Count == 0)
                return;

            IsShowTransactions = true;
            this.CategoryTransactions = new ObservableCollection<TransactionItem>(item.Transactions);
            this.TransactionsTitle = string.Format("פירוט תנועות של קטגוריה {0}", item.Category.Name);
        }

        private async void OnRemoveBugetCommand(BugetItem bugetItem)
        {
            Action removeCallBack = () =>
            {
                var result = BugetBL.DeleteBuget(bugetItem.Buget.ID);
                if (result)
                {
                    if (bugetItem.Category.TypeID == (int)Types.Income)
                    {
                        this.IncomeItems.Remove(bugetItem);
                    }
                    else
                    {
                        this.ExpensesItems.Remove(bugetItem);
                    }
                    this.RemoveCallBack(bugetItem);
                    this.CategoryTransactions = null;
                    this.Categories.Add(bugetItem.Category);
                }
            };

            var dialogArgs = new Dictionary<string, object>()
                {
                    { "Title", "מחיקת שורת תקציב" },
                    { "Content", "האם אתה בטוח שברצונך למחוק את השורה?" },
                    {"CallBack", removeCallBack }
                };

            await this.DialogService.ShowDialog(DialogKeys.CONFIRM, dialogArgs);
        }

        private void SetIncomeItemsAndExpensesItems()
        {
            this.CategoryTransactions = null;
            var firstDayOfMonth = new DateTime(this.CurrentMonth.Year, CurrentMonth.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var allTransactions = this.DataService.GetTransactionsByDateAndType(firstDayOfMonth, lastDayOfMonth, (int)Types.Expenses).Select(tran => new TransactionItem(tran, tran.Category)).OrderBy(t => t.Transaction.Date).ToList();
            this.AllTransactionsItems = new ObservableCollection<TransactionItem>(allTransactions);
            var expensesBuget = BugetBL.GetBugetByDatesAndType(firstDayOfMonth, lastDayOfMonth, (int)Types.Expenses).ToList();
            var incomeBuget = BugetBL.GetBugetByDatesAndType(firstDayOfMonth, lastDayOfMonth, (int)Types.Income).ToList();
            this.IncomeItems = new ObservableCollection<BugetItem>(this.GetBugetItems(incomeBuget));
            this.ExpensesItems = new ObservableCollection<BugetItem>(this.GetBugetItems(expensesBuget));
            this.SetCategories();
        }

        private void SetCategories()
        {
            if (this.ExpensesItems == null || this.IncomeItems == null)
                return;

            var existsCategories = this.IncomeItems.Select(b => b.Category).ToList();
            existsCategories.AddRange(this.ExpensesItems.Select(b => b.Category).ToList());
            this.Categories = CategoryBL.GetAllCategories().ToList();
            existsCategories.ForEach(c => Categories.RemoveAll(cat => c.ID == cat.ID));
            //this.Categories = this.Categories.Except(existsCategories).ToList();
        }

        public List<BugetItem> GetBugetItems(List<Buget> bugets)
        {
            List<BugetItem> bugetItems = new List<BugetItem>();
            foreach (var buget in bugets.OrderBy(b2 => b2.Date).ToList())
            {
                var bugetItem = new BugetItem(buget, buget.Category);
                bugetItem.Transactions = GetTrnsactionsByBuget(buget);
                bugetItem.UseMoney = bugetItem.Transactions.Sum(t => t.Transaction.Amount);
                bugetItem.LeftMoney = bugetItem.Buget.Amount - bugetItem.UseMoney;
                bugetItems.Add(bugetItem);
            }
            return bugetItems;
        }
        private List<TransactionItem> GetTrnsactionsByBuget(Buget buget)
        {
            return this.AllTransactionsItems.Where(t => t.Category.ID == buget.CategoryID).ToList();
        }

        #endregion

        #region INavigable Methods

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                var args = e.Parameter as Dictionary<string, object>;
                this.CurrentMonth = (DateTime)args["Month"];
                this.AddCallBack = args["AddCallBack"] as Action<BugetItem>;
                this.RemoveCallBack = args["RemoveCallBack"] as Action<BugetItem>;
                this.UpdateCallBack = args["UpdateCallBack"] as Action<Tuple<BugetItem, double>>;
                this.SetIncomeItemsAndExpensesItems();
                this.IsShowTransactions = false;
                this.Title = string.Format("פירוט תנועות של חודש {0}", CurrentMonth.ToString("MMMMM"));
            }
        }

        #endregion
    }
}
