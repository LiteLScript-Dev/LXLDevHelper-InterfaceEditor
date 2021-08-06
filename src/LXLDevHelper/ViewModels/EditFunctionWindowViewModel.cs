using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXLDevHelper.ViewModels
{
    public class EditFunctionWindowViewModel : BindableBase
    {
        public LXLFunctionAnonymous Func
        {
            get { return _func; }
            set { SetProperty(ref _func, value); }
        }
        private LXLFunctionAnonymous _func = new() { };
    }
}
