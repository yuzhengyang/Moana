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
        public static class Year
        {
            /// <summary>
            /// 获取所有年份
            /// </summary>
            /// <returns></returns>
            public static List<int> AllYears()
            {
                List<int> years = new List<int>();
                using (Muse db = new Muse())
                {
                    if (db.Any<WakaTimeDatas>(x => x.ID != null, null))
                    {
                        var data = db.Do<WakaTimeDatas>().
                              GroupBy(x => new { x.BeginDate.Year }).
                              Select(x => new
                              {
                                  Date = x.Max(y => y.BeginDate),
                              }).OrderByDescending(x => x.Date).ToList();

                        if (ListTool.HasElements(data))
                        {
                            data.ForEach(x =>
                            {
                                years.Add(x.Date.Year);
                            });
                        }
                    }
                }
                int year = DateTime.Now.Year;
                if (!years.Contains(year)) years.Add(year);
                return years;
            }
            public static List<Tuple<DateTime, double>> MonthlyColumn(int year)
            {
                List<Tuple<DateTime, double>> rs = null;
                DateTime date = new DateTime(year, 1, 1);
                using (Muse db = new Muse())
                {
                    if (db.Any<WakaTimeDatas>(x => x.BeginDate.Year == date.Year, null))
                    {
                        var data = db.Do<WakaTimeDatas>().
                                    Where(x => x.BeginDate.Year == date.Year).
                                    GroupBy(x => new { x.BeginDate.Year, x.BeginDate.Month }).
                                    Select(x => new
                                    {
                                        Date = x.Max(y => y.BeginDate),
                                        Duration = x.Sum(y => y.Duration),
                                    }).OrderByDescending(x => x.Duration).ToList();

                        //整理数据库数据
                        if (ListTool.HasElements(data))
                        {
                            rs = new List<Tuple<DateTime, double>>();
                            data.ForEach(x =>
                            {
                                rs.Add(new Tuple<DateTime, double>(x.Date, x.Duration));
                            });
                        }
                        //补充缺失数据
                        if (ListTool.HasElements(rs))
                        {
                            for (int i = 0; i < 12; i++)
                            {
                                var dt = date.AddMonths(i);
                                if (!rs.Any(x => x.Item1.Year == dt.Year && x.Item1.Month == dt.Month))
                                {
                                    rs.Add(new Tuple<DateTime, double>(dt, 0));
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
