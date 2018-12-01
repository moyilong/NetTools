using Phenom.Logger;
using Phenom.WPF.Extension;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Shapes;

namespace 诊断工具
{
    /// <summary>
    /// ScreenTest.xaml 的交互逻辑
    /// </summary>
    public partial class ScreenTest : Window
    {
        public ScreenTest()
        {
            InitializeComponent();
            WindowState = WindowState.Normal;
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;
            Topmost = true;
            Left = 0.0;
            Top = 0.0;
            Width = SystemParameters.PrimaryScreenWidth;
            Height = SystemParameters.PrimaryScreenHeight;
            image = new Bitmap((int)Width, (int)Height);
            gpu = Graphics.FromImage(image);
            image_MouseClick(null, null);
        }
        System.Drawing.Image  image = null;
        Graphics gpu = null;
        int Step = -1;
        void Fill(System.Drawing.Brush color)
        {
            gpu.FillRectangle(color, new System.Drawing.Rectangle(0, 0, (int)Width,(int) Height));
            UpdateImage();
        }
        DebugNode node = new DebugNode("ScreenTester");
        void FillGrayLevel(System.Drawing.Color color,int level)
        {
            float step = (float)image.Width / level;

            //int c_step = 255 / level;
            int r_step = color.R == 0 ? 0 : 256 / level;
            int g_step = color.G == 0 ? 0 : 256 / level;
            int b_step = color.B == 0 ? 0 : 256 / level;
            node.Push($"共{level}级{step}步长 R:{r_step} G:{g_step} B:{b_step}");
            for (int n = 0; n < level ; n++)
            {
                var vcolor = System.Drawing.Color.FromArgb(255, r_step * n, g_step * n, b_step * n);
                var vrect = new System.Drawing.Rectangle((int)(n * step), 0,(int) step+5, image.Height);
                node.Push($"绘制:{n} R={vcolor.R} G={vcolor.G} B={vcolor.B} A={vcolor.A} Rect={vrect.X},{vrect.Y},{vrect.Width},{vrect.Height}"); 
                gpu.FillRectangle(new SolidBrush(vcolor), vrect);
            }
            UpdateImage(); 
        }
        void UpdateImage()
        {
            Background = new ImageBrush(image.ToSource());
        }
        System.Drawing.Brush[] IntelliDefaultFill = new System.Drawing.Brush[]
        {
            System.Drawing.Brushes.White,
            System.Drawing.Brushes.Black,
            System.Drawing.Brushes.Red,
            System.Drawing.Brushes.Green,
            System.Drawing.Brushes.Blue,
            System.Drawing.Brushes.Yellow,
        };
 

        private void image_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            Step++;
            switch (Step)
            {
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                    System.Drawing.Color color = System.Drawing.Color.White;
                    int level = Step - 5;
                    if (Step == 9 || Step == 10 || Step == 11)
                    {
                        color = System.Drawing.Color.Red;
                        level = Step - 8;
                    }
                    else if (Step == 12 || Step == 13 || Step == 14)
                    {
                        color = System.Drawing.Color.Green;
                        level = Step - 11;
                    }
                    else if (Step == 15 || Step == 16 || Step == 17)
                    {
                        color = System.Drawing.Color.Blue;
                        level = Step - 14;
                    }
                    level = (int)Math.Pow(2, 4 + level);
                    current_item.Content = $"{level}级灰阶";
                    FillGrayLevel(color, level);
                    break;
                default:
                    if (Step < IntelliDefaultFill.Length)
                    {
                        Fill(IntelliDefaultFill[Step]);
                        current_item.Content = $"静态图形:{Step+1}/{IntelliDefaultFill.Length}";
                    }
                    else
                    {
                        this.Tips("测试已完成!");
                        Close();
                    }
                    break;
            }
        }
        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            image_MouseClick(null, null);
        }
    }
}
