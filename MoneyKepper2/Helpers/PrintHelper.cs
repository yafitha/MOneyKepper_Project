
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Printing;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Printing;

namespace MoneyKepper2.Helpers
{
    public class PrintHelper
    {
        /// <summary>
        /// PrintDocument is used to prepare the pages for printing.
        /// Prepare the pages to print in the handlers for the Paginate, GetPreviewPage, and AddPages events.
        /// </summary>
        protected PrintDocument printDocument;

        /// <summary>
        /// Marker interface for document source
        /// </summary>
        protected IPrintDocumentSource printDocumentSource;

        /// <summary>
        /// A list of UIElements used to store the print preview pages.  This gives easy access
        /// to any desired preview page.
        /// </summary>
        internal List<UIElement> printPreviewPages;

        // Event callback which is called after print preview pages are generated.  Photos scenario uses this to do filtering of preview pages
        protected event EventHandler PreviewPagesCreated;

        /// <summary>
        /// First page in the printing-content series
        /// From this "virtual sized" paged content is split(text is flowing) to "printing pages"
        /// </summary>
        protected ListViewBase RootListView;

        /// <summary>
        ///  A reference back to the scenario page used to access XAML elements on the scenario page
        /// </summary>
        //protected Page scenarioPage;
        protected int lastIndex = 0;

