using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.ComponentModel;


namespace PerformanceWebApp.Controller {
    [Description("Sql Server数据库底层类")]
    public class MSSQLHelper
    {
        private string connectionText = ConfigurationManager.ConnectionStrings["sqlconnectionString"].ToString();
        /// <summary>
        /// 使用文本获取结果集（单表）
        /// </summary>
        /// <param name="sqltext">SQL文本</param>
        /// <param name="paras">参数组</param>
        /// <returns>DataTable</returns>
        public DataTable GetTableByText(string sqltext, SqlParameter[] paras)
        {
            SqlConnection conn = new SqlConnection(connectionText);
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand
            {
                CommandText = sqltext,
                CommandType = CommandType.Text,
                Connection = conn,
            };
            cmd.Parameters.AddRange(paras);
            try
            {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);               
                if (sdr.HasRows)
                {
                    dt.Load(sdr);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return dt;
        }
        /// <summary>
        /// 使用文本获取结果集（单表）
        /// </summary>
        public DataTable GetTableByText(string sqltext) {
            SqlConnection conn = new SqlConnection(connectionText);
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand {
                CommandText = sqltext,
                CommandType = CommandType.Text,
                Connection = conn,
            };
            try {
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (sdr.HasRows) {
                    dt.Load(sdr);
                }
            }
            catch (Exception ex) {

                throw ex;
            }
            return dt;
        }
        /// <summary>
        /// 使用Proc获取数据集
        /// </summary>
        /// <param name="sqltext">proc名称</param>
        /// <param name="paras">proc参数</param>
        /// <returns>DataSet</returns>
        public DataSet GetDataSetByProc(string sqltext, SqlParameter[] paras)
        {
            SqlConnection conn = new SqlConnection(connectionText);
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand
            {
                Connection = conn,
                CommandText = sqltext,
                CommandType = CommandType.StoredProcedure,
            };
            cmd.Parameters.AddRange(paras);

            try
            {
                conn.Open();
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { conn.Close(); }
            return ds;
        }
        /// <summary>
        /// 使用文本查询获取返回值
        /// </summary>
        /// <param name="sqltext">查询文本</param>
        /// <param name="paras">参数组</param>
        /// <returns>object</returns>
        public object GetValueByText(string sqltext, SqlParameter[] paras)
        {
            SqlConnection conn = new SqlConnection(connectionText);
            object resultValue = null;
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.Text,
                CommandText = sqltext,
                Connection = conn,
            };
            cmd.Parameters.AddRange(paras);
            try
            {
                conn.Open();
                resultValue = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { conn.Close(); }
            return resultValue;
        }
        /// <summary>
        /// 使用文本增删改，返回结果值
        /// </summary>
        /// <param name="sqltext">SQL文本</param>
        /// <param name="paras">参数组</param>
        /// <returns>int</returns>
        public int GetUpdateByText(string sqltext, SqlParameter[] paras)
        {
            SqlConnection conn = new SqlConnection(connectionText);
            int result = 0;
            SqlCommand cmd = new SqlCommand
            {
                CommandType = CommandType.Text,
                CommandText = sqltext,
                Connection = conn,
            };
            cmd.Parameters.AddRange(paras);
            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally { conn.Close(); }
            return result;
        }

    }
}
