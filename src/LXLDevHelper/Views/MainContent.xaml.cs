using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        /// <summary>
        /// 对话框显示错误
        /// </summary>
        /// <param name="s">内容</param>
        private void ShowWarn(string s)
        {
            ModernWpf.MessageBox.Show(s, "出错啦！");
        }
        private bool ConfirmDialog(string s)
        {
            return ModernWpf.MessageBox.Show(s, "确认执行", MessageBoxButton.OKCancel) == MessageBoxResult.OK;
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
            Data.CurrentFuncCollection.Add(new());
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
                var item = Data.CurrentFuncCollection[i];
                if (ConfirmDialog($"确认删除\"{item.FuncName}\"方法定义？"))
                {
                    Data.CurrentFuncCollection.RemoveAt(i);
                }
            }
        }
        private void AddPropertyButton_Click(object sender, RoutedEventArgs e)
        {
            Data.CurrentPropertyCollection.Add(new());
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
                var item = Data.CurrentPropertyCollection[i];
                if (ConfirmDialog($"确认删除\"{item.PropertyName}\"属性定义？"))
                {
                    Data.CurrentPropertyCollection.RemoveAt(i);
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
            if (i == -1) { Data.CurrentFuncCollectionHasSet = false; Data.CurrentPropertyCollectionHasSet = false; }
            else
            {
                Data.CurrentFuncCollection = Data.CurrentClassCollection[i].AllFunc;
                Data.CurrentFuncCollectionHasSet = true;
                Data.CurrentPropertyCollection = Data.CurrentClassCollection[i].AllProperty;
                Data.CurrentPropertyCollectionHasSet = true;
            }
        }
        private void FuncListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = FuncListBox.SelectedIndex;
            if (i == -1) { Data.CurrentFuncHasSet = false; Data.CurrentPropertyHasSet = false; }
            else
            {
                Data.CurrentFunc = Data.CurrentFuncCollection[i];
                Data.CurrentFuncHasSet = true;
                Data.CurrentProperty = Data.CurrentPropertyCollection[i];
                Data.CurrentPropertyHasSet = true;
            }
        }
        private void PropertyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = PropertyListBox.SelectedIndex;
            if (i == -1) { Data.CurrentPropertyHasSet = false; Data.CurrentPropertyHasSet = false; }
            else
            {
                Data.CurrentProperty = Data.CurrentPropertyCollection[i];
                Data.CurrentPropertyHasSet = true;
                Data.CurrentProperty = Data.CurrentPropertyCollection[i];
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
                int dircount = 0, classcount = 0, funccount = 0;

                string root = Path.GetFullPath(RootDir + "_temp");
                if (!Directory.Exists(root)) { Directory.CreateDirectory(root); }
                foreach (var dir in Data.DirCollection)
                {
                    string DirPath = Path.Combine(root, dir.DirName);
                    if (!Directory.Exists(DirPath)) { Directory.CreateDirectory(DirPath); }
                    foreach (var cla in dir.AllClass)
                    {
                        string fileName = Path.Combine(DirPath, cla.ClassName + ".json");
                        File.WriteAllText(fileName, Newtonsoft.Json.JsonConvert.SerializeObject(cla, Newtonsoft.Json.Formatting.Indented));
                        funccount += cla.AllFunc.Count;
                        classcount++;
                    }
                    dircount++;
                }
                string rootbase = Path.GetFullPath(RootDir);
                if (Directory.Exists(rootbase)) { Directory.Delete(rootbase, true); }
                Directory.Move(root, rootbase);
                ShowMessage($"保存成功！\n总计：\n    {dircount}个文件夹\n    {classcount}个类\n    {funccount}个方法");
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
            Process.Start("explorer.exe", Path.GetFullPath(RootDir));
        }
        #endregion

    }
}
