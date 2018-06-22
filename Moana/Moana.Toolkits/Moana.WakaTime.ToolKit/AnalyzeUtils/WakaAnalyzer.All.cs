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
        public static class All
        {
            /// <summary>
            /// 获取编程总时长
            /// </summary>
            /// <returns></returns>
            public static double Career()
            {
                double career = 0;
                using (Muse db = new Muse())
                {
                    if (db.Any<WakaTimeDatas>(x => x.ID != null, null))
                        career = db.Do<WakaTimeDatas>().Sum(x => x.Duration);
                }
                return career;
            }
            /// <summary>
            /// 统计所有编程语言使用占用情况
            /// </summary>
            /// <param name="userId"></param>
            /// <returns></returns>
            public static List<Tuple<string, double>> LanguageCount()
            {
                List<Tuple<string, double>> rs = null;
                using (Muse db = new Muse())
                {
                    if (db.Any<WakaTimeDatas>(x => x.ID != null, null))
                    {
                        var languageGroup = db.Do<WakaTimeDatas>().Where(x => x.Language != null).GroupBy(x => x.Language).
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
        }
    }
}
