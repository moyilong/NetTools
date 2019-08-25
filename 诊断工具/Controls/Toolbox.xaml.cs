using System.Windows;
using System.Windows.Controls;
using 诊断工具.Methods;

namespace 诊断工具.Controls
{
    /// <summary>
    /// Toolbox.xaml 的交互逻辑
    /// </summary>
    [AutoLoadTemplate(Catalog = AutoLoadTemplate.CateLogType.System,TabName = "工具箱")]
    public partial class Toolbox : UserControl
    {
        public Toolbox()
        {
            InitializeComponent();
            bool[,] Map = new bool[toolbox.ColumnDefinitions.Count, toolbox.RowDefinitions.Count];
            foreach (System.Collections.Generic.KeyValuePair<string, System.Action> i in CommandGroup.CMDS)
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
                    {
                        if (Map[n, x] == default)
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
                    }

                    if (success)
                    {
                        break;
                    }
                }
            }
        }

        public string TabName => "工具箱";

        public string Catalog => null;
    }
}