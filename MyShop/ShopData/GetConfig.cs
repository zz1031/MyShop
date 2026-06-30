using System;
using System.Collections.Generic;
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


namespace MyShop.ShopData
{

    public class GetConfig
    {

        public string INI_R_W(bool IsWrite,string name, string value=null, string sectionName=null, string key=null)
        {
            string result = null;
            if (!name.Contains(AppDomain.CurrentDomain.BaseDirectory))
            {
               name= AppDomain.CurrentDomain.BaseDirectory + name;
            }
            // 1. 读取INI文件
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile("config.ini");
            if (!IsWrite)
            {
                // 2. 获取配置值
                value = data[sectionName][key];
                result=value;
       
            }
            else
            {
                // 3. 修改或新增配置
                data[sectionName][key] = value;

                // 4. 保存回文件
                parser.WriteFile(name, data);
            }
            return result;
        }





    }






}
