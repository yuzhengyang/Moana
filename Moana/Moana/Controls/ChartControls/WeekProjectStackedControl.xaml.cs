using Azylee.Core.DataUtils.CollectionUtils;
using Azylee.Core.DataUtils.DateTimeUtils;
using LiveCharts;
using LiveCharts.Wpf;
using Moana.Commons;
using Moana.WakaTime.ToolKit.AnalyzeUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Moana.Controls.ChartControls
{
    /// <summary>
    /// WeekStackedControl.xaml 的交互逻辑
    /// </summary>
    public partial class WeekProjectStackedControl : UserControl, INotifyPropertyChanged
    {
        Task LoadTask;
        public WeekProjectStackedControl()
        {
            InitializeComponent();
            DataContext = this;
        }
        private void WeekProjectStacked_Loaded(object sender, RoutedEventArgs e)
        {
            if (!R.DesignMode) Refresh();
        }
        public void Refresh()
        {
            if (LoadTask == null || LoadTask.IsCompleted)
                LoadTask = Task.Factory.StartNew(() =>
                {
                    var datas = WakaAnalyzer.Recent7.Report();
                    if (ListTool.HasElements(datas))
                    {
                        var first = datas.FirstOrDefault();
                        if (first != null && ListTool.HasElements(first.Item2))
                        {
                            List<DateTime> dt = first.Item2.OrderBy(x => x.Item1).Select(x => x.Item1).ToList();
                            List<string> lb = new List<string>();
                            dt.ForEach(x =>
                            {
                                lb.Add(x.ToString("MM月dd日"));
                            });
                            App.Current.Dispatcher.Invoke((Action)(() =>
                            {
                                Labels = lb.ToArray();
                            }));
                        }

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            Formatter = value =>
                            {
                                if (value > 0)
                                {
                                    StringBuilder sb = new StringBuilder();
                                    var time = DateTimeTool.ToHMS(value);
                                    if (time.Item1 > 0) sb.Append($"{time.Item1} 小时");
                                    if (time.Item2 > 0) sb.Append($" {time.Item2} 分钟");
                                    return sb.ToString();
                                }
                                return null;
                            };
                        });

                        SeriesCollection = new SeriesCollection();
                        datas.ForEach(x =>
                        {
                            string title = x.Item1;
                            double[] values = x.Item2.OrderBy(y => y.Item1).Select(y => y.Item2).ToArray();

                            App.Current.Dispatcher.Invoke((Action)(() =>
                            {
                                SeriesCollection.Add(
                                new StackedColumnSeries
                                {
                                    Title = title,
                                    Values = new ChartValues<double>(values),
                                    StackMode = StackMode.Values
                                });
                            }));
                        });
                    }
                });
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private SeriesCollection _SeriesCollection;
        public SeriesCollection SeriesCollection
        {
            get { return _SeriesCollection; }
            set { _SeriesCollection = value; OnPropertyChanged("SeriesCollection"); }
        }
        private string[] _Labels;
        public string[] Labels
        {
            get { return _Labels; }
            set { _Labels = value; OnPropertyChanged("Labels"); }
        }
        private Func<double, string> _Formatter;
        public Func<double, string> Formatter
        {
            get { return _Formatter; }
            set { _Formatter = value; OnPropertyChanged("Formatter"); }
        }
    }
}
