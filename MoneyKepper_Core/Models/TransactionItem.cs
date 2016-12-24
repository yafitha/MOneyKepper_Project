using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.Models
{
    public class TransactionItem
    {
        public Transaction Transaction { get; set; }
        public Category Category { get; set; }

        public TransactionItem(Transaction transaction , Category category)
        {
            this.Transaction = transaction;
            this.Category = category;
        }
    }
}
