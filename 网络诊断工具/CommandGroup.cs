using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 诊断工具
{
    static class CommandGroup
    {
        static public Dictionary<string, Action> CMDS = new Dictionary<string, Action>
        {
            ["任务管理器"] = () => Process.Start("taskmgr"),
            ["设备管理器"] = () => Process.Start("devmgmt.msc"),
            ["磁盘管理器"] = () => Process.Start("diskmgmt.msc"),
            ["刷新DNS缓存"] = () => Process.Start("ipconfig", "/flushdns"),
            ["重置Socket"] = () => Process.Start("netsh", "winsock reset "),
        };
    }
}