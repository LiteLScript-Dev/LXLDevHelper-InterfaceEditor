using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LXLDevHelper.Tools
{
    public static class Message
    {
        /// <summary>
        /// 对话框显示错误
        /// </summary>
        /// <param name="s">内容</param>
        public static void ShowWarn(string s)
        {
            ModernWpf.MessageBox.Show(s, "出错啦！");
        }
        public static bool ConfirmDialog(string s)
        {
            return ModernWpf.MessageBox.Show(s, "确认执行", MessageBoxButton.OKCancel) == MessageBoxResult.OK;
        }
    }
}
