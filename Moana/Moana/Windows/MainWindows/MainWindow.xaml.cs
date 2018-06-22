using MahApps.Metro.Controls;
using Moana.Commons;
using Moana.Modules.SettingsModule;
using Moana.Modules.WakaTimeModule;
using System.Windows;

namespace Moana.Windows.MainWindows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            R.Log.v("程序启动");

            WakaTask wakaTask = new WakaTask();
            wakaTask.Start();

            //string szTmp = "http://www.baidu.com";
            //Uri uri = new Uri(szTmp);
            //YearReport.Navigate(uri);
        }
    }
}
