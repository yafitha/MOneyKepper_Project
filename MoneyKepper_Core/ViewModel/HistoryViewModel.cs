using GalaSoft.MvvmLight.Command;
using MoneyKepper_Core.Managers;
using MoneyKepper_Core.Models;
using Models;
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
    public class HistoryViewModel : NavigableViewModel, IHistoryViewModel
    {
        #region Members

        private IDataService DataServcie { get; set; }
        private IActionsService ActionsService { get; set; }

        #endregion

        #region Bindable Proerty

        private DateTimeOffset? _startDate;
        public DateTimeOffset? StartDate
        {
            get { return _startDate; }
            set
            {
                this.Set(ref _startDate, value);
                if (value > this.EndDate)
                {
                    this.EndDate = value;
                }
            }
        }

        private DateTimeOffset? _endDate;
        public DateTimeOffset? EndDate
        {
            get { return _endDate; }
            set { this.Set(ref _endDate, value); }
        }

        private List<Graph> _graphTypes;
        public List<Graph> GraphTypes
        {
            get { return _graphTypes; }
            set { this.Set(ref _graphTypes, value); }
        }

        private Graph _selectedGraph;
        public Graph SelectedGraph
        {
            get { return _selectedGraph; }
            set
            {
                this.Set(ref _selectedGraph, value);
                this.IsCategoryGraphSelected = _selectedGraph == Graph.CategoriesMonthColumns ? true : false;
            }
        }

        private ObservableCollection<Category> _categotries;
        public ObservableCollection<Category> Categotries
        {
            get { return _categotries; }
            set { this.Set(ref _categotries,value); }
        }

        private bool _isMaxCategoryShow;
        public bool IsMaxCategoryShow
        {
            get { return _isMaxCategoryShow; }
            set { this.Set(ref _isMaxCategoryShow, value); }
        }

        private bool _isCategoryGraphSelected;
        public bool IsCategoryGraphSelected
        {
            get { return _isCategoryGraphSelected; }
            set { this.Set(ref _isCategoryGraphSelected, value); }
        }
        
        private List<Category> _selectedCategories;
        public List<Category> SelectedCategories
        {
            get { return _selectedCategories; }
            set
            {
                this.Set(ref _selectedCategories, value);
            }
        }

        #endregion

        #region Commands

        public RelayCommand ShowGraphCommmand { get; private set; }
        public RelayCommand<IList<object>> SelectionChangedCommand { get; private set; }

        #endregion

        #region Ctor's
        public HistoryViewModel(IDataService dataService, IActionsService actionsService)
        {
            this.DataServcie = dataService;
            this.ActionsService = actionsService;
            this.SetCommands();
        }

        #endregion

        #region Methods
        private void SetCommands()
        {
            this.ShowGraphCommmand = new RelayCommand(OnShowGraphCommmand);
            this.SelectionChangedCommand = new RelayCommand<IList<object>>(OnSelectionChangedCommand);
        }

        private void OnSelectionChangedCommand(IList<object> obj)
        {
            this.SelectedCategories = obj.OfType<Category>().ToList();
            if (this.SelectedCategories == null)
            {
                IsMaxCategoryShow = false;
                return;
            }
            IsMaxCategoryShow = this.SelectedCategories.Count > 6 ? true : false;
        }

        private void OnShowGraphCommmand()
        {
            var endDate = this.EndDate == null ? this.StartDate.Value.Date : this.EndDate.Value.Date;
            this.ActionsService.ShowHistoryGraphs(this.StartDate.Value.Date, endDate, this.SelectedCategories,this.SelectedGraph);
        }

        #endregion

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                this.SetGraphsTypes();
                this.Categotries = new ObservableCollection<Category>(CacheManager.Instance.Categories.Values.ToList());
                this.StartDate = DateTime.Today;
                this.EndDate = null;
            }
        }

        private void SetGraphsTypes()
        {
            if (this.GraphTypes == null)
            {
                this.GraphTypes = new List<Graph>();
            }
            this.GraphTypes.Add(Graph.CategoriesMonthColumns);
            this.GraphTypes.Add(Graph.IncomeExpenses);
            this.SelectedGraph = Graph.IncomeExpenses;
        }

        public override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.GraphTypes.Clear();
            this.ActionsService.ShowEmptyPage();
        }
    }

    public enum Graph
    {
        pie,
        CategoriesColumns,
        CategoriesMonthColumns,
        IncomeExpenses
    }
}
