using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.Managers;
using MoneyKepper_Core.Models;
using MoneyKepper2.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;
using Models;

namespace MoneyKepper_Core.ViewModel
{
    class HistoryDetailsViewModel : NavigableViewModel, IHistoryDetailsViewModel
    {
        #region Members

        private List<Category> Categories { get; set; }
        private IDataService DataServcie { get; set; }
        private DateTime StartDateTime { get; set; }
        private DateTime EndDateTime { get; set; }
        private Graph GraphType { get; set; }

        #endregion

        #region Bindable Proerty

        private ObservableCollection<CategoryItem> _categoryItems;
        public ObservableCollection<CategoryItem> CategoryItems
        {
            get { return _categoryItems; }
            set { _categoryItems = value; }
        }

        private ObservableCollection<MonthItem> _monthItems;
        public ObservableCollection<MonthItem> MonthItems
        {
            get { return _monthItems; }
            set { _monthItems = value; }
        }

        private bool _showCategoriesGraph;
        public bool ShowCategoriesGraph
        {
            get { return _showCategoriesGraph; }
            set { this.Set(ref _showCategoriesGraph, value); }
        }

        #endregion

        #region Commands

        #endregion

        #region Ctor's
        public HistoryDetailsViewModel(IDataService dataService)
        {
            this.DataServcie = dataService;
        }

        #endregion

        #region Methods

        private void ShowGraph()
        {
            this.CategoryItems = new ObservableCollection<CategoryItem>();
            this.MonthItems = new ObservableCollection<MonthItem>();
            Random r = new Random(123345);
            int count = (EndDateTime.Month - StartDateTime.Month) + 12 * (EndDateTime.Year - StartDateTime.Year);
            if (this.ShowCategoriesGraph)
            {
                this.SetCategoryItems();
            }
            else
            {
                this.SetMonthItems();
            }
        }

        private void SetMonthItems()
        {
            var expensesTransactions = this.DataServcie.GetTransactionsByType(Types.Expenses);
            var expenses = expensesTransactions.Sum(e => e.Amount);
            var incomeTransactions = this.DataServcie.GetTransactionsByType(Types.Income);
            var incomes = incomeTransactions.Sum(e => e.Amount);
            int count = (EndDateTime.Month - StartDateTime.Month) + 12 * (EndDateTime.Year - StartDateTime.Year);
            string month;
            for (int i = 0; i < count+1; i++)
            {
                month = month = this.StartDateTime.AddMonths(i).ToString("MMMM");
                var monthItem = new MonthItem();
                monthItem.Expenses = expenses;
                monthItem.Income = incomes;
                monthItem.Month = month;
                this.MonthItems.Add(monthItem);
            }
        }

        private void SetCategoryItems()
        {
            int count = (EndDateTime.Month - StartDateTime.Month) + 12 * (EndDateTime.Year - StartDateTime.Year);
            string month;
            Random r = new Random(123345);
            for (int i = 0; i < count+1; i++)
            {
                for (int j = 0; j < Categories.Count; j++)
                {
                    month = month = this.StartDateTime.AddMonths(i).ToString("MMMM");
                    var categoryItem = new CategoryItem(Categories[j], month, (3000 * r.Next(1, 12)));
                    this.CategoryItems.Add(categoryItem);
                }
            }
        }

        #endregion

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                var args = e.Parameter as Dictionary<string, object>;
                this.StartDateTime = (DateTime)args["StartDateTime"];
                this.EndDateTime = (DateTime)args["EndDateTime"];
                this.GraphType = (Graph)args["GraphType"];
                this.Categories = (List<Category>)args["Categories"];
                this.ShowCategoriesGraph = this.GraphType == Graph.CategoriesMonthColumns ? true : false;
                ShowGraph();
            }
        }

        public override void OnNavigatedFrom(NavigationEventArgs e)
        {

        }
    }
}
