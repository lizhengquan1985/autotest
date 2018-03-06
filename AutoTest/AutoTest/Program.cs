using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoTest
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("请输入");
                decimal lastlow, nowOpen;
                var k = Console.ReadLine();

                var res = CoinAnalyze.Analyze(k, out lastlow, out nowOpen);
                Console.WriteLine(k + $" -->   {lastlow}, {nowOpen}");
                foreach (var item in res)
                {
                    Console.WriteLine(Utils.GetDateById(item.id).ToString("yyyy-MM-dd HH:mm:ss") + " -->   " + JsonConvert.SerializeObject(item));
                }

                res = CoinAnalyze.Analyze(k, out lastlow, out nowOpen, 1.01);
                Console.WriteLine(k + $" -->   {lastlow}, {nowOpen}");
                foreach (var item in res)
                {
                    Console.WriteLine(Utils.GetDateById(item.id).ToString("yyyy-MM-dd HH:mm:ss") + " -->   " + JsonConvert.SerializeObject(item));
                }

                res = CoinAnalyze.Analyze(k, out lastlow, out nowOpen, 1.02);
                Console.WriteLine(k + $" -->   {lastlow}, {nowOpen}");
                foreach (var item in res)
                {
                    Console.WriteLine(Utils.GetDateById(item.id).ToString("yyyy-MM-dd HH:mm:ss") + " -->   " + JsonConvert.SerializeObject(item));
                }

                res = CoinAnalyze.Analyze(k, out lastlow, out nowOpen, 1.025);
                Console.WriteLine(k + $" -->   {lastlow}, {nowOpen}");
                foreach (var item in res)
                {
                    Console.WriteLine(Utils.GetDateById(item.id).ToString("yyyy-MM-dd HH:mm:ss") + " -->   " + JsonConvert.SerializeObject(item));
                }

                res = CoinAnalyze.Analyze(k, out lastlow, out nowOpen, 1.035);
                Console.WriteLine(k + $" -->   {lastlow}, {nowOpen}");
                foreach (var item in res)
                {
                    Console.WriteLine(Utils.GetDateById(item.id).ToString("yyyy-MM-dd HH:mm:ss") + " -->   " + JsonConvert.SerializeObject(item));
                }

                //res = CoinAnalyze.AnalyzeBs(k, out lastlow, out nowOpen);
                //Console.WriteLine(k + $" -->   {lastlow}, {nowOpen}");
                //foreach (var item in res)
                //{
                //    Console.WriteLine(Utils.GetDateById(item.id).ToString("yyyy-MM-dd HH:mm:ss")+" -->   "+JsonConvert.SerializeObject(item));
                //}
            }


            Console.ReadLine();
        }
    }
}
