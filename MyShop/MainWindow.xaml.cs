using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MyShop.ShopData;

namespace MyShop
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        int a = 0;
        public void Label_MouseDown_TEST(object sender, MouseButtonEventArgs e)
        {
            string iniPath =  "proaaaq.ini";
            GetConfig.INI_R_W(true, iniPath, "sec111", "key223", "value355");
        }

   
        public void AddButton(string text)
        {
            // 创建新的 TabItem
            TabItem newTab = new TabItem();

            // 应用 Style（从资源中获取）
            newTab.Style = (Style)this.Resources["LableStyle_sub"];

            newTab.Header= text;
           
        }
        private void AddTabItem(string header)
        {
            // 1. 创建新的 TabItem
            TabItem newTab = new TabItem();

            // 2. 设置 Header
            newTab.Header = header;

            // 或者用 FindResource
             newTab.Style = (Style)FindResource("LableStyle_sub");

            // 4. 添加到 TabControl
            TabControl_ProductCategory.Items.Add(newTab);
        }

    }
    public class ButtonItem
    {
        public string Text { get; set; }
        public ICommand Command { get; set; }
        public object CommandParameter { get; set; }
        public bool IsEnabled { get; set; } = true;

        // 如果需要不同颜色，用数据属性，不用样式
        public string ButtonType { get; set; } = "Default";  // Default, Primary, Danger
    }
}
