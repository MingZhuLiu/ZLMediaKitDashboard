﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZLServerDashboard.Commons
{
    public class QxHttpClient
    {
        public static long RequestCount;
        public HttpClient _httpClient = null;
        HttpClientHandler handler = null;


        public static Stopwatch stopWatch=new Stopwatch();

        public static long UplodaDataSize=0;

        public static long DownloadDataSize=0;

        public static void StartWatch()
        {
            UplodaDataSize=0;
            DownloadDataSize=0;
            RequestCount=0;
            stopWatch=new Stopwatch();
            stopWatch.Start();
        }
         public static void StopWatch()
        {
            stopWatch.Stop();
        }

        /// <summary>
        /// 默认语言
        /// </summary>
        public Encoding DefaultEncoding = Encoding.UTF8;


        public delegate void HttpProgress(int progress);
        //public event HttpProgress HttpDownloadProgressEvent;

        public QxHttpClient()
        {
            cookieContainer = new CookieContainer();
            handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = true, UseCookies = true };
            _httpClient = new HttpClient();
        }
        public QxHttpClient(string proxyIp, int proxyPort, string proxyAccount, string proxyPassword)
        {
            cookieContainer = new CookieContainer();

            WebProxy wp = new WebProxy(proxyIp, proxyPort);
            //代理地址
            //设置身份验证凭据 账号 密码
            wp.Credentials = new NetworkCredential(proxyAccount, proxyPassword);

            handler = new HttpClientHandler() { CookieContainer = cookieContainer, AllowAutoRedirect = true, UseCookies = true, Proxy = wp };
            _httpClient = new HttpClient();

        }


        #region Head相关

        public void AddHeader(string key, String value)
        {

            RemoveHeader(key);
            _httpClient.DefaultRequestHeaders.Add(key, value);
        }

        public Dictionary<String, String> Heads
        {
            get
            {
                Dictionary<String, String> head = new Dictionary<string, string>();
                foreach (var item in _httpClient.DefaultRequestHeaders)
                {
                    head.Add(item.Key, String.Join(";", item.Value));
                }
                return head;
            }
        }

        public void RemoveHeader(string key)
        {
            if (_httpClient.DefaultRequestHeaders.Contains(key))
                _httpClient.DefaultRequestHeaders.Remove(key);
        }


        #endregion
        #region Cookie相关

        public CookieContainer cookieContainer = null;
        public Dictionary<String, String> Cookies
        {
            get
            {
                Dictionary<String, String> cookies = new Dictionary<string, string>();
                if (cookieContainer != null)
                {

                    Hashtable table = (Hashtable)cookieContainer.GetType().InvokeMember("m_domainTable",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField |
                        System.Reflection.BindingFlags.Instance, null, cookieContainer, new object[] { });
                    foreach (object pathList in table.Values)
                    {
                        SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list",
                            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField
                            | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                        foreach (CookieCollection colCookies in lstCookieCol.Values)
                            foreach (Cookie c in colCookies)
                            {

                                if (cookies.ContainsKey(c.Name))
                                    cookies[c.Name] = c.Value;
                                else
                                    cookies.Add(c.Name, c.Value);
                            }
                    }
                }
                return cookies;
            }
        }

        #endregion
        public T Get<T>(string url, bool ignoreHttpErrorrCode = false)
        {
            Interlocked.Increment(ref RequestCount);
            Object result = null;
            
            UplodaDataSize+=Encoding.UTF8.GetBytes(url).Length*2;

            var httpResult = _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url)).Result;
            if (ignoreHttpErrorrCode || httpResult.StatusCode == HttpStatusCode.OK)
            {
                var buffer = httpResult.Content.ReadAsByteArrayAsync().Result;
                result = CastObj<T>(buffer);
                if(buffer!=null)
                    DownloadDataSize+= buffer.Length;

            }
            else
            {
                //return default(T);
            }
            return (T)result;
        }
        public T Post<T>(string url, string data, string contentType, bool ignoreHttpErrorrCode = false)
        {
            Interlocked.Increment(ref RequestCount);
            Object result = null;
             UplodaDataSize+=Encoding.UTF8.GetBytes(url).Length*2+Encoding.UTF8.GetBytes(data).Length;
            StringContent content = new StringContent(data, DefaultEncoding, contentType);
            HttpResponseMessage httpResult = _httpClient.PostAsync(new Uri(url), content).Result;
            if (ignoreHttpErrorrCode || httpResult.StatusCode == HttpStatusCode.OK)
            {
                var buffer = httpResult.Content.ReadAsByteArrayAsync().Result;
                result = CastObj<T>(buffer);
                  if(buffer!=null)
                    DownloadDataSize+= buffer.Length;
            }
            else
            {
                //return default(T);
            }
            return (T)result;
        }

        public T Post<T>(string url, QxHttpPara data, string contentType, bool ignoreHttpErrorrCode = false)
        {
            return Post<T>(url, data.ParaStr, contentType, ignoreHttpErrorrCode);
        }
        private T CastObj<T>(byte[] buffer)
        {
            if (buffer == null)
                return default(T);
            Object result = null;
            if (typeof(T) == typeof(String))
            {
                result = DefaultEncoding.GetString((byte[])buffer);
            }
            else if (typeof(T) == typeof(byte[]))
            {
                result = buffer;
            }
            else //不是String 不是byte[] ,那肯定是对象,直接转为对象
            {
                var json = DefaultEncoding.GetString((byte[])buffer);
                result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            return (T)result;
        }
    }

    public class QxHttpPara
    {
        private Dictionary<String, String> _data = null;
        public QxHttpPara()
        {
            _data = new Dictionary<string, string>();
        }
        public QxHttpPara(string key, string value) : this()
        {
            _data.Add(key, value);
        }
        public QxHttpPara AddPara(string key, string value)
        {
            if (_data.ContainsKey(key))
                _data[key] = value;
            else
                _data.Add(key, value);
            return this;
        }
        public QxHttpPara AddUrlEncodePara(string key, string value)
        {
            if (_data.ContainsKey(key))
                _data[key] = Tools.URLEncode(value);
            else
                _data.Add(key, Tools.URLEncode(value));
            return this;
        }

        public String ParaStr
        {
            get
            {
                StringBuilder result = new StringBuilder();
                if (_data.Count != 0)
                {
                    int index = 0;
                    foreach (var item in _data.Keys)
                    {
                        if (index != 0)
                            result.Append("&");
                        result.Append(item);
                        result.Append("=");
                        result.Append(_data[item]);
                        index++;
                    }
                }
                return result.ToString();
            }
        }
    }
}
