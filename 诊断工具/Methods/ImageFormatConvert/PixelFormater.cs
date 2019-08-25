using System.Drawing;
using System.Text;

namespace 诊断工具.Methods.ImageFormatConvert
{
    internal class PixelFormater : IFormatConverter
    {
        public string DisplayName => "像素采样";

        public string ImageProcess(Bitmap img)
        {
            StringBuilder ret = new StringBuilder();
            ret.Append($"{img.Width};{img.Height};");
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                    ret.Append($"{ColorTranslator.ToHtml(img.GetPixel(x, y))};");
                ret.Append("\n");
            }
            return ret.ToString();
        }

        public Bitmap LoadFrom(string data)
        {
            string[] array = data.Split(';');
            Bitmap map = new Bitmap(int.Parse(array[0]), int.Parse(array[1]));
            for (int y = 0; y < map.Height; y++)
                for (int x = 0; x < map.Width; x++)

                    map.SetPixel(x, y, ColorTranslator.FromHtml(array[map.Width * y + 2 + x]));
            return map;
        }

        override public string ToString()
              => DisplayName;
    }
}