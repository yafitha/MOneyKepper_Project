using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MoneyKepper_Core.ViewModel.TransactionsViewModel;

namespace MoneyKepper_Core.Models
{
    public class ImageItem
    {
        public string Path { get; set; }
        public Types Type {get; set;}
    }
}
