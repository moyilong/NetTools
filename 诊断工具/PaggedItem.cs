using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 诊断工具
{
    public interface PaggedItem
    {
        string TabName { get; }
        string Catalog { get; }
    }
}
