using MoneyKepper_Core.Models;
using MoneyKepper_Core.Repositories;
using MoneyKepper2.View.Pages;
using MoneyKepper2.View.Views;
using MoneyKepperCore.Service;
using MoneyKepperCore.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MoneyKepper2
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private bool IsFirstLaunched { get; set; }
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            FactoriesRepository.InitializeDialogServiceFactory(new Factories.DialogServiceFactory());
            
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();

                if (this.IsFirstLaunched == false)
                {

                    GalaSoft.MvvmLight.Threading.DispatcherHelper.Initialize();
                    this.ConfigareNavigationPageKeys();

                    //  he-il: for hebraw
                    //  en-US: for english
                    this.SetLocalization("he-il");
                }

            }
        }

        private void ConfigareNavigationPageKeys()
        {

            ViewModelLocator locator = Application.Current.Resources["Locator"] as ViewModelLocator;
            ICoreNavigationService primaryNS = locator.NavigationService.PrimaryFrame;
            ICoreNavigationService detailsNS = locator.NavigationService.DetailsFrame;
            primaryNS.Configure(NavigationPageKeys.HOME_PAGE, typeof(TransactionsPage));
            primaryNS.Configure(NavigationPageKeys.HISTORY, typeof(View.Pages.HistoryPage));
            primaryNS.Configure(NavigationPageKeys.GRAPHS, typeof(GraphsPage));
            primaryNS.Configure(NavigationPageKeys.CATEGORY, typeof(CategoryPage));
            primaryNS.Configure(NavigationPageKeys.REPORT, typeof(ReportPage));

            detailsNS.Configure(NavigationPageKeys.TRANSACTION_DETAILS, typeof(View.Pages.TransactionsDetailsPage));
            detailsNS.Configure(NavigationPageKeys.HISTORY_Details, typeof(View.Pages.HistoryDetailsPage));
            detailsNS.Configure(NavigationPageKeys.EMPTY_PAGE, typeof(View.Pages.EmptyPage));
            detailsNS.Configure(NavigationPageKeys.GRAPHS_DETAILS, typeof(View.Pages.GraphsDetailsPage));
            detailsNS.Configure(NavigationPageKeys.CATEGORYDETAILS, typeof(View.Pages.CategoryDetailPage));
            detailsNS.Configure(NavigationPageKeys.REPORT_DETAILS, typeof(View.Pages.ReportDetailsPage));
        }

        private void SetLocalization(string v)
        {
            var culture = new CultureInfo(v);
            Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = culture.Name;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            //(Window.Current.Content as Frame).FlowDirection = Windows.UI.Xaml.FlowDirection.LeftToRight;
            //var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
        }
        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
