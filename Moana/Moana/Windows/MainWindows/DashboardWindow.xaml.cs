using MahApps.Metro.Controls;
using Moana.Commons;
using Moana.Modules.SettingsModule;
using Moana.Modules.WakaTimeModule;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Moana.Windows.MainWindows
{
    /// <summary>
    /// DashboardWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DashboardWindow : MetroWindow
    {
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
        }

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
    }
}
