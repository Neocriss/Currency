using System.Collections.Generic;
using System.Threading.Tasks;


namespace Banks.Cash
{
    public interface IFinancialInfoProvider
    {
        Task<IEnumerable<CurrencyPair>> GetActualCurrencyPairsAsync(string[] names);
    }
}
