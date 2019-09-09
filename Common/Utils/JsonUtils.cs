using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;

namespace ZoneTop.Application.SSO.Common.Utils
{
    /// <summary>
    /// Json操作
    /// </summary>
    public static class JsonUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static object ToJson(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject(Json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="datetimeformats"></param>
        /// <returns></returns>
        public static string ToJson(this object obj, string datetimeformats)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string Json)
        {
            return Json == null ? default(T) : JsonConvert.DeserializeObject<T>(Json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static DataTable ToTable(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static JObject ToJObject(this string Json)
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }

        /// <summary>
        /// 返回sql查询
        /// </summary>
        /// <param name="Json"></param>
        /// <param name="param"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static string GetLike(this JObject Json, string param, string tableName = "")
        {
            if (Json[param] == null || string.IsNullOrEmpty(Json[param].ToString()))
            {
                return "";
            }
            else
            {
                tableName = string.IsNullOrWhiteSpace(tableName) ? param : (tableName + "." + param);
                return " and " + tableName + " like '%" + Json[param].ToString() + "%' ";
            }

        }

        /// <summary>
        /// 返回sql查询
        /// </summary>
        /// <param name="Json"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GetAnd(this JObject Json, string param)
        {
            if (Json[param] == null || string.IsNullOrEmpty(Json[param].ToString()))
            {
                return "";
            }
            else
            {
                return " and " + param + " ='" + Json[param].ToString() + "' ";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Json"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GetTime(this JObject Json, string param)
        {
            string s = string.Empty;
            string ss = param + "StartTime";
            string se = param + "EndTime";
            if (Json[ss] == null || string.IsNullOrEmpty(Json[ss].ToString()))
            {
                s = "";
            }
            else
            {
                DateTime result;
                result = DateTime.TryParse(Json[ss].ToString(), out result) ? result : DateTime.MinValue;
                s += " and " + param + " >= '" + result + "' ";
            }

            if (Json[se] == null || string.IsNullOrEmpty(Json[se].ToString()))
            {
                s += "";
            }
            else
            {
                DateTime result;
                result = DateTime.TryParse(Json[se].ToString(), out result) ? result : DateTime.MinValue;
                s += " and " + param + " <= '" + result + "' ";
            }
            return s;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Json"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string GetTimeRange(this JObject Json, string param)
        {
            string s1 = string.Empty;
            string s2 = string.Empty;
            string ss = param + "StartTime";
            string se = param + "EndTime";
            if (Json[ss] == null || string.IsNullOrEmpty(Json[ss].ToString()))
            {
                s1 = "";
            }
            else
            {
                DateTime result;
                result = DateTime.TryParse(Json[ss].ToString(), out result) ? result : DateTime.MinValue;
                s1 = param + "JZRQ < '" + result + "' ";
            }

            if (Json[se] == null || string.IsNullOrEmpty(Json[se].ToString()))
            {
                s2 = "";
            }
            else
            {
                DateTime result;
                result = DateTime.TryParse(Json[se].ToString(), out result) ? result : DateTime.MinValue;
                s2 = param + "KSRQ > '" + result + "' ";
            }
            if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
            {
                return "";
            }
            else if (string.IsNullOrEmpty(s2))
            {
                return "and NOT(" + s1 + ")";
            }
            else
            {
                return " and NOT(" + (string.IsNullOrEmpty(s1) ? s2 : (s1 + " or " + s2)) + ")";
            }

        }

        #region 对象转Jobject
        /// <summary>
        /// model 转
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static JObject ModelToJObject<T>(T t)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            return JObject.Parse(JsonConvert.SerializeObject(t, settings));
        }
        #endregion

        #region 读取json文件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static JObject ReadJsonConfig(string fileName)
        {
            string data = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/Configs/Json/" + fileName));
            return JObject.Parse(data);
        }
        #endregion 
    }
}
