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
    /// MonthStackedControl.xaml 的交互逻辑
    /// </summary>
    public partial class MonthProjectStackedControl : UserControl, INotifyPropertyChanged
    {
        Task LoadTask;
        public MonthProjectStackedControl()
        {
            InitializeComponent();
            DataContext = this;

            WakaAnalyzer.Year.AllYears().ForEach(x => { CBYear.Items.Add(x); });
            for (int i = 1; i <= 12; i++) CBMonth.Items.Add(i);
            CBYear.SelectedValue = DateTime.Now.Year;
            CBMonth.SelectedValue = DateTime.Now.Month;
        }
        private void MonthProjectStacked_Loaded(object sender, RoutedEventArgs e)
        {
            if (!R.DesignMode) Refresh();
        }
        private void BTRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (!R.DesignMode) Refresh();
        }
        public void Refresh()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            if (CBYear.SelectedValue != null && CBMonth.SelectedValue != null)
            {
                year = (int)CBYear.SelectedValue;
                month = (int)CBMonth.SelectedValue;
            }

            if (LoadTask == null || LoadTask.IsCompleted)
                LoadTask = Task.Factory.StartNew(() =>
                {
                    var datas = WakaAnalyzer.Month.Report(year, month);
                    if (ListTool.HasElements(datas))
                    {
                        var first = datas.FirstOrDefault();
                        if (first != null && ListTool.HasElements(first.Item2))
                        {
                            List<DateTime> dt = first.Item2.OrderBy(x => x.Item1).Select(x => x.Item1).ToList();
                            List<string> lb = new List<string>();
                            dt.ForEach(x =>
                            {
                                lb.Add(x.ToString("dd日"));
                            });
                            var career = WakaAnalyzer.Month.Career(year, month);
                            var careerFmt = DateTimeTool.ToHMS(career);
                            var avgFmt = DateTimeTool.ToHMS(career / DateTool.MonthDays(year, month));
                            App.Current.Dispatcher.Invoke((Action)(() =>
                            {
                                Labels = lb.ToArray();
                                TimeDesc = $"{year} 年 {month} 月，共计：{careerFmt.Item1} 时 {careerFmt.Item2} 分，日均：{avgFmt.Item1} 时 {avgFmt.Item2} 分";
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
        private string _TimeDesc;
        public string TimeDesc
        {
            get { return _TimeDesc; }
            set { _TimeDesc = value; OnPropertyChanged("TimeDesc"); }
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
