using Phenom.ProgramMethod;
using Phenom.WPF;
using Phenom.WPF.Extension;
using System;

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