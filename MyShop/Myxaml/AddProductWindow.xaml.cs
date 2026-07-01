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

namespace MyShop.Myxaml
{
    /// <summary>
    /// AddProductWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AddProductWindow : Window
    {
        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void AddProductWindow_Save(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

            }
            MessageBox.Show("保存成功");
        }
        private void AddProductWindow_Cancel(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
            //Window.GetWindow(this).Close();
        }
        private void JudgeEnable(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
