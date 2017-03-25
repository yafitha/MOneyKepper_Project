using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.Models
{
    public class BugetItem
    {
        public Buget Buget { get; set; }
        public Category Category { get; set; }

        public List<TransactionItem> Transactions { get; set; }

        public double UseMoney { get; set; }
        public double LeftMoney { get; set; }

        public BugetItem(Buget buget ,Category category)
        {
            this.Buget = buget;
            this.Category = category;
        }
    }
}
