using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace LXLDevHelper.ViewModels
{
    /// <summary>
    /// 方法定义
    /// </summary>
    public class LXLProperty : BindableBase
    {
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
    }
}
