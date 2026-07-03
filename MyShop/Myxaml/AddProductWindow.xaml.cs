using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
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
using MyShop.ShopData;

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

           //ProductData.GetProductAll();
        }

        private void AddProductWindow_Save(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                ProductData.ProductInfo product = new ProductData.ProductInfo()
                {
                    name = AddProductWindow_txt_name.Text.Trim(),
                   // picPath = "path/to/pic1.png",
                    ItemNum = "001",
                    barcode = AddProductWindow_txt_barcode.Text.Trim(),
                    productType = ProductData.ConvertStrToInt( AddProductWindow_txt_type.Text.Trim()),
                    productUnit = ProductData.ConvertStrToInt(AddProductWindow_txt_Unit.Text.Trim()),
                    remark = AddProductWindow_txt_remark.Text.Trim(),
                    price = AddProductWindow_txt_price.Text==""?0: decimal.Parse(AddProductWindow_txt_price.Text.Trim()),
                    price_Vip = AddProductWindow_txt_price_Vip.Text == "" ? 0 : decimal.Parse(AddProductWindow_txt_price_Vip.Text.Trim()),
                    cost = AddProductWindow_txt_cost.Text == "" ? 0 : decimal.Parse(AddProductWindow_txt_cost.Text.Trim()),
                    count = AddProductWindow_txt_count.Text == "" ? 0 : decimal.Parse(AddProductWindow_txt_count.Text.Trim()),
                    //sn = ,
                    //expiryDays = ,
                    //ProductionDate = DateTime.Now.AddDays(-30),
                    //ExpirationDate = DateTime.Now.AddDays(335)
                };

                string fileName = "商品列表";
                if (!Directory.Exists(ProductData.appDataPath))
                {
                    Directory.CreateDirectory(ProductData.appDataPath);
                }
                if (!fileName.Contains(".xlsx"))
                {
                    fileName += ".xlsx";
                }
                ////写csv
                if (!fileName.Contains(ProductData.appDataPath))
                {
                    fileName =System.IO.Path.Combine(ProductData.appDataPath, fileName);
                }

                GetConfig.EXCEL_R_W(true, fileName,  ProductData.ConvertToStrList(product, !File.Exists(fileName)));

                MessageBox.Show("保存成功");
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败"+ex);
            }
           
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
