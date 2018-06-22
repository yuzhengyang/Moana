using Azylee.Core.LogUtils;
using Azylee.Core.LogUtils.SimpleLogUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Moana.Commons
{
    public static partial class R
    {
        public static string WakaTimeKey = "";
        public static Log Log = new Log(true);

        /// <summary>
        /// 当前是否为设计模式
        /// </summary>
        public static bool DesignMode
        {
            get { return (bool)DesignerProperties.IsInDesignModeProperty.GetMetadata(typeof(DependencyObject)).DefaultValue; }
        }
    }
}
