using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketData.Source.PriceSource;

namespace MarketDataSourceConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var yahooDataSource = new YahooMarketDataPriceSource();
            var fixings = yahooDataSource.GetHistoricalFixings("MSFT", new DateTime(2016,03,22),new DateTime(2016,06,17) );
        }
    }
}
