using System;
using System.Collections.Generic;

namespace MarketData.Source.PriceSource
{
    public interface IMarketDataPriceSource
    {
        Fixings GetFixing(string ticker);
        IEnumerable<HistoricalFixing> GetHistoricalFixings(string ticker, DateTime dateFrom, DateTime dateTo);
    }
}