using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MoneyKepperCore.Service
{
    public class CoreNavigationService : ICoreNavigationService
    {
        public event NavigatingCancelEventHandler Navigating;
        public event NavigatedEventHandler Navigated;

        public event EventHandler BackStackCleared;
        public event EventHandler FrameClosed;

        #region Members

        private bool IsInitialize { get; set; }

        public bool CanGoBack
        {
            get { return this.Frame.CanGoBack; }
        }

        private Frame _frame;
        private Frame Frame
        {
            get { return _frame; }
            set
            {
                if (_frame != value)
                {
                    var oldValue = _frame;
                    _frame = value;
                    this.OnFramePropertyChanged(oldValue, value);
                }
            }
        }

        private void OnFramePropertyChanged(Frame oldValue, Frame newValue)
        {
            if (oldValue != null)
            {
                oldValue.Navigating -= Frame_Navigating;
                oldValue.Navigated -= Frame_Navigated;
            }

            if (newValue != null)
            {
                newValue.Navigating += Frame_Navigating;
                newValue.Navigated += Frame_Navigated;
            }
        }

        private readonly Dictionary<string, Type> _pageIndex = new Dictionary<string, Type>();


        private Dictionary<string, Type> PageIndex
        {
            get { return _pageIndex; }
        }

        public string CurrentPageKey { get; private set; }

        #endregion

        #region Constructors

        public CoreNavigationService() { }

        #endregion

        #region Private Methods

        private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            this.Navigating?.Invoke(this, e);
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            this.Navigated?.Invoke(this, e);
        }

        #endregion

        #region Public Methods

        public void GoBack()
        {
            if (this.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        public void NavigateTo(string pageKey)
        {
            this.Frame.Navigate(this.PageIndex[pageKey]);
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            var samePage = this.PageIndex[pageKey] == this.Frame.CurrentSourcePageType;
            this.Frame.Navigate(this.PageIndex[pageKey], parameter);

            if (samePage)
            {
                if (this.Frame.BackStack != null && this.Frame.BackStack.Count > 0)
                {
                    this.Frame.BackStack.Remove(this.Frame.BackStack.Last());
                }
            }
        }

        public void Initialize(Frame frame, IFramePresenter framePresenter)
        {
            if (this.IsInitialize == true)
            {
                throw new Exception("Initialization can excute only once");
            }

            this.Frame = frame;
            if (framePresenter != null)
            {
                framePresenter.Closed += FramePresenter_Closed;
                framePresenter.ClearBackStackRequestRaised += FramePresenter_ClearBackStackRequestRaised;
            }

            this.IsInitialize = true;
        }

        private void FramePresenter_ClearBackStackRequestRaised(object sender, EventArgs e)
        {
            this.ClearBackStack();
        }

        private void FramePresenter_Closed(object sender, EventArgs e)
        {
            this.FrameClosed?.Invoke(this, null);
        }

        public void Configure(string key, Type pageType)
        {
            this.PageIndex.Add(key, pageType);
        }

        private void ClearBackStack()
        {
            this.Frame.BackStack.Clear();
            this.BackStackCleared?.Invoke(this, null);
        }

        public void ClearLastBackStackEntry()
        {
            this.Frame.BackStack.Remove(this.Frame.BackStack.LastOrDefault());
        }

        #endregion
    }

    public interface IFramePresenter
    {
        event EventHandler Closed;
        event EventHandler ClearBackStackRequestRaised;
    }
}
