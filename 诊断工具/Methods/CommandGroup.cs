using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace 诊断工具.Methods
{
    internal static class CommandGroup
    {
        public static Dictionary<string, Action> CMDS = new Dictionary<string, Action>
        {
            ["任务管理器"] = () => Process.Start("taskmgr"),
            ["设备管理器"] = () => Process.Start("devmgmt.msc"),
            ["磁盘管理器"] = () => Process.Start("diskmgmt.msc"),
            ["刷新DNS缓存"] = () => Process.Start("ipconfig", "/flushdns"),
            ["重置Socket"] = () => Process.Start("netsh", "winsock reset "),
            ["组策略编辑器"] = () => Process.Start("gpedit.msc"),
            ["系统管理控制器"] = () => Process.Start("mmc"),
            ["屏幕坏点检测"] = () => new ScreenTest().ShowDialog()
        };
    }
}