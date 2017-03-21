using GalaSoft.MvvmLight;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.Models
{
    public class CategoryItem : ObservableObject
    {
        private Category _category;
        public Category Category
        {
            get { return _category; }
            set { this.Set(ref _category, value); }
        }

        private Category _categoryName;
        public string CategoryName
        {
            get { return Category.Name; }
        }

        private string _month;
        public string Month
        {
            get { return _month; }
            set { this.Set(ref _month, value); }
        }

        private DateTime _monthDate;
        public DateTime MonthDate
        {
            get { return _monthDate; }
            set { this.Set(ref _monthDate, value); }
        }

        private double _amount;
        public double Amount
        {
            get { return _amount; }
            set { this.Set(ref _amount, value); }
        }
        public CategoryItem(Category category,DateTime monthDate)
        {
            this.Category = category;
            this.Month = monthDate.ToString("MMMM");
            this.MonthDate = monthDate;
        }
    }
}
