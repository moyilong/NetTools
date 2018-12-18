using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 诊断工具
{
    public interface AutoLoad
    {
        string TabName { get; }
        string Catalog { get; }
    }

    public interface HelpedAutoLoad: AutoLoad
    {
        string HelpDoc { get; }
    }


}
