using Microsoft.Win32;
using Phenom.Extension;
using Phenom.ProgramMethod;

using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using 诊断工具.Methods;

namespace 诊断工具.Controls.Disks
{
    /// <summary>
    /// ImageWritter.xaml 的交互逻辑
    /// </summary>
    public partial class ImageWritter : UserControl, HelpedAutoLoad
    {
        public string TabName => "镜像写入";

        public string Catalog => "磁盘";

        public string HelpDoc => @"电机按钮，查看当前的CPU详细信息";

        public ImageWritter()
        {
            InitializeComponent();
        }

        private void disk_write_refresh_Click(object sender, RoutedEventArgs e)
        {
            disk_write_disk.ItemsSource = WriteLib.DiskList;
        }

        private void disk_write_browse_img_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Title = "镜像",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };
            if ((bool)dialog.ShowDialog())
            {
                disk_write_image.Text = dialog.FileName;
            }
        }

        private void disk_wrte_Click(object sender, RoutedEventArgs e)
        {
            if (!(disk_write_disk.SelectedItem is DriverInfo info))
            {
                this.Error("请选择磁盘");
                return;
            }
            if (disk_write_image.IsEmpty())
            {
                this.Error("请选择文件");
                return;
            }
            if (!File.Exists(disk_write_image.Text))
            {
                this.Error("文件不存在!");
                return;
            }
            if (!this.Confirm($"即将写入磁盘{info.DispName}{Environment.NewLine}容量:{info.DiskSize.FormatStroageUnit()}文件:{disk_write_image.Text}{Environment.NewLine}磁盘上的所有数据将被删除"))
            {
                this.Tips("操作已经取消!");
                return;
            }
            new Action(() =>
            {
                using (FileStream fs = new FileStream(disk_write_image.Text, FileMode.Open))
                {
                    WriteLib.Write(info.PathName, true, fs, UpdateDiskWriteProgress);
                }
            }).ThreadStart( () =>
            {
                Dispatcher.Invoke(() => disk_wrte.IsEnabled = false);
            },
            () =>
            {
                Dispatcher.Invoke(() => disk_wrte.IsEnabled = true);
            });
        }

        private void UpdateDiskWriteProgress(long Max, long Cur, long Min, string Tips, WorkingMode mode)
        {
            Dispatcher.Invoke(() =>
            {
                if (Tips != null)
                {
                    disk_write_result_run.Text += Tips + Environment.NewLine;
                    disk_write_result_box.ScrollToEnd();
                }
                disk_write_progress.Maximum = Max;
                disk_write_progress.Minimum = Min;
                disk_write_progress.Value = Cur;
                switch (mode)
                {
                    case WorkingMode.Syncing:
                        disk_write_title.Header = "同步";
                        break;

                    case WorkingMode.Verifying:
                        disk_write_title.Header = "校验";
                        break;

                    case WorkingMode.Writting:
                        disk_write_title.Header = "写入";
                        break;
                }
            });
        }
    }
}