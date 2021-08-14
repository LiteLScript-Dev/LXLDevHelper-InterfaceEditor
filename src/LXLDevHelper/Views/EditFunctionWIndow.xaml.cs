using Newtonsoft.Json;
using System.Windows;
using System.Windows.Controls;
using static LXLDevHelper.Tools.Message;

namespace LXLDevHelper.Views
{
    /// <summary>
    /// EditFunctionWIndow.xaml 的交互逻辑
    /// </summary>
    public partial class EditFunctionWindow : Window
    {
        public static string ShowEditFunctionDialog(string target, Window parent)
        {
            var dialog = new EditFunctionWindow(target)
            {
                Top = parent.Top + 30,
                Left = parent.Left + 30
            };
            dialog.ShowDialog();
            return dialog.Result;
        }
        private bool edited = false;
        public ViewModels.EditFunctionWindowViewModel Data = new();
        public EditFunctionWindow(string text)
        {
            input = text;
            const string prefix = "Function@";//匿名函数前缀
            //匿名函数样式:
            //                      Function@{json信息}
            if (text.StartsWith(prefix))//判断方式，以Function@开头
            {
                try
                {//反序列化到结构体
                    Data = ViewModels.EditFunctionWindowViewModel.GetFromRaw(text);
                }
                catch (System.Exception ex) { ShowWarn($"加载数据失败！\n{text}\n{ex}"); }
            }
            InitializeComponent();
            DataContext = Data;
        }
        public readonly string input;
        public string Result
        {
            get
            {
                if (edited)
                {
                    try
                    {
                        //ShowWarn("Function@" + Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.None));
                        return Data.ToString();
                    }
                    catch (System.Exception ex) { ShowWarn("保存方法参数出错\n" + ex.ToString()); }
                }
                return input;
            }
        } 
        #region 事件

        private void InsertParams_Click(object sender, RoutedEventArgs e)
        {
            if (ParamsDataGrid.SelectedIndex == -1)
                return;
            Data.Func.Params.Insert(ParamsDataGrid.SelectedIndex, new());
            ParamsDataGrid.SelectedIndex = -1;//不选中任何行    
        }
        private void AddParams_Click(object sender, RoutedEventArgs e)
        {
            Data.Func.Params.Add(new());
        }
        private void DeleteParams_Click(object sender, RoutedEventArgs e)
        {
            if (ParamsDataGrid.SelectedIndex != -1 && ParamsDataGrid.SelectedIndex < ParamsDataGrid.Items.Count - 1 && ConfirmDialog("确认删除当前选中行？"))
            {
                try
                {
                    while (ParamsDataGrid.SelectedIndex != -1)
                        Data.Func.Params.RemoveAt(ParamsDataGrid.SelectedIndex);
                }
                catch { }
            }
        }
        private void SelectTypeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var me = (MenuItem)sender;
            var text = ((ViewModels.LXLFuncParamsBase)me.Tag).ParamType;
            var result = EditFunction(text);
            if (me.Tag.GetType() == typeof(ViewModels.LXLFuncParamsBase))
            {
                ((ViewModels.LXLFuncParamsBase)me.Tag).ParamType = result;
                Dispatcher.InvokeAsync(() => ((ViewModels.LXLFuncParamsBase)me.Tag).ParamType = result);
            }
            else
            {
                ((ViewModels.LXLFunctionAnonymous)me.Tag).ReturnType = result;
                Dispatcher.InvokeAsync(() => ((ViewModels.LXLFunctionAnonymous)me.Tag).ReturnType = result);
            }
        }
        private void SelectTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var me = (ComboBox)sender;
            if (me.SelectedItem?.ToString() == "Function")
            {
                var result = EditFunction(me.Text);
                if (me.Tag.GetType() == typeof(ViewModels.LXLFuncParamsBase))
                {
                    ((ViewModels.LXLFuncParamsBase)me.Tag).ParamType = result;
                    Dispatcher.InvokeAsync(() => ((ViewModels.LXLFuncParamsBase)me.Tag).ParamType = result);
                }
                else
                {
                    ((ViewModels.LXLFunctionAnonymous)me.Tag).ReturnType = result;
                    Dispatcher.InvokeAsync(() => ((ViewModels.LXLFunctionAnonymous)me.Tag).ReturnType = result);
                }
            }
        }
        private string EditFunction(string text)
        {
            CommitEdit();
            return ShowEditFunctionDialog(text, this);
        }
        private void CommitEdit()
        {
            if (ParamsDataGrid.SelectedIndex != 1)
            {
                ParamsDataGrid.CancelEdit();//DataGridEditingUnit.Row, true
            }
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            CommitEdit();
            Close();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            CommitEdit();
            edited = true;
            Close();
        }
        #endregion
    }
}
