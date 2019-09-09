using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using ZoneTop.Application.SSO.Common.Model;

namespace ZoneTop.Application.SSO.Common.Utils
{
    /// <summary>
    /// http请求通用类
    /// </summary>
    public class HttpUtils
    {
        private static LogModel log = LogUtils.GetLogger("HttpUtils");

        #region http post请求通用类
        /// <summary>
        /// 根据参数执行HTTP请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="parameters"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static string HttpPost(string url, IEnumerable<HttpModel> parameters, CookieContainer container)
        {
            string paramContent = parameters != null
                ? string.Join("&", parameters.Select(p => p.ToString()))
                : "";

            return HttpPost(url, paramContent, container);
        }

        /// <summary>
        /// POST请求
        /// </summary>
        /// <param name="url">请求url</param>
        /// <param name="postContent">post content</param>
        /// <param name="container">附带的CookieContainer</param>
        /// <returns></returns>
        public static string HttpPost(string url, string postContent, CookieContainer container)
        {
            try
            {
                ServicePointManager.DefaultConnectionLimit = 200;
                GC.Collect();

                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
                request.KeepAlive = false;

                request.ProtocolVersion = HttpVersion.Version11;
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/49.0.2623.221 Safari/537.36 SE 2.X MetaSr 1.0";
                request.Accept = "*/*";

                byte[] content = Encoding.UTF8.GetBytes(postContent);
                request.ContentLength = content.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = WebRequestMethods.Http.Post;
                request.Timeout = 5 * 60 * 1000;

                if (container != null)
                {
                    request.CookieContainer = container;
                }

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(content, 0, content.Length);
                    requestStream.Close();
                    WebResponse response = request.GetResponse();
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(responseStream))
                        {
                            string strResult = reader.ReadToEnd();
                            reader.Close();
                            return strResult;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                throw ex;
            }
        }
        #endregion

        #region rest
        /// <summary>
        /// rest request
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="type">请求类型</param>
        /// <param name="parameters">参数</param>
        /// <param name="cookies">cookies 对应value需要为支付类型</param>
        /// <returns></returns>
        public static string RestRequest(string url, Method type, IEnumerable<HttpModel> parameters, IEnumerable<HttpModel> cookies)
        {
            try
            {
                var client = new RestClient(url);
                var request = new RestRequest(type);
                client.Proxy = null;
                request.Timeout = 5 * 60 * 1000;

                //文件头
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                //参数
                if (parameters != null)
                {
                    foreach (HttpModel parameter in parameters)
                    {
                        request.AddParameter(parameter.Name, parameter.Value);
                    }
                }
                //cookies
                if (cookies != null)
                {
                    foreach (HttpModel cookie in cookies)
                    {
                        request.AddCookie(cookie.Name, cookie.Value.ToString());
                    }
                }
                //开始执行请求操作
                IRestResponse resp = client.Execute(request);

                return resp.Content;
            }
            catch (Exception ex)
            {
                LogUtils.myError(log, ex);
                throw ex;
            }
        }
        #endregion
    }
}
