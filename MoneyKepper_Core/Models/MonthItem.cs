using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.Models
{
    public class MonthItem
    {
        public double Expenses { get; set; }
        public double Income { get; set; }
        public string Month { get; set; }
        public DateTime MonthDate { get; set; }

        public MonthItem(double expenses, double income , DateTime monthDate)
        {
            this.Expenses = expenses;
            this.Income = income;
            this.Month = monthDate.ToString("MMMM");
            this.MonthDate = monthDate;
        }
    }
}
