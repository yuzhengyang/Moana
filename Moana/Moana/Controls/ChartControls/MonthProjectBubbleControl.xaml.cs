using Azylee.Core.DataUtils.CollectionUtils;
using Azylee.Core.DataUtils.DateTimeUtils;
using LiveCharts;
using LiveCharts.Defaults;
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
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Moana.Controls.ChartControls
{
    /// <summary>
    /// MonthBubbleControl.xaml 的交互逻辑
    /// </summary>
    public partial class MonthProjectBubbleControl : UserControl, INotifyPropertyChanged
    {
        Task LoadTask;
        public MonthProjectBubbleControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void MonthBubble_Loaded(object sender, RoutedEventArgs e)
        {
            if (!R.DesignMode) Refresh();
        }
        public void Refresh()
        {
            R.Log.v($"{DateTime.Now} 开始获取项目散点图");
            if (LoadTask == null || LoadTask.IsCompleted)
                LoadTask = Task.Factory.StartNew(() =>
                {
                    R.Log.v($"{DateTime.Now} 开始获取项目散点源数据");
                    DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month-1, 1);
                    var datas = WakaAnalyzer.Month.BubbleReport(dt.Year, dt.Month);

                    if (ListTool.HasElements(datas))
                    {
                        R.Log.v($"{DateTime.Now} 获取项目散点源数据完成，准备开始组装标签");
                        #region 组装标签
                        List<string> lb = new List<string>();
                        for (int i = 0; i < 31; i++)
                        {
                            if (dt.AddDays(i).Month == dt.Month)
                            {
                                lb.Add(dt.AddDays(i).ToString("dd日"));
                            }
                            else break;
                        }
                        App.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            Labels = lb.ToArray();
                        }));
                        #endregion

                        R.Log.v($"{DateTime.Now} 组装标签完成，准备组装Formatter");
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            Formatter = value =>
                            {
                                string rs = $"{value}时";
                                return rs;
                            };
                        });

                        R.Log.v($"{DateTime.Now} 组装Formatter完成，准备组装散点数据");

                        SeriesCollection = new SeriesCollection();
                        datas.ForEach(x =>
                        {
                            if (ListTool.HasElements(x.Item2))
                            {
                                ScatterSeries series = null;

                                App.Current.Dispatcher.Invoke(() =>
                                {
                                    series = new ScatterSeries
                                    {
                                        Title = x.Item1,
                                        Values = new ChartValues<ScatterPoint>()
                                    };
                                });
                                List<ScatterPoint> values = new List<ScatterPoint>();
                                x.Item2.ForEach(y =>
                                { 
                                    values.Add(new ScatterPoint(y.Item1.Day, y.Item1.Hour, y.Item2));
                                });
                                series.Values.AddRange(values);
                                SeriesCollection.Add(series);
                                Thread.Sleep(100);
                            }
                        });


                        //SeriesCollection = new SeriesCollection();
                        //datas.ForEach(x =>
                        //{
                        //    App.Current.Dispatcher.Invoke((Action)(() =>
                        //    {
                        //        if (ListTool.HasElements(x.Item2))
                        //        {
                        //            var series = new ScatterSeries
                        //            {
                        //                Title = x.Item1,
                        //                Values = new ChartValues<ScatterPoint>(),
                        //                MinPointShapeDiameter = 10,
                        //                MaxPointShapeDiameter = 20
                        //            };
                        //            x.Item2.ForEach(y =>
                        //            {
                        //                series.Values.Add(new ScatterPoint(y.Item1.Day, y.Item1.Hour, y.Item2));
                        //            });
                        //            SeriesCollection.Add(series);
                        //        }
                        //    }));
                        //    Thread.Sleep(100);
                        //});
                        R.Log.v($"{DateTime.Now} 组装散点数据完成");
                    }
                    R.Log.v($"{DateTime.Now} 完成项目散点图展示");
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
