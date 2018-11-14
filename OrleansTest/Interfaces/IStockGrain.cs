using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Interfaces
{
    public interface IStockGrain : Orleans.IGrainWithStringKey
    {
        Task<string> GetPrice();
    }
}
