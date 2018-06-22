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
        public static class Month
        {

            /// <summary>
            /// 统计月总时长
            /// </summary>
            /// <returns></returns>
            public static double Career(int year, int month)
            {
                double career = 0;
                using (Muse db = new Muse())
                {
                    if (db.Any<WakaTimeDatas>(x => x.BeginDate.Year == year && x.BeginDate.Month == month, null))
                        career = db.Do<WakaTimeDatas>().Where(x => x.BeginDate.Year == year &&
                       x.BeginDate.Month == month).Sum(x => x.Duration);
                }
                return career;
            }

            /// <summary>
            /// 统计月度所有项目报表（堆叠柱状图）
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="year"></param>
            /// <param name="month"></param>
            /// <returns></returns>
            public static List<Tuple<string, List<Tuple<DateTime, double>>>> Report(int year, int month)
            {
                List<Tuple<string, List<Tuple<DateTime, double>>>> rs = null;
                DateTime monthStart = new DateTime(year, month, 1);
                DateTime monthEnd = monthStart.AddMonths(1);
                int totalDays = (int)(monthEnd - monthStart).TotalDays;

                using (Muse db = new Muse())
                {
                    if (db.Any<WakaTimeDatas>(x => x.BeginDate >= monthStart && x.BeginDate < monthEnd, null))
                    {
                        var monthCount = db.Do<WakaTimeDatas>().
                                    Where(x => x.BeginDate >= monthStart && x.BeginDate < monthEnd).
                                    GroupBy(x => new { x.BeginDate.Year, x.BeginDate.Month, x.BeginDate.Day, x.Project }).
                                    Select(x => new
                                    {
                                        Date = x.Max(y => y.BeginDate),
                                        Project = x.Max(y => y.Project),
                                        Duration = x.Sum(y => y.Duration),
                                    }).OrderByDescending(x => x.Duration).ToList();

                        //整理数据库数据
                        if (ListTool.HasElements(monthCount))
                        {
                            rs = new List<Tuple<string, List<Tuple<DateTime, double>>>>();
                            monthCount.ForEach(x =>
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
                                    for (int i = 0; i < totalDays; i++)
                                    {
                                        var dt = new DateTime(monthStart.AddDays(i).Year, monthStart.AddDays(i).Month, monthStart.AddDays(i).Day, 0, 0, 0);
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
            /// <summary>
            /// 统计月度所有项目时间段情况（散点图）
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="year"></param>
            /// <param name="month"></param>
            /// <returns></returns>
            public static List<Tuple<string, List<Tuple<DateTime, double>>>> BubbleReport(int year, int month)
            {
                List<Tuple<string, List<Tuple<DateTime, double>>>> rs = null;
                DateTime monthStart = new DateTime(year, month, 1);
                DateTime monthEnd = monthStart.AddMonths(1);
                using (Muse db = new Muse())
                {
                    if (db.Any<WakaTimeDatas>(x => x.BeginDate >= monthStart && x.BeginDate < monthEnd, null))
                    {
                        var heartbeats = db.Do<WakaTimeDatas>().Where(x => x.BeginDate >= monthStart && x.BeginDate < monthEnd).OrderByDescending(x => x.Duration).Take(1000).ToList();
                        if (ListTool.HasElements(heartbeats))
                        {
                            rs = new List<Tuple<string, List<Tuple<DateTime, double>>>>();
                            heartbeats.ForEach(x =>
                            {
                                if (!rs.Any(y => y.Item1 == x.Project))
                                {
                                    rs.Add(new Tuple<string, List<Tuple<DateTime, double>>>(x.Project, new List<Tuple<DateTime, double>>()));
                                }
                                else
                                {
                                    var item = rs.FirstOrDefault(y => y.Item1 == x.Project);
                                    if (item != null)
                                    {
                                        item.Item2.Add(new Tuple<DateTime, double>(x.BeginDate, x.Duration));
                                    }
                                }
                            });
                        }
                    }
                }
                return rs;
            }







            /// <summary>
            /// 获取当月项目计时
            /// </summary>
            public static void GetMonthProjectTime()
            {
                using (Muse db = new Muse())
                {
                    var projectCount = db.Do<WakaTimeDatas>().
                            Where(x => x.BeginDate.Year == DateTime.Now.Year && x.BeginDate.Month == DateTime.Now.Month).
                            GroupBy(x => new { x.Project }).
                            Select(x => new
                            {
                                Project = x.Max(y => y.Project),
                                Duration = x.Sum(y => y.Duration),
                            }).OrderByDescending(x => x.Duration).Take(MAX_PROJECT).ToList();
                }
            }

            /// <summary>
            /// 获取所有项目统计列表
            /// </summary>
            /// <param name="userId"></param>
            /// <returns></returns>
            public static void GetProjectList(Guid userId)
            {
                using (Muse db = new Muse())
                {
                    var projectCount = db.Do<WakaTimeDatas>().
                            GroupBy(x => new { x.Project }).
                            Select(x => new
                            {
                                Project = x.Max(y => y.Project),
                                Duration = x.Sum(y => y.Duration),
                            }).OrderByDescending(x => x.Duration).ToList();
                }
            }
            /// <summary>
            /// 获取项目所有文件详细统计信息
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="project"></param>
            /// <returns></returns>
            public static void GetProjectDetail(Guid userId, string project)
            {
                using (Muse db = new Muse())
                {
                    var projectCount = db.Do<WakaTimeDatas>().
                        Where(x => x.Project == project).
                        GroupBy(x => new { x.Entity }).
                        Select(x => new
                        {
                            Entity = x.Max(y => y.Entity),
                            Duration = x.Sum(y => y.Duration),
                        }).OrderByDescending(x => x.Duration).ToList();
                }
            }
            /// <summary>
            /// 统计作息时间表
            /// </summary>
            /// <param name="userId"></param>
            /// <param name="year"></param>
            /// <param name="month"></param>
            /// <returns></returns>
            public static void GetRestTimeReport(Guid userId, int year, int month)
            {
                DateTime beginDate = new DateTime(year, month, 1);
                DateTime endDate = beginDate.AddMonths(1);
                int monthDays = DateTime.DaysInMonth(beginDate.Year, beginDate.Month);
                int restHour = 22, warningHour = 3, totalHour = 24;

                using (Muse db = new Muse())
                {
                    var normalRest = db.Do<WakaTimeDatas>().
                   Where(x => x.EndDate >= beginDate && x.EndDate < endDate &&
                   x.EndDate.Hour >= restHour && x.EndDate.Hour < totalHour).
                   GroupBy(x => x.EndDate.Day).
                   Select(x => new
                   {
                       Time = x.Max(e => e.EndDate),
                   }).OrderBy(x => x.Time).ToList();
                    DateTime tempbeginDate = beginDate.AddDays(1), tempendDate = endDate.AddDays(1);
                    var dangerRest = db.Do<WakaTimeDatas>().
                       Where(x => x.EndDate >= tempbeginDate && x.EndDate < tempendDate &&
                       x.EndDate.Hour >= 0 && x.EndDate.Hour < warningHour).
                       GroupBy(x => x.EndDate.Day).
                       Select(x => new
                       {
                           Time = x.Max(e => e.EndDate),
                       }).OrderBy(x => x.Time).ToList();
                }
            }


        }
    }
}
