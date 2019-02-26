using Tahiti.Extension;
using Tahiti.ProgramMethod;
using Tahiti.UnionCore.API;
using Tahiti.WPF;
using Tahiti.UnionCore.APPAuth;
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
            TahitiWPF.QueryFormStart<App>(new QueryRunner.QueryRunnerItem[]
            {
            }, new Loading());
        }
    }
}