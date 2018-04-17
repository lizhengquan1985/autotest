using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AutoTest
{
    public class AnaylyzeApi
    {
        private static string domain = "api.huobipro.com/market";
        private static string baseUrl = $"https://{domain}";

        /*
         "data": [
{
    "id": K线id,
    "amount": 成交量,
    "count": 成交笔数,
    "open": 开盘价,
    "close": 收盘价,当K线为最晚的一根时，是最新成交价
    "low": 最低价,
    "high": 最高价,
    "vol": 成交额, 即 sum(每一笔成交价 * 该笔的成交量)
  }
]*/
        public static ResponseKline kline(string symbol, string period, int size = 300)
        {
            var url = $"{baseUrl}/history/kline";
            url += $"?symbol={symbol}&period={period}&size={size}";

            int httpCode = 0;
            var result = RequestDataSync(url, "GET", null, null, out httpCode);
            return JsonConvert.DeserializeObject<ResponseKline>(result);
        }

        /*
         "tick": {
    "id": K线id,
    "amount": 成交量,
    "count": 成交笔数,
    "open": 开盘价,
    "close": 收盘价,当K线为最晚的一根时，是最新成交价
    "low": 最低价,
    "high": 最高价,
    "vol": 成交额, 即 sum(每一笔成交价 * 该笔的成交量)
    "bid": [买1价,买1量],
    "ask": [卖1价,卖1量]
  }*/
        public static ResponseMerged Merged(string symbol)
        {
            var url = $"{baseUrl}/detail/merged";
            url += $"?symbol={symbol}";

            int httpCode = 0;
            var result = RequestDataSync(url, "GET", null, null, out httpCode);
            //Console.WriteLine(result);
            //Console.WriteLine(httpCode);
            return JsonConvert.DeserializeObject<ResponseMerged>(result);
        }

        /*
         "tick": {
    "id": 消息id,
    "ts": 消息生成时间，单位：毫秒,
    "bids": 买盘,[price(成交价), amount(成交量)], 按price降序,
    "asks": 卖盘,[price(成交价), amount(成交量)], 按price升序
  }
             */
        public static ResponseDepth Depth(string symbol, string type)
        {
            var url = $"{baseUrl}/depth";
            url += $"?symbol={symbol}&type={type}";

            int httpCode = 0;
            var result = RequestDataSync(url, "GET", null, null, out httpCode);
            //Console.WriteLine(result);
            //Console.WriteLine(httpCode);
            return JsonConvert.DeserializeObject<ResponseDepth>(result);
        }

        /*
         * "tick": {
    "id": 消息id,
    "ts": 最新成交时间,
    "data": [
      {
        "id": 成交id,
        "price": 成交价钱,
        "amount": 成交量,
        "direction": 主动成交方向,
        "ts": 成交时间
      }
    ]
  }
         */
        public static ResponseTrade trade(string symbol)
        {
            var url = $"{baseUrl}/trade";
            url += $"?symbol={symbol}";

            int httpCode = 0;
            var result = RequestDataSync(url, "GET", null, null, out httpCode);
            return JsonConvert.DeserializeObject<ResponseTrade>(result);
        }

        public static ResponseHistoryTrade historytrade(string symbol, int size = 2000)
        {
            var url = $"{baseUrl}/history/trade";
            url += $"?symbol={symbol}&size={size}";

            int httpCode = 0;
            var result = RequestDataSync(url, "GET", null, null, out httpCode);
            return JsonConvert.DeserializeObject<ResponseHistoryTrade>(result);
        }

        /*
         "tick": {
    "id": 消息id,
    "ts": 24小时统计时间,
    "amount": 24小时成交量,
    "open": 前推24小时成交价,
    "close": 当前成交价,
    "high": 近24小时最高价,
    "low": 近24小时最低价,
    "count": 近24小时累积成交数,
    "vol": 近24小时累积成交额, 即 sum(每一笔成交价 * 该笔的成交量)
  }*/
        public static ResponseDetail detail(string symbol)
        {
            var url = $"{baseUrl}/detail";
            url += $"?symbol={symbol}";

            int httpCode = 0;
            var result = RequestDataSync(url, "GET", null, null, out httpCode);
            return JsonConvert.DeserializeObject<ResponseDetail>(result);
        }

        private static string RequestDataSync(string url, string method, Dictionary<string, object> param, WebHeaderCollection headers, out int httpCode)
        {
            string resp = string.Empty;
            httpCode = 200;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Accept-Encoding", "gzip");
            request.Method = method;

            if (headers != null)
            {
                foreach (var key in headers.AllKeys)
                {
                    request.Headers.Add(key, headers[key]);
                }
            }
            try
            {
                if (method == "POST" && param != null)
                {
                    byte[] bs = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(param));
                    request.ContentType = "application/json";
                    request.ContentLength = bs.Length;
                    using (var reqStream = request.GetRequestStream())
                    {
                        reqStream.Write(bs, 0, bs.Length);
                    }
                }
                //如果是Get 请求参数附加在URL之后
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (resp == null)
                        throw new Exception("Response is null");
                    resp = GetResponseBody(response);
                    httpCode = (int)response.StatusCode;
                }
            }
            catch (WebException ex)
            {
                using (HttpWebResponse response = ex.Response as HttpWebResponse)
                {
                    resp = GetResponseBody(response);
                    httpCode = (int)response.StatusCode;
                }
            }
            return resp;
        }

        private static string GetResponseBody(HttpWebResponse response)
        {
            var readStream = new Func<Stream, string>((stream) =>
            {
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            });

            using (var responseStream = response.GetResponseStream())
            {
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    using (GZipStream stream = new GZipStream(responseStream, CompressionMode.Decompress))
                    {
                        return readStream(stream);
                    }
                }
                if (response.ContentEncoding.ToLower().Contains("deflate"))
                {
                    using (DeflateStream stream = new DeflateStream(responseStream, CompressionMode.Decompress))
                    {
                        return readStream(stream);
                    }
                }
                return readStream(responseStream);
            }
        }
    }
}
