using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXLDevHelper.ViewModels
{
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
}
