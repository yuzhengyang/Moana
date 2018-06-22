using Azylee.Core.DataUtils.CollectionUtils;
using Moana.DB.SQLite.Engine;
using Moana.WakaTime.Models.WebModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moana.WakaTime.ToolKit.AnalyzeUtils
{
    public partial class WakaAnalyzer
    {
        public static class Recent7
        {
            /// <summary>
            /// 获取最近七天时长
            /// </summary>
            public static double Career()
            {
                double career = 0;
                DateTime today = DateTime.Now;
                DateTime sevenDaysAgo = new DateTime(today.AddDays(-6).Year, today.AddDays(-6).Month, today.AddDays(-6).Day, 0, 0, 0);
                using (Muse db = new Muse())
                {
                    if (db.Any<WakaTimeDatas>(x => x.BeginDate >= sevenDaysAgo, null))
                        career = db.Do<WakaTimeDatas>().Where(x => x.BeginDate >= sevenDaysAgo).Sum(x => x.Duration);
                }
                return career;
            }
            /// <summary>
            /// 统计最近七天编程语言使用占用情况
            /// </summary>
            /// <param name="userId"></param>
            /// <returns></returns>
            public static List<Tuple<string, double>> LanguageCount()
            {
                DateTime today = DateTime.Now;
                DateTime sevenDaysAgo = new DateTime(today.AddDays(-6).Year, today.AddDays(-6).Month, today.AddDays(-6).Day, 0, 0, 0);
                List<Tuple<string, double>> rs = null;
                using (Muse db = new Muse())
                {
                    if (db.Any<WakaTimeDatas>(x => x.ID != null, null))
                    {
                        var languageGroup = db.Do<WakaTimeDatas>().Where(x => x.Language != null && x.BeginDate >= sevenDaysAgo).GroupBy(x => x.Language).
                            Select(x => new
                            {
                                Language = x.Max(e => e.Language),
                                Duration = x.Sum(y => y.Duration),
                            }).OrderByDescending(x => x.Duration).Take(MAX_LANGUAGE).ToList();

                        if (ListTool.HasElements(languageGroup))
                        {
                            rs = new List<Tuple<string, double>>();
                            languageGroup.ForEach(x =>
                            {
                                Tuple<string, double> rec = new Tuple<string, double>(x.Language, x.Duration);
                                rs.Add(rec);
                            });
                        }
                    }
                }
                return rs;
            }
            /// <summary>
            /// 获取最近七天信息
            /// </summary>
            public static List<Tuple<string, List<Tuple<DateTime, double>>>> Report()
            {
                List<Tuple<string, List<Tuple<DateTime, double>>>> rs = null;
                DateTime today = DateTime.Now;
                DateTime sevenDaysAgo = new DateTime(today.AddDays(-6).Year, today.AddDays(-6).Month, today.AddDays(-6).Day, 0, 0, 0);
                using (Muse db = new Muse())
                {
                    if (db.Any<WakaTimeDatas>(x => x.BeginDate >= sevenDaysAgo, null))
                    {
                        //查询数据库
                        var weekCount = db.Do<WakaTimeDatas>().
                                Where(x => x.BeginDate >= sevenDaysAgo).
                                GroupBy(x => new { x.BeginDate.Year, x.BeginDate.Month, x.BeginDate.Day, x.Project }).
                                Select(x => new
                                {
                                    Date = x.Max(y => y.BeginDate),
                                    Project = x.Max(y => y.Project),
                                    Duration = x.Sum(y => y.Duration),
                                }).OrderByDescending(x => x.Date).ToList();
                        //整理数据库数据
                        if (ListTool.HasElements(weekCount))
                        {
                            rs = new List<Tuple<string, List<Tuple<DateTime, double>>>>();
                            weekCount.ForEach(x =>
                            {
                                if (!rs.Any(y => y.Item1 == x.Project))
                                {
                                    var rec = new List<Tuple<DateTime, double>>();
                                    rec.Add(new Tuple<DateTime, double>(x.Date, x.Duration));
                                    rs.Add(new Tuple<string, List<Tuple<DateTime, double>>>(x.Project, rec));
                                }
                                else
                                {
                                    var item = rs.FirstOrDefault(y => y.Item1 == x.Project);
                                    if (item != null)
                                    {
                                        item.Item2.Add(new Tuple<DateTime, double>(x.Date, x.Duration));
                                    }
                                }
                            });
                        }
                        //补充缺失数据
                        if (ListTool.HasElements(rs))
                        {
                            foreach (var project in rs)
                            {
                                if (ListTool.HasElements(project.Item2))
                                {
                                    for (int i = 0; i < 7; i++)
                                    {
                                        var dt = new DateTime(today.AddDays(-i).Year, today.AddDays(-i).Month, today.AddDays(-i).Day, 0, 0, 0);
                                        if (!project.Item2.Any(x => x.Item1.Year == dt.Year && x.Item1.Month == dt.Month && x.Item1.Day == dt.Day))
                                        {
                                            project.Item2.Add(new Tuple<DateTime, double>(dt, 0));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return rs;
            }
        }
    }
}
