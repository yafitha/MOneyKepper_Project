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

        private DateTimeOffset? _startDate;
        public DateTimeOffset? StartDate
        {
            get { return _startDate; }
            set
            {
                this.Set(ref _startDate, value);
                this.MaxDate = (new DateTime(_startDate.Value.DateTime.Year, _startDate.Value.DateTime.Month, 1)).AddDays(-1);
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

        private DateTimeOffset? _maxDate;
        public DateTimeOffset? MaxDate
        {
            get { return _maxDate; }
            set { this.Set(ref _maxDate, value); }
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
            set { this.Set(ref _selectedGraph, value); }
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
            var endDate = this.EndDate == null ? this.StartDate.Value.Date : this.EndDate.Value.Date;
            this.ActionsService.ShowMonthGraphs(this.StartDate.Value.Date, endDate, this.SelectedGraph);
        }

        private void SetGraphsTypes()
        {
            if (this.GraphTypes == null)
            {
                this.GraphTypes = new List<Graph>();
            }
            this.GraphTypes.Clear();
            this.GraphTypes.Add(Graph.pie);
            this.GraphTypes.Add(Graph.CategoriesColumns);
            this.SelectedGraph = Graph.pie;
        }
        #endregion

        public override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode == NavigationMode.New)
            {
                this.SetGraphsTypes();
                this.StartDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                this.MaxDate = StartDate.Value.DateTime.AddMonths(1).AddDays(-1);
                this.EndDate = null;
            }
        }
        public override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.ActionsService.ShowEmptyPage();
        }
    }
}
