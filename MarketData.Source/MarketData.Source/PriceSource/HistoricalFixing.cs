using System;

namespace MarketData.Source.PriceSource
{
    public class HistoricalFixing
    {
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal Volume { get; set; }
        public decimal AdjClose { get; set; }
    }
}