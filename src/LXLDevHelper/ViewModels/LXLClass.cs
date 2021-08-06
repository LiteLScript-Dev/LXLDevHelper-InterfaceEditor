using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace LXLDevHelper.ViewModels
{
    /// <summary>
    /// 类定义
    /// </summary>
    public class LXLClass : BindableBase
    {
        private string _className = "类名称";
        public string ClassName { get => _className; set => SetProperty(ref _className, value.Trim()); }

        private ObservableCollection<LXLFunction> _AllFunc = new() { new LXLFunction() };
        /// <summary>
        /// 当前类定义的所有方法集合
        /// </summary>
        public ObservableCollection<LXLFunction> AllFunc
        {
            get { return _AllFunc; }
            set { SetProperty(ref _AllFunc, value); }
        }
    }
}
