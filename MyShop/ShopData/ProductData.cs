using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace MyShop.ShopData
{
    public class ProductData
    {
       
        /// <summary>
        /// 产品清单文件路径
        /// </summary>
        public static string appDataPath =Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"MyShop");

        public struct ProductInfo
        {
            /// <summary>
            /// 条形码
            /// </summary>
            [Description("条形码")] public string barcode;
            /// <summary>
            /// 商品名称
            /// </summary>
            [Description("商品名称")] public string name;
            /// <summary>
            /// 件数   
            /// </summary>
            [Description("件数")] public decimal countCartons;
            /// <summary>
            /// 个数（每件里有几个）
            /// </summary>
            [Description("个数")] public int countPcs;
            /// <summary>
            /// 总数
            /// </summary>
            [Description("总数")] public decimal countTotal;

            /// <summary>
            /// 产品单位（个/件/斤）
            /// </summary>
            [Description("产品单位")] public string productUnit;
            /// <summary>
            /// 零售价格
            /// </summary>
            [Description("零售价格")] public decimal price;
            /// <summary>
            /// 会员价格
            /// </summary>
            [Description("会员价格")] public decimal priceVip;
            /// <summary>
            /// 进货成本
            /// </summary>
            [Description("进货成本")] public decimal cost;
            /// <summary>
            /// 保质期设置为0则表示不限制保质期
            /// </summary>
            [Description("保质期")] public int expiryDays;
            /// <summary>
            /// 生产日期
            /// </summary>
            [Description("生产日期")] public DateTime ProductionDate;
            /// <summary>
            /// 到期日
            /// </summary>
            [Description("到期日")] public DateTime ExpirationDate;
            /// <summary>
            /// 备注
            /// </summary>
            [Description("备注")] public string remark;
            /// <summary>
            /// 商品图片
            /// </summary>
            [Description("商品图片")] public string picPath;




            ///// <summary>
            ///// 货号/款号
            ///// </summary>
            //[Description("货号/款号")] public string ItemNum;
           

            ////用int类型存储类别和单位，读取某文件去对应int和文本描述
            ///// <summary>
            ///// 类别
            ///// </summary>
            //[Description("类别")] public string productType;
            

           
           
            ///// <summary>
            ///// 唯一序列号/部分商品使用（拟定为条形码+日期+流水号）
            ///// 如123456789+20260627+0003
            ///// </summary>
            //[Description("唯一序列号")] public string sn;

          
        }
        /// <summary>
        /// 获取ProductInfo结构体的字段描述或名称
        /// </summary>
        /// <param name="DescriptionOrName">true输出描述，false输出名称</param>
        /// <returns></returns>
        public static List<string> GetProductInfo(bool DescriptionOrName)
        {
            var list = new List<string>();
            var fields = typeof(ProductInfo).GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (DescriptionOrName)
                {
                    DescriptionAttribute attr = field.GetCustomAttribute<DescriptionAttribute>();

                    if (attr != null)
                    {
                        list.Add(attr.Description);
                    }
                    else
                    {
                        list.Add(field.Name);
                    }
                }
                else
                {
                    list.Add(field.Name);
                }
            }
            return list;
        }
        /// <summary>
        /// 获取一个ProductInfo结构体的字段描述或名称
        /// </summary>
        /// <param name="DescriptionOrName">true输出描述，false输出名称</param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetOneProductInfo(bool DescriptionOrName,string name)
        {
            var list = "";
            var fields = typeof(ProductInfo).GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (DescriptionOrName)
                {
                    if (name== field.Name)
                    {
                        DescriptionAttribute attr = field.GetCustomAttribute<DescriptionAttribute>();
                        return attr != null ? attr.Description : field.Name;
                    }
                }
                else
                {
                    DescriptionAttribute attr = field.GetCustomAttribute<DescriptionAttribute>();
                    if (name == attr.Description)
                    {
                        return  field.Name;
                    }
                }
            }
            return list;
        }
        /// <summary>
        /// 将ProductInfo列表转换为字符串列表，每个字段用逗号分隔
        /// </summary>
        /// <param name="productInfos">信息列表</param>
        /// <param name="IsSetHeader">true为带表头</param>
        /// <returns></returns>
        public static List<string> ConvertToStrList(List<ProductInfo> productInfos,bool IsSetHeader)
        {
            
            List<string> list = new List<string>();
            if (IsSetHeader)
            {
                List<string> headerList = GetProductInfo(true);
                string header = string.Join(",", headerList);
                list.Add(header);
            }

            foreach (var product in productInfos)
            {
                var fields = typeof(ProductInfo).GetFields(BindingFlags.Public | BindingFlags.Instance);
                var values = new List<string>();
                foreach (var field in fields)
                {
                    var val = field.GetValue(product);
                    values.Add(val?.ToString() ?? "");
                }
                list.Add( string.Join(",", values));
            }
            return list;
        }
        /// <summary>
        /// 将单个ProductInfo转换为字符串列表，每个字段用逗号分隔
        /// </summary>
        /// <param name="product"></param>
        /// <param name="IsSetHeader"></param>
        /// <returns></returns>
        public static List<string> ConvertToStrList(ProductInfo product, bool IsSetHeader)
        {

            List<string> list = new List<string>();
            if (IsSetHeader)
            {
                List<string> headerList = GetProductInfo(true);
                string header = string.Join(",", headerList);
                list.Add(header);
            }

           
                var fields = typeof(ProductInfo).GetFields(BindingFlags.Public | BindingFlags.Instance);
                var values = new List<string>();
                foreach (var field in fields)
                {
                    var val = field.GetValue(product);
                    values.Add(val?.ToString() ?? "");
                }
                list.Add(string.Join(",", values));
            
            return list;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static List<ProductInfo> GetProductAll()
        {
            List<ProductInfo> productInfos=new List<ProductInfo>();
           string path= Path.Combine(appDataPath,"ProductList.txt");
           StreamReader streamReader= GetConfig.CSV_R_W(false, path, new List<string>());

            return productInfos;
        }

        public static List<ProductInfo> GetProductFromCsv(string fullName)
        {
            if (!fullName.Contains(appDataPath))
            {
                fullName=Path.Combine(appDataPath, fullName);
            }
            List<ProductInfo> productInfos = new List<ProductInfo>();
            productInfos = GetConfig.EXCEL_R_W(false, fullName, new List<string>());

            return productInfos;

        }
       

    }
}