        public string PrintDetails { get; private set; }
        public string PrintHeader { get; private set; }
        //public string ExtraInfo { get; private set; }
        public int AmountItemInPage { get; private set; }
        public int PageNumber { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scenarioPage">The scenario page constructing us</param>
        public PrintHelper(ListView uiListView, string printHeader, string printDetails, int amountItemInPage)
        {
            if (RootListView == null)
            {
                RootListView = new ListView();
                this.PrintHeader = printHeader;
                this.PrintDetails = printDetails;
                this.AmountItemInPage = amountItemInPage;
                this.PageNumber = 1;
                RootListView.ItemsSource = uiListView.ItemsSource;
                RootListView.ItemTemplate = uiListView.ItemTemplate;
                RootListView.ItemContainerStyle = uiListView.ItemContainerStyle;
                RootListView.ItemsPanel = uiListView.ItemsPanel;
                RootListView.Header = uiListView.Header;
                RootListView.HeaderTemplate = uiListView.HeaderTemplate;
                RootListView.FlowDirection = FlowDirection.RightToLeft;
            }
            printPreviewPages = new List<UIElement>();
        }

        /// <summary>
        /// This function registers the app for printing with Windows and sets up the necessary event handlers for the print process.
        /// </summary>
        public virtual void RegisterForPrinting()
        {
            printDocument = new PrintDocument();
            printDocumentSource = printDocument.DocumentSource;
            printDocument.Paginate += CreatePrintPreviewPages;
            printDocument.GetPreviewPage += GetPrintPreviewPage;
            printDocument.AddPages += AddPrintPages;
            PrintManager printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested += PrintTaskRequested;
        }

        /// <summary>
        /// This function unregisters the app for printing with Windows.
        /// </summary>
        public virtual void UnregisterForPrinting()
        {
            if (printDocument == null)
            {
                return;
            }
            // Clear the cache of preview pages
            printPreviewPages.Clear();
            printDocument.Paginate -= CreatePrintPreviewPages;
            printDocument.GetPreviewPage -= GetPrintPreviewPage;
            printDocument.AddPages -= AddPrintPages;

            // Remove the handler for printing initialization.
            PrintManager printMan = PrintManager.GetForCurrentView();
            printMan.PrintTaskRequested -= PrintTaskRequested;
        }

        public async Task ShowPrintUIAsync()
        {
            // Catch and print out any errors reported
            try
            {
                await PrintManager.ShowPrintUIAsync();
            }
            catch (Exception e)
            {
                //MainPage.Current.NotifyUser("Error printing: " + e.Message + ", hr=" + e.HResult, NotifyType.ErrorMessage);
            }
        }

        /// <summary>
        /// This is the event handler for PrintManager.PrintTaskRequested.
        /// </summary>
        /// <param name="sender">PrintManager</param>
        /// <param name="e">PrintTaskRequestedEventArgs </param>
        protected virtual void PrintTaskRequested(PrintManager sender, PrintTaskRequestedEventArgs e)
        {
            PrintTask printTask = null;
            printTask = e.Request.CreatePrintTask("המארחת", sourceRequested =>
            {
                // Print Task event handler is invoked when the print job is completed.
                printTask.Completed += async (s, args) =>
                {
                    //// Notify the user when the print operation fails.
                    //if (args.Completion == PrintTaskCompletion.Failed)
                    //{
                    //    await scenarioPage.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    //    {
                    //        MainPage.Current.NotifyUser("Failed to print.", NotifyType.ErrorMessage);
                    //    });
                    //}
                };
                sourceRequested.SetSource(printDocumentSource);
            });
        }

        /// <summary>
        /// This is the event handler for PrintDocument.Paginate. It creates print preview pages for the app.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Paginate Event Arguments</param>
        protected virtual void CreatePrintPreviewPages(object sender, PaginateEventArgs e)
        {
            // Clear the cache of preview pages
            if (printPreviewPages != null && printPreviewPages.Count > 0)
            {
                printPreviewPages.Clear();
                this.lastIndex = 0;
                this.PageNumber = 1;
            }

            // This variable keeps track of the last element that was added to a page which will be printed
            ListViewBase lastRTBOOnPage;

            // Get the PrintTaskOptions
            PrintTaskOptions printingOptions = ((PrintTaskOptions)e.PrintTaskOptions);

            // Get the page description to deterimine how big the page is
            PrintPageDescription pageDescription = printingOptions.GetPageDescription(0);

            // We know there is at least one page to be printed. passing null as the first parameter to
            // AddOnePrintPreviewPage tells the function to add the first page.
            lastRTBOOnPage = AddOnePrintPreviewPage(null, pageDescription);

            // We know there are more pages to be added as long as the last listview added to a print preview
            // page has extra content
            while (lastRTBOOnPage.Items.Count > 0)
            {
                this.PageNumber++;
                lastRTBOOnPage = AddOnePrintPreviewPage(lastRTBOOnPage, pageDescription);

            }

            if (PreviewPagesCreated != null)
            {
                PreviewPagesCreated.Invoke(printPreviewPages, null);
            }
            //this.printPreviewPages.Add(listView);
            PrintDocument printDoc = (PrintDocument)sender;

            // Report the number of preview pages created
            printDoc.SetPreviewPageCount(printPreviewPages.Count, PreviewPageCountType.Intermediate);
        }

        /// <summary>
        /// This is the event handler for PrintDocument.GetPrintPreviewPage. It provides a specific print preview page,
        /// in the form of an UIElement, to an instance of PrintDocument. PrintDocument subsequently converts the UIElement
        /// into a page that the Windows print system can deal with.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Arguments containing the preview requested page</param>
        protected virtual void GetPrintPreviewPage(object sender, GetPreviewPageEventArgs e)
        {
            PrintDocument printDoc = (PrintDocument)sender;
            printDoc.SetPreviewPage(e.PageNumber, printPreviewPages[e.PageNumber - 1]);
        }

        /// <summary>
        /// This is the event handler for PrintDocument.AddPages. It provides all pages to be printed, in the form of
        /// UIElements, to an instance of PrintDocument. PrintDocument subsequently converts the UIElements
        /// into a pages that the Windows print system can deal with.
        /// </summary>
        /// <param name="sender">PrintDocument</param>
        /// <param name="e">Add page event arguments containing a print task options reference</param>
        protected virtual void AddPrintPages(object sender, AddPagesEventArgs e)
        {
            //// Loop over all of the preview pages and add each one to  add each page to be printied
            for (int i = 0; i < printPreviewPages.Count; i++)
            {
                // We should have all pages ready at this point...
                printDocument.AddPage(printPreviewPages[i]);
            }
            PrintDocument printDoc = (PrintDocument)sender;

            // Indicate that all of the print pages have been provided
            printDoc.AddPagesComplete();
        }

        /// <summary>
        /// This function creates and adds one print preview page to the internal cache of print preview
        /// pages stored in printPreviewPages.
        /// </summary>
        /// <param name="lastListView">Last RichTextBlockOverflow element added in the current content</param>
        /// <param name="printPageDescription">Printer's page description</param>
        protected virtual ListViewBase AddOnePrintPreviewPage(ListViewBase lastListView, PrintPageDescription printPageDescription)
        {
            // XAML element that is used to represent to "printing page"
            ListViewBase listview = new ListView();

            // The link container for listview overflowing
            ListViewBase restListView = new ListView();

            // If this is the first page add the specific scenario content
            if (lastListView == null)
            {
                if (RootListView.Items.Count > (AmountItemInPage - 2))
                {
                    for (int i = 0; i < (AmountItemInPage - 2); i++)
                    {
                        listview.Items.Add(RootListView.Items[i]);
                    }
                }
                else
                {
                    listview = RootListView;
                }

                this.lastIndex = this.lastIndex + AmountItemInPage - 2;
            }
            else if (lastListView.Items.Count > 0)
            {
                if (lastListView.Items.Count > this.AmountItemInPage)
                {
                    for (int i = 0; i < this.AmountItemInPage; i++)
                    {
                        listview.Items.Add(lastListView.Items[i]);
                    }
                }
                else
                {
                    listview = lastListView;
                }

                this.lastIndex = this.lastIndex + AmountItemInPage;
            }

            this.SetListViewStyle(listview, printPageDescription);

            // fill the rest of the listview in a new list
            for (int i = lastIndex; i < RootListView.Items.Count; i++)
            {
                restListView.Items.Add(RootListView.Items[i]);
            }

            //first page -set header and sub title
            if (lastListView == null)
            {
                StackPanel stackPanel = new StackPanel();
                this.SetHeaderAndPrintDetailsText(listview);
            }
            else
            {
                StackPanel stackPanel = new StackPanel();
                this.SetStackPanelStyle(stackPanel);
                stackPanel.Children.Add(listview);
                this.SetPageNumber(stackPanel);
                // Add the page to the page preview collection
                printPreviewPages.Add(stackPanel);
            }

            return restListView;
        }

        private void SetHeaderAndPrintDetailsText(ListViewBase listview)
        {
            try
            {
                StackPanel stackPanel = new StackPanel();
                this.SetStackPanelStyle(stackPanel);
                TextBlock headerTextBlock = new TextBlock();
                headerTextBlock.SetValue(TextBlock.FontWeightProperty, FontWeights.Bold);
                headerTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
                this.SetTextBlockStyle(headerTextBlock, this.PrintHeader, 24, 70, new Thickness(20, 0, 0, 0));
                stackPanel.Children.Add(headerTextBlock);
                TextBlock reportDetasilsTextBlock = new TextBlock();
                this.SetTextBlockStyle(reportDetasilsTextBlock, this.PrintDetails, 18, reportDetasilsTextBlock.Height, new Thickness(10, 0, 10, 10));
                reportDetasilsTextBlock.SetValue(TextBlock.FontWeightProperty, FontWeights.SemiBold);
                stackPanel.Children.Add(reportDetasilsTextBlock);
                //if (!string.IsNullOrEmpty(this.ExtraInfo))
                //{
                //    TextBlock extraInfoTextBlock = new TextBlock();
                //    this.SetTextBlockStyle(extraInfoTextBlock, this.ExtraInfo, 18, 40, new Thickness(10, 0, 0, 0));
                //    stackPanel.Children.Add(extraInfoTextBlock);
                //}
                stackPanel.Children.Add(listview);
                printPreviewPages.Add(stackPanel);
            }
            catch (Exception ex)
            {

            }
        }

        private void SetTextBlockStyle(TextBlock textBlock, string text, int fontSize, double height, Thickness thickness)
        {
            textBlock.FontSize = fontSize;
            textBlock.FlowDirection = FlowDirection.RightToLeft;
            textBlock.Text = text;
            textBlock.Padding = thickness;
            textBlock.Height = height;
        }

        private void SetStackPanelStyle(StackPanel stackPanel)
        {
            stackPanel.Padding = new Thickness(20, 50, 20, 50);
            this.SetPageNumber(stackPanel);
        }

        private void SetPageNumber(StackPanel stackPanel)
        {
            TextBlock pageNumberTextBlock = new TextBlock();
            this.SetTextBlockStyle(pageNumberTextBlock, this.PageNumber.ToString(), 15, 50, new Thickness(10, 0, 0, 0));
            pageNumberTextBlock.HorizontalAlignment = HorizontalAlignment.Center;
            stackPanel.Children.Add(pageNumberTextBlock);
        }

        private void SetListViewStyle(ListViewBase listView, PrintPageDescription printPageDescription)
        {
            listView.ItemTemplate = RootListView.ItemTemplate;
            listView.ItemContainerStyle = RootListView.ItemContainerStyle;
            listView.ItemsPanel = RootListView.ItemsPanel;
            listView.Header = RootListView.Header;
            listView.HeaderTemplate = RootListView.HeaderTemplate;
            listView.FlowDirection = FlowDirection.RightToLeft;
            listView.HorizontalAlignment = HorizontalAlignment.Center;
            // Set "paper" width
            listView.Width = printPageDescription.PageSize.Width * 0.95;
            listView.Height = printPageDescription.PageSize.Height * 0.95;
            listView.UpdateLayout();
        }
    }
}
