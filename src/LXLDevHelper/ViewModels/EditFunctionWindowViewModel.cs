using Prism.Mvvm;

namespace LXLDevHelper.ViewModels
{
    public class EditFunctionWindowViewModel : BindableBase
    {
        public LXLFunctionAnonymous Func
        {
            get { return _func; }
            set { SetProperty(ref _func, value); }
        }
        private LXLFunctionAnonymous _func = new() ;
    }
}
