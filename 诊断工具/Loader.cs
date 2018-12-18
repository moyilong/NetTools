using Phenom.ProgramMethod;
using Phenom.WPF;
using Phenom.WPF.Extension;
using System;

namespace 诊断工具
{
    public static class Loader
    {
        [STAThread]
        private static void Main(string[] args)
        {
            PhenomWPF.QueryFormStart<App>(new QueryRunner.QueryRunnerItem[]
            {
            }, new Loading());
        }
    }
}