﻿using GalaSoft.MvvmLight.Command;
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

namespace MoneyKepper_Core.ViewModel
{
    class GraphsDetailsViewModel : NavigableViewModel, IGraphsDetailsViewModel
    {
        #region Members
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

        private bool _showPieGraph;
        public bool ShowPieGraph
        {
            get { return _showPieGraph; }
            set { this.Set(ref _showPieGraph, value); }
        }

        #endregion

        #region Commands

        public RelayCommand ShowGraphCommmand { get; private set; }
        public IDataService DataService { get; private set; }
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
        }

        private void ShowGraph()
        {
            var categories = this.DataService.GetAllCategories();
            this.CategoryItems = new ObservableCollection<CategoryItem>();
            Random r = new Random(123345);
            string month;
            month = DateTime.Now.ToString("MMMM");
            for (int j = 0; j < categories.Count; j++)
            {
                var categoryItem = new CategoryItem(categories[j], month, (3000 * r.Next(1, 12)));
                CategoryItems.Add(categoryItem);
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
                this.ShowPieGraph = this.GraphType == Graph.pie ? true : false;
                ShowGraph();
            }
        }

        public override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.CategoryItems.Clear();
        }
    }
}
