﻿using Newtonsoft.Json;
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
    public class ResponseMerged
    {
        public string status { get; set; }
        public TickData tick { get; set; }
    }

    public class TickData
    {
        public decimal close { get; set; }
    }

    public class AnaylyzeApi
    {
        private static string domain = "";
        private static string baseUrl = $"https://{domain}";

        public static ResponseMerged Merged(string symbol)
        {
            var url = $"{baseUrl}/";
            url += $"?";

            int httpCode = 0;
            var result = RequestDataSync(url, "GET", null, null, out httpCode);
            return JsonConvert.DeserializeObject<ResponseMerged>(result);
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
