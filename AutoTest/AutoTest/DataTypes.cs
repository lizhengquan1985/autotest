using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTest
{
    #region 辅助函数

    public class Utils
    {
        public static DateTime GetDateById(long id)
        {
            return new DateTime(id * 10000000 + new DateTime(1970, 1, 1, 8, 0, 0).Ticks);
        }
    }

    #endregion

    #region 基础数据
    public class ResponseMerged
    {
        public string status { get; set; }
        public long ts { get; set; }
        public string ch { get; set; }
        public MergedTick tick { get; set; }
    }

    public class MergedTick
    {
        public long id { get; set; }
        public decimal amount { get; set; }
        public decimal open { get; set; }
        public decimal close { get; set; }
        public decimal high { get; set; }
        public decimal count { get; set; }
        public decimal low { get; set; }
        public long version { get; set; }
        public List<decimal> ask { get; set; }
        public decimal vol { get; set; }
        public List<decimal> bid { get; set; }
    }

    public class ResponseKline
    {
        public string status { get; set; }
        public string ch { get; set; }
        public string ts { get; set; }
        public List<KlineData> data { get; set; }
    }

    public class KlineData
    {
        public long id { get; set; }
        public decimal amount { get; set; }
        public decimal count { get; set; }
        public decimal open { get; set; }
        public decimal close { get; set; }
        public decimal low { get; set; }
        public decimal high { get; set; }
        public decimal vol { get; set; }
    }

    public class ResponseDepth
    {
        public string status { get; set; }
        public long ts { get; set; }
        public string ch { get; set; }
        public DepthTick tick { get; set; }
    }

    public class DepthTick
    {
        public List<List<long>> bids { get; set; }
        public List<List<long>> asks { get; set; }
        public long ts { get; set; }
        public long version { get; set; }
    }

    public class ResponseTrade
    {
        public string status { get; set; }
        public long ts { get; set; }
        public string ch { get; set; }
        public TradeTick tick { get; set; }
    }

    public class TradeTick
    {
        public long id { get; set; }
        public long ts { get; set; }
        public List<TradeData> data { get; set; }
    }

    public class ResponseHistoryTrade
    {
        public string status { get; set; }
        public long ts { get; set; }
        public string ch { get; set; }
        public List<HistoryTradeTick> data { get; set; }
    }

    public class HistoryTradeTick
    {
        public long id { get; set; }
        public long ts { get; set; }
        public List<TradeData> data { get; set; }
    }

    public class TradeData
    {
        public string id { get; set; }
        public decimal price { get; set; }
        public decimal amount { get; set; }
        public string direction { get; set; }
        public long ts { get; set; }
    }


    public class ResponseDetail
    {
        public string status { get; set; }
        public long ts { get; set; }
        public string ch { get; set; }
        public DetailTick tick { get; set; }
    }

    public class DetailTick
    {
        public long id { get; set; }
        public long ts { get; set; }
        public decimal amount { get; set; }
        public decimal open { get; set; }
        public decimal close { get; set; }
        public decimal high { get; set; }
        public decimal low { get; set; }
        public decimal vol { get; set; }
        public long count { get; set; }
    }

    #endregion

    #region 分析数据

    public class FlexPoint
    {
        public bool isHigh { get; set; }
        public decimal open { get; set; }
        public long id { get; set; }
    }

    #endregion
}
