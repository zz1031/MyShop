using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace MyShop.ShopData
{
    public class MySqlCtrl
    {
        //MySqlCtrl.Instance.Init("localhost", "3306", "root", "1234", "abab12");
        #region 单例模式

        private static MySqlCtrl instance = null;

        private static object obj = new object();

        private MySqlCtrl()
        {
        }

        /// <summary>
        /// 实例化
        /// </summary>
        public static MySqlCtrl Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (obj)
                    {
                        if (instance == null)
                        {
                            instance = new MySqlCtrl();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        MySqlConnection MySqlConnection;

        private string _mysqlIP = "localhost";
        private string _mysqlPort = "3306";
        private string _userID = "root";
        private string _password = "root";
        private string _dataBsaeName = "player";
        private string connectInfo = "";

        private MySqlConnection conn;



        public void Init(string ip, string port, string userid, string password, string dataBsaeName)
        {
            _mysqlIP = ip;
            _mysqlPort = port;
            _userID = userid;
            _password = password;
            _dataBsaeName = dataBsaeName;
        }


        public bool OpenMysql()
        {
            bool isOpen = false;
            try
            {
                connectInfo = string.Format
                    ("Server = {0}; port = {1}; User ID = {2}; Password = {3}; " +
                    "Pooling=true; Charset = utf8; SslMode = None; ",
             _mysqlIP, _mysqlPort, _userID, _password);

                using (conn = new MySqlConnection(connectInfo))
                {
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    conn.Open();
                    isOpen = true;
                    cmd.Dispose();
                    conn.Close();
                }
            }
            catch (Exception e)
            {
            }
            return isOpen;
        }

        #region
        /// <summary>
        /// 执行mysql的语句
        /// </summary>
        /// <param name="str_sql">mysql语句</param>
        /// <returns>返回受执行语句影响的函数或者执行错误的错误原因</returns>
        public string ExcuteMysql(string str_sql)
        {
            int result = 0;
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
            }
            catch { }
            using (MySqlCommand cmd = new MySqlCommand(str_sql, conn))
            {
                try
                {
                    result = cmd.ExecuteNonQuery();
                }
                catch (MySqlException ex)
                {
                    return ex.Message;
                }
                finally
                {
                    conn.Close();
                }
            }
            return result.ToString();
        }
        

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns></returns>
        public string CreateDatabase(string databaseName)
        {
            string ret = "0";

            string query = $"create database IF NOT EXISTS `{databaseName}`";

            ret = ExcuteMysql(query);
            _dataBsaeName = databaseName;
            return ret;
        }
        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns></returns>
        public string DeleteDatabase(string databaseName)
        {
            string ret = "0";

            string query = $"drop database IF EXISTS `{databaseName}`";

            ret = ExcuteMysql(query);

            return ret;
        }
        /// <summary>
        /// 使用数据库-初始要调用后续所以表操作会在这个数据库下进行
        /// </summary>
        /// <param name="databaseName">数据库名称</param>
        /// <returns></returns>
        public string UseDatabase(string databaseName)
        {
            string ret = "0";

            string query = $"use `{databaseName}`";

            ret = ExcuteMysql(query);

            return ret;
        }


        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="databaseName">表名称</param>
        /// <param name="colsName">表头名</param>
        /// <param name="colType">表头类型</param>
        /// <returns></returns>
        public string CreatDatatable(string tableName, string[] colsName, string[] colType)
        {
            
            if (colsName.Length != colType.Length)
            {
                return "表头名和表头类型数量不一致";
            }
            string ret = "0";

            string query = $"create table IF NOT EXISTS `{tableName}` (";
            for (int i = 0; i < colsName.Length-1; i++)
            {
                query += $"{colsName[i]} {colType[i]},"; 
            }
            query += $"{colsName[colsName.Length-1]} {colType[colsName.Length-1]})";

            ret = ExcuteMysql(query);

            return ret;
        }
        /// <summary>
        /// 插入数据到表中
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="values">插入值</param>
        /// <returns></returns>
        public string InsertDataToTable(string tableName, List<string> values )
        {
            
            string ret = "0";

            string query = $"insert into {tableName} values (";

            for (int i = 0; i < values.Count - 1; i++)
            {
                query += $"'{values[i]}',";
            }
            query += $"'{values[values.Count-1]}')";

            ret = ExcuteMysql(query);

            return ret;
        }
        /// <summary>
        /// 更新数据在表中
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="changeStr">更改项,例： count=0</param>
        /// <param name="conditionStr">判断条件,例：name="abc"</param>
        /// <param name="judge">条件是and或or</param>
        /// <returns></returns>
        public string UpdateDataToTable(string tableName, List<string> changeStr, List<string> conditionStr, string judge = "and")
        {

            string ret = "0";

            string query = $"update {tableName} set ";

            for (int i = 0; i < changeStr.Count - 1; i++)
            {
                query += $"{changeStr[i]},";
            }
            query += $"{changeStr[changeStr.Count - 1]} where ";

            for (int i = 0; i < conditionStr.Count - 1; i++)
            {
                query += $"{conditionStr[i]} {judge} ";
            }
            query += $"{conditionStr[conditionStr.Count - 1]}";

            ret = ExcuteMysql(query);

            return ret;
        }

        public string DeleteDataToTable(string tableName, List<string> conditionStr,string judge="and")
        {

            string ret = "0";

            string query = $"DELETE FROM {tableName} WHERE ";

            for (int i = 0; i < conditionStr.Count - 1; i++)
            {
                query += $"{conditionStr[i]} {judge} ";
            }
            query += $"{conditionStr[conditionStr.Count - 1]}";

            ret = ExcuteMysql(query);

            return ret;
        }
        #endregion
    }
}
