using Azylee.Core.DataUtils.DateTimeUtils;
using Moana.Commons;
using Moana.WakaTime.ToolKit.AnalyzeUtils;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Moana.Controls.ChartControls
{
    /// <summary>
    /// TodayGaugeControl.xaml 的交互逻辑
    /// </summary>
    public partial class TodayGaugeControl : UserControl, INotifyPropertyChanged
    {
        Task LoadTask; 
        public TodayGaugeControl()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void TodayGaugeControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!R.DesignMode) Refresh();
        }
        public void Refresh()
        {
            if (LoadTask == null || LoadTask.IsCompleted)
                Task.Factory.StartNew(() =>
                {
                    var today = WakaAnalyzer.Today.Career();//获取当天时长
                    var last7days = WakaAnalyzer.Recent7.Career();//获取最近7天时长
                    var avg = last7days / 7;//获取7日平均

                    var todayFormat = DateTimeTool.ToHMS(today);
                    var last7daysFormat = DateTimeTool.ToHMS(last7days);
                    var avgFormat = DateTimeTool.ToHMS(avg);

                    double increase = today * 100 / (last7days / 7);

                    App.Current.Dispatcher.Invoke((Action)(() =>
                    {
                        GaugeValue = (int)(increase > 100 ? 100 : increase);
                        TodayTimeContent = $"今天 {todayFormat.Item1} 小时 {todayFormat.Item2} 分钟";
                        IncreaseContent = increase >= 100 ? $"{(int)increase}% ＋" : $"{100 - (int)increase}% －";
                        DailyAverageContent = $"7日 平均 {avgFormat.Item1} 小时 {avgFormat.Item2} 分钟";
                    }));
                });
        }
        private int _GaugeValue;
        public int GaugeValue
        {
            get { return _GaugeValue; }
            set
            {
                _GaugeValue = value; OnPropertyChanged("GaugeValue");
            }
        }
        private string _TodayTimeContent;
        public string TodayTimeContent
        {
            get { return _TodayTimeContent; }
            set { _TodayTimeContent = value; OnPropertyChanged("TodayTimeContent"); }
        }
        private string _IncreaseContent;
        public string IncreaseContent
        {
            get { return _IncreaseContent; }
            set { _IncreaseContent = value; OnPropertyChanged("IncreaseContent"); }
        }
        private string _DailyAverageContent;
        public string DailyAverageContent
        {
            get { return _DailyAverageContent; }
            set { _DailyAverageContent = value; OnPropertyChanged("DailyAverageContent"); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
