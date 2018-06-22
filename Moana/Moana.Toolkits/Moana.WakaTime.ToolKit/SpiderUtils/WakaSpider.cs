using Azylee.Core.DataUtils.CollectionUtils;
using Azylee.Core.DataUtils.DateTimeUtils;
using Azylee.YeahWeb.HttpUtils;
using Moana.DB.SQLite.Engine;
using Moana.Waka.Toolkit.Models;
using Moana.WakaTime.Models.WebModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Moana.WakaTime.ToolKit.SpiderUtils
{
    public class WakaSpider
    {
        const int MAX_PROJECT = 8;
        const int MAX_LANGUAGE = 10;
        private static string BaseUrl = "https://wakatime.com/api/v1/users/current/";
        private const double MinDuration = 0.01;

        public static int GetDatas(string apiKey, DateTime date)
        {
            int count = 0;
            var durations = GetDurations(apiKey, date);
            var heartbeats = GetHeartbeats(apiKey, date);
            List<WakaTimeDatas> datas = AssembleHeartbeats(durations, heartbeats);
            List<WakaTimeDatas> notExistDatas = new List<WakaTimeDatas>();
            if (ListTool.HasElements(datas))
            {
                using (Muse db = new Muse())
                {
                    foreach (var data in datas)
                    {
                        if (!db.Any<WakaTimeDatas>(x => x.ID == data.ID, null))
                        {
                            if (!notExistDatas.Any(x => x.ID == data.ID))
                            {
                                notExistDatas.Add(data);
                            }
                        }
                    }
                    if (ListTool.HasElements(notExistDatas))
                        count = db.Adds(notExistDatas);
                }
            }
            return count;
        }

        /// <summary>
        /// 联网获取 Durations 数据
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private static List<Durations> GetDurations(string apiKey, DateTime date)
        {
            try
            {
                string durationsUrl = string.Format("{0}durations?api_key={1}&date={2}", BaseUrl, apiKey, date.ToString("yyyy-MM-dd"));
                JToken root = HttpTool.Get<JToken>(durationsUrl, "gb2312");
                JToken durations = root.SelectToken("data");
                List<Durations> result = new List<Durations>();
                foreach (var item in durations)
                {
                    Durations obj = item.ToObject<Durations>();
                    result.Add(obj);
                }
                return result;
            }
            catch (Exception e) { }
            return null;
        }
        /// <summary>
        /// 联网获取 Heartbeats 数据
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        private static List<Heartbeats> GetHeartbeats(string apiKey, DateTime date)
        {
            try
            {
                string heartbeatsUrl = string.Format("{0}heartbeats?api_key={1}&date={2}", BaseUrl, apiKey, date.ToString("yyyy-MM-dd"));
                JToken root = HttpTool.Get<JToken>(heartbeatsUrl, "gb2312");
                JToken heartbeats = root.SelectToken("data");
                List<Heartbeats> result = new List<Heartbeats>();
                foreach (var item in heartbeats)
                {
                    Heartbeats obj = item.ToObject<Heartbeats>();
                    obj.time = Math.Round(obj.time, 2);
                    result.Add(obj);
                }
                return result;
            }
            catch (Exception e) { }
            return null;
        }
        /// <summary>
        /// 融合数据（Durations 和 Heartbeats）
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="durations"></param>
        /// <param name="beats"></param>
        /// <returns></returns>
        private static List<WakaTimeDatas> AssembleHeartbeats(List<Durations> durations, List<Heartbeats> beats)
        {
            if (ListTool.HasElements(durations) && ListTool.HasElements(beats))
            {
                List<WakaTimeDatas> resultList = new List<WakaTimeDatas>();
                durations = durations.OrderByDescending(x => x.time).ToList();
                foreach (var dr in durations)
                {
                    double begin = dr.time, end = dr.time + dr.duration;
                    var tempBeats = beats.Where(x => x.time >= begin && x.time <= end).OrderByDescending(x => x.time).ToList();
                    foreach (var tb in tempBeats)
                    {
                        double objDuration = Math.Round(end - tb.time, 2);
                        if (objDuration >= MinDuration)
                        {
                            WakaTimeDatas obj = new WakaTimeDatas()
                            {
                                ID = tb.id.ToString(),
                                Project = tb.project ?? dr.project,
                                Branch = tb.branch,
                                Entity = tb.entity,
                                Type = tb.type,
                                Language = tb.language,
                                BeginTime = tb.time,
                                EndTime = end,
                                BeginDate = UnixTimeTool.Parse(tb.time),
                                EndDate = UnixTimeTool.Parse(end),
                                Duration = objDuration,
                                IsDebugging = tb.is_debugging,
                                IsWrite = tb.is_write,
                            };
                            resultList.Add(obj);
                        }
                        end = tb.time;
                    }
                }
                return resultList;
            }
            return null;
        }
    }
}
