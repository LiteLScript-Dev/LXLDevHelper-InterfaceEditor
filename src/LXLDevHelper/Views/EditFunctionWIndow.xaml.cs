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
        public static string ShowEditFunctionDialog(string target)
        {
            var dialog = new EditFunctionWindow(target);
            dialog.ShowDialog();
            return dialog.Result;
        }
        private bool edited = false;
        public ViewModels.EditFunctionWindowViewModel Data = new();
        public EditFunctionWindow(string text)
        {
            input = text;
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
                        return "Function@" + Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.None);
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
            var text = ((ViewModels.LXLFuncParamsBase)((MenuItem)sender).Tag).ParamType;
            var result = EditFunction(text);
            ((ViewModels.LXLFuncParamsBase)((MenuItem)sender).Tag).ParamType = result;

        }
        private void SelectTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var me = (ComboBox)sender;
            if (me.SelectedItem?.ToString() == "Function")
            {
                var result = EditFunction("Function");
                ((ViewModels.LXLFuncParamsBase)me.Tag).ParamType = result;
            }
        }
        private string EditFunction(string text)
        {
            CommitEdit();
            return ShowEditFunctionDialog(text);
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
