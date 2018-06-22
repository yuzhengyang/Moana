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
        public static int MAX_PROJECT { get { return 8; } }
        public static int MAX_LANGUAGE { get { return 10; } }
        public static double MinDuration { get { return 0.01; } }
    }
}
