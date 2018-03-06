using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTest
{
    /// <summary>
    /// 分析
    /// </summary>
    public class CoinAnalyze
    {
        private static string toCoin = "usdt";

        /// <summary>
        /// 分析价位走势
        /// </summary>
        /// <param name="coin"></param>
        /// <param name="lastLow">最近一次最低的价格</param>
        /// <param name="nowOpen"></param>
        /// <param name="min"></param>
        /// <returns></returns>
        public static List<FlexPoint> Analyze(string coin, out decimal lastLow, out decimal nowOpen, double percentAuto = 1.005, string min = "1min")
        {
            nowOpen = 0;
            lastLow = 999999999;

            decimal smallest = 9999999;
            decimal biggest = 0;

            try
            {
                ResponseKline res = new AnaylyzeApi().kline(coin + toCoin, min, 1440);
                Console.WriteLine($"总数：{res.data.Count}");
                Console.WriteLine(Utils.GetDateById(res.data[0].id));
                Console.WriteLine(Utils.GetDateById(res.data[res.data.Count - 1].id));

                nowOpen = res.data[0].open;

                List<FlexPoint> flexPointList = new List<FlexPoint>();

                decimal openHigh = res.data[0].open;
                decimal openLow = res.data[0].open;
                long idHigh = res.data[0].id;
                long idLow = res.data[0].id;
                //DateTime dtHight = Utils.GetDateById(idHigh);
                //DateTime dtLow = Utils.GetDateById(idLow);
                int lastHighOrLow = 0; // 1 high, -1: low
                for (var i = 1; i < res.data.Count; i++)
                {
                    var item = res.data[i];
                    if (item.open > biggest)
                    {
                        biggest = item.open;
                    }
                    if (item.open < smallest)
                    {
                        smallest = item.open;
                    }

                    if (item.open > openHigh)
                    {
                        openHigh = item.open;
                        idHigh = item.id;
                    }
                    if (item.open < openLow)
                    {
                        openLow = item.open;
                        idLow = item.id;
                    }

                    // 相差了2%， 说明是一个节点了。
                    if (openHigh >= openLow * (decimal)percentAuto) //1.005
                    {
                        var dtHigh = Utils.GetDateById(idHigh);
                        var dtLow = Utils.GetDateById(idLow);

                        if (idHigh > idLow && lastHighOrLow != 1)
                        {
                            flexPointList.Add(new FlexPoint() { isHigh = true, open = openHigh, id = idHigh });
                            lastHighOrLow = 1;
                            openHigh = openLow;
                            idHigh = idLow;
                        }
                        else if (idHigh < idLow && lastHighOrLow != -1)
                        {
                            flexPointList.Add(new FlexPoint() { isHigh = false, open = openLow, id = idLow });
                            lastHighOrLow = -1;
                            openLow = openHigh;
                            idLow = idHigh;
                        }
                        else if (lastHighOrLow == 1)
                        {

                        }
                    }
                }

                if (flexPointList[0].isHigh)
                {
                    // 
                    foreach (var item in res.data)
                    {
                        if (item.id < flexPointList[0].id && lastLow > item.open)
                        {
                            lastLow = item.open;
                        }
                    }
                }

                //if (flexPointList.Count < 0)
                //{
                //    Console.WriteLine($"--------------{idHigh}------{idLow}------------------");
                //    Console.WriteLine(JsonConvert.SerializeObject(flexPointList));
                //    Console.WriteLine(JsonConvert.SerializeObject(res.data));
                //}

                Console.WriteLine($"最大： {biggest}, 最小: {smallest}, 比率: {biggest / smallest}， 现在:{res.data[0].open}");

                return flexPointList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return new List<FlexPoint>();
        }

        public static List<FlexPoint> AnalyzeBs(string coin, out decimal lastLow, out decimal nowOpen, string min = "1min")
        {
            nowOpen = 0;
            lastLow = 999999999;

            decimal smallest = 9999999;
            decimal biggest = 0;

            try
            {
                ResponseKline res = new AnaylyzeApi().kline(coin + toCoin, min, 1440);
                Console.WriteLine($"总数：{res.data.Count}");
                Console.WriteLine(Utils.GetDateById(res.data[0].id));
                Console.WriteLine(Utils.GetDateById(res.data[res.data.Count - 1].id));

                nowOpen = res.data[0].open;

                List<FlexPoint> flexPointList = new List<FlexPoint>();

                decimal comOpen = res.data[0].open;
                long comId = res.data[0].id;
                int lastHighOrLow = 0; // 1 high, -1: low
                for (var i = 1; i < res.data.Count; i++)
                {
                    var item = res.data[i];
                    if (item.open > biggest)
                    {
                        biggest = item.open;
                    }
                    if (item.open < smallest)
                    {
                        smallest = item.open;
                    }

                    if (item.open > comOpen && lastHighOrLow != 0)
                    {
                        flexPointList.Add(new FlexPoint() { isHigh = false, open = comOpen, id = comId });
                        lastHighOrLow = 0;
                    }
                    else if (item.open < comOpen && lastHighOrLow != 1)
                    {
                        flexPointList.Add(new FlexPoint() { isHigh = true, open = comOpen, id = comId });
                        lastHighOrLow = 1;
                    }

                    comOpen = item.open;
                    comId = item.id;
                }

                Console.WriteLine($"总数：{flexPointList.Count} 最大： {biggest}, 最小: {smallest}, 比率: {biggest / smallest}");

                return flexPointList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
            return new List<FlexPoint>();
        }

        public static decimal AnalyzeNeedSell(decimal comparePrice, DateTime compareDate, string coin, string toCoin, out decimal nowOpen)
        {
            // 当前open
            nowOpen = 0;

            decimal higher = new decimal(0);

            try
            {
                ResponseKline res = new AnaylyzeApi().kline(coin + toCoin, "1min", 1440);

                nowOpen = res.data[0].open;

                List<FlexPoint> flexPointList = new List<FlexPoint>();

                decimal openHigh = res.data[0].open;
                decimal openLow = res.data[0].open;
                long idHigh = 0;
                long idLow = 0;
                int lastHighOrLow = 0; // 1 high, -1: low
                foreach (var item in res.data)
                {
                    if (Utils.GetDateById(item.id) < compareDate)
                    {
                        continue;
                    }

                    if (item.open > higher)
                    {
                        higher = item.open;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("1111111111111111111111 over");
            }
            return higher;
        }
    }
}
