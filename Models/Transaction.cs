using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class Transaction
    {
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public int CategoryID { get; set; }
        public byte[] IsActive { get; set; }
        public string Note { get; set; }
        public double Amount { get; set; }

        public virtual Category Category { get; set; }
    }
}
