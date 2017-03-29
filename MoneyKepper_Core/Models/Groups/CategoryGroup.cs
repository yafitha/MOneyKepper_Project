using GalaSoft.MvvmLight;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.Models.Groups
{
    public class CategoryGroup : ObservableObject
    {
        #region Members

        private CategoryModel _CategoryModel;
        public CategoryModel CategoryModel
        {
            get { return _CategoryModel; }
            set { this.Set(ref _CategoryModel,value); }
        }

        private ObservableCollection<CategoryModel> _categories;
        public ObservableCollection<CategoryModel> Categories
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
            this.Categories = new ObservableCollection<CategoryModel>();
        }

        #endregion
    }
}
