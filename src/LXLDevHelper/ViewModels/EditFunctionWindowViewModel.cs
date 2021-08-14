using Newtonsoft.Json;
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
        private LXLFunctionAnonymous _func = new();
        public static EditFunctionWindowViewModel GetFromRaw(string text)
        {
            const string prefix = "Function@";//匿名函数前缀
            return JsonConvert.DeserializeObject<EditFunctionWindowViewModel>(text.Substring(prefix.Length));
        }
        public override string ToString()
        {
            return "Function@" + JsonConvert.SerializeObject(this, Formatting.None);
        }
    }
}
