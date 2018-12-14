using Phenom.ProgramMethod;
using Phenom.WPF;
using Phenom.WPF.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 诊断工具
{
    static public class Loader
    {
        [STAThread]
        static void Main(string[] args)
        {
            PhenomWPF.QueryFormStart<App>( new QueryRunner.QueryRunnerItem[]
            {

            }, new Loading());
        }

    }
}