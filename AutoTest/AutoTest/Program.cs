using log4net;
using log4net.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTest
{
    class Program
    {
        // k线数据                  /market/history/kline
        // 获取聚合行情             /market/detail/merged？？？
        // 获取 Market Depth 数据   /market/depth
        // 获取 Market Detail 24小时成交量数据
        // 批量获取最近的交易记录  /market/history/trade

        static string toCoin = "usdt";
        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            ILog logger = LogManager.GetLogger("program");
            var k = "ltc";
            while (true)
            {
                decimal lastlow, nowOpen;

                //var res = CoinAnalyze.Analyze(k, out lastlow, out nowOpen);
                //Console.WriteLine(k + $" -->   {lastlow}, {nowOpen}");
                //foreach (var item in res)
                //{
                //    Console.WriteLine(Utils.GetDateById(item.id).ToString("yyyy-MM-dd HH:mm:ss") + " -->   " + JsonConvert.SerializeObject(item));
                //}

                //res = CoinAnalyze.Analyze(k, out lastlow, out nowOpen, 1.035);
                //Console.WriteLine(k + $" -->   {lastlow}, {nowOpen}");
                //foreach (var item in res)
                //{
                //    Console.WriteLine(Utils.GetDateById(item.id).ToString("yyyy-MM-dd HH:mm:ss") + " -->   " + JsonConvert.SerializeObject(item));
                //}
                // 1. kline
                var dt = DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss");
                ResponseKline kline = AnaylyzeApi.kline(k + toCoin, "1min", 1440);
                logger.Error(JsonConvert.SerializeObject(kline));
                FileUtils.write(JsonConvert.SerializeObject(kline), dt + "-kline.txt");
                // 2. merged
                var merged = AnaylyzeApi.Merged(k + "usdt");
                logger.Error(JsonConvert.SerializeObject(merged));
                FileUtils.write(JsonConvert.SerializeObject(merged), dt + "-merged.txt");

                // 3. depth
                var depth = AnaylyzeApi.Depth(k + "usdt", "step0");
                logger.Error(JsonConvert.SerializeObject(depth));
                FileUtils.write(JsonConvert.SerializeObject(depth), dt + "-depth.txt");

                // 4. trade
                var trade = AnaylyzeApi.trade(k + "usdt");
                logger.Error(JsonConvert.SerializeObject(trade));
                FileUtils.write(JsonConvert.SerializeObject(trade), dt + "-trade.txt");

                // 5. histroy trade
                var historytrade = AnaylyzeApi.historytrade(k + "usdt");
                logger.Error(JsonConvert.SerializeObject(historytrade));
                FileUtils.write(JsonConvert.SerializeObject(historytrade), dt + "-historytrade.txt");

                // 6. detail
                var detail = AnaylyzeApi.detail(k + "usdt");
                logger.Error(JsonConvert.SerializeObject(detail));
                FileUtils.write(JsonConvert.SerializeObject(detail), dt + "-detail.txt");


                //res = CoinAnalyze.AnalyzeBs(k, out lastlow, out nowOpen);
                //Console.WriteLine(k + $" -->   {lastlow}, {nowOpen}");
                //foreach (var item in res)
                //{
                //    Console.WriteLine(Utils.GetDateById(item.id).ToString("yyyy-MM-dd HH:mm:ss")+" -->   "+JsonConvert.SerializeObject(item));
                //}
                Console.WriteLine("请输入");
                k = Console.ReadLine();
            }


            Console.ReadLine();
        }
    }
}
