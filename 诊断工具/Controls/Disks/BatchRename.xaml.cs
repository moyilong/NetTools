﻿using Phenom.Extension;
using Phenom.UI;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using 诊断工具.Methods.Rename;
using Path = System.IO.Path;

namespace 诊断工具.Controls.Disks
{
    /// <summary>
    /// BatchRename.xaml 的交互逻辑
    /// </summary>
    [AutoLoadTemplate(Catalog = AutoLoadTemplate.CateLogType.Disk, TabName = "批量重命名")]
    public partial class BatchRename : UserControl
    {
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
            using (System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                ObservableCollection<RenameOperator> collect = rename_list.ItemsSource as ObservableCollection<RenameOperator>;

                string path = dialog.SelectedPath;
                rename_list.ItemsSource = collect;
                new Action(() =>
                {
                    foreach (string i in Directory.GetFiles(path))
                    {
                        Dispatcher.Invoke(() =>
                        {
                            collect.Add(new RenameOperator(new FileInfo(Path.Combine(path, i))));
                        });
                    }
                }).ThreadStart();
            }
        }

        private void Preview_rename_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Regex reg = new Regex(regex.Text.Trim());
                Regex ireg = new Regex(input_regex.Text.Trim());
            }
            catch (Exception ex)
            {
                this.Error(ex, "解析正则表达式失败");
                return;
            }
            RenameOperator.UpdateRegexArray(regex.Text.Trim(), input_regex.Text.Trim(), (rename_list.ItemsSource as ObservableCollection<RenameOperator>).ToArray());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}