using Newtonsoft.Json;
using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace LXLDevHelper.ViewModels
{
    /// <summary>
    /// 方法定义
    /// </summary>
    public class LXLProperty : BindableBase
    {
        [JsonIgnore] public LXLProperty Me { get => this; }

        /// <summary>
        /// 属性名
        /// </summary>
        public string PropertyName { get => _propertyName; set => SetProperty(ref _propertyName, value); }
        private string _propertyName =
#if DEBUG
"属性名"
#else
""
#endif
            ;
        /// <summary>
        /// 属性描述
        /// </summary>
        public string Description { get => _description; set => SetProperty(ref _description, value); }
        private string _description =
#if DEBUG
"属性描述"
#else
""
#endif
            ;
        /// <summary>
        /// 属性类型
        /// </summary>
        public string PropertyType { get => _propertyType; set => SetProperty(ref _propertyType, value); }
        private string _propertyType = "";
        /// <summary>
        /// 只读
        /// </summary>
        public bool IsReadonly { get => _isReadonly; set => SetProperty(ref _isReadonly, value); }
        private bool _isReadonly = false;
        /// <summary>
        /// 是否静态属性
        /// </summary>
        public bool IsStatic { get => _isStatic; set => SetProperty(ref _isStatic, value); }
        private bool _isStatic = false;
    }
}
