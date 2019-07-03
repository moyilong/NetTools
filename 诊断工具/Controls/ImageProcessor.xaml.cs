using Microsoft.Win32;
using Phenom.Extension;
using Phenom.UI;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using 诊断工具.Methods.ImageFormatConvert;

namespace 诊断工具.Controls
{
    /// <summary>
    /// YSImageProcessor.xaml 的交互逻辑
    /// </summary>
    public partial class ImageProcessor : UserControl, AutoLoadTemplate, HelpedAutoLoad
    {
  readonly      IFormatConverter[] converter = new IFormatConverter[]
        {
            new PixelFormater()
        };
        public ImageProcessor()
        {
            InitializeComponent();
            timer.Elapsed += Update;
            convert_type.ItemsSource = converter;
        }
        double Filter => vertex.Value;

        public string TabName => "Logo转换";

        public string Catalog => "图形图像";

        public string HelpDoc => @"
首先打开Logo源文件(jpg/png/bmp格式)
然后根据预览，调整阈值
最后保存为图片文件
";

        Color BlackConvert(Color color)
        {
            if (color.GetBrightness() > Filter)
                return Color.White;
            else
                return Color.Black;
        }

        private void Process_Click(object sender, RoutedEventArgs e)
        {
            if (source.Source != null)
                using (MemoryStream ms = new MemoryStream())
                {
                    source.Source.ToImage().Save(ms, ImageFormat.Bmp);
                    Bitmap image = new Bitmap(ms);
                    Bitmap to_image = new Bitmap(image.Width, image.Height);
                    for (int x = 0; x < image.Width; x++)
                        for (int y = 0; y < image.Height; y++)
                            to_image.SetPixel(x, y, BlackConvert(image.GetPixel(x, y)));
                    target.Source = to_image.ToSource();
                }
        }

        bool NeedUpdate = false;

        void Update(object sender, EventArgs e)
        {
            if (NeedUpdate)
                Dispatcher.Invoke(() => Process_Click(null, null));
        }
    readonly    System.Timers.Timer timer = new System.Timers.Timer
        {
            Interval = 1000
        };
        private void Open_source_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                Title = "打开图片",
                CheckFileExists = true,
                CheckPathExists = true
            };
            if (!(bool)dialog.ShowDialog())
                return;
            try

            {
                source.Source = System.Drawing.Image.FromFile(dialog.FileName).ToSource();
                Process_Click(null, null);
            }
            catch (Exception ex)
            {
                this.Error(ex, "打开图片失败");
            }

        }

        private void Save_source_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Title = "保存图片",
                CheckPathExists = true
            };
            if (!(bool)dialog.ShowDialog())
                return;
            try
            {
                target.Source.ToImage().Save(dialog.FileName);
            }
            catch (Exception ex)
            {
                this.Error(ex);
            }
        }

        private void Vertex_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            NeedUpdate = true;
        }

        private void Import_to_clipboard_Click(object sender, RoutedEventArgs e)
        {
            if (!(convert_type.SelectedItem is IFormatConverter convert))
            {
                this.Error("请选择采样类型");
                return;
            }
            try
            {
                source.Source = convert.LoadFrom(Clipboard.GetText()).ToSource();
            }
            catch (Exception ex)
            {
                this.Error(ex);
            }
        }

        private void Export_to_clipboard_Click(object sender, RoutedEventArgs e)
        {
            if (!(convert_type.SelectedItem is IFormatConverter convert))
            {
                this.Error("请选择采样类型");
                return;
            }
            if (target.Source == null)
            {
                this.Error("请选择图片");
                return;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                target.Source.ToImage().Save(ms, ImageFormat.Bmp);
                Bitmap image = new Bitmap(ms);
                Clipboard.SetText(convert.ImageProcess(image));
            }
        }
    }
}