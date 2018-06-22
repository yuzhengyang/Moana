using Azylee.Core.DataUtils.DateTimeUtils;
using Moana.Commons;
using Moana.WakaTime.ToolKit.AnalyzeUtils;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Moana.Controls.ChartControls
{
    /// <summary>
    /// StatusCountControl.xaml 的交互逻辑
    /// </summary>
    public partial class StatusCountControl : UserControl
    {
        Task LoadTask;
        public StatusCountControl()
        {
            InitializeComponent();
        }

        private void StatusCount_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!R.DesignMode) Refresh();
        }
        public void Refresh()
        {
            if (LoadTask == null || LoadTask.IsCompleted)
                Task.Factory.StartNew(() =>
                {
                    //获取编程总时长
                    var CareerTime = DateTimeTool.ToHMS(WakaAnalyzer.All.Career());
                    //统计当月总时长
                    var MonthCareerTime = DateTimeTool.ToHMS(WakaAnalyzer.Month.Career(DateTime.Now.Year, DateTime.Now.Month));
                    //获取当天时长
                    var TodayCareerTime = DateTimeTool.ToHMS(WakaAnalyzer.Today.Career());
                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        LBCareer.Content = $"总计：{CareerTime.Item1} 小时 {CareerTime.Item2} 分钟";
                        LBMonthCareer.Content = $"本月：{MonthCareerTime.Item1} 小时 {MonthCareerTime.Item2} 分钟";
                        LBTodayCareer.Content = $"当天：{TodayCareerTime.Item1} 小时 {TodayCareerTime.Item2} 分钟";
                    }));
                });
        }
    }
}
