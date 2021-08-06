using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
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
            if (ParamsDataGrid.SelectedIndex != -1 && ParamsDataGrid.SelectedIndex < ParamsDataGrid.Items.Count-1 && ConfirmDialog("确认删除当前选中行？"))
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
            if (i == -1) { Data.CurrentFuncCollectionHasSet = false; }
            else
            {
                Data.CurrentFuncCollection = Data.CurrentClassCollection[i].AllFunc;
                Data.CurrentFuncCollectionHasSet = true;
            }
        }
        private void FuncListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = FuncListBox.SelectedIndex;
            if (i == -1) { Data.CurrentFuncHasSet = false; }
            else
            {
                Data.CurrentFunc = Data.CurrentFuncCollection[i];
                Data.CurrentFuncHasSet = true;
            }
        }
        #endregion
        #region 数据
        public static ViewModels.MainContentViewModel Data = new();
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.None);
            ModernWpf.MessageBox.Show(json);
            json = Newtonsoft.Json.JsonConvert.SerializeObject(Data, Newtonsoft.Json.Formatting.Indented);
            Clipboard.SetText(json);
        }
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
        }
        #endregion
    }
}
