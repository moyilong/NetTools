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

namespace 诊断工具.Controls
{
    /// <summary>
    /// Toolbox.xaml 的交互逻辑
    /// </summary>
    public partial class Toolbox : UserControl
    {
        public Toolbox()
        {
            InitializeComponent();
            bool[,] Map = new bool[toolbox.ColumnDefinitions.Count, toolbox.RowDefinitions.Count];
            foreach (var i in CommandGroup.CMDS)
            {
                Button btn = new Button()
                {
                    Content = i.Key,
                };
                btn.Click += (e, s) => i.Value();
                for (int n = 0; n < toolbox.ColumnDefinitions.Count; n++)
                {
                    bool success = false;
                    for (int x = 0; x < toolbox.RowDefinitions.Count; x++)
                        if (Map[n, x] == default(bool))
                        {
                            Map[n, x] = !Map[n, x];
                            btn.HorizontalAlignment = HorizontalAlignment.Stretch;
                            btn.VerticalAlignment = VerticalAlignment.Stretch;
                            Grid.SetRow(btn, x);
                            Grid.SetColumn(btn, n);
                            toolbox.Children.Add(btn);
                            success = true;
                            break;
                        }
                    if (success)
                        break;
                }
            }
        }
    }
}
