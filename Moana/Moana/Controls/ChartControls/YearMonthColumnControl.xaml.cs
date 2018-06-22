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
    /// YearMonthColumnControl.xaml 的交互逻辑
    /// </summary>
    public partial class YearMonthColumnControl : UserControl, INotifyPropertyChanged
    {
        Task LoadTask;
        public YearMonthColumnControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void YearMonthColumn_Loaded(object sender, RoutedEventArgs e)
        {
            if (!R.DesignMode) Refresh();
        }
        public void Refresh()
        {
            int year = DateTime.Now.Year;
            //if (CBYear.SelectedValue != null )
            //{
            //    year = (int)CBYear.SelectedValue;
            //}

            if (LoadTask == null || LoadTask.IsCompleted)
                LoadTask = Task.Factory.StartNew(() =>
                {
                    var data = WakaAnalyzer.Year.MonthlyColumn(year);
                    if (ListTool.HasElements(data))
                    {
                        List<string> lb = new List<string>();
                        for (int i = 1; i <= 12; i++) lb.Add($"{i}月");
                        App.Current.Dispatcher.Invoke((Action)(() =>
                        {
                            Labels = lb.ToArray();
                        }));

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
                        string title = $"{data.FirstOrDefault().Item1.Year} 年";
                        double[] values = data.OrderBy(y => y.Item1).Select(y => y.Item2).ToArray();

                        App.Current.Dispatcher.Invoke(() =>
                        {
                            SeriesCollection.Add(
                                new ColumnSeries
                                {
                                    Title = title,
                                    Values = new ChartValues<double>(values)
                                });
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
