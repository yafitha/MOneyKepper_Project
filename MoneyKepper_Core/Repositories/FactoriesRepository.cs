using MoneyKepper_Core.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.Repositories
{
    public class FactoriesRepository
    {
        public static IDialogServiceFactory DialogServiceFactory { get; private set; }

        public static void InitializeDialogServiceFactory(IDialogServiceFactory dialogServiceFactory)
        {
            DialogServiceFactory = dialogServiceFactory;
        }
    }
}
