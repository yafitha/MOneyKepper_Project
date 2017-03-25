using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class Buget
    {
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public int CategoryID { get; set; }
        public double Amount { get; set; }
        public string Note { get; set; }
        public bool IsActive { get; set; }

        public virtual Category Category { get; set; }
        public Buget()
        {

        }
    }
}
