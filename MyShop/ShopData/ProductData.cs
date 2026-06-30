using System;
using System.Collections.Generic;
using System.Linq;
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
            /// 名称
            /// </summary>
            public string name;
            /// <summary>
            /// 货号/款号
            /// </summary>
            public string ItemNum;
            /// <summary>
            /// 条形码
            /// </summary>
            public string barcode;

            //用int类型存储类别和单位，读取某文件去对应int和文本描述
            /// <summary>
            /// 类别
            /// </summary>
            public int productType;
            /// <summary>
            /// 产品单位（个/件/斤）
            /// </summary>
            public int productUnit;

            /// <summary>
            /// 备注
            /// </summary>
            public string remark;
            /// <summary>
            /// 零售价格
            /// </summary>
            decimal price;
            /// <summary>
            /// 会员价格
            /// </summary>
            public decimal price_Vip;
            /// <summary>
            /// 进货成本
            /// </summary>
            public decimal cost;
            /// <summary>
            /// 数量
            /// </summary>
            public decimal count;
            /// <summary>
            /// 唯一序列号/部分商品使用（拟定为条形码+日期+流水号）
            /// 如123456789+20260627+0003
            /// </summary>
            public string sn;

            /// <summary>
            /// 保质期设置为0则表示不限制保质期
            /// </summary>
            public int expiryDays;
            /// <summary>
            /// 生产日期
            /// </summary>
            public DateTime ProductionDate;
            /// <summary>
            /// 保质期
            /// </summary>
            public DateTime ExpirationDate;
        }

        
    }
}
