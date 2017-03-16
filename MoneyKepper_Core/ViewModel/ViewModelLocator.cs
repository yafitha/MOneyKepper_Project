using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MoneyKepper_Core.DesignTime.ViewModel;
using MoneyKepper_Core.Managers;
using MoneyKepper_Core.Repositories;
using MoneyKepper_Core.Services;
using MoneyKepper_Core.ViewModel;
using MoneyKepper2.Service;
using MoneyKepperCore.DesignTime.ViewModel;
using MoneyKepperCore.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepperCore.ViewModel
{
    public class ViewModelLocator
    {
        #region Properties

        public MainViewModel MainViewModel
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public IAddTransactionViewModel AddTransactionViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IAddTransactionViewModel>(); }
        }
        public IControlSplitViewModel ControlSplitViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IControlSplitViewModel>(); }
        }

        public IConfirmViewModel ConfirmViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IConfirmViewModel>(); }
        }


        public IGraphsViewModel GraphsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IGraphsViewModel>(); }
        }

        public IHistoryViewModel HistoryViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IHistoryViewModel>(); }
        }

        public IReportViewModel ReportViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IReportViewModel>(); }
        }

        public IReportDetailsVewModel ReportDetailsVewModel
        {
            get { return ServiceLocator.Current.GetInstance<IReportDetailsVewModel>(); }
        }

        public ITransactionsDetailsViewModel TransactionsDetailsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ITransactionsDetailsViewModel>(); }
        }

        public IControlPanelViewModel ControlPanelViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IControlPanelViewModel>(); }
        }
        public IPrimaryPanelViewModel PrimaryPanelViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IPrimaryPanelViewModel>(); }
        }
        public ITransactionsViewModel TransactionsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ITransactionsViewModel>(); }
        }
        public IDetailsViewModel DetailsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IDetailsViewModel>(); }
        }
        public IHistoryDetailsViewModel HistoryDetailsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IHistoryDetailsViewModel>(); }
        }
        public IGraphsDetailsViewModel GraphsDetailsViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IGraphsDetailsViewModel>(); }
        }

        public ICategoryDetailViewModel CategoryDetailViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ICategoryDetailViewModel>(); }
        }
        public ICategoryViewModel CategoryViewModel
        {
            get { return ServiceLocator.Current.GetInstance<ICategoryViewModel>(); }
        }
        public IAddNewCategoryViewModel AddNewCategoryViewModel
        {
            get { return ServiceLocator.Current.GetInstance<IAddNewCategoryViewModel>(); }
        }

        public INavigationService NavigationService
        {
            get { return ServiceLocator.Current.GetInstance<INavigationService>(); }
        }

        public IDialogService DialogService
        {
            get { return ServiceLocator.Current.GetInstance<IDialogService>(); }
        }

        #endregion

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            if (ViewModelBase.IsInDesignModeStatic)
            {
                SimpleIoc.Default.Register<ITransactionsViewModel, TransactionsViewModelDesign>();
                SimpleIoc.Default.Register<IGraphsViewModel, GraphsViewModelDesgin>();
                SimpleIoc.Default.Register<IHistoryViewModel, HistoryViewModelDesign>();
                SimpleIoc.Default.Register<ITransactionsDetailsViewModel, TransactionsDetailsViewModelDesign>();
            }
            else
            {
                SimpleIoc.Default.Register<IDataService, DataService>();
                SimpleIoc.Default.Register<IActionsService, ActionService>();
                SimpleIoc.Default.Register(() => FactoriesRepository.DialogServiceFactory.GetInstance());
                SimpleIoc.Default.Register<INavigationService, NavigationService>();
                SimpleIoc.Default.Register<IControlSplitViewModel, ControlSplitViewModel>();
                SimpleIoc.Default.Register<ICategoryViewModel, CategoryViewModel>();
                SimpleIoc.Default.Register<ICategoryDetailViewModel, CategoryDetailViewModel>();
                SimpleIoc.Default.Register<IControlPanelViewModel, ControlPanelViewModel>();
                SimpleIoc.Default.Register<IDetailsViewModel, DetailsViewModel>();
                SimpleIoc.Default.Register<IPrimaryPanelViewModel, PrimaryPanelViewModel>();
                SimpleIoc.Default.Register<IGraphsDetailsViewModel, GraphsDetailsViewModel>();
                SimpleIoc.Default.Register<ITransactionsViewModel, TransactionsViewModel>();
                SimpleIoc.Default.Register<IAddNewCategoryViewModel, AddNewCategoryViewModel>();
                SimpleIoc.Default.Register<ITransactionsDetailsViewModel, TransactionsDetailsViewModel>();
                SimpleIoc.Default.Register<IAddTransactionViewModel, AddTransactionViewModel>();
                SimpleIoc.Default.Register<IGraphsViewModel, GraphsViewModel>();
                SimpleIoc.Default.Register<IHistoryViewModel, HistoryViewModel>();
                SimpleIoc.Default.Register<IConfirmViewModel, ConfirmViewModel>();
                SimpleIoc.Default.Register<IReportViewModel, ReportViewModel>();
                SimpleIoc.Default.Register<IReportDetailsVewModel, ReportDetailsViewModel>();
                SimpleIoc.Default.Register<IHistoryDetailsViewModel, HistoryDetailsViewModel>();
            }
            SimpleIoc.Default.Register<MainViewModel>();
        }
    }
}
