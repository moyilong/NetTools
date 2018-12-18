using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace 诊断工具.Methods.Rename
{
    public class RenameOperator : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string OrignalFilename { get; private set; }

        public string TargetFilename { get; private set; }

        public string DirectoryName { get; private set; }

        public RenameOperator(FileInfo file)
        {
            OrignalFilename = file.Name;
            TargetFilename = file.Name;
            DirectoryName = Path.GetDirectoryName(file.FullName);
        }

        public bool HaveReplace { get; set; } = false;

        public void UpdateRegex(string reg, string ireg)
        {
            TargetFilename = Regex.Replace(OrignalFilename, ireg, reg);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TargetFilename"));
        }

        public static void UpdateRegexArray(string reg, string ireg, RenameOperator[] array)
        {
            Parallel.ForEach(array, self => self.UpdateRegex(reg, ireg));
            Parallel.ForEach(array, self =>
            {
                self.HaveReplace = false;
                foreach (RenameOperator i in array)
                {
                    if (i.DirectoryName == self.DirectoryName && self.TargetFilename.ToLower() == i.TargetFilename.ToLower())
                    {
                        self.HaveReplace = true;
                        self.PropertyChanged?.Invoke(self, new PropertyChangedEventArgs("HaveReplace"));
                        break;
                    }
                }
            });
        }
    }
}