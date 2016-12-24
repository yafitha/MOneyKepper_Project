using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyKepper_Core.Managers
{
    public interface ICacheManager
    {
        Dictionary<int, Category> Categories { get; }
    }
}
