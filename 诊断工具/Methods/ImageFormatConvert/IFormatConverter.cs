using System.Drawing;

namespace 诊断工具.Methods.ImageFormatConvert
{
    public interface IFormatConverter
    {
        string DisplayName { get; }

        string ImageProcess(Bitmap img);

        Bitmap LoadFrom(string data);

        string ToString();
    }
}