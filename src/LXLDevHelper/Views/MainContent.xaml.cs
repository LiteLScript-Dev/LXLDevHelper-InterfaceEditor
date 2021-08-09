using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static LXLDevHelper.Tools.Message;
namespace LXLDevHelper.Views
{
    /// <summary>
    /// MainContent.xaml 的交互逻辑
    /// </summary>
    public partial class MainContent : UserControl
    {
        public MainContent()
        {
            InitializeComponent();
            DataContext = Data;//设置数据源
        }
        #region 交互
        /// <summary>
        /// 对话框显示信息
        /// </summary>
        /// <param name="s">内容</param>
        private void ShowMessage(string s)
        {
            ModernWpf.MessageBox.Show(s, "提示");
        }
        #endregion
        #region 增删
        private void AddDirButton_Click(object sender, RoutedEventArgs e)
        {
            Data.DirCollection.Add(new());
        }
        private void DeleteDirButton_Click(object sender, RoutedEventArgs e)
        {
            var i = DirListBox.SelectedIndex;
            if (i == -1)//未选中
            {
                ShowMessage("请选中一个文件夹后再删除！");
            }
            else
            {
                var item = Data.DirCollection[i];
                if (ConfirmDialog($"确认删除\"{item.DirName}\"文件夹？"))
                {
                    Data.DirCollection.RemoveAt(i);
                }
            }
        }
        private void AddClassButton_Click(object sender, RoutedEventArgs e)
        {
            Data.CurrentClassCollection.Add(new());
        }
        private void DeleteClassButton_Click(object sender, RoutedEventArgs e)
        {
            var i = ClassListBox.SelectedIndex;
            if (i == -1)//未选中
            {
                ShowMessage("请选中一个方法后再删除！");
            }
            else
            {
                var item = Data.CurrentClassCollection[i];
                if (ConfirmDialog($"确认删除\"{item.ClassName}\"类定义及其所有子内容？"))
                {
                    Data.CurrentClassCollection.RemoveAt(i);
                }
            }
        }
        private void AddFuncButton_Click(object sender, RoutedEventArgs e)
        {
            Data.CurrentClass.AllFunc.Add(new());
        }
        private void DeleteFuncButton_Click(object sender, RoutedEventArgs e)
        {
            var i = FuncListBox.SelectedIndex;
            if (i == -1)//未选中
            {
                ShowMessage("请选中一个方法后再删除！");
            }
            else
            {
                var item = Data.CurrentClass.AllFunc[i];
                if (ConfirmDialog($"确认删除\"{item.FuncName}\"方法定义？"))
                {
                    Data.CurrentClass.AllFunc.RemoveAt(i);
                }
            }
        }
        private void AddPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            Data.CurrentClass.AllProperty.Add(new());
        }
        private void DeletePropertyButton_Click(object sender, RoutedEventArgs e)
        {
            var i = PropertyListBox.SelectedIndex;
            if (i == -1)//未选中
            {
                ShowMessage("请选中一个方法后再删除！");
            }
            else
            {
                var item = Data.CurrentClass.AllProperty[i];
                if (ConfirmDialog($"确认删除\"{item.PropertyName}\"属性定义？"))
                {
                    Data.CurrentClass.AllProperty.RemoveAt(i);
                }
            }
        }
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
        #region 事件
        private void DirListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = DirListBox.SelectedIndex;
            if (i == -1) { Data.CurrentClassCollectionHasSet = false; }
            else
            {
                Data.CurrentClassCollection = Data.DirCollection[i].AllClass;
                Data.CurrentClassCollectionHasSet = true;
            }
        }
        private void ClassListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = ClassListBox.SelectedIndex;
            if (i == -1) { Data.CurrentClassHasSet = false; }
            else
            {
                Data.CurrentClass = Data.CurrentClassCollection[i];
                Data.CurrentClassHasSet = true;
            }
        }
        private void FuncListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = FuncListBox.SelectedIndex;
            if (i == -1) { Data.CurrentFuncHasSet = false; }
            else
            {
                Data.CurrentFunc = Data.CurrentClass.AllFunc[i];
                Data.CurrentFuncHasSet = true;
            }
        }
        private void PropertyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = PropertyListBox.SelectedIndex;
            if (i == -1) {; Data.CurrentPropertyHasSet = false; }
            else
            {
                Data.CurrentProperty = Data.CurrentClass.AllProperty[i];
                Data.CurrentPropertyHasSet = true;
            }
        }
        #endregion
        #region 数据
        public static ViewModels.MainContentViewModel Data = new();
        const string RootDir = "src";
        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckData()) { return; }   //检查
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.None);
            ModernWpf.MessageBox.Show(json, "已复制到剪切板！");
            json = Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented);
            Clipboard.SetText(json);
        }
        private bool CheckData()
        {
            if (Data.DirCollection.Count == 0)
            {
                ShowWarn("无数据！"); return false;
            }
            foreach (var dir in Data.DirCollection)
            {
                if (string.IsNullOrWhiteSpace(dir.DirName))
                {
                    DirListBox.SelectedItem = dir;
                    ShowWarn("文件名不能为空！"); return false;
                }
                if (Data.DirCollection.Any(x => x != dir && x.DirName == dir.DirName))
                {
                    DirListBox.SelectedItem = Data.DirCollection.First(x => x != dir && x.DirName == dir.DirName);
                    ShowWarn("文件名不得重复！"); return false;
                }
                foreach (var cla in dir.AllClass)
                {
                    if (string.IsNullOrWhiteSpace(cla.ClassName))
                    {
                        DirListBox.SelectedItem = dir;
                        ClassListBox.SelectedItem = cla;
                        ShowWarn("类名不能为空！"); return false;
                    }
                    if (dir.AllClass.Any(x => x != cla && x.ClassName == cla.ClassName))
                    {
                        DirListBox.SelectedItem = dir;
                        ClassListBox.SelectedItem = dir.AllClass.First(x => x != cla && x.ClassName == cla.ClassName);
                        ShowWarn("类名不得重复！"); return false;
                    }
                }
            }
            return true;
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!CheckData()) { return; }   //检查
                //写入
                int dircount = 0, classcount = 0, funccount = 0, propertycount = 0;

                string root = Path.GetFullPath(RootDir + "_temp");
                if (!Directory.Exists(root)) { Directory.CreateDirectory(root); }
                foreach (var dir in Data.DirCollection)
                {
                    string DirPath = Path.Combine(root, dir.DirName);
                    if (!Directory.Exists(DirPath)) { Directory.CreateDirectory(DirPath); }
                    foreach (var cla in dir.AllClass)
                    {
                        if (cla.IsStatic)
                        {
                            foreach (var fun in cla.AllFunc) { fun.IsStatic = true; }
                            foreach (var p in cla.AllProperty) { p.IsStatic = true; }
                        }
                        string fileName = Path.Combine(DirPath, cla.ClassName + ".json");
                        File.WriteAllText(fileName, Newtonsoft.Json.JsonConvert.SerializeObject(cla, Newtonsoft.Json.Formatting.Indented));
                        funccount += cla.AllFunc.Count;
                        propertycount += cla.AllProperty.Count;
                        classcount++;
                    }
                    dircount++;
                }
                string rootbase = Path.GetFullPath(RootDir);
                if (Directory.Exists(rootbase)) { Directory.Delete(rootbase, true); }
                Directory.Move(root, rootbase);
                ShowMessage($"保存成功！\n总计：\n    {dircount}个文件夹\n    {classcount}个类\n    {funccount}个方法\n    {propertycount}个属性");
                LoadButton.Tag = true;
                LoadButton.Content = "重新载入";
            }
            catch (System.Exception ex)
            {
                ShowWarn(ex.ToString());
            }
        }
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool loaded = !(((Button)sender).Tag is null);
                if (loaded)
                {
                    if (!ConfirmDialog("确认重新载入？\n当前编辑将无法恢复。"))
                    {
                        return;
                    }
                }
                string root = Path.GetFullPath(RootDir);
                if (!Directory.Exists(root))
                {
                    ShowWarn("载入失败！\n未找到数据。"); return;
                }
                Data.DirCollection.Clear();
                foreach (var dir in Directory.GetDirectories(root))
                {
                    var dirInfo = new ViewModels.LXLDirectory() { DirName = Path.GetFileName(dir) };
                    foreach (var file in Directory.GetFiles(dir))
                    {
                        try
                        {
                            var raw = File.ReadAllText(file);
                            dirInfo.AllClass.Add(Newtonsoft.Json.JsonConvert.DeserializeObject<ViewModels.LXLClass>(raw));
                        }
                        catch (System.Exception ex)
                        {
                            ShowWarn($"读取{file}时遇到错误\n{ex}");
                        }
                    }
                    Data.DirCollection.Add(dirInfo);
                }
                ((Button)sender).Tag = true;
                ((Button)sender).Content = "重新载入";
            }
            catch (System.Exception ex)
            { ShowWarn($"载入失败！\n{ex}"); }
        }
        private void OpenDirButton_Click(object sender, RoutedEventArgs e)
        {
            var dir = Path.GetFullPath(RootDir);
            if (!Directory.Exists(dir))
            {
                ShowWarn("未找到数据！");
                return;
            }
            Process.Start("explorer.exe", dir);
        }
        #endregion

        private void DirNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var c = (ViewModels.LXLDirectory)((TextBox)sender).Tag;
                DirListBox.SelectedItem = c;
            }
            catch { }
        }
        private void ClassNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var c = (ViewModels.LXLClass)((TextBox)sender).Tag;
                ClassListBox.SelectedItem = c;
            }
            catch { }
        }
        private void FuncNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var c = (ViewModels.LXLFunction)((TextBox)sender).Tag;
                FuncListBox.SelectedItem = c;
            }
            catch { }
        }
        private void PropertyNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                var c = (ViewModels.LXLProperty)((TextBox)sender).Tag;
                PropertyListBox.SelectedItem = c;
            }
            catch { }
        }

        private void SelectTypeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var me = (MenuItem)sender;
            var tag = me.Tag;
            var tagType = tag.GetType();
            if (tagType == typeof(ViewModels.LXLFuncParams))
            {
                var t = (ViewModels.LXLFuncParams)tag;
                var text = t.ParamType;
                var result = EditFunction(text);
                t.ParamType = result;
            }
            else if (tagType == typeof(ViewModels.LXLFunction))
            {
                var t = (ViewModels.LXLFunction)tag;
                var text = t.ReturnType;
                var result = EditFunction(text);
                t.ReturnType = result;
            }
            else if (tagType == typeof(ViewModels.LXLProperty))
            {
                var t = (ViewModels.LXLProperty)tag;
                var text = t.PropertyType;
                var result = EditFunction(text);
                t.PropertyType = result;
            }
        }
        private void SelectTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var me = (ComboBox)sender;
            if (me.SelectedItem?.ToString() == "Function")
            {
                //e.Handled = true;
                var result = EditFunction(me.Text);//还未因选择而更改的Text
                Dispatcher.InvokeAsync(() => me.Text = result);//奇怪的bug，事件内直接改没效果，所以只能post到事件完成后运行
            }
        }
        private string EditFunction(string text)
        {
            return EditFunctionWindow.ShowEditFunctionDialog(text, Application.Current.MainWindow);
        }

        private void EditCurrentFuncJsonButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var raw = Newtonsoft.Json.Linq.JToken.FromObject(Data.CurrentFunc);
                var result = EditJsonWindow.ShowEditJsonDialog(raw, Application.Current.MainWindow);
                var i = Data.CurrentClass.AllFunc.IndexOf(Data.CurrentFunc);
                if (i == -1) { return; }
                Data.CurrentClass.AllFunc[i] = result.ToObject<ViewModels.LXLFunction>();
                FuncListBox.SelectedIndex = i;
            }
            catch { }
        }
        private void CopyCurrentFuncJsonButton_Click(object sender, RoutedEventArgs e)
        {
            var raw = Newtonsoft.Json.JsonConvert.SerializeObject(Data.CurrentFunc);
            Clipboard.SetText(raw);
        }
        private void PasteCurrentFuncJsonButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ConfirmDialog("粘贴将会覆盖当前编辑的内容！")) { 
                    var i = Data.CurrentClass.AllFunc.IndexOf(Data.CurrentFunc);
                if (i == -1) { return; }
                Data.CurrentClass.AllFunc[i] = Newtonsoft.Json.JsonConvert.DeserializeObject<ViewModels.LXLFunction>(Clipboard.GetText());
                FuncListBox.SelectedIndex = i;}
            }
            catch (System.Exception ex)
            {
                ShowWarn(ex.Message, "粘贴失败");
            }
        }
        private void EditCurrentPropertyJsonButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var raw = Newtonsoft.Json.Linq.JToken.FromObject(Data.CurrentProperty);
                var result = EditJsonWindow.ShowEditJsonDialog(raw, Application.Current.MainWindow);
                var i = Data.CurrentClass.AllProperty.IndexOf(Data.CurrentProperty);
                if (i == -1) { return; }
                Data.CurrentClass.AllProperty[i] = result.ToObject<ViewModels.LXLProperty>();
                PropertyListBox.SelectedIndex = i;
            }
            catch { }
        }
        private void CopyCurrentPropertyJsonButton_Click(object sender, RoutedEventArgs e)
        {
            var raw = Newtonsoft.Json.JsonConvert.SerializeObject(Data.CurrentProperty);
            Clipboard.SetText(raw);
        }
        private void PasteCurrentPropertyJsonButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ConfirmDialog("粘贴将会覆盖当前编辑的内容！"))
                {
 var i = Data.CurrentClass.AllProperty.IndexOf(Data.CurrentProperty);
                if (i == -1) { return; }
                Data.CurrentClass.AllProperty[i] = Newtonsoft.Json.JsonConvert.DeserializeObject<ViewModels.LXLProperty>(Clipboard.GetText());
                PropertyListBox.SelectedIndex = i;
                }
            }
            catch (System.Exception ex)
            {
                ShowWarn(ex.Message, "粘贴失败");
            }
        }
    }
}
