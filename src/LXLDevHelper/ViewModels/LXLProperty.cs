using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace LXLDevHelper.ViewModels
{
    /// <summary>
    /// 方法定义
    /// </summary>
    public class LXLFunction : BindableBase
    {
        /// <summary>
        /// 方法名
        /// </summary>
        public string FuncName { get => _funcName; set => SetProperty(ref _funcName, value); }
        private string _funcName =
#if DEBUG
"方法名称"
#else
""
#endif
            ;
        /// <summary>
        /// 方法描述
        /// </summary>
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        private string _description =
#if DEBUG
"方法描述"
#else
""
#endif
            ;
        /// <summary>
        /// 方法参数
        /// </summary>
        public ObservableCollection<LXLFuncParams> Params { get => _params; set => SetProperty(ref _params, value); }
        private ObservableCollection<LXLFuncParams> _params = new();
    }
}
