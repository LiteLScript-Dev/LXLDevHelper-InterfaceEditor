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
        /// <summary>
        /// 当前正在编辑的类的所有方法集合
        /// </summary>
        [JsonIgnore]
        public ObservableCollection<LXLFunction> CurrentFuncCollection
        {
            get { return _CurrentFuncCollection; }
            set { SetProperty(ref _CurrentFuncCollection, value); }
        }
        private ObservableCollection<LXLFunction> _CurrentFuncCollection = new() { };
        private bool _currentFuncCollectionHasSet = false;
        [JsonIgnore]
        public bool CurrentFuncCollectionHasSet
        {
            get => _currentFuncCollectionHasSet; set
            {
                SetProperty(ref _currentFuncCollectionHasSet, value);
                if (!value)//设置false=>未设定，移除当前项
                {
                    CurrentFuncCollection = new();
                }
            }
        }
        /// <summary>
        /// 当前正在编辑的方法定义
        /// </summary>
        [JsonIgnore]
        public LXLFunction CurrentFunc
        {
            get { return _CurrentFunc; }
            set { SetProperty(ref _CurrentFunc, value); }
        }
        private LXLFunction _CurrentFunc = new() { };
        private bool _currentFuncHasSet = false;
        [JsonIgnore]
        public bool CurrentFuncHasSet
        {
            get => _currentFuncHasSet; set
            {
                SetProperty(ref _currentFuncHasSet, value);
                if (!value)//设置false=>未设定，移除当前项
                {
                    CurrentFunc = new();
                }
            }
        }
    }
}
