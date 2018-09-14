using MahApps.Metro.Controls;
using Moana.Commons;
using Moana.Modules.SettingsModule;
using Moana.Modules.WakaTimeModule;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace Moana.Windows.MainWindows
{
    /// <summary>
    /// DashboardWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DashboardWindow : MetroWindow
    {
        bool Closeable = false;
        NotifyIcon NIMain;
        private Dictionary<string, Uri> AllPages = new Dictionary<string, Uri>(); //包含所有页面  
        public DashboardWindow()
        {
            R.Log.v("程序启动");
            AppConfigInitial.Init();

            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            new WakaTask().Start();

            //添加页面的Uri地址，采用相对路径，根路径是项目名,实现allViews的初始化  
            AllPages.Add("BaseChartPage", new Uri("Pages/BaseChartPage.xaml", UriKind.Relative));
            AllPages.Add("MonthChartPage", new Uri("Pages/MonthChartPage.xaml", UriKind.Relative));
            AllPages.Add("YearChartPage", new Uri("Pages/YearChartPage.xaml", UriKind.Relative));
            AllPages.Add("SettingsPage", new Uri("Pages/SettingsPage.xaml", UriKind.Relative));
            MainFrame.Navigate(AllPages["BaseChartPage"]);

            Hide();
            NIMain = new NotifyIcon();
            NIMain.Text = "Moana";
            NIMain.Icon = new Icon(System.Windows.Application.GetResourceStream(new Uri("Images/Icon/Moana.ico", UriKind.Relative)).Stream);
            NIMain.Visible = true;
            //notifyIcon.BalloonTipText = "Hi";
            //notifyIcon.ShowBalloonTip(1000);
            NIMain.DoubleClick += NIMain_DoubleClick;

            MenuItem MIMain = new MenuItem("主界面");
            MIMain.Click += MIMain_Click;
            MenuItem MIExit = new MenuItem("退出");
            MIExit.Click += MIExit_Click;
            MenuItem[] m = new MenuItem[] { MIMain, MIExit };
            NIMain.ContextMenu = new ContextMenu(m);
        }

        #region 菜单和任务栏图标
        private void MIMain_Click(object sender, EventArgs e)
        {
            Show();
            Activate();
            WindowState = WindowState.Normal;
        }
        private void MIExit_Click(object sender, EventArgs e)
        {
            Closeable = true;
            Close();
        }
        private void NIMain_DoubleClick(object sender, EventArgs e)
        {
            Show();
            Activate();
            WindowState = WindowState.Normal;
        }
        #endregion


        #region 主界面按钮
        private void BTBaseChart_Click(object sender, RoutedEventArgs e)
        {
            //Frame类的导航函数，参数时页面的Uri  
            MainFrame.Navigate(AllPages["BaseChartPage"]);
        }
        private void BTMonthChart_Click(object sender, RoutedEventArgs e)
        {
            //Frame类的导航函数，参数时页面的Uri  
            MainFrame.Navigate(AllPages["MonthChartPage"]);
        }
        private void BTYearChart_Click(object sender, RoutedEventArgs e)
        {
            //Frame类的导航函数，参数时页面的Uri  
            MainFrame.Navigate(AllPages["YearChartPage"]);
        }
        private void BTSettings_Click(object sender, RoutedEventArgs e)
        {
            //Frame类的导航函数，参数时页面的Uri  
            MainFrame.Navigate(AllPages["SettingsPage"]);
        }
        #endregion

        #region 窗口事件
        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !Closeable;
            Hide();
        }
        #endregion
    }
}
