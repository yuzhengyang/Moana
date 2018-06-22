using Azylee.Core.IOUtils.TxtUtils;
using Moana.Commons;
using System;
using System.Collections.Generic;
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

namespace Moana.Pages
{
    /// <summary>
    /// SettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            TBWakaTimeKey.Text = R.WakaTimeKey;
        }

        private void BTSave_Click(object sender, RoutedEventArgs e)
        {
            //IniTool.Set(R.Files.Settings,"Wakatime","ApiKey",);
            ConfigTool.Set("WakaTimeKey", TBWakaTimeKey.Text);
        }
    }
}
