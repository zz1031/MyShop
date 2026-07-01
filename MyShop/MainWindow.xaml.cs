using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Policy;
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
using MySql.Data.MySqlClient;

namespace MyShop
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 商品页面的商品列表
        /// </summary>
        public ObservableCollection<Product> ProductList { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ProductList = new ObservableCollection<Product>();

            // 绑定到 ListView（在 XAML 中已完成）
            // ListView 的 ItemsSource="{Binding ProductList}" 
            // 或者在后台直接赋值：MyListView.ItemsSource = ProductList;

            DataContext = this;
        }
        int a = 0;
        public void Label_MouseDown_TEST(object sender, MouseButtonEventArgs e)
        {
            MySqlCtrl.Instance.Init("localhost", "3306", "root", "1234", "abab12");
            MySqlCtrl.Instance.OpenMysql();
            MySqlCtrl.Instance.ExcuteMysql("DROP database " + "myTestDatabase2");
            //MySqlCtrl.Instance.ExcuteMysql("CREATE DATABASE `" + "myTestDatabase" + "`;");
            //string iniPath =  "proaaaq.ini";
            //GetConfig.INI_R_W(true, iniPath, "sec111", "key223", "value355");
            //AddTabItem("123" + a);

            //ProductList.Add(new Product
            //{
            //    ImageUrl = "\\Resources\\productPics\\cywl.png",  // 图片路径   
            //    Name = "示例"+a,
            //        Price =a,
            //    Stock = 50-a
            //});
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


    public class Product
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 图片路径，要在Resource文件夹下
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 商品价格
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// 库存数量
        /// </summary>
        public int Stock { get; set; }
    }


}
