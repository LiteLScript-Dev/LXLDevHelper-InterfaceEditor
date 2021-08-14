using LXLDevHelper.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace LXLDevHelper.Views
{
    /// <summary>
    /// EditEvents.xaml 的交互逻辑
    /// </summary>
    public partial class EditEventsWindow : Window
    {
        public static void ShowEditEventsWindow(ObservableCollection<LXLFunction> AllFunc, Window parent)
        {
            var win = new EditEventsWindow
            {
                Data = new() { AllListenFunc = new() },
                Top = parent.Top + 30,
                Left = parent.Left + 30,
                Width = parent.Width - 60,
                Height = parent.Height - 60
            };
            foreach (var fun in AllFunc)
            {
                if (fun.FuncName == "listen") { win.Data.AllListenFunc.Add(new WLXLFunction(fun)); }
            }
            if (win.Data.AllListenFunc.Count > 0)
            {
                win.Data.GlobalDescription = win.Data.AllListenFunc[0].Description;
                win.Data.GlobalReturnDescription = win.Data.AllListenFunc[0].ReturnDescription;
                win.Data.GlobalReturnType = win.Data.AllListenFunc[0].ReturnType;
                win.Data.GlobalIsStatic = win.Data.AllListenFunc[0].IsStatic;
            }
            win.DataContext = win.Data;
            win.ShowDialog();
            for (int i = AllFunc.Count - 1; i >= 0; i--)
            {
                if (AllFunc[i].FuncName == "listen") { AllFunc.RemoveAt(i); }
            }//移除旧的listen
            foreach (var funcNew in win.Data.AllListenFunc)
            {
                funcNew.FuncName = "listen";
                funcNew.Description = win.Data.GlobalDescription;
                funcNew.ReturnDescription = win.Data.GlobalReturnDescription;
                funcNew.ReturnType = win.Data.GlobalReturnType;
                funcNew.IsStatic = win.Data.GlobalIsStatic;
                AllFunc.Add(funcNew.ConverntBack());//添加回
            }
        }
        public EditEventsWindowViewModel Data;
        public EditEventsWindow()
        {
            InitializeComponent();
        }

        #region 事件
        #region DataGrid
        private void InsertParams_Click(object sender, RoutedEventArgs e)
        {
            if (ParamsDataGrid.SelectedIndex == -1)
                return;
            Data.CurrentFunc.Params.Insert(ParamsDataGrid.SelectedIndex, new());
            ParamsDataGrid.SelectedIndex = -1;//不选中任何行    
        }
        private void AddParams_Click(object sender, RoutedEventArgs e)
        {
            Data.CurrentFunc.Params.Add(new());
        }
        private void DeleteParams_Click(object sender, RoutedEventArgs e)
        {
            if (ParamsDataGrid.SelectedIndex != -1 && ParamsDataGrid.SelectedIndex < ParamsDataGrid.Items.Count - 1 && ConfirmDialog("确认删除当前选中行？"))
            {
                try
                {
                    while (ParamsDataGrid.SelectedIndex != -1)
                        Data.CurrentFunc.Params.RemoveAt(ParamsDataGrid.SelectedIndex);
                }
                catch { }
            }
        }

        #endregion
        private void AddEventButton_Click(object sender, RoutedEventArgs e)
        {
            Data.AllListenFunc.Add(new(new()));
        }
        private void DeleteEventButton_Click(object sender, RoutedEventArgs e)
        {
            var i = EventListBox.SelectedIndex;
            if (i == -1)//未选中
            { ShowMessage("请选中一个事件后再删除！"); }
            else
            {
                var item = Data.AllListenFunc[i];
                if (ConfirmDialog($"确认删除\"{item.FuncName}\"方法定义？"))
                {
                    Data.AllListenFunc.RemoveAt(i);
                }
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
                if (me.Tag.GetType() == typeof(LXLFuncParamsBase))
                {
                    ((LXLFuncParamsBase)me.Tag).ParamType = result;
                    Dispatcher.InvokeAsync(() => ((LXLFuncParamsBase)me.Tag).ParamType = result);
                }
                else
                {
                    ((LXLFunctionAnonymous)me.Tag).ReturnType = result;
                    Dispatcher.InvokeAsync(() => ((LXLFunctionAnonymous)me.Tag).ReturnType = result);
                }
            }
        }
        #endregion
        private string EditFunction(string text)
        {
            CommitEdit();
            return EditFunctionWindow.ShowEditFunctionDialog(text, this);
        }
        private void CommitEdit()
        {
            if (ParamsDataGrid.SelectedIndex != 1)
            {
                ParamsDataGrid.CancelEdit();//DataGridEditingUnit.Row, true
            }
        }
        private bool ConfirmDialog(string v) => Tools.Message.ConfirmDialog(v);
        private void ShowMessage(string v) => Tools.Message.ShowMessage(v);

        private void EventListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = EventListBox.SelectedIndex;
            if (i == -1) { Data.CurrentFunc = new(new()); }
            else
            {
                Data.CurrentFunc = Data.AllListenFunc[i];
            }

        }
        private void EventNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var c = (WLXLFunction)((TextBox)sender).Tag;
                EventListBox.SelectedItem = c;
            }
            catch { }
        }

        //private void CheckBox_Checked(object sender, RoutedEventArgs e)//可拦截
        //{
        //    CallbackReturnType .Text= "Boolean|Void"; 
        //}

        //private void CheckBox_Unchecked(object sender, RoutedEventArgs e)//可拦截
        //{
        //    CallbackReturnType.Text = "Void";
        //}
    }
}
