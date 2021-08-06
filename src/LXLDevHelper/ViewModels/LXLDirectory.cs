using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXLDevHelper.ViewModels
{
    public class LXLDirectory : BindableBase
    {
        private string _DirName = "新建文件夹";
        public string DirName { get => _DirName; set => SetProperty(ref _DirName, value); }
        /// <summary>
        /// 当前文件夹下所有类型集合
        /// </summary>
        public ObservableCollection<LXLClass> AllClass
        {
            get { return _AllClass; }
            set { SetProperty(ref _AllClass, value); }
        }
        private ObservableCollection<LXLClass> _AllClass = new() { new LXLClass() };
    }
}
