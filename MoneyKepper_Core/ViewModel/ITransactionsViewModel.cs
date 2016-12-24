using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.ViewModel
{
    public interface ITransactionsViewModel
    {
        double Income { get; }
        double Expenses { get;}
        double Balance { get; }
    }
}
