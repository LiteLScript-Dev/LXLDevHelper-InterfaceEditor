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
            var text = (string)((MenuItem)sender).Tag;
            var result = EditFunction(text);
            //System.Threading.Tasks.Task.Delay(1000).ContinueWith(_ =>
            //{
            //    Dispatcher.InvokeAsync(() =>);//奇怪的bug，事件内直接改没效果，所以只能post到事件完成后运行
            //});
            ((MenuItem)sender).Tag = result;
        }
        private void SelectTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var me = (ComboBox)sender;
            if (me.SelectedItem?.ToString() == "Function")
            {
                //e.Handled = true;
                var result = EditFunction("Function");
                //ShowWarn(result);
                System.Threading.Tasks.Task.Delay(1000).ContinueWith(_ =>
                {
                    Dispatcher.InvokeAsync(() => {
                        me.Text = result;
                        ShowWarn(result);
                    });//奇怪的bug，事件内直接改没效果，所以只能post到事件完成后运行
                });
                //me.Text = result;
                //ShowWarn(me.Text);
                //e.Handled = true;
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
