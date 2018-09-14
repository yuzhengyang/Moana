using Azylee.Core.DataUtils.StringUtils;
using Azylee.Core.TaskUtils;
using Moana.Commons;
using Moana.WakaTime.Models.WebModels;
using Moana.WakaTime.ToolKit.SpiderUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Moana.Modules.WakaTimeModule
{
    public class WakaTask : TaskSupport
    {
        public override void BeforeTODO()
        {
            //SetInterval(60 * 60 * 1000);
        }
        public override void TODO()
        {
            if (Str.Ok(R.WakaTimeKey))
            {
                for (int i = 0; i < 15; i++)
                {
                    R.Log.v(string.Format("正在同步 WakaTime 数据，日期：{0}", DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd")));

                    int count = WakaSpider.GetDatas(R.WakaTimeKey, DateTime.Now.AddDays(-i));

                    R.Log.v(string.Format("完成同步 WakaTime 数据，日期：{0}，条数：{1}", DateTime.Now.AddDays(-i).ToString("yyyy-MM-dd"), count));
                    Thread.Sleep(30 * 1000);
                }
                Thread.Sleep(60 * 60 * 1000);
            }
        }
    }
}
