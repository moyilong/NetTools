using Phenom.UI;
using Phenom.UI.Controls;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace 诊断工具.Controls.Generic
{
    /// <summary>
    /// CAPBiosSpliter.xaml 的交互逻辑
    /// </summary>
    [AutoLoadTemplate(Catalog = AutoLoadTemplate.CateLogType.System, TabName = "CAPBios分离")]
    public partial class CAPBiosSpliter : UserControl
    {
        static readonly int[] biosLength =
      {
            1024*1024,
            2048*1024,
            4096*1024,
            8192*1024,
            16384*1024,
            32768*1024,
        };
        public CAPBiosSpliter()
        {
            InitializeComponent();
        }

        byte[] CAPData = null;
        byte[] RAWData = null;

        private void select_file_OnFileSelected(object sender, SelectFileDialog.SelectFileEventArgs e)
        {
            StringBuilder information = new StringBuilder();
            cap_file.Text = e.FileName;
            try
            {
                using (FileStream stream = File.OpenRead(e.FileName))
                {
                    CAPData = new byte[stream.Length];
                    stream.Read(CAPData, 0, CAPData.Length);
                    information.AppendLine($"文件:{e.FileName}");
                    information.AppendLine($"原始数据长度:{stream.Length}");
                    int realsize = 0;
                    foreach (int i in biosLength)
                        if (CAPData.Length > i)
                            realsize = i;
                        else
                            break;
                    information.AppendLine($"匹配BIOS大小:{realsize / 1024 / 1024} MB");
                    RAWData = new byte[realsize];
                    for (int n = 0; n < realsize; n++)
                        RAWData[n] = CAPData[n + (stream.Length - realsize)];
                    information.AppendLine($"分离成功!");
                }
            }
            catch (Exception ex)
            {
                information.AppendLine("失败!");
                information.AppendLine(ex.ToString());
            }
            mesg.Text = information.ToString();
        }

        private void save_dialog_OnFileSelected(object sender, SelectFileDialog.SelectFileEventArgs e)
        {
            if (RAWData != null)
            {
                File.WriteAllBytes(e.FileName, RAWData);
            }
            else
            {
                this.Error("没有有效的数据");
            }

        }
    }
}