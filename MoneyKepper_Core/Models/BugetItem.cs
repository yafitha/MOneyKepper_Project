using GalaSoft.MvvmLight;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.Models
{
    public class BugetItem : ObservableObject
    {
        public Buget Buget { get; set; }
        public Category Category { get; set; }

        public List<TransactionItem> Transactions { get; set; }


        private double _amount;
        public double Amount
        {
            get { return _amount; }
            set { this.Set(ref _amount, value); }
        }

        private string _note;
        public string Note
        {
            get { return _note; }
            set { this.Set(ref _note, value); }
        }


        public double UseMoney { get; set; }


        private double _leftMoney;
        public double LeftMoney
        {
            get { return _leftMoney; }
            set { this.Set(ref _leftMoney, value); }
        }

        public BugetItem(Buget buget ,Category category)
        {
            this.Buget = buget;
            this.Category = category;
            this.Amount = this.Buget.Amount;
            this.Note = this.Buget.Note;
        }
    }
}
