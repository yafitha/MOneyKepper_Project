using GalaSoft.MvvmLight;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.Models
{
    public class CategoryModel : ObservableObject
    {
        private Category _category;
        public Category Category
        {
            get { return _category; }
            set { this.Set(ref _category, value); }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { this.Set(ref _name, value); }
        }

        private string _pictureName;
        public string PictureName
        {
            get { return _pictureName; }
            set { this.Set(ref _pictureName, value); }
        }
    }
}
