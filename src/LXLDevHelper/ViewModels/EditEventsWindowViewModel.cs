using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXLDevHelper.ViewModels
{
    public class WLXLFunction : LXLFunction
    {
        public WLXLFunction(LXLFunction funcbase)
        {
            this.Description = funcbase.Description;
            this.ReturnDescription = funcbase.ReturnDescription;
            this.FuncName = funcbase.FuncName;
            this.IsStatic = funcbase.IsStatic;
            this.Params = funcbase.Params;
            this.ReturnType = funcbase.ReturnType;
        }
        public LXLFunction ConverntBack()
        {
            return new LXLFunction
            {
                Description = Description,
                ReturnDescription = ReturnDescription,
                FuncName = FuncName,
                IsStatic = IsStatic,
                Params = Params,
                ReturnType = ReturnType
            };
        }
        public string EventKey
        {
            get
            {
                FixParams();
                var s = Params.First().ParamType;
                if (s.StartsWith("\"")) { s = s.Substring(1); }
                if (s.EndsWith("\"")) { s = s.Remove(s.Length - 1); }
                return s;
            }
            set
            {
                FixParams();
                if (Params.Count == 0) { Params.Add(new LXLFuncParams()); }
                Params.First().ParamType = "\"" + value + "\"";
                //SetProperty(ref Params.First().ParamType, value); 
            }
        }
        private void FixParams()
        {
            if (Params.Count == 0) { Params.Add(new LXLFuncParams()); }
            if (Params.Count == 1) { Params.Add(new LXLFuncParams() { ParamName = "event", Description = "事件回调" }); }
        }
        public string EventCallbackFunctionReturnType
        {
            get => EventCallbackFunction.ReturnType;
            set
            {
                EventCallbackFunction.ReturnType = value;
                string temp = "";
                SetProperty(ref temp, value);
            }
        }
        public LXLFunctionAnonymous EventCallbackFunction
        {
            get
            {
                FixParams();
                var functype = Params[1].ParamType;
                EditFunctionWindowViewModel ret;
                try
                {
                    ret = EditFunctionWindowViewModel.GetFromRaw(functype);
                }
                catch (Exception)
                {
                    ret = new() { Func = new() { ReturnType = "Void" } };
                }
                System.ComponentModel.PropertyChangedEventHandler fb = new((_, _) => EventCallbackFunction = ret.Func);
                System.Collections.Specialized.NotifyCollectionChangedEventHandler cfb = new((_, e) =>
                {
                    if (e.NewItems.Count > 0)
                    {
                        foreach (LXLFuncParamsBase item in e.NewItems)
                        {
                            item.PropertyChanged += fb;
                        }
                    }
                    EventCallbackFunction = ret.Func;
                });
                ret.Func.PropertyChanged += fb;
                foreach (LXLFuncParamsBase item in ret.Func.Params)
                { item.PropertyChanged += fb; }
                ret.Func.Params.CollectionChanged += cfb;
                return ret.Func;
            }
            set
            {
                Params[1].ParamType = new EditFunctionWindowViewModel { Func = value }.ToString();
            }
        }
        //private ObservableCollection<LXLFuncParams> _params = new();
        public bool CallbackReturnIntercept
        {
            get
            {
                return !(EventCallbackFunctionReturnType == "Void" || string.IsNullOrWhiteSpace(EventCallbackFunctionReturnType));
            }
            set
            {
                if (value)
                {
                    EventCallbackFunctionReturnType = "Boolean|Void";
                }
                else
                {
                    EventCallbackFunctionReturnType = "Void";
                }
            }
        }
        public string EventName
        {
            get
            {
                FixParams(); return Params[0].ParamName;
            }
            set
            {
                FixParams(); Params[0].ParamName = value;
            }
        }
        public string EventDescription
        {
            get
            {
                FixParams(); return Params[0].Description;
            }
            set
            {
                FixParams(); Params[0].Description = value;
            }
        }
        public string CallbackName
        {
            get
            {
                FixParams(); return Params[1].ParamName;
            }
            set
            {
                FixParams(); Params[1].ParamName = value;
            }
        }
        public string CallbackDescription
        {
            get
            {
                FixParams(); return Params[1].Description;
            }
            set
            {
                FixParams(); Params[1].Description = value;
            }
        }
    }
    public class EditEventsWindowViewModel : BindableBase
    {
        public ObservableCollection<WLXLFunction> AllListenFunc
        {
            get { return _AllListenFunc; }
            set { SetProperty(ref _AllListenFunc, value); }
        }
        private ObservableCollection<WLXLFunction> _AllListenFunc = new();

        private string _globalDescription;
        public string GlobalDescription { get => _globalDescription; set { SetProperty(ref _globalDescription, value); } }
        private string _globalReturnDescription;
        public string GlobalReturnDescription { get => _globalReturnDescription; set => SetProperty(ref _globalReturnDescription, value); }
        private string _globalReturnType;
        public string GlobalReturnType { get => _globalReturnType; set => SetProperty(ref _globalReturnType, value); }
        private bool _globalIsStatic;
        public bool GlobalIsStatic { get => _globalIsStatic; set => SetProperty(ref _globalIsStatic, value); }
        //public System.Collections.IEnumerable AvaliableTypes { get => MainContentViewModel.AvaliableTypes;   }
        public WLXLFunction _currentFunc = new(new());
        public WLXLFunction CurrentFunc { get => _currentFunc; set => SetProperty(ref _currentFunc, value); }
    }
}
