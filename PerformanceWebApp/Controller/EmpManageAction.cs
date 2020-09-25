using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace PerformanceWebApp.Controller {
    [Description("员工管理逻辑层")]
    public class EmpManageAction {
        private MSSQLHelper mSSQL = new MSSQLHelper();
        /// <summary>
        /// 获取员工信息
        /// </summary>
        public List<Module.Employee> GetEmployees(out string mag, params string[] args) {
            mag = string.Empty;
            string strSQL = @"Select FID,FEmCode,FEmName,FExamerID,FExmName,FCDate,ISNULL(FMDate,'2020-08-03') FMDate,FDpID,FDpName,FPosID,FPosName
            ,FRank,FDaiID,FDaiPGrade,FStatus,FCancellation,FPassword,FIsDirector,FIsDaiGrader 
			From (Select emp.FID,emp.FEmCode,emp.FEmName,emp.FExamerID,exm.FEmName FExmName,emp.FCDate
            ,emp.FMDate,dep.FID FDpID,dep.FDpName,pos.FID FPosID,pos.FFCName FPosName,pos.FRank,case when dai.FEmCode is null then '' else dai.FEmCode end as FDaiID
			,case when dai.FEmName is null then '' else dai.FEmName end as FDaiPGrade
            ,case when emp.FStatus = 0 then '正式' when emp.FStatus = 1 then '试用' when emp.FStatus = 2 then '离职' 
            when emp.FStatus = 3 then '代评分' when emp.FStatus = 4 then '退休' when emp.FStatus = 98 then '管理员' when emp.FStatus = 99 then 'HR权限' 
            else '未知' end as FStatus,emp.FCancellation,emp.FPassword,emp.FIsDirector,emp.FIsDaiGrader From FY_Performance_Employee emp 
            Left Join FY_Performance_Department dep on emp.FDepartment=dep.FID 
            Left Join FY_Performance_Position pos on emp.FPosition=pos.FID 
            Left Join FY_Performance_Employee exm on emp.FExamerID=exm.FEmCode
            Left Join (select a.FEmCode FEmcode_1,b.FEmCode,b.FEmName from FY_Performance_Employee_DaiGrade a 
			left join FY_Performance_Employee b on a.FDaiPGradeID=b.FEmCode and a.FCancellation=0) dai on emp.FEmCode=dai.FEmcode_1	
            Where emp.FLockTB = 0) a Where a.FCancellation like '%'+@FCancellation+'%' and a.FStatus like '%'+@FStatus+'%' 
            and (a.FEmCode like '%'+@text+'%' or a.FEmName like '%'+@text+'%' or a.FDpName 
            like '%'+@text+'%' or a.FPosName like '%'+@text+'%')";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FCancellation",args[0]),
                new SqlParameter("@FStatus",args[1]),
                new SqlParameter("@text",args[2]),
            };
            List<Module.Employee> employees = new List<Module.Employee>();
            try {
                DataTable dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count > 0) {
                    for (int i = 0; i < dt.Rows.Count; i++) {
                        Module.Employee employee =
                        new Module.Employee {
                            FID = dt.Rows[i]["FID"].ToString(),
                            FEmCode = dt.Rows[i]["FEmCode"].ToString(),
                            FEmName = dt.Rows[i]["FEmName"].ToString(),
                            FExamerID = dt.Rows[i]["FExamerID"].ToString(),
                            FExamerName = dt.Rows[i]["FExmName"].ToString(),
                            FCDate = Convert.ToDateTime(dt.Rows[i]["FCDate"]),
                            FMDate = Convert.ToDateTime(dt.Rows[i]["FMDate"]),
                            FDepartment = dt.Rows[i]["FDpName"].ToString(),
                            FPosition = dt.Rows[i]["FPosName"].ToString(),
                            FRank = dt.Rows[i]["FRank"].ToString(),
                            FDaiPGrade = dt.Rows[i]["FDaiPGrade"].ToString(),
                            FStatus = dt.Rows[i]["FStatus"].ToString(),
                            FCancellation = Convert.ToBoolean(dt.Rows[i]["FCancellation"].ToString()),
                            FDepartmentID = dt.Rows[i]["FDpID"].ToString(),
                            FPositionID = dt.Rows[i]["FPosID"].ToString(),
                            FDaiPGradeID = dt.Rows[i]["FDaiID"].ToString(),
                            FPassword = dt.Rows[i]["FPassword"].ToString(),
                            FIsDirector = Convert.ToBoolean(dt.Rows[i]["FIsDirector"]),
                            FIsDaiGrader = Convert.ToBoolean(dt.Rows[i]["FIsDaiGrader"]),
                        };
                        employees.Add(employee);
                    }
                }
                if (employees.Count <= 0) {
                    return null;
                }
                return employees;
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 复评人查询表
        /// </summary>
        public DataTable GetRepetGrader(out string mag, params string[] parameters) {
            mag = string.Empty;
            string strSQL = @"Select FID,FEmCode,FEmName From FY_Performance_Employee Where FIsDirector=1 And FStatus not in (1,2,3,4) 
            and (FEmCode like '%'+@text+'%' or FEmName like '%'+@text+'%')";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@text",parameters[0]),
            };
            try {
                DataTable dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count <= 0) {
                    dt = null;
                    mag = "没有查询的数据，请联系管理员。";
                }
                return dt;
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 职位查询表
        /// </summary>
        public DataTable GetPosition(out string mag, params string[] parameters) {
            mag = string.Empty;
            string strSQL = @"Select FID,FFCName From FY_Performance_Position Where FFCName like '%'+@text+'%'";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@text",parameters[0]),
            };
            try {
                DataTable dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count <= 0) {
                    dt = null;
                    mag = "没有查询的数据，请联系管理员。";
                }
                return dt;
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 部门查询表
        /// </summary>
        public DataTable GetDepartment(out string mag, params string[] parameters) {
            mag = string.Empty;
            string strSQL = @" Select FID,FDpName From FY_Performance_Department Where FDpName like '%'+@text+'%'";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@text",parameters[0]),
            };
            try {
                DataTable dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count <= 0) {
                    dt = null;
                    mag = "没有查询的数据，请联系管理员。";
                }
                return dt;
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 代评分用户查询表
        /// </summary>
        public DataTable GetDaiGrader(out string mag, params string[] parameters) {
            mag = string.Empty;
            string strSQL = @"Select FID,FEmCode,FEmName From FY_Performance_Employee Where FIsDaiGrader = 1 
            and (FEmCode like '%'+@text+'%' or FEmName like '%'+@text+'%')";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@text",parameters[0]),
            };
            try {
                DataTable dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count <= 0) {
                    dt = null;
                    mag = "没有查询的数据，请联系管理员。";
                }
                return dt;
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 更新员工信息表
        /// </summary>
        public bool UpdateEmployeeDetail(Module.Employee employee, out string mag) {
            mag = string.Empty;
            string strSQL = @"Update FY_Performance_Employee 
            Set FExamerID=@FExamerID,FCDate=@FCDate,FMDate=@FMDate,FEmCode=@FEmCode,FEmName=@FEmName,FPassword=@FPassword,FDepartment=@FDpID,FPosition=@FPostID,FStatus=@FStatus
            ,FIsDirector=@FIsDirector,FIsDaiGrader=@FIsDaiGrader Where FID=@FID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FExamerID",employee.FExamerID),
                new SqlParameter("@FCDate",employee.FCDate),
                new SqlParameter("@FMDate",employee.FMDate),
                new SqlParameter("@FEmCode",employee.FEmCode),
                new SqlParameter("@FEmName",employee.FEmName),
                new SqlParameter("@FPassword",employee.FPassword),
                new SqlParameter("@FDpID",employee.FDepartmentID),
                new SqlParameter("@FPostID",employee.FPositionID),
                new SqlParameter("@FStatus",employee.FStatus),
                new SqlParameter("@FIsDirector",employee.FIsDirector),
                new SqlParameter("@FIsDaiGrader",employee.FIsDaiGrader),
                new SqlParameter("@FID",employee.FID),
            };
            try {
                object x = mSSQL.GetUpdateByText(strSQL, paras);
                if (x == null || Convert.ToInt32(x) != 1) {
                    mag = "数据更新失败，请联系管理查找原因。";
                    return false;
                }
                return true;
            }
            catch (Exception ex) {
                mag = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 校验是否新的代评分用户
        /// </summary>
        public bool ValidateNewDaiGrade(out string mag,out int status, params string[] parameters) {
            mag = string.Empty;
            string strSQL = @"Select top 1 FCancellation From FY_Performance_Employee_DaiGrade Where FEmCode=@FEmCode";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FEmCode",parameters[0]),
            };
            try {
                object x = mSSQL.GetValueByText(strSQL, paras);
                if (x != null) {
                    status = Convert.ToInt32(x);
                    return true;
                }
                else {
                    status = 0;
                    return false;
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
                status = -1;
                return false;
            }
        }
        /// <summary>
        /// 新增代评分表记录
        /// </summary>
        public bool InsertDaiGradeRecord(out string mag, params string[] parameters) {
            mag = string.Empty;
            string strSQL = @"Insert Into FY_Performance_Employee_DaiGrade(FCDate,FExamerID,FEmCode,FDaiPGradeID) 
            Values(GETDATE(),@FExamerID,@FEmCode,@FDaiPGradeID)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FExamerID",parameters[0]),
                new SqlParameter("@FEmCode",parameters[1]),
                new SqlParameter("@FDaiPGradeID",parameters[2]),
            };
            try {
                object x = mSSQL.GetUpdateByText(strSQL, paras);
                if (x == null || Convert.ToInt32(x) != 1) {
                    mag = "新增代评分表记录失败，请联系管理员查找原因。";
                    return false;
                }
                return true;
            }
            catch (Exception ex) {
                mag = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 更新代评分表记录
        /// </summary>
        public bool UpdateDaiGradeRecord(out string mag, params string[] parameters) {
            mag = string.Empty;
            string strSQL = @"Update FY_Performance_Employee_DaiGrade Set FExamerID=@FExamerID,FDaiPGradeID=@FDaiPGradeID,FCancellation=0
            Where FEmCode=@FEmCode";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FExamerID",parameters[0]),
                new SqlParameter("@FDaiPGradeID",parameters[1]),
                new SqlParameter("@FEmCode",parameters[2]),
            };
            try {
                object x = mSSQL.GetUpdateByText(strSQL, paras);
                if (x == null || Convert.ToInt32(x) != 1) {
                    mag = "更新代评分表记录失败，请联系管理员查找原因。";
                    return false;
                }
                return true;
            }
            catch (Exception ex) {
                mag = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 更新代评分表记录--状态改为失效
        /// </summary>
        public bool UpdateDaiGradeRecordStatus(out string mag, params string[] parameters) {
            mag = string.Empty;
            string strSQL = @"Update FY_Performance_Employee_DaiGrade Set FCancellation=1 Where FEmCode=@FEmCode";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FEmCode",parameters[0]),
            };
            try {
                object x = mSSQL.GetUpdateByText(strSQL, paras);
                if (x == null || Convert.ToInt32(x) != 1) {
                    mag = "更新代评分表记录失败，请联系管理员查找原因。";
                    return false;
                }
                return true;
            }
            catch (Exception ex) {
                mag = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 新增员工信息
        /// </summary>
        public bool InsertEmployeeDetail(out string mag, Module.Employee emp) {
            mag = string.Empty;
            string strSQL = @"Insert Into FY_Performance_Employee(FCDate,FMDate,FExamerID,FEmCode,FEmName,FPassword,FIsDirector,FDepartment,FPosition,FStatus,FIsDaiGrader) 
            values(@FCDate,@FMDate,@FExamerID,@FEmCode,@FEmName,@FPassword,@FIsDirector,@FDepartment,@FPosition,@FStatus,@FIsDaiGrader)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FCDate",emp.FCDate),
                new SqlParameter("@FMDate",emp.FMDate),
                new SqlParameter("@FExamerID",emp.FExamerID),
                new SqlParameter("@FEmCode",emp.FEmCode),
                new SqlParameter("@FEmName",emp.FEmName),
                new SqlParameter("@FPassword",emp.FPassword),
                new SqlParameter("@FIsDirector",emp.FIsDirector),
                new SqlParameter("@FDepartment",emp.FDepartmentID),
                new SqlParameter("@FPosition",emp.FPositionID),
                new SqlParameter("@FStatus",emp.FStatus),
                new SqlParameter("@FIsDaiGrader",emp.FIsDaiGrader),
            };
            try {
                object x = mSSQL.GetUpdateByText(strSQL, paras);
                if (x == null || Convert.ToInt32(x) != 1) {
                    mag = "新增员工信息失败，请联系管理员查找原因。";
                    return false;
                }
                return true;
            }
            catch (Exception ex) {
                mag = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// 验证员工编号是否已经存在
        /// </summary>
        public object ExistsTheSameEmployeeCode(out string mag,string emcode) {
            mag = string.Empty;
            string strSQL = @"Select top 1 FEmCode From FY_Performance_Employee Where FEmCode=@FEmCode";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FEmCode",emcode),
            };
            try {
                object x = mSSQL.GetValueByText(strSQL, paras);
                return x;
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
    }
}