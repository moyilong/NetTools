using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
