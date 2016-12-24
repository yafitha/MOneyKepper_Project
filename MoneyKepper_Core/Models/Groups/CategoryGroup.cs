using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.Models.Groups
{
    public class CategoryGroup
    {
        #region Members

        private Category _category;
        public Category Category
        {
            get { return _category; }
            set { _category = value; }
        }

        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get { return _categories; }
            set { _categories = value; }
        }

        #endregion

        #region Bindable Members

        #endregion

        #region Constructors

        public CategoryGroup()
        {
            this.Categories = new ObservableCollection<Category>();
        }

        #endregion
    }
}
