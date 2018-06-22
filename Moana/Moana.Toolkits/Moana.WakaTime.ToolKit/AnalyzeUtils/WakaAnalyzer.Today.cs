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
        public static class Today
        {
            /// <summary>
            /// 获取今天时长
            /// </summary>
            public static double Career()
            {
                double career = 0;
                DateTime today = DateTime.Now;
                DateTime todayBegin = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0);
                using (Muse db = new Muse())
                {
                    if (db.Any<WakaTimeDatas>(x => x.BeginDate >= todayBegin, null))
                        career = db.Do<WakaTimeDatas>().Where(x => x.BeginDate >= todayBegin).Sum(x => x.Duration);
                }
                return career;
            }
        }
    }
}
