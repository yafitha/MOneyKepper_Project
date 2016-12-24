using MoneyKepper_Core.Managers;
using MoneyKepper_Core.Models;
using MoneyKepper_Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.DesignTime.ViewModel
{
    public class GraphsViewModelDesgin : IGraphsViewModel
    {
        #region Bindable Members

        private ObservableCollection<CategoryItem> _categoryItems;
        public ObservableCollection<CategoryItem> CategoryItems
        {
            get { return _categoryItems; }
            set { _categoryItems = value; }
        }

        #endregion

        #region Ctor's
        public GraphsViewModelDesgin()
        {
            this.GetCategortiesTransactionsByDate();
        }

        private void GetCategortiesTransactionsByDate()
        {
            var categories = CacheManager.Instance.Categories.Values.ToList();
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

        #region Methods

        #endregion
    }
}
