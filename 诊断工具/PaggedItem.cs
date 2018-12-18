using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 诊断工具
{
    public interface AutoLoadTemplate
    {
        string TabName { get; }
        string Catalog { get; }
    }

    public interface HelpedAutoLoad: AutoLoadTemplate
    {
        string HelpDoc { get; }
    }

    public interface WIPTemplate
    {

    }
}
