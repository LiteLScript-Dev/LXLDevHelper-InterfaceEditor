using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
    public class LXLClass : BindableBase
    {
        private string _className = "类名称";
        public string ClassName { get => _className; set => SetProperty(ref _className, value); }

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

    public class MainContentViewModel : BindableBase
    {
        private ObservableCollection<LXLClass> _AllClass = new() { new LXLClass() };
        /// <summary>
        /// 所有类型集合
        /// </summary>
        public ObservableCollection<LXLClass> AllClass
        {
            get { return _AllClass; }
            set { SetProperty(ref _AllClass, value); }
        }
        private ObservableCollection<LXLFunction> _CurrentFuncCollection = new() { };
        /// <summary>
        /// 当前正在编辑的类的所有方法集合
        /// </summary>
        [JsonIgnore]
        public ObservableCollection<LXLFunction> CurrentFuncCollection
        {
            get { return _CurrentFuncCollection; }
            set { SetProperty(ref _CurrentFuncCollection, value); }
        }
    }
}
