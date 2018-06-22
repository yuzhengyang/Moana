using Azylee.Core.IOUtils.DirUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moana.Commons
{
    public static partial class R
    {

        public static class Paths
        {
            public static string App = AppDomain.CurrentDomain.BaseDirectory;
            public static string Data = DirTool.Combine(App, "Data");//应用根目录
        }
    }
}
