using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using IniParser;
using IniParser.Model;
using MySqlX.XDevAPI.Relational;
using OfficeOpenXml;
using static MyShop.ShopData.ProductData;
using static MyShop.ShopData.ToExcel;


namespace MyShop.ShopData
{

    public class GetConfig
    {

        /// <summary>
        /// ini文件读写方法
        /// </summary>
        /// <param name="IsWrite">true为写入，不返回值；false为读取</param>
        /// <param name="fileName">文件名称，路径固定为基目录</param>
        /// <param name="sectionName">节点名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static string INI_R_W(bool IsWrite, string fileName, string sectionName = null, string key = null, string value = null)
        {
            string result = null;
            try
            {


                if (!fileName.Contains(ProductData.appDataPath))
                {
                    fileName = Path.Combine(ProductData.appDataPath, fileName);
                }
                // 1. 读取INI文件
                var parser = new FileIniDataParser();
                IniData data = parser.ReadFile(fileName);
                if (!IsWrite)
                {
                    if (sectionName == null || key == null)
                    {
                        result = "INI_R_W error:";
                        MessageBox.Show(result + "输入键为空");
                    }
                    else
                    {
                        // 2. 获取配置值
                        value = data[sectionName][key];

                        result = value;
                        if (value == null)
                        {
                            MessageBox.Show(result + "不存在键");
                        }
                    }
                }
                else
                {
                    if (key == null)
                    {
                        data.Sections.RemoveSection(sectionName);
                    }
                    else if (value == null)
                    {
                        data[sectionName].RemoveKey(key);
                    }
                    else
                    {
                        // 3. 修改或新增配置
                        data[sectionName][key] = value;
                    }

                    // 4. 保存回文件
                    parser.WriteFile(fileName, data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("INI_R_W error: " + ex.Message);
            }
            return result;
        }


        public static void XML_R_W(bool IsWrite, string fileName, string sectionName = null, string key = null, string value = null)
        {
            ////写xml
            if (!fileName.Contains(ProductData.appDataPath))
            {
                fileName = Path.Combine(ProductData.appDataPath, fileName);
            }
            XmlInfo xmlInfo1 = new XmlInfo()
            {
                num = 111,
                name = "汉字1",
                enName = "abc"
            };
            XmlInfo xmlInfo2 = new XmlInfo()
            {
                num = 222,
                name = "汉字2",
                enName = "def"
            };
            List<XmlInfo> xmlInfoAll = new List<XmlInfo>();
            xmlInfoAll.Add(xmlInfo1);
            xmlInfoAll.Add(xmlInfo2);
            using (FileStream sw = new FileStream(fileName, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(xmlInfoAll.GetType());
                serializer.Serialize(sw, xmlInfoAll);
                sw.Close();
            }
            using (FileStream sr = new FileStream(fileName, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(xmlInfoAll.GetType());
                List<XmlInfo> xmlInforesult = (List<XmlInfo>)serializer.Deserialize(sr);
                foreach (var item in xmlInforesult)
                {
                    Console.WriteLine($"{item.num}{item.name}{item.enName}");
                }
            }

        }
        public struct XmlInfo
        {
            public int num;
            public string name;
            public string enName;

        }
        public static StreamReader CSV_R_W(bool IsWrite, string fileName, List<string> strs)
        {
            if (!Directory.Exists(ProductData.appDataPath))
            {
                Directory.CreateDirectory(ProductData.appDataPath);
            }
            StreamReader sr;
            try
            {

                if (!fileName.Contains(".csv"))
                {
                    fileName += ".csv";
                }
                ////写csv
                if (!fileName.Contains(ProductData.appDataPath))
                {
                    fileName = Path.Combine(ProductData.appDataPath, fileName);
                }

                if (!IsWrite)
                {
                    if (!File.Exists(fileName))
                    {
                        File.Create(fileName);
                    }
                    using (sr = new StreamReader(fileName))
                    {
                        return sr;
                    }
                }
                else
                {


                    //var gbk = Encoding.GetEncoding("GB2312");
                    using (StreamWriter sw = new StreamWriter(fileName, true, new UTF8Encoding(true)))
                    {

                        for (int i = 0; i < strs.Count; i++)
                        {
                            sw.WriteLine(strs[i]);
                        }
                    }
                    return StreamReader.Null;
                }

            }
            catch (Exception)
            {
                return StreamReader.Null;
            }


        }
        public static List<ProductInfo> EXCEL_R_W(bool IsWrite, string fileName, List<string> strs)
        {
            var products = new List<ProductInfo>();
            if (!Directory.Exists(ProductData.appDataPath))
            {
                Directory.CreateDirectory(ProductData.appDataPath);
            }
            if (!fileName.Contains(".xlsx"))
            {
                fileName += ".xlsx";
            }
            if (IsWrite)
            {
                ////写csv
                if (!fileName.Contains(ProductData.appDataPath))
                {
                    fileName = Path.Combine(ProductData.appDataPath, fileName);
                }
                ToExcel.InsertDataToExcel(fileName, strs);
                return null;
            }
            else
            {
                
                using (var package = new ExcelPackage(new FileInfo(fileName)))
                {
                    // 获取第一个工作表
                    var worksheet = package.Workbook.Worksheets[0];

                    // 获取总行数（包括表头）
                    int rowCount = worksheet.Dimension?.Rows ?? 0;
                    if (rowCount < 2) // 只有表头或空表
                        return products;

                    // 从第二行开始（跳过表头）
                    for (int row = 2; row <= rowCount; row++)
                    {
                        try
                        {
                            ProductInfo product = new ProductInfo();

                            // 逐列读取，注意列索引从1开始
                            product.name           = GetCellValueByColumnName(worksheet, GetOneProductInfo(true, "name"), row)?.ToString() ?? "";
                            //product.ItemNum        = GetCellValueByColumnName(worksheet, GetOneProductInfo(true, "ItemNum"), row)?.ToString() ?? "";
                            product.barcode        = GetCellValueByColumnName(worksheet, GetOneProductInfo(true, "barcode"), row)?.ToString() ?? "";
                            //product.productType    = (int)GetCellValueByColumnName(worksheet, GetOneProductInfo(true, "productType"), 5);
                            //product.productUnit    = (int)GetCellValueByColumnName(worksheet, GetOneProductInfo(true, "productUnit"), 5);
                            product.remark         = GetCellValueByColumnName(worksheet, GetOneProductInfo(true, "remark"), row)?.ToString() ?? "";
                            product.price          = Convert.ToDecimal(GetCellValueByColumnName(worksheet, GetOneProductInfo(true, "price"), row));
                            product.priceVip      = Convert.ToDecimal(GetCellValueByColumnName(worksheet, GetOneProductInfo(true, "price_Vip"), row));
                            product.cost           = Convert.ToDecimal(GetCellValueByColumnName(worksheet, GetOneProductInfo(true, "cost"), row     ));
                            //product.count          = Convert.ToDecimal(GetCellValueByColumnName(worksheet, GetOneProductInfo(true, "count"), row));
                            //product.sn             = GetCellValueByColumnName(worksheet, GetOneProductInfo(true, "sn"), row)?.ToString() ?? "";
                            product.expiryDays     = Convert.ToInt32( GetCellValueByColumnName(worksheet, GetOneProductInfo(true, "expiryDays"), row));
                            product.ProductionDate = Convert.ToDateTime(GetCellValueByColumnName(worksheet, GetOneProductInfo(true, "ProductionDate"), row));
                            product.ExpirationDate = Convert.ToDateTime(GetCellValueByColumnName(worksheet, GetOneProductInfo(true, "ExpirationDate"), row));

                            products.Add(product);
                        }
                        catch (Exception ex)
                        {
                            // 记录错误行，可以根据需求处理
                            Console.WriteLine($"读取第 {row} 行出错: {ex.Message}");
                        }
                    }
                }

                return products;
            }
            //ToExcel.GetDataFromCsv("");
        }
    }

    /// <summary>
    /// using OfficeOpenXml;引用epplus
    /// </summary>
    internal class ToExcel
    {
        public static List<string> HeaderNames = new List<string> { "型号", "图号", "流程卡号", "用户", "批次号", "检验点", "批数量", "检验数量", "结论", "检验员", "日期" };
        public static List<string> headerInfoEx = new List<string> { "", "", "", "", "", "", "", "", "", "", "" };
        public struct HeaderInfo
        {
            public string Model;
            public string DrawingNum;
            public string ProcessCardNum;
            public string User;
            public string BatchNum;
            public string InspectionPoint;
            public string BatchQuantity;
            public string InspectionQuantity;
            public string Conclusion;
            public string Inspector;
            public string Date;
        }


        public static string templatePath = Path.Combine(AppContext.BaseDirectory, "DataTemplate.xlsx");

        /// <summary>
        /// 写入表头数据
        /// </summary>
        /// <param name="FileFullName"></param>
        /// <param name="headerInfo"></param>
        /// <param name="pageName"></param>
        public static void InsertReportHeader(string FileFullName, List<string> headerInfo, List<string> dataInfosAll)
        {
            if (!File.Exists(templatePath))
            {
                MessageBox.Show("模板表格缺失");
                return;
            }

            File.Copy(templatePath, FileFullName, true); // 第三个参数表示覆盖

            for (int i = 0; i < dataInfosAll[0].Split(',').Length / 15; i++)
            {
                // 加载源文件
                using (var sourcePackage = new ExcelPackage(new FileInfo(templatePath)))
                {
                    // 获取源文件的第一个工作表
                    var sourceWorksheet = sourcePackage.Workbook.Worksheets[0];

                    // 创建或加载目标文件
                    using (var destPackage = new ExcelPackage(new FileInfo(FileFullName)))
                    {
                        // 如果目标文件是新建的，确保有工作表
                        if (destPackage.Workbook.Worksheets.Count == 0)
                        {
                            destPackage.Workbook.Worksheets.Add("Sheet1");
                        }

                        // 计算目标位置（如果目标位置超过现有工作表数量，则在末尾添加）
                        int insertPosition = destPackage.Workbook.Worksheets.Count;
                        // 复制源工作表到目标包
                        var copiedWorksheet = destPackage.Workbook.Worksheets.Add(
                            $"{new string(sourceWorksheet.Name.Where(c => !char.IsDigit(c)).ToArray())}{i + 2}", // 新工作表名称
                            sourceWorksheet // 要复制的工作表
                        );
                        // 保存目标文件
                        destPackage.Save();
                    }
                }
            }



            int Length = dataInfosAll[0].Split(',').Length;//33
            //int pageCount = Length / 15;//15列数据为一页
            //List<string> dataInfos = new List<string>();
            //for (int i = 0; i < dataInfosAll.Count; i++)
            //{
            //    dataInfos= dataInfosAll[i].Split(',').ToList();
            //}


            //ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(new FileInfo(FileFullName)))
            {
                try
                {
                    int allPage = Length / 15 + 1;
                    for (int page = 0; page < allPage; page++)
                    {


                        ExcelWorksheet worksheet = package.Workbook.Worksheets[page];

                        #region 写入表头内容--表头单元格有合并，插入数据时需要注意插入列数
                        int headerRow = 2;
                        int dataRow = 3;
                        // 收集表头单元格的列位置
                        List<int> headerColumns = new List<int>();
                        int maxColumn = worksheet.Dimension?.Columns ?? 0;

                        for (int col = 1; col <= maxColumn; col++)
                        {
                            var headerCell = worksheet.Cells[headerRow, col];
                            // 跳过空表头
                            if (headerCell.Value == null || string.IsNullOrEmpty(headerCell.Value.ToString()))
                            { continue; }

                            headerColumns.Add(col);
                        }

                        for (int i = 0; i < headerInfo.Count; i++)
                        {
                            // 插入数据
                            worksheet.Cells[dataRow, headerColumns[i]].Value = headerInfo[i];
                        }
                        #endregion

                        #region 写入数据

                        #region 复制结果行
                        int afterRow = 10;//第一行数据的行数
                        int maxCol = 16;//第一行数据的列数
                        int rowCount = dataInfosAll.Count > 1 ? (dataInfosAll.Count - 1) : 0;//插入新数据行数量
                                                                                             //int ResultRow = afterRow + rowCount+1;//结果行的行数

                        // 在第afterRow行下面插入rowCount行
                        worksheet.InsertRow(afterRow + 1, rowCount);

                        // 如果需要复制上一行的内容和格式
                        if (afterRow > 0 && maxCol > 0)
                        {
                            // 获取要复制的源行（插入位置的前一行）
                            var sourceRow = afterRow;

                            // 复制到每一行新插入的行
                            for (int i = 0; i < rowCount; i++)
                            {
                                int targetRow = afterRow + 1 + i;

                                // 复制每个单元格
                                for (int col = 1; col <= maxCol; col++)
                                {
                                    var sourceCell = worksheet.Cells[sourceRow, col];
                                    var targetCell = worksheet.Cells[targetRow, col];

                                    // 复制值
                                    targetCell.Value = sourceCell.Value;

                                    // 复制公式（如果存在）
                                    if (!string.IsNullOrEmpty(sourceCell.Formula))
                                    {
                                        targetCell.Formula = sourceCell.Formula;
                                    }

                                    // 复制样式
                                    targetCell.StyleID = sourceCell.StyleID;

                                    // 复制数字格式
                                    //targetCell.Style.Numberformat.Format = "0.00"; //sourceCell.Style.Numberformat.Format;
                                }
                                worksheet.Cells[targetRow, 1].Value = i + 2;
                                // 复制行高
                                worksheet.Row(targetRow).Height = worksheet.Row(sourceRow).Height;
                            }
                        }
                        #endregion


                        #region 结果行设置公式


                        // 计算数据范围
                        int dataStartRow = afterRow;//数据开始行
                        int dataEndRow = dataStartRow + rowCount;//数据结束行
                        int resultRow = dataEndRow + 1;  // 结果行
                        int resultCol = 2;//结果行开始写公式的列数

                        // 为每一列设置公式
                        for (int col = resultCol; col <= maxCol; col++)
                        {
                            string columnLetter = GetExcelColumnName(col);
                            string formula = BuildFormula(columnLetter, dataStartRow, dataEndRow);
                            // 设置公式
                            worksheet.Cells[resultRow, col].Formula = formula;

                            // 设置单元格样式
                            //worksheet.Cells[resultRow, col].Style.Font.Bold = true;
                            worksheet.Cells[resultRow, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                        #endregion

                        #region 插入数据
                        for (int Row = afterRow; Row < afterRow + rowCount + 1; Row++)
                        {

                            List<string> numbersAll = dataInfosAll[Row - afterRow].Split(',').ToList();
                            List<string> numbers = numbersAll.GetRange(page * 15, (page * 15 + 15) > numbersAll.Count ? numbersAll.Count - page * 15 : 15);
                            for (int i = 0; i < numbers.Count; i++)
                            {
                                worksheet.Cells[Row, i + 2].Value = double.Parse(numbers[i]);
                                worksheet.Cells[Row, i + 2].Style.Numberformat.Format = "0.00";
                            }

                        }

                        #endregion


                        #endregion

                        #region 打印设置
                        //ConfigurePrintSettingsForA4Portrait(worksheet, 1,  worksheet.Dimension.Rows, 1, worksheet.Dimension.Columns);
                        #endregion

                        package.Save();
                    }
                }
                catch (Exception ex)
                {

                }
            }

        }


        /// <summary>
        /// 写入表头数据
        /// </summary>
        /// <param name="FileFullName"></param>
        /// <param name="headerInfo"></param>
        /// <param name="pageName"></param>
        public static void InsertReportHeaderNew(string FileFullName, DataInfosStruct dataInfosStruct)
        {
            List<string> dataInfosAll = dataInfosStruct.dataInfos;
            List<string> headerInfo = dataInfosStruct.headerInfo;
            if (!File.Exists(templatePath))
            {
                MessageBox.Show("模板表格缺失");
                return;
            }

            File.Copy(templatePath, FileFullName, true); // 第三个参数表示覆盖

            for (int i = 0; i < dataInfosAll[0].Split(',').Length / 15; i++)
            {
                // 加载源文件
                using (var sourcePackage = new ExcelPackage(new FileInfo(templatePath)))
                {
                    // 获取源文件的第一个工作表
                    var sourceWorksheet = sourcePackage.Workbook.Worksheets[0];

                    // 创建或加载目标文件
                    using (var destPackage = new ExcelPackage(new FileInfo(FileFullName)))
                    {
                        // 如果目标文件是新建的，确保有工作表
                        if (destPackage.Workbook.Worksheets.Count == 0)
                        {
                            destPackage.Workbook.Worksheets.Add("Sheet1");
                        }

                        // 计算目标位置（如果目标位置超过现有工作表数量，则在末尾添加）
                        int insertPosition = destPackage.Workbook.Worksheets.Count;
                        // 复制源工作表到目标包
                        var copiedWorksheet = destPackage.Workbook.Worksheets.Add(
                            $"{new string(sourceWorksheet.Name.Where(c => !char.IsDigit(c)).ToArray())}{i + 2}", // 新工作表名称
                            sourceWorksheet // 要复制的工作表
                        );
                        // 保存目标文件
                        destPackage.Save();
                    }
                }
            }



            int Length = dataInfosAll[0].Split(',').Length;
            using (var package = new ExcelPackage(new FileInfo(FileFullName)))
            {
                try
                {
                    int allPage = Length / 15 + 1;
                    for (int page = 0; page < allPage; page++)
                    {


                        ExcelWorksheet worksheet = package.Workbook.Worksheets[page];

                        #region 写入表头内容--表头单元格有合并，插入数据时需要注意插入列数
                        int headerRow = 2;
                        int dataRow = 3;
                        // 收集表头单元格的列位置
                        List<int> headerColumns = new List<int>();
                        int maxColumn = worksheet.Dimension?.Columns ?? 0;

                        for (int col = 1; col <= maxColumn; col++)
                        {
                            var headerCell = worksheet.Cells[headerRow, col];
                            // 跳过空表头
                            if (headerCell.Value == null || string.IsNullOrEmpty(headerCell.Value.ToString()))
                            { continue; }

                            headerColumns.Add(col);
                        }

                        for (int i = 0; i < headerInfo.Count; i++)
                        {
                            // 插入数据
                            worksheet.Cells[dataRow, headerColumns[i]].Value = headerInfo[i];
                        }
                        #endregion

                        #region 写入数据

                        #region 复制结果行
                        int afterRow = 10;//第一行数据的行数
                        int maxCol = 16;//第一行数据的列数
                        int rowCount = dataInfosAll.Count > 1 ? (dataInfosAll.Count - 1) : 0;//插入新数据行数量
                                                                                             //int ResultRow = afterRow + rowCount+1;//结果行的行数
                        if (rowCount != 0)
                        {
                            // 在第afterRow行下面插入rowCount行
                            worksheet.InsertRow(afterRow + 1, rowCount);
                        }


                        // 如果需要复制上一行的内容和格式
                        if (afterRow > 0 && maxCol > 0)
                        {
                            // 获取要复制的源行（插入位置的前一行）
                            var sourceRow = afterRow;

                            // 复制到每一行新插入的行
                            for (int i = 0; i < rowCount; i++)
                            {
                                int targetRow = afterRow + 1 + i;

                                // 复制每个单元格
                                for (int col = 1; col <= maxCol; col++)
                                {
                                    var sourceCell = worksheet.Cells[sourceRow, col];
                                    var targetCell = worksheet.Cells[targetRow, col];

                                    // 复制值
                                    targetCell.Value = sourceCell.Value;

                                    // 复制公式（如果存在）
                                    if (!string.IsNullOrEmpty(sourceCell.Formula))
                                    {
                                        targetCell.Formula = sourceCell.Formula;
                                    }

                                    // 复制样式
                                    targetCell.StyleID = sourceCell.StyleID;

                                    // 复制数字格式
                                    //targetCell.Style.Numberformat.Format = "0.00"; //sourceCell.Style.Numberformat.Format;
                                }
                                worksheet.Cells[targetRow, 1].Value = i + 2;
                                // 复制行高
                                worksheet.Row(targetRow).Height = worksheet.Row(sourceRow).Height;
                            }
                        }
                        #endregion


                        #region 结果行设置公式


                        // 计算数据范围
                        int dataStartRow = afterRow;//数据开始行
                        int dataEndRow = dataStartRow + rowCount;//数据结束行
                        int resultRow = dataEndRow + 1;  // 结果行
                        int resultCol = 2;//结果行开始写公式的列数

                        // 为每一列设置公式
                        for (int col = resultCol; col <= maxCol; col++)
                        {
                            string columnLetter = GetExcelColumnName(col);
                            string formula = BuildFormula(columnLetter, dataStartRow, dataEndRow);
                            // 设置公式
                            worksheet.Cells[resultRow, col].Formula = formula;

                            // 设置单元格样式
                            //worksheet.Cells[resultRow, col].Style.Font.Bold = true;
                            worksheet.Cells[resultRow, col].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        }
                        #endregion

                        #region 插入数据
                        for (int Row = afterRow; Row < afterRow + rowCount + 1; Row++)
                        {
                            if (Row - afterRow >= dataInfosAll.Count)
                            {
                                break;
                            }
                            List<string> numbersAll = dataInfosAll[Row - afterRow].Split(',').ToList();
                            List<string> numbers = numbersAll.GetRange(page * 15, (page * 15 + 15) > numbersAll.Count ? numbersAll.Count - page * 15 : 15);
                            for (int i = 0; i < numbers.Count; i++)
                            {
                                if (numbers[i] != "" && numbers[i] != null && numbers[i] != string.Empty)
                                {
                                    worksheet.Cells[Row, i + 2].Value = double.Parse(numbers[i]);
                                    worksheet.Cells[Row, i + 2].Style.Numberformat.Format = "0.000";
                                }

                            }
                            if (Row == afterRow)
                            {


                                List<string> dataInfos_name = dataInfosStruct.dataInfos_name[Row - afterRow].Split(',').ToList()
                                    .GetRange(page * 15, (page * 15 + 15) > numbersAll.Count ? numbersAll.Count - page * 15 : 15);
                                List<string> dataInfos_standard = dataInfosStruct.dataInfos_standard[Row - afterRow].Split(',').ToList()
                                                   .GetRange(page * 15, (page * 15 + 15) > numbersAll.Count ? numbersAll.Count - page * 15 : 15);
                                List<string> dataInfos_upperTolerance = dataInfosStruct.dataInfos_upperTolerance[Row - afterRow].Split(',').ToList()
                                  .GetRange(page * 15, (page * 15 + 15) > numbersAll.Count ? numbersAll.Count - page * 15 : 15);
                                List<string> dataInfos_lowerTolerance = dataInfosStruct.dataInfos_lowerTolerance[Row - afterRow].Split(',').ToList()
                                  .GetRange(page * 15, (page * 15 + 15) > numbersAll.Count ? numbersAll.Count - page * 15 : 15);

                                for (int i = 0; i < dataInfos_name.Count; i++)
                                {
                                    worksheet.Cells[5, i + 2].Value = dataInfos_name[i];
                                    worksheet.Cells[6, i + 2].Value = dataInfos_standard[i];
                                    worksheet.Cells[7, i + 2].Value = dataInfos_upperTolerance[i];
                                    worksheet.Cells[8, i + 2].Value = dataInfos_lowerTolerance[i];
                                    worksheet.Cells[6, i + 2].Style.Numberformat.Format = "0.000";
                                    worksheet.Cells[7, i + 2].Style.Numberformat.Format = "0.000";
                                    worksheet.Cells[8, i + 2].Style.Numberformat.Format = "0.000";

                                }
                            }

                        }


                        #endregion


                        #endregion

                        #region 打印设置
                        //ConfigurePrintSettingsForA4Portrait(worksheet, 1,  worksheet.Dimension.Rows, 1, worksheet.Dimension.Columns);
                        #endregion

                        package.Save();
                    }
                }
                catch (Exception ex)
                {

                }
            }

        }

        public static void InsertDataToExcel(string FileFullName, List<string> dataInfosAll)
        {
            if (!File.Exists(FileFullName))
            {
                using (File.Create(FileFullName))
                {

                }

            }
            // 创建或加载目标文件
            using (var package = new ExcelPackage(new FileInfo(FileFullName)))
            {
                // 如果目标文件是新建的，确保有工作表
                if (package.Workbook.Worksheets.Count == 0)
                {
                    package.Workbook.Worksheets.Add("Sheet1");
                }
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];


                #region 111



                // 获取当前已使用的最后一行索引
                int lastRow = worksheet.Dimension?.End.Row ?? 0; // 如果工作表为空，Dimension 为 null，则返回 0

                // 从下一行开始追加数据（假设数据从第 1 列开始）
                int startRow = lastRow + 1;


                // 将每个字符串按分隔符拆分为 object[]，并组装成 List<object[]>
                List<object[]> rows = dataInfosAll
                    .Select(line => line.Split(',').Cast<object>().ToArray())
                    .ToList();

                // 从下一行开始批量插入
                worksheet.Cells[lastRow + 1, 1].LoadFromArrays(rows);
                // 从下一行开始加载数据

                // 可选：调整列宽
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                #endregion

                // 保存目标文件
                package.Save();
            }

        }
        #region
        // ==================== 优化的A4纵向打印设置 ====================
        public static void ConfigurePrintSettingsForA4Portrait(ExcelWorksheet worksheet, int startRow = 1, int endRow = 30, int startCol = 1, int endCol = 30, int zoom = 100)
        {
            // 1. 设置打印区域
            worksheet.PrinterSettings.PrintArea = worksheet.Cells[startRow, startCol, endRow, endCol];

            // 2. 设置纸张方向和大小
            worksheet.PrinterSettings.Orientation = eOrientation.Portrait; // A4纵向
            worksheet.PrinterSettings.PaperSize = ePaperSize.A4;

            // 3. 设置页边距（A4纵向推荐值）
            worksheet.PrinterSettings.TopMargin = 1.0M;      // 上边距 1.0英寸
            worksheet.PrinterSettings.BottomMargin = 1.0M;   // 下边距 1.0英寸
            worksheet.PrinterSettings.LeftMargin = 0.75M;    // 左边距 0.75英寸
            worksheet.PrinterSettings.RightMargin = 0.75M;   // 右边距 0.75英寸
            worksheet.PrinterSettings.HeaderMargin = 0.5M;   // 页眉边距 0.5英寸
            worksheet.PrinterSettings.FooterMargin = 0.5M;   // 页脚边距 0.5英寸

            // 4. 设置缩放（100%正常大小）
            worksheet.PrinterSettings.Scale = zoom;
            worksheet.PrinterSettings.FitToPage = false;

            // 5. 设置每页都打印表头（前9行）
            worksheet.PrinterSettings.RepeatRows = new ExcelAddress("1:9");

            // 6. 设置网格线和标题
            worksheet.PrinterSettings.ShowGridLines = true;
            //worksheet.PrinterSettings.PrintTitlesRow = "1:9";

            // 7. 设置页眉
            worksheet.HeaderFooter.OddHeader.CenteredText = "物理尺寸检验记录单";
            worksheet.HeaderFooter.OddHeader.RightAlignedText = string.Format("打印日期: {0:yyyy-MM-dd}", DateTime.Now);
            //worksheet.HeaderFooter.OddHeader.FontName = "宋体";
            //worksheet.HeaderFooter.OddHeader.FontSize = 10;

            // 8. 设置页脚
            worksheet.HeaderFooter.OddFooter.CenteredText = "第 &P 页 / 共 &N 页";
            //worksheet.HeaderFooter.OddFooter.FontName = "宋体";
            //worksheet.HeaderFooter.OddFooter.FontSize = 9;

            // 9. 设置页面居中
            worksheet.PrinterSettings.HorizontalCentered = true;

            // 10. 计算并设置分页符
            int rowsPerPageForA4Portrait = CalculateRowsPerPageForA4Portrait();
            SetPageBreaksForA4Portrait(worksheet, startRow, endRow, rowsPerPageForA4Portrait);

            // 11. 调整列宽以适应A4纵向
            AdjustColumnWidthsForA4Portrait(worksheet, startCol, endCol);

            Console.WriteLine("A4纵向打印设置已配置完成");
        }

        // 计算A4纵向每页可打印的行数
        private static int CalculateRowsPerPageForA4Portrait()
        {
            // A4纸张尺寸：21.0cm × 29.7cm
            // 减去上下边距后，可用高度约为：29.7cm - 2.54cm*2 = 24.62cm (上边距1英寸=2.54cm)
            // 假设行高为0.5cm，则每页可打印行数约为：24.62 / 0.5 ≈ 49行
            // 再减去表头9行，实际数据行约为40行
            return 49; // 包含表头的总行数
        }

        // 为A4纵向设置分页符
        private static void SetPageBreaksForA4Portrait(ExcelWorksheet worksheet, int startRow, int endRow, int rowsPerPage)
        {
            int totalRows = endRow - startRow + 1;
            int pageCount = (int)Math.Ceiling((double)totalRows / rowsPerPage);

            if (pageCount > 1)
            {
                // 清除现有的分页符
                //worksheet.PrinterSettings.ClearPageBreaks();

                for (int page = 1; page < pageCount; page++)
                {
                    int pageBreakRow = startRow + (page * rowsPerPage) - 1;
                    if (pageBreakRow < endRow)
                    {
                        worksheet.Row(pageBreakRow).PageBreak = true;
                    }
                }

                Console.WriteLine($"设置了 {pageCount - 1} 个分页符");
            }
        }

        // 调整列宽以适应A4纵向
        private static void AdjustColumnWidthsForA4Portrait(ExcelWorksheet worksheet, int startCol, int endCol)
        {
            // A4纵向可用宽度：21.0cm - 1.905cm*2 = 17.19cm (左右边距各0.75英寸=1.905cm)
            // Excel中1个单位宽度 ≈ 0.18cm，所以可用宽度单位约为：17.19 / 0.18 ≈ 95

            double maxTotalWidthForA4Portrait = 95; // A4纵向最大总列宽
            double currentTotalWidth = 0;

            // 计算当前总列宽
            for (int col = startCol; col <= endCol; col++)
            {
                currentTotalWidth += worksheet.Column(col).Width;
            }

            // 如果超过最大宽度，按比例缩放
            if (currentTotalWidth > maxTotalWidthForA4Portrait)
            {
                double scaleFactor = maxTotalWidthForA4Portrait / currentTotalWidth;

                for (int col = startCol; col <= endCol; col++)
                {
                    double originalWidth = worksheet.Column(col).Width;
                    double newWidth = originalWidth * scaleFactor;

                    // 确保最小列宽为3
                    if (newWidth < 3)
                    {
                        newWidth = 3;
                    }

                    worksheet.Column(col).Width = newWidth;
                }

                Console.WriteLine($"列宽已从 {currentTotalWidth:F1} 调整到 {maxTotalWidthForA4Portrait:F1} 以适应A4纵向");
            }
        }
        #endregion

        /// <summary>
        /// 构建结果行公式
        /// </summary>
        /// <param name="col">列名B-P</param>
        /// <param name="dataStartRow">数据开始行</param>
        /// <param name="dataEndRow">数据结束行</param>
        /// <param name="gaugeRow">量具行</param>
        /// <param name="standardRow">标准行</param>
        /// <param name="upperTolRow">上公差</param>
        /// <param name="lowerTolRow">下公差</param>
        /// <returns></returns>
        private static string BuildFormula(string col, int dataStartRow, int dataEndRow,
                                          int gaugeRow = 9, int standardRow = 6, int upperTolRow = 7, int lowerTolRow = 8)
        {
            return
                $"=IF(AND(SUM({col}{dataStartRow}:{col}{dataEndRow})=0,{col}{gaugeRow}=\"\"),\"\"," +
                $"IF(AND({col}{gaugeRow}=\"三坐标\",{col}{dataStartRow}=\"\"),\"合格\"," +
                $"IF(OR(COUNTIF({col}{dataStartRow}:{col}{dataEndRow},\">\"&{col}{standardRow}+{col}{upperTolRow})>0," +
                $"COUNTIF({col}{dataStartRow}:{col}{dataEndRow},\"<\"&{col}{standardRow}+{col}{lowerTolRow})>0),\"不合格\",\"合格\")))";
        }
        /// <summary>
        /// 将列索引转换为Excel列名（A, B, C, ...）
        /// </summary>
        private static string GetExcelColumnName(int columnIndex)
        {
            int dividend = columnIndex;
            string columnName = string.Empty;

            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo) + columnName;
                dividend = (dividend - modulo) / 26;
            }

            return columnName;
        }
        /// <summary>
        /// 根据第一行的列名称，获取指定行在该列的值（返回 object）
        /// </summary>
        public static object GetCellValueByColumnName(ExcelWorksheet worksheet, string columnName, int rowIndex)
        {
            // 1. 建立列名 → 列索引映射（第一行）
            var columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            int colCount = worksheet.Dimension?.Columns ?? 0;
            for (int col = 1; col <= colCount; col++)
            {
                string header = worksheet.Cells[1, col].Value?.ToString()?.Trim();
                if (!string.IsNullOrEmpty(header))
                {
                    columnMap[header] = col;
                }
            }

            // 2. 查找列名是否存在
            if (!columnMap.TryGetValue(columnName, out int columnIndex))
            {
                throw new ArgumentException($"未找到列名 '{columnName}'，请检查第一行表头。");
            }

            // 3. 返回目标行该列的值
            return worksheet.Cells[rowIndex, columnIndex].Value;
        }

        #region
        /// <summary>
        /// 获取csv 补偿文件信息
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>返回补偿值，每个string内包含一个穴的补偿值 并以“，”隔开</returns>
        public static List<string> GetCSVData(string path)
        {
            List<string> strs = new List<string>();
            string strline = null;

            FileStream fs = null;
            Encoding encoding = Encoding.GetEncoding("GB2312");
            if (!File.Exists(path))
            {
                return null;
            }

            using (fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                StreamReader sr = null;
                using (sr = new System.IO.StreamReader(fs, encoding))
                {
                    while ((strline = sr.ReadLine()) != null)
                    {
                        strs.Add(strline);
                    }
                }
            }

            return strs;
        }


        public static DataInfosStruct GetDataFromCsv(string path)
        {
            List<string> dataInfos = new List<string>();
            List<string> dataInfos_standard = new List<string>();
            List<string> dataInfos_upperTolerance = new List<string>();
            List<string> dataInfos_lowerTolerance = new List<string>();
            List<string> dataInfos_name = new List<string>();
            try
            {



                List<string> csvData = ToExcel.GetCSVData(path);
                #region 获取数据范围
                var data0 = csvData[0].Split(',');
                int col_start = 0;//数据列开始
                int col_end = data0.Length - 1;//数据列结束
                int row_start = 10;//数据行开始
                int row_end = 0;//数据行结束
                int row_max = 0, row_min = 0, row_standard = 0;//上限，下限，标准值的行数

                for (int i = 0; i < data0.Length; i++)
                {
                    if (data0[i].Contains("[1]"))
                    {
                        col_start = i;
                        break;
                    }
                }
                bool IsGetStart = true;
                for (int j = 0; j < csvData.Count; j++)
                {
                    var dataTemp = csvData[j].Split(',')[0];
                    bool isInt = int.TryParse(dataTemp, out int num);
                    if (isInt && IsGetStart)
                    {
                        IsGetStart = false;
                        row_start = j;
                    }
                    if (!isInt && j > row_start && row_end == 0)
                    {
                        row_end = j - 1;
                    }
                    if (dataTemp == "上限规格值")
                    {
                        row_max = j;
                    }
                    else if (dataTemp == "下限规格值")
                    {
                        row_min = j;
                    }
                    else if (dataTemp == "设计值")
                    {
                        row_standard = j;
                    }
                }

                Console.WriteLine($"col_start{col_start}col_end{col_end}row_start{row_start}row_end{row_end}");
                #endregion
                //int rows = row_end-row_start;
                //int cols = col_end- col_start;
                for (int row = row_start; row < row_end + 1; row++)
                {
                    string dataInfo = "";
                    for (int col = col_start; col < col_end + 1; col++)
                    {
                        if (col < col_end)
                        {
                            dataInfo += csvData[row].Split(',')[col] + ",";
                        }
                        else
                        {
                            dataInfo += csvData[row].Split(',')[col];
                        }

                    }
                    dataInfos.Add(dataInfo);
                }
                string dataInfo_standard = "";
                string dataInfo_upperTolerance = "";
                string dataInfo_lowerTolerance = "";
                string dataInfo_name = "";
                string Decimal = "G29";
                for (int col = col_start; col < col_end + 1; col++)
                {

                    if (col < col_end)
                    {
                        dataInfo_standard += (decimal.Parse(csvData[row_standard].Split(',')[col])).ToString(Decimal) + ",";
                        dataInfo_upperTolerance += (decimal.Parse(csvData[row_max].Split(',')[col]) - decimal.Parse(csvData[row_standard].Split(',')[col])).ToString(Decimal) + ",";
                        dataInfo_lowerTolerance += (decimal.Parse(csvData[row_min].Split(',')[col]) - decimal.Parse(csvData[row_standard].Split(',')[col])).ToString(Decimal) + ",";
                        dataInfo_name += csvData[0].Split(',')[col] + ",";
                    }
                    else
                    {
                        dataInfo_standard += (decimal.Parse(csvData[row_standard].Split(',')[col])).ToString(Decimal);
                        dataInfo_upperTolerance += (decimal.Parse(csvData[row_max].Split(',')[col]) - decimal.Parse(csvData[row_standard].Split(',')[col])).ToString(Decimal);
                        dataInfo_lowerTolerance += (decimal.Parse(csvData[row_min].Split(',')[col]) - decimal.Parse(csvData[row_standard].Split(',')[col])).ToString(Decimal);
                        dataInfo_name += csvData[0].Split(',')[col];
                    }

                }
                dataInfos_standard.Add(dataInfo_standard);
                dataInfos_upperTolerance.Add(dataInfo_upperTolerance);
                dataInfos_lowerTolerance.Add(dataInfo_lowerTolerance);
                dataInfos_name.Add(dataInfo_name);
            }
            catch (Exception ex)
            {

            }
            DataInfosStruct dataInfosStruct = new DataInfosStruct()
            {
                //headerInfo =  new List<string> {"FPP6230-00-02-001QX","UG6.106" ,"W231277-0","无锡华测",
                //"","成品检验","","13","合格","",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")  },
                headerInfo = headerInfoEx,
                dataInfos_name = dataInfos_name,
                dataInfos_standard = dataInfos_standard,
                dataInfos_upperTolerance = dataInfos_upperTolerance,
                dataInfos_lowerTolerance = dataInfos_lowerTolerance,
                dataInfos = dataInfos,
            };
            return dataInfosStruct;
        }
        public struct DataInfosStruct
        {
            public List<string> headerInfo;
            public List<string> dataInfos_name;
            public List<string> dataInfos_standard;
            public List<string> dataInfos_upperTolerance;
            public List<string> dataInfos_lowerTolerance;
            public List<string> dataInfos;
        }
        #endregion


    }




}
