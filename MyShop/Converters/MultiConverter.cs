using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace MyShop.Converters
{
    public class MultiConverter : IMultiValueConverter
    {
        /// <summary>
        /// 将多个值转换为一个 Thickness
        /// </summary>
        /// <param name="values">输入的多个值</param>
        /// <param name="targetType">目标类型</param>
        /// <param name="parameter">额外参数</param>
        /// <param name="culture">文化信息</param>
        /// <returns>组合后的 Thickness</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            // values[0] 对应第一个 Binding
            // values[1] 对应第二个 Binding
             
            // 获取 Left 值
            double left = 0;
            if (values[0] is double leftValue)
            {
                left = leftValue;
            }

            // 获取 Top 值
            double top = 0;
            if (values[1] is double topValue)
            {
                top = topValue;
            }

            // 组合成 Thickness：Left, Top, Right, Bottom
            // Right 和 Bottom 设为 0
            return new Thickness(left, top, 0, 0);
        }

        /// <summary>
        /// 反向转换（不需要实现）
        /// </summary>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
