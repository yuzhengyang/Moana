using Azylee.Core.IOUtils.TxtUtils;
using Moana.Commons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moana.Modules.SettingsModule
{
    public class AppConfigInitial
    {
        public static void Init()
        {
            R.WakaTimeKey = ConfigTool.Get("WakaTimeKey", "");
        }
    }
}
