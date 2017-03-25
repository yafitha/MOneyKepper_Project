using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyKepper2.Behaviors
{
    public class PrintPageBehavior : DependencyObject, IBehavior
    {
        /// <summary>
        /// A boolean which tracks whether the app has been registered for printing
        /// </summary>
        private bool isRegisteredForPrinting = false;

        private Helpers.PrintHelper printHelper;
        public FrameworkElement FrameworkElement
        {
            get; set;
        }

        public DependencyObject AssociatedObject { get; set; }

        public int AmountItemInPage
        {
            get { return (int)GetValue(AmountItemInPageProperty); }
            set { SetValue(AmountItemInPageProperty, value); }
        }

        public static readonly DependencyProperty AmountItemInPageProperty =
            DependencyProperty.Register("AmountItemInPage", typeof(int), typeof(PrintPageBehavior), new PropertyMetadata(null));


        public ListView RootListView
        {
            get { return (ListView)GetValue(RootListViewProperty); }
            set { SetValue(RootListViewProperty, value); }
        }

        public static readonly DependencyProperty RootListViewProperty =
            DependencyProperty.Register("RootListView", typeof(ListView), typeof(PrintPageBehavior), new PropertyMetadata(null));

        public string ReportHeader
        {
            get { return (string)GetValue(ReportHeaderProperty); }
            set { SetValue(ReportHeaderProperty, value); }
        }

        public static readonly DependencyProperty ReportHeaderProperty =
            DependencyProperty.Register("ReportHeader", typeof(string), typeof(PrintPageBehavior), new PropertyMetadata(null));

        public string ReportInfo
        {
            get { return (string)GetValue(ReportInfoProperty); }
            set { SetValue(ReportInfoProperty, value); }
        }

        public static readonly DependencyProperty ReportInfoProperty =
            DependencyProperty.Register("ReportInfo", typeof(string), typeof(PrintPageBehavior), new PropertyMetadata(null));

        public string ExtraInfo
        {
            get { return (string)GetValue(ExtraInfoProperty); }
            set { SetValue(ExtraInfoProperty, value); }
        }

        public static readonly DependencyProperty ExtraInfoProperty =
            DependencyProperty.Register("ExtraInfo", typeof(string), typeof(PrintPageBehavior), new PropertyMetadata(string.Empty));

        public void Attach(DependencyObject associatedObject)
        {
            this.FrameworkElement = (FrameworkElement)associatedObject;
            this.FrameworkElement.Tapped += AssociatedObject_Tapped;
            this.FrameworkElement.Unloaded += FrameworkElement_Unloaded;
        }

        private void FrameworkElement_Unloaded(object sender, RoutedEventArgs e)
        {
            // Unregister for printing 
            if (this.printHelper != null)
            {
                printHelper.UnregisterForPrinting();
            }
        }

        private async void AssociatedObject_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (this.printHelper == null)
            {
                this.printHelper = new Helpers.PrintHelper(this.RootListView, this.ReportHeader, this.ReportInfo,this.ExtraInfo, this.AmountItemInPage);
                // Register for printing               
                printHelper.RegisterForPrinting();
            }
            await printHelper.ShowPrintUIAsync();
        }

        public void Detach()
        {
            this.FrameworkElement.Tapped -= AssociatedObject_Tapped;
            this.FrameworkElement.Tapped -= AssociatedObject_Tapped;

            if (printHelper != null)
            {
                printHelper.UnregisterForPrinting();
            }
        }
    }
}
