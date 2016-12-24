using MoneyKepper2.Service;
using MoneyKepperCore.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Navigation;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;

namespace MoneyKepper_Core.ViewModel
{
    public class CategoryViewModel : NavigableViewModel, ICategoryViewModel
    {
        #region Members
        private IActionsService ActionsService { get; set; }
        private IDataService DataService { get; set; }

        #endregion

        #region Bindable Proerty

        private int _incomesCategoriesCount;
        public int IncomesCategoriesCount
        {
            get { return _incomesCategoriesCount; }
            set { this.Set(ref _incomesCategoriesCount, value); }
        }

        private int _expensesCategoriesCount;
        public int ExpensesCategoriesCount
        {
            get { return _expensesCategoriesCount; }
            set { this.Set(ref _expensesCategoriesCount, value); }
        }
        
        #endregion

        #region Commands


        #endregion

        public CategoryViewModel(IActionsService actionsService, IDataService dataService)
        {
            this.ActionsService = actionsService;
            this.DataService = dataService;
            this.SetCommands();
        }

        #region Methods
        private void SetCommands()
        {
        }

        #endregion
        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                this.CalculateCategoriesCount();
            }
        }

        private void CalculateCategoriesCount()
        {
            var categories = this.DataService.GetAllCategories();
            this.IncomesCategoriesCount = categories.Where(cat => cat.TypeID == (int)Types.Income).ToList().Count;
            this.ExpensesCategoriesCount = categories.Where(cat => cat.TypeID == (int)Types.Expenses).ToList().Count;
        }

        public override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.ActionsService.ShowEmptyPage();
        }
    }
}
