using Newtonsoft.Json.Linq;
using Prism.Mvvm;

namespace LXLDevHelper.ViewModels
{
    public class EditJsonWindowViewModel : BindableBase
    {
        public EditJsonWindowViewModel(JToken raw)
        {
            Val = raw;
        }

        public delegate void OnJsonErrorEventArgs(System.Exception ex);
        public event OnJsonErrorEventArgs OnJsonError;
        public string Json
        {
            get { return Val.ToString(Newtonsoft.Json.Formatting.Indented); }
            set
            {
                try
                {
                    SetProperty(ref Val, JToken.Parse(value));
                }
                catch (System.Exception ex)
                {
                    OnJsonError(ex);
                }
            }
        }
        public JToken Val;
    }
}
