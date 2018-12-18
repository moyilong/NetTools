using Phenom.ProgramMethod;
using Phenom.WPF.Extension;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using 诊断工具.Methods.Rename;
using Path = System.IO.Path;

namespace 诊断工具.Controls.Disks
{
    /// <summary>
    /// BatchRename.xaml 的交互逻辑
    /// </summary>
    public partial class BatchRename : UserControl,AutoLoadTemplate
    {
        public string TabName => "批量重命名";

        public string Catalog => "磁盘";

        public BatchRename()
        {
            InitializeComponent();
            rename_list.ItemsSource = new ObservableCollection<RenameOperator>();
        }


        private void Execute_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Add_floder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;
            ObservableCollection<RenameOperator> collect = rename_list.ItemsSource as ObservableCollection<RenameOperator>;

            string path = dialog.SelectedPath;
            rename_list.ItemsSource = collect;
            Async.NoneWaitStart(() =>
            {
                foreach (var i in Directory.GetFiles(path))
                    Dispatcher.Invoke(() =>
                    {
                        collect.Add(new RenameOperator(new FileInfo(Path.Combine(path, i))));
                    });

            });
        }

        private void Preview_rename_Click(object sender, RoutedEventArgs e)
        {

            Regex reg = null;
            Regex ireg = null;
            try
            {
                reg = new Regex(regex.Text.Trim());
                ireg = new Regex(input_regex.Text.Trim());
            }
            catch (Exception ex)
            {
                this.Error(ex, "解析正则表达式失败");
                return;
            }
            RenameOperator.UpdateRegexArray(regex.Text.Trim(), input_regex.Text.Trim(), (rename_list.ItemsSource as ObservableCollection<RenameOperator>).ToArray());

        }
    }
}
