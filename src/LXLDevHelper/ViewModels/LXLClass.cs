using Newtonsoft.Json;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace LXLDevHelper.ViewModels
{
    /// <summary>
    /// 类定义
    /// </summary>
    public class LXLClass : BindableBase
    {
        [JsonIgnore] public LXLClass Me { get => this; }
        private string _className =
#if DEBUG
            "类名称"
#else
""
#endif
            ;
        public string ClassName { get => _className; set => SetProperty(ref _className, value.Trim()); }

        /// <summary>
        /// 当前类定义的所有方法集合
        /// </summary>
        public ObservableCollection<LXLFunction> AllFunc
        {
            get { return _AllFunc; }
            set { SetProperty(ref _AllFunc, value); }
        }
        private ObservableCollection<LXLFunction> _AllFunc = new();
        /// <summary>
        /// 当前类定义的所有属性集合
        /// </summary>
        public ObservableCollection<LXLProperty> AllProperty
        {
            get { return _AllProperty; }
            set { SetProperty(ref _AllProperty, value); }
        }
        private ObservableCollection<LXLProperty> _AllProperty = new();
        /// <summary>
        /// 是否静态类
        /// </summary>
        public bool IsStatic { get => _isStatic; set => SetProperty(ref _isStatic, value); }
        private bool _isStatic = false;
        /// <summary>
        /// 是否接口
        /// </summary>
        public bool IsInterface { get => _isInterface; set => SetProperty(ref _isInterface, value); }
        private bool _isInterface = false;
        /// <summary>
        /// 类描述
        /// </summary>
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        private string _description ="" ;
    }
}
