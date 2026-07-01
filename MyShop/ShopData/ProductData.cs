using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.ShopData
{
    public class ProductData
    {
        public static Dictionary<int, string> productTypeDict = new Dictionary<int, string>();
        public static Dictionary<int, string> productUnitDict = new Dictionary<int, string>();

        public struct ProductInfo
        {
            /// <summary>
            /// 商品名称
            /// </summary>
            [Description("商品名称")] public string name;
            /// <summary>
            /// 商品图片
            /// </summary>
            [Description("商品图片")] public string picPath;
            /// <summary>
            /// 货号/款号
            /// </summary>
            [Description("货号/款号")] public string ItemNum;
            /// <summary>
            /// 条形码
            /// </summary>
            [Description("条形码")] public string barcode;

            //用int类型存储类别和单位，读取某文件去对应int和文本描述
            /// <summary>
            /// 类别
            /// </summary>
            [Description("类别")] public int productType;
            /// <summary>
            /// 产品单位（个/件/斤）
            /// </summary>
            [Description("产品单位")] public int productUnit;

            /// <summary>
            /// 备注
            /// </summary>
            [Description("备注")] public string remark;
            /// <summary>
            /// 零售价格
            /// </summary>
            [Description("零售价格")] public decimal price;
            /// <summary>
            /// 会员价格
            /// </summary>
            [Description("会员价格")] public decimal price_Vip;
            /// <summary>
            /// 进货成本
            /// </summary>
            [Description("进货成本")] public decimal cost;
            /// <summary>
            /// 数量
            /// </summary>
            [Description("数量")] public decimal count;
            /// <summary>
            /// 唯一序列号/部分商品使用（拟定为条形码+日期+流水号）
            /// 如123456789+20260627+0003
            /// </summary>
            [Description("唯一序列号")] public string sn;

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

        public static int ConvertStrToInt(string txt)
        {
            return 0;
        }



        /// <summary>
        /// 判断字段值是否非默认值（即被显式赋值）
        /// </summary>
        private static bool IsFieldAssigned(object value, Type type)
        {
            if (type.IsValueType)
            {
                // 获取该类型的默认值
                object defaultValue = Activator.CreateInstance(type);
                return !value.Equals(defaultValue);
            }
            else
            {
                // 引用类型：非 null 即为已赋值
                return value != null;
            }
        }

    }
}
