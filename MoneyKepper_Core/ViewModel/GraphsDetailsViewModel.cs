using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.Managers;
using MoneyKepper_Core.Models;
using MoneyKepper2.Service;
using Syncfusion.UI.Xaml.Charts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;

namespace MoneyKepper_Core.ViewModel
{
    class GraphsDetailsViewModel : NavigableViewModel, IGraphsDetailsViewModel
    {
        #region Members
        private DateTime StartDateTime { get; set; }
        private DateTime EndDateTime { get; set; }
        public ObservableCollection<TransactionItem> AllTransactions { get; set; }

        #endregion

        #region Bindable Proerty

        private ObservableCollection<CategoryItem> _expensescategoryItems;
        public ObservableCollection<CategoryItem> ExpensesCategoryItems
        {
            get { return _expensescategoryItems; }
            set { _expensescategoryItems = value; }
        }


        private ObservableCollection<CategoryItem> _incomeCategoryItems;
        public ObservableCollection<CategoryItem> IncomeCategoryItems
        {
            get { return _incomeCategoryItems; }
            set { _incomeCategoryItems = value; }
        }

        private bool _showPieGraph;
        public bool ShowPieGraph
        {
            get { return _showPieGraph; }
            set { this.Set(ref _showPieGraph, value); }
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

        private ObservableCollection<TransactionItem> _transactions;
        public ObservableCollection<TransactionItem> Transactions
        {
            get { return _transactions; }
            set { this.Set(ref _transactions, value); }
        }

        #endregion

        #region Commands

        public RelayCommand ShowGraphCommmand { get; private set; }

        public RelayCommand<TappedRoutedEventArgs> ShowTransactionCommand { get; set; }
        public IDataService DataService { get; private set; }
        public DateTime Month { get; private set; }
        #endregion

        #region Ctor's
        public GraphsDetailsViewModel(IDataService dataService)
        {
            this.DataService = dataService;
            this.SetCommands();
        }

        #endregion

        #region Methods
        private void SetCommands()
        {
            this.ShowTransactionCommand = new RelayCommand<TappedRoutedEventArgs>(OnShowTransactionCommand);
        }

        private void OnShowTransactionCommand(TappedRoutedEventArgs obj)
        {
            try
            {
                if (!(obj.OriginalSource is Path))
                {
                    return;
                }

                var path = obj.OriginalSource as Path;
                var pieSegment = path.Tag as PieSegment;
                if (pieSegment == null || !pieSegment.IsExploded)
                    return;

                var categoryItem = pieSegment.Item as CategoryItem;
                this.Transactions = new ObservableCollection<TransactionItem>(AllTransactions.Where(t => t.Category.ID == categoryItem.Category.ID));
            }
            catch { }
        }

        private void ShowGraph()
        {
            this.IncomeCategoryItems = new ObservableCollection<CategoryItem>();
            this.ExpensesCategoryItems = new ObservableCollection<CategoryItem>();
            this.Transactions = null;
            var transactions = this.DataService.GetTransactionsByDate(this.Month);
            if (transactions == null || transactions.Count == 0)
            {
                this.Expenses = 0;
                this.Income = 0;
                return;
            }

            this.AllTransactions = new ObservableCollection<TransactionItem>(transactions.Select(tran => new TransactionItem(tran, tran.Category)).OrderBy(t => t.Transaction.Date).ToList());
            var groupedList = transactions
           .GroupBy(c => c.Category.ID).Distinct()
           .Select(grp => grp.ToList())
           .ToList();

            foreach (var groupTransactions in groupedList)
            {
                var categoryItem = new CategoryItem(groupTransactions.FirstOrDefault().Category, this.Month);
                categoryItem.Amount = groupTransactions.Sum(t => t.Amount);
                if (categoryItem.Category.TypeID == (int)Types.Income)
                {
                    IncomeCategoryItems.Add(categoryItem);
                }
                else
                {
                    ExpensesCategoryItems.Add(categoryItem);
                }
            }

            this.Expenses = transactions.Where(t => t.Category.TypeID == (int)Types.Expenses).Sum(t => t.Amount);
            this.Income = transactions.Where(t => t.Category.TypeID == (int)Types.Income).Sum(t => t.Amount);
        }

        #endregion

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                var args = e.Parameter as Dictionary<string, object>;
                this.Month = (DateTime)args["Month"];
                this.ShowGraph();
            }
        }

        public override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.IncomeCategoryItems.Clear();
            this.ExpensesCategoryItems.Clear();
        }
    }
}
