using Azylee.Core.DataUtils.CollectionUtils;
using LiveCharts;
using LiveCharts.Wpf;
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
    /// LanguagePieControl.xaml 的交互逻辑
    /// </summary>
    public partial class WeekLanguagePieControl : UserControl, INotifyPropertyChanged
    {
        Task LoadTask;
        private SeriesCollection _Series;
        public SeriesCollection Series
        {
            get { return _Series; }
            set { _Series = value; OnPropertyChanged("Series"); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public WeekLanguagePieControl()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void WeekLanguagePie_Loaded(object sender, RoutedEventArgs e)
        {
            if (!R.DesignMode) Refresh();
        }
        public void Refresh()
        {
            if (LoadTask == null || LoadTask.IsCompleted)
                Task.Factory.StartNew(() =>
                {
                    var data = WakaAnalyzer.Recent7.LanguageCount();//获取所有语言统计信息
                    if (ListTool.HasElements(data))
                    {
                        Series = new SeriesCollection();
                        data.ForEach(x =>
                        {
                            App.Current.Dispatcher.Invoke((Action)(() =>
                            {
                                var p = new PieSeries
                                {
                                    Values = new ChartValues<double> { x.Item2 },
                                    Title = x.Item1
                                };
                                Series.Add(p);
                            }));
                        });
                    }
                });
        }
    }
}
