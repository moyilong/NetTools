using Microsoft.Win32;
using Phenom.WPF.Extension;
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

namespace 诊断工具.Controls.ADB
{
    /// <summary>
    /// ADB.xaml 的交互逻辑
    /// </summary>
    public partial class ADBCtl : UserControl
    {
        public ADBCtl()
        {
            InitializeComponent();
        }

        private void install_apk_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false,
            };
            if (!(bool)dialog.ShowDialog())
                return;
            this.Tips(ADBBasic.InstallAPK(dialog.FileName));
        }

        private void adb_reboot_Click(object sender, RoutedEventArgs e)
        {
            ADBBasic.Reboot( ADBBasic.RebootMode.Normally);
        }

        private void adb_reboot_fastboot_Click(object sender, RoutedEventArgs e)
        {
            ADBBasic.Reboot(ADBBasic.RebootMode.Fastboot);
        }

        private void adb_reboot_recovery_Click(object sender, RoutedEventArgs e)
        {
            ADBBasic.Reboot(ADBBasic.RebootMode.Recovery);
        }

        private void install_adb_Click(object sender, RoutedEventArgs e)
        {
            ADBBasic.InstallADB();
        }

        private void restart_adb_services_Click(object sender, RoutedEventArgs e)
        {
            ADBBasic.RestartServices();
        }

        private void refresh_adb_Click(object sender, RoutedEventArgs e)
        {
            adb_devices.ItemsSource = ADBBasic.DeviceList;
        }

        private void connect_adb_Click(object sender, RoutedEventArgs e)
        {
            this.Tips(ADBBasic.Connect(connect_string.Text.Trim()));
        }
    }
}
