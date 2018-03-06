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
        public TickData tick { get; set; }
    }

    public class TickData
    {
        public decimal close { get; set; }
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
