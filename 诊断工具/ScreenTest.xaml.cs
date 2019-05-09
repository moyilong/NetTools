using Tahiti.Extension;
using Tahiti.Logger;

using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;
using Rectangle = System.Drawing.Rectangle;

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

        private System.Drawing.Image image = null;
        private Graphics gpu = null;
        private int Step = -1;

        private void Fill(System.Drawing.Brush color)
        {
            gpu.FillRectangle(color, new Rectangle(0, 0, (int)Width, (int)Height));
            UpdateImage();
        }

        private const string node = "ScreenTester";

        private enum RenderMode
        {
            Vertex,
            Hornorlize
        }

        private void FillGrayLevel(Color color, int level, RenderMode mode)
        {
            float step = 0;
            switch (mode)
            {
                case RenderMode.Hornorlize:
                    step = (float)image.Width / level;
                    break;

                case RenderMode.Vertex:
                    step = (float)image.Height / level;
                    break;
            }

            //int c_step = 255 / level;
            int r_step = color.R == 0 ? 0 : 256 / level;
            int g_step = color.G == 0 ? 0 : 256 / level;
            int b_step = color.B == 0 ? 0 : 256 / level;
            node.Note($"共{level}级{step}步长 R:{r_step} G:{g_step} B:{b_step}");
            for (int n = 0; n < level; n++)
            {
                Color vcolor = Color.FromArgb(255, r_step * n, g_step * n, b_step * n);

                Rectangle vrect = new Rectangle();
                switch (mode)
                {
                    case RenderMode.Hornorlize:
                        vrect = new Rectangle((int)(n * step), 0, (int)step + 5, image.Height);
                        break;

                    case RenderMode.Vertex:
                        vrect = new Rectangle(0, (int)(n * step), image.Width, (int)step + 5);
                        break;
                }

                node.Note($"绘制:{n} R={vcolor.R} G={vcolor.G} B={vcolor.B} A={vcolor.A} Rect={vrect.X},{vrect.Y},{vrect.Width},{vrect.Height}");
                gpu.FillRectangle(new SolidBrush(vcolor), vrect);
            }
            UpdateImage();
        }

        private void UpdateImage()
        {
            Background = new ImageBrush(image.ToSource());
        }

        private System.Drawing.Brush[] IntelliDefaultFill = new System.Drawing.Brush[]
        {
            System.Drawing.Brushes.White,
            System.Drawing.Brushes.Black,
            System.Drawing.Brushes.Red,
            System.Drawing.Brushes.Green,
            System.Drawing.Brushes.Blue,
            System.Drawing.Brushes.AntiqueWhite
        };

        private enum SublineMode
        {
            Vertex,
            Hornorlize,
            VertexAndHornorlize
        }

        private void PowerSubline(SublineMode mode, int dest)
        {
            gpu.FillRectangle(System.Drawing.Brushes.Black, new Rectangle(0, 0, image.Width, image.Height));
            if (mode == SublineMode.Vertex || mode == SublineMode.VertexAndHornorlize)
            {
                for (int n = 0; n < image.Height; n += dest)
                {
                    gpu.DrawLine(Pens.White, new Point(0, n), new Point(image.Width, n));
                }
            }

            if (mode == SublineMode.Hornorlize || mode == SublineMode.VertexAndHornorlize)
            {
                for (int n = 0; n < image.Width; n += dest)
                {
                    gpu.DrawLine(Pens.White, new Point(n, 0), new Point(n, image.Height));
                }
            }

            UpdateImage();
        }

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
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:
                case 27:
                case 28:
                case 29:
                    RenderMode mode = RenderMode.Hornorlize;
                    if (Step > 17)
                    {
                        Step -= 12;
                        mode = RenderMode.Vertex;
                    }
                    Color color = Color.White;
                    int level = Step - 5;
                    if (Step == 9 || Step == 10 || Step == 11)
                    {
                        color = Color.Red;
                        level = Step - 8;
                    }
                    else if (Step == 12 || Step == 13 || Step == 14)
                    {
                        color = Color.Green;
                        level = Step - 11;
                    }
                    else if (Step == 15 || Step == 16 || Step == 17)
                    {
                        color = Color.Blue;
                        level = Step - 14;
                    }
                    level = (int)Math.Pow(2, 4 + level);
                    current_item.Content = $"{level}级灰阶";
                    FillGrayLevel(color, level, mode);
                    if (mode == RenderMode.Vertex)
                    {
                        Step += 12;
                    }

                    break;

                case 30:
                    PowerSubline(SublineMode.Hornorlize, 2);
                    current_item.Content = "线条测试";
                    break;

                case 31:
                    PowerSubline(SublineMode.Vertex, 2);
                    current_item.Content = "线条测试";
                    break;

                case 32:
                    PowerSubline(SublineMode.VertexAndHornorlize, 2);
                    current_item.Content = "线条测试";
                    break;

                case 33:
                    PowerSubline(SublineMode.VertexAndHornorlize, 50);
                    current_item.Content = "矩形";
                    break;

                default:
                    if (Step < IntelliDefaultFill.Length)
                    {
                        Fill(IntelliDefaultFill[Step]);
                        current_item.Content = $"静态图形:{Step + 1}/{IntelliDefaultFill.Length}";
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