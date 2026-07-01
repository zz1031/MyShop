using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
            DataContext = this;
        }
        #region
        int a = 0;
        public void Label_MouseDown_TEST(object sender, MouseButtonEventArgs e)
        {
            #region mysql
            if (false)
            {

                MySqlCtrl.Instance.Init("localhost", "3306", "root", "1234", "abab12");
                MySqlCtrl.Instance.OpenMysql();
                MySqlCtrl.Instance.CreateDatabase("myTestDatabase");
                MySqlCtrl.Instance.UseDatabase("myTestDatabase");

                MySqlCtrl.Instance.CreatDatatable("myTestTable", new string[] { "id", "name", "age" }, new string[] { "INT PRIMARY KEY AUTO_INCREMENT", "TEXT", "INT" });
                MySqlCtrl.Instance.InsertDataToTable("myTestTable", new List<string>() { "3", "zzasd", "826" });
                MySqlCtrl.Instance.InsertDataToTable("myTestTable", new List<string> { "0", "zza1", "827" });
                MySqlCtrl.Instance.InsertDataToTable("myTestTable", new List<string> { "0", "zza2", "828" });
                MySqlCtrl.Instance.InsertDataToTable("myTestTable", new List<string> { "0", "zza3", "829" });
                MySqlCtrl.Instance.UpdateDataToTable("myTestTable", new List<string> { "age=222" }, new List<string> { "id>6" });
                MySqlCtrl.Instance.UpdateDataToTable("myTestTable", new List<string> { "age=333", "name=\"zz33zz\"" }, new List<string> { "id<5", "id>2" });
               
                MySqlCtrl.Instance.ExcuteMysql("CREATE DATABASE `" + "myTestDatabase" + "`;");
                MySqlCtrl.Instance.DeleteDataToTable("myTestTable", new List<string> { "id<5", "id>7" }, "or");
                DataSet dataSet = new DataSet();
                MySqlCtrl.Instance.SelectDataFromTable("myTestTable", out dataSet, new List<string>() { "id", "name" }, new List<string> { "id>5", "id>2" });
            }
            #endregion
            #region 配置文件测试
            if (false)
            {

                //string iniPath =  "proaaaq.ini";
                //GetConfig.INI_R_W(true, iniPath, "sec111", "key223", "value355");
                
            }
            #endregion
            #region 动态添加测试
            if (false)
            {
                //ProductList.Add(new Product
                //{
                //    ImageUrl = "\\Resources\\productPics\\cywl.png",  // 图片路径   
                //    Name = "示例"+a,
                //        Price =a,
                //    Stock = 50-a
                //});

                //AddTabItem("123" + a);
            }
            #endregion
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
        #endregion



        private void AddProduct1(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("添加商品1");
        }
        private void JudgeEnable(object sender,  CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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
