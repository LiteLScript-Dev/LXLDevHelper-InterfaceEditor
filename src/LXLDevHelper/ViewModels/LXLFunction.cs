using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace LXLDevHelper.ViewModels
{
    public class LXLFunction : BindableBase
    {
        private string _funcName = "方法名称";
        /// <summary>
        /// 方法名
        /// </summary>
        public string FuncName { get => _funcName; set => SetProperty(ref _funcName, value); }
    }
}
