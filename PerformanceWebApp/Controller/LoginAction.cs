using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace PerformanceWebApp.Controller {
    [Description("登陆页面业务类")]
    public class LoginAction {
        //*连接数据库*
        private Controller.MSSQLHelper dbSQL = new Controller.MSSQLHelper();
        private Module.Employee emp = null;

        /// <summary>
        /// --== 登陆验证账号、密码 ==
        ///--== 排除作废、获取状态、是否主管的字段 ==
        /// </summary>
        public Module.Employee GetEmployee(string emCode, string pwd, out string mag) {
            mag = string.Empty;
            emp = new Module.Employee();
            string strSQL = @"Select FID,FEmCode,FStatus,FIsDirector From FY_Performance_Employee 
                     Where FEmCode = @FEmCode And FPassword = @FPwd and FCancellation = 0 and FStatus <> 3";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FEmCode",emCode),
                new SqlParameter("@FPwd",pwd),
            };
            try {
                DataTable dt = dbSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count > 0) {
                    emp.FID = dt.Rows[0]["FID"].ToString();
                    emp.FEmCode = dt.Rows[0]["FEmCode"].ToString();
                    emp.FStatus = dt.Rows[0]["FStatus"].ToString();
                    emp.FIsDirector = Convert.ToBoolean(dt.Rows[0]["FIsDirector"]);
                }
                else {
                    emp = null;
                    mag = "用户或密码不正确，请重新输入。";
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
                emp = null;
            }
            return emp;
        }
        /// <summary>
        /// 验证是否【代评分】的员工
        /// </summary>
        public object VerifyDaiGradeEmloye(string emCode) {
            string strSQL = @"Select FEmCode From FY_Performance_Employee Where FStatus = 3 and FCancellation = 0 and FEmCode = @FEmCode";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FEmCode",emCode),
            };
            try {
                return dbSQL.GetValueByText(strSQL, paras);
            }
            catch (Exception ex) {
                throw ex;
            }
        }
        /// <summary>
        /// 验证是否【代评分账号】
        /// </summary>
        public object VerifyDaiGrader(string emCode) {
            string strSQL = @"Select top 1 1 From FY_Performance_Employee_DaiGrade Where FDaiPGradeID = @FEmCode";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FEmCode",emCode),
            };
            try {
                return dbSQL.GetValueByText(strSQL, paras);
            }
            catch (Exception ex) {
                throw ex;
            }
        }
    }
}