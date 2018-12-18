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
using 诊断工具.Methods;

namespace 诊断工具.Controls.Networks
{
    /// <summary>
    /// DiagonStick.xaml 的交互逻辑
    /// </summary>
    public partial class DiagonStick : UserControl,AutoLoadTemplate,HelpedAutoLoad
    {
        public DiagonStick()
        {
            InitializeComponent();
        }

        public string TabName => "网络诊断";

        public string Catalog => "网络";

        public string HelpDoc => "进行网络连接诊断";

        private void diagonstick_Click(object sender, RoutedEventArgs e)
        {
            diagon_result.ItemsSource = Diagon.RunResult(Application.Current.MainWindow as MainWindow,
                () => Dispatcher.Invoke(() => diagonstick.IsEnabled = false),
                () => Dispatcher.Invoke(() => diagonstick.IsEnabled = true)
                );
        }

    }
}
