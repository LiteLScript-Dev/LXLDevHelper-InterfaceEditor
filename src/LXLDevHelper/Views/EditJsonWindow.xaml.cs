using Newtonsoft.Json.Linq;
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
using System.Windows.Shapes;

namespace LXLDevHelper.Views
{
    /// <summary>
    /// EditJsonWindow.xaml 的交互逻辑
    /// </summary>
    public partial class EditJsonWindow : Window
    {
        public static JToken ShowEditJsonDialog(JToken target, Window parent)
        {
            var dialog = new EditJsonWindow(target)
            {
                Top = parent.Top + 30,
                Left = parent.Left + 30,
                Width = parent.Width - 60,
                Height = parent.Height - 60,
            };
            dialog.ShowDialog();
            return dialog.Result ?? target;
        }
        public EditJsonWindow(JToken raw)
        {
            InitializeComponent();
            Data = new(raw);
            Data.OnJsonError += Data_OnJsonError;
            DataContext = Data;
        }

        private void Data_OnJsonError(Exception ex)
        {
            Tools.Message.ShowWarn(ex.Message, "JSON数据错误？");
        }

        private readonly ViewModels.EditJsonWindowViewModel Data;
        public JToken Result { get => edited ? Data.Val : null; }

        private bool edited = false;
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            edited = true;
            Close();
        }
    }
}
