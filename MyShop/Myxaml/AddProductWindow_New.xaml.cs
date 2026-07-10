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
using MyShop.ShopData;

namespace MyShop.Myxaml
{
    //添加商品的新页面
    //带*为必填不可空项
    //不同控件更改文本后有不同更新方式：
    //条码框：输入后按下回车——数据库里已存在?自动补充后续信息；
    //商品名称框：填写时模糊搜索结果放到选择框——回车/选择后——数据库里已存在?自动补充后续信息；
    //总数框：不可编辑；件数更改后自动更新总数；个数若为空则默认为1；总数=件数*个数
    //日期框：到期日期??生产日期+保质期天数；默认生产日期为当前日期，保质期天数为10000天；
    //图片框：点击添加图片按钮——选择图片——保存时路径保存到数据库；图片会显示到按钮上；

    /// <summary>
    /// AddProductWindow_New.xaml 的交互逻辑
    /// </summary>
    public partial class AddProductWindow_New : Window
    {

        TextBox TextBox_barcode;//SN*
        Button Button_CreatBarcode;//自动生成SN

        ComboBox ComboBox_Name;//商品名称*

        TextBox TextBox_CountCartons;//件数*
        TextBox TextBox_CountPcs;//个数
        TextBox TextBox_CountTotal;//总数

        TextBox TextBox_Unit;//单位

        TextBox TextBox_Cost;//进价*
        TextBox TextBox_Price;//售价*
        TextBox TextBox_PriceVip;//vip售价

        DatePicker DatePicker_ProductionDate;//生产日期
        TextBox TextBox_ExpiryDays;//保质期天数
        DatePicker DatePicker_ExpirationDate;//到期日期

        TextBox TextBox_Remark;//备注

        Button Button_Save;//保存
        Button Button_Cancel;//取消
        Button Button_AddProductPic;//添加图片

        ProductData.ProductInfo _productInfo;


        public AddProductWindow_New()
        {
            InitializeComponent();
        }
        private void CheckFormat() 
        {
            //检查格式是否正确，如价格框里内容只能为数字
            //检查必填项是否为空，如sn、商品名称、件数、进价、售价等
        }

        private void UpdateInfoAll() 
        {
            _productInfo.barcode = TextBox_barcode.Text;
            _productInfo.name= ComboBox_Name.Text;
            _productInfo.countCartons = int.Parse(TextBox_CountCartons.Text);
            _productInfo.countPcs = int.Parse(TextBox_CountPcs.Text ?? "1");
            _productInfo.countTotal = _productInfo.countCartons * _productInfo.countPcs;
            _productInfo.productUnit = TextBox_Unit.Text ?? "个";
            _productInfo.cost= Decimal.Parse(TextBox_Cost.Text);
            _productInfo.price= Decimal.Parse(TextBox_Price.Text);
            _productInfo.priceVip = Decimal.Parse(TextBox_PriceVip.Text ?? TextBox_Price.Text);
            //_productionDate= DateTime.Parse(TextBox_ProductionDate.Text ?? DateTime.Now.ToString());
            //_expiryDays= int.Parse(TextBox_ExpiryDays.Text ?? "10000");
            //_expirationDate = _productionDate.AddDays(_expiryDays);
            _productInfo.remark = TextBox_Remark.Text;
            //_picPath = "";
            




        }
        private void GetInfoWithSN(string sn) { }
        private void GetInfoWithName(string name) { }
        private void CreatSN() { }
        private void AddPic() { }

    }
}
