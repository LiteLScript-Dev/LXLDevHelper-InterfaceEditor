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
        public static ViewModels.MainContentViewModel Data = new();
        public ViewModels.LXLFunction CurrentFunction = new();
        public MainContent()
        {
            InitializeComponent();
            //ClassListBox.ItemsSource
            DataContext = Data;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ModernWpf.MessageBox.Show(Newtonsoft.Json.JsonConvert.SerializeObject(Data));
        }
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            ((Button)sender).IsEnabled = false;
        }

        private void ClassListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var i = ClassListBox.SelectedIndex;
            if (i == -1) { return; }
            Data.CurrentFuncCollection = Data.AllClass[i].AllFunc;
            //ShowMessage(i.ToString());
            //ShowMessage(Data.CurrentFuncCollection.Count.ToString());
        }
        /// <summary>
        /// 对话框显示信息
        /// </summary>
        /// <param name="s">内容</param>
        private void ShowMessage(string s)
        {
            ModernWpf.MessageBox.Show("提示", s);
        }
        private bool ConfirmDialog(string s )
        {
            return ModernWpf.MessageBox.Show("确认执行", s, MessageBoxButton.OKCancel) == MessageBoxResult.OK;
        }

        private void AddClassButton_Click(object sender, RoutedEventArgs e)
        {
            Data.AllClass.Add(new());
        }
        private void DeleteClassButton_Click(object sender, RoutedEventArgs e)
        {
            var i = ClassListBox.SelectedIndex;
            if (i == -1)//未选中
            {
                ShowMessage("请选中一个类后再删除！");
            }
            else
            {
                var item = Data.AllClass[i];
                if (ConfirmDialog($"确认删除{item.ClassName}类定义及其所有子内容？"))
                {
                    Data.AllClass.RemoveAt(i);
                }
            }
        }
        private void AddFuncButton_Click(object sender, RoutedEventArgs e)
        {
            Data.CurrentFuncCollection.Add(new());
        }
        private void DeleteFuncButton_Click(object sender, RoutedEventArgs e)
        {
            var i =FuncListBox.SelectedIndex;
            if (i == -1)//未选中
            {
                ShowMessage("请选中一个函数后再删除！");
            }
            else
            {
                var item = Data.CurrentFuncCollection[i];
                if (ConfirmDialog($"确认删除{item.FuncName}函数定义？"))
                {
                    Data.CurrentFuncCollection.RemoveAt(i);
                }
            }
        }
    }
}
