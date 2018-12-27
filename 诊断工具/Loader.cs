using Phenom.Extension;
using Phenom.ProgramMethod;
using Phenom.UnionCore.API;
using Phenom.WPF;
using Phenom.UnionCore.APPAuth;
using System;

namespace 诊断工具
{
    public static class Loader
    {
        [STAThread]
        private static void Main(string[] args)
        {
            APIS.InitAPP("4b37a1736c5db70ebccd5add3f9dae27");
            AuthProvider provider = new AuthProvider();
            provider.Monited = true;
            PhenomWPF.QueryFormStart<App>(new QueryRunner.QueryRunnerItem[]
            {
            }, new Loading());
        }
    }
}