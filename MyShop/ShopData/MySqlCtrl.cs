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

            string query = "create database IF NOT EXISTS " + databaseName;

            ret = ExcuteMysql(query);

            return ret;
        }


        #endregion
    }
}
