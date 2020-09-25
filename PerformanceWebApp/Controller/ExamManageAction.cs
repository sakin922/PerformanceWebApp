using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace PerformanceWebApp.Controller {
    [Description("绩效管理逻辑层")]
    public class ExamManageAction {
        private MSSQLHelper mSSQL = new MSSQLHelper();

        /// <summary>
        /// 获取员工原考核基数信息表
        /// </summary>
        public List<Module.Employee> GetBaseExamineInfo(string textQuery, out string mag) {
            mag = string.Empty;
            string strSQL = $@"Select emp.FID,emp.FEmCode,emp.FEmName,ISNULL(per.FBaseExamine,0) FBaseExamine,ISNULL(per.FCoefficient,1) FCoefficient,dep.FDpName
            ,pos.FFCName FPosName,pos.FRank From FY_Performance_Employee emp Left Join FY_Performance_Department dep on emp.FDepartment=dep.FID 
            Left Join FY_Performance_Position pos on emp.FPosition=pos.FID Left Join FY_Performance_EmployeePerInfo per on emp.FEmCode = per.FEmCode 
            Where emp.FCancellation = 0 and emp.FLockTB = 0 and (emp.FEmCode like '%'+@text+'%' or emp.FEmName like '%'+@text+'%' or dep.FDpName like '%'+@text+'%')";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@text",textQuery),
            };
            List<Module.Employee> employees = new List<Module.Employee>();
            try {
                mSSQL = new MSSQLHelper();
                DataTable dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count > 0) {
                    for (int i = 0; i < dt.Rows.Count; i++) {
                        Module.Employee item = new Module.Employee {
                            FID = dt.Rows[i]["FID"].ToString(),
                            FEmCode = dt.Rows[i]["FEmCode"].ToString(),
                            FEmName = dt.Rows[i]["FEmName"].ToString(),
                            FBaseExamine = Convert.ToDecimal(dt.Rows[i]["FBaseExamine"]),
                            FCoefficient = Convert.ToDecimal(dt.Rows[i]["FCoefficient"]),
                            FDepartment = dt.Rows[i]["FDpName"].ToString(),
                            FPosition = dt.Rows[i]["FPosName"].ToString(),
                            FRank = dt.Rows[i]["FRank"].ToString(),
                        };
                        employees.Add(item);
                    }
                    return employees;
                }
                else {
                    return null;
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 更新原绩效考核基数
        /// </summary>
        public object UpdateBaseExamineValue(string emCode, decimal baseExam, out string mag) {
            mag = string.Empty;
            string strSQL = @"Update FY_Performance_EmployeePerInfo Set FBaseExamine = @FBaseExamine Where FEmCode = @FEmCode";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FBaseExamine",baseExam),
                new SqlParameter("@FEmCode",emCode),
            };
            try {
                mSSQL = new MSSQLHelper();
                return mSSQL.GetUpdateByText(strSQL, paras);
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 查询该员工是否存在
        /// </summary>
        public object CheckNullEmployee(string emcode, out string mag) {
            mag = string.Empty;
            string strSQL = @"select a.FEmCode from FY_Performance_Employee a left join FY_Performance_EmployeePerInfo b on a.FEmCode=b.FEmCode 
            where b.FEmCode is null and a.FEmCode = @FEmCode";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FEmCode",emcode),
            };
            try {
                mSSQL = new MSSQLHelper();
                return mSSQL.GetValueByText(strSQL, paras);
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 新增原绩效考核基数
        /// </summary>
        public object InsertBaseExamineValue(string emCode, decimal baseExam, out string mag) {
            mag = string.Empty;
            string strSQL = @"Insert Into FY_Performance_EmployeePerInfo(FEmCode,FBaseExamine) Values(@FEmCode,@FBaseExamine)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FBaseExamine",baseExam),
                new SqlParameter("@FEmCode",emCode),
            };
            try {
                mSSQL = new MSSQLHelper();
                return mSSQL.GetUpdateByText(strSQL, paras);
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 获取完成复评的考核信息
        /// </summary>
        public DataTable GetExamInformation(string year, string month, string text, string dep, string reDays, out string mag) {
            mag = string.Empty;
            string strSQL = @"Select per.FID,emp.FEmCode,emp.FEmName,dep.FDpName,per.FBaseExamine,ISNULL(rut.FCoefficient,0)FCoefficient
            ,(per.FBaseExamine * rut.FCoefficient) FBaseResult, @reDays FReAttend,0 FFloatAmount,rut.FExamMessage 
            From FY_Performance_EmployeePerInfo per Left Join FY_Performance_Employee emp on per.FEmCode=emp.FEmCode and emp.FCancellation = 0 
            Left Join FY_Performance_Department dep on emp.FDepartment=dep.FID 
            Left Join FY_Performance_ExamineResult rut on per.FEmCode=rut.FEmCode and rut.FCancellation = 0 Where rut.FStatus = 3 
			and rut.FYear = @FYear and rut.FMonth = @FMonth 
            and dep.FDpName like '%'+@dep+'%' and (emp.FEmCode like '%'+@text+'%' or emp.FEmName like '%'+@text+'%') 
            and emp.FEmCode not in (Select distinct FEmCode From FY_Performance_ExamineInformation Where FYear=@FYear and FMonth=@FMonth and FStatus=1) 
            Union All Select 0 FID,a.FEmCode,a.FEmName,b.FDpName,ISNULL(c.FBaseExamine,0)FBaseExamine,ISNULL(d.FCoefficient,0)FCoefficient
            ,(ISNULL(c.FBaseExamine,0) * ISNULL(d.FCoefficient,0)) FBaseResult, @reDays FReAttend,0 FFloatAmount
            ,case when d.FStatus is null then '未自评' when d.FStatus =1 then '未复评' when d.FStatus=2 then '未修改自评' else '' end as FExamMessage 
            From FY_Performance_Employee a Left Join FY_Performance_Department b on a.FDepartment=b.FID Left Join FY_Performance_EmployeePerInfo c on a.FEmCode=c.FEmCode 
            Left Join FY_Performance_ExamineResult d on a.FEmCode=d.FEmCode and (d.FYear=@FYear and d.FMonth=@FMonth) 
            Where a.FStatus not in (2,4) and (d.FStatus < 3 or d.FStatus is null) 
            and a.FEmCode not in (Select distinct FEmCode From FY_Performance_ExamineInformation Where FYear=@FYear and FMonth=@FMonth) 
            and b.FDpName like '%'+@dep+'%' and (a.FEmCode like '%'+@text+'%' or a.FEmName like '%'+@text+'%') ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FYear",year),
                new SqlParameter("@FMonth",month),
                new SqlParameter("@text",text),
                new SqlParameter("@dep",dep),
                new SqlParameter("@reDays",reDays),
            };
            DataTable dt = new DataTable();
            try {
                dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count < 1) {
                    dt = null;
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
            }
            return dt;
        }
        /// <summary>
        /// 统计考核人数
        /// </summary>
        public DataTable GetPeopleCount(string year, string month, out string mag) {
            mag = string.Empty;
            string strSQL = @"Select (select count(distinct FEmCode) from FY_Performance_Employee where FCancellation = 0 and FStatus<>2 and FStatus<>4) FTotal
            ,count(distinct FEmCode) FFinished,(select count(distinct FEmCode) from FY_Performance_Employee where FEmCode not in (select distinct FEmCode 
            from FY_Performance_ExamineResult where FStatus = 4 and FYear=@FYear and FMonth=@FMonth) and FStatus<>2 and FStatus<>4) FUnfinished 
            From FY_Performance_ExamineInformation Where FYear=@FYear and FMonth=@FMonth and FStatus = 1";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FYear",year),
                new SqlParameter("@FMonth",month),
            };
            DataTable dt = null;
            try {
                dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count < 1) {
                    dt = null;
                    mag = "【统计考核人数】查询异常，请联系管理员检查。";
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
            }
            return dt;
        }
        /// <summary>
        /// 绩效考核报表--新增
        /// </summary>
        public object InsertExamInformation(Module.ExamManager exam, out string mag) {
            mag = string.Empty;
            string strSQL = @"Insert Into FY_Performance_ExamineInformation(FCDate,FEmCode,FYear,FMonth,FBaseExamine,FCoefficient,FBaseResult,FAcAttend
            ,FAbsented,FAmount,FFloatAmount,FRemark,FStatus) Values(@FCDate,@FEmCode,@FYear,@FMonth,@FBaseExamine,@FCoefficient
            ,@FBaseResult,@FAcAttend,@FAbsented,@FAmount,@FFloatAmount,@FRemark,@FStatus)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FCDate",exam.FCDate),
                new SqlParameter("@FEmCode",exam.FEmCode),
                new SqlParameter("@FYear",exam.FYear),
                new SqlParameter("@FMonth",exam.FMonth),
                new SqlParameter("@FBaseExamine",exam.FBaseExamine),
                new SqlParameter("@FCoefficient",exam.FCoefficient),
                new SqlParameter("@FBaseResult",exam.FBaseResult),
                new SqlParameter("@FAcAttend",exam.FAcAttend),
                new SqlParameter("@FAbsented",exam.FAbsented),
                new SqlParameter("@FAmount",exam.FAmount),
                new SqlParameter("@FFloatAmount",exam.FFloatAmount),
                new SqlParameter("@FRemark",exam.FRemark),
                new SqlParameter("@FStatus",exam.FStatus),
            };
            object result = null;
            try {
                result = mSSQL.GetUpdateByText(strSQL, paras);
                if (Convert.ToInt32(result) != 1) {
                    result = null;
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 绩效考核报表--修改
        /// </summary>
        public object UpdateExamInformation(Module.ExamManager exam, out string mag) {
            mag = string.Empty;
            string strSQL = @"Update FY_Performance_ExamineInformation Set FBaseExamine=@FBaseExamine,FCoefficient=@FCoefficient,FBaseResult=@FBaseResult
            ,FAcAttend=@FAcAttend,FAbsented=@FAbsented,FAmount=@FAmount,FFloatAmount=@FFloatAmount,FRemark=@FRemark Where FID=@FID ";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FBaseExamine",exam.FBaseExamine),
                new SqlParameter("@FCoefficient",exam.FCoefficient),
                new SqlParameter("@FBaseResult",exam.FBaseResult),
                new SqlParameter("@FAcAttend",exam.FAcAttend),
                new SqlParameter("@FAbsented",exam.FAbsented),
                new SqlParameter("@FAmount",exam.FAmount),
                new SqlParameter("@FFloatAmount",exam.FFloatAmount),
                new SqlParameter("@FRemark",exam.FRemark),
                new SqlParameter("@FID",exam.FID),
            };
            object result = null;
            try {
                result = mSSQL.GetUpdateByText(strSQL, paras);
                if (Convert.ToInt32(result) != 1) {
                    result = null;
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 绩效考核报表--已完成
        /// </summary>
        public DataTable GetExamInfoFinished(string year, string month, string text, string dep, out string mag) {
            mag = string.Empty;
            string strSQL = @"Select inf.FID,inf.FEmCode,emp.FEmName,dep.FDpName,inf.FBaseExamine,inf.FCoefficient,inf.FBaseResult
            ,inf.FAcAttend,inf.FAbsented,inf.FAmount,inf.FFloatAmount,inf.FRemark,inf.FStatus 
            From FY_Performance_ExamineInformation inf Left Join FY_Performance_Employee emp on inf.FEmCode=emp.FEmCode and emp.FCancellation = 0 
            Left Join FY_Performance_Department dep on emp.FDepartment=dep.FID Where inf.FStatus > 0 and inf.FYear=@FYear and inf.FMonth=@FMonth 
            and dep.FDpName like '%'+@dep+'%' and (emp.FEmCode like '%'+@text+'%' or emp.FEmName like '%'+@text+'%')";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FYear",year),
                new SqlParameter("@FMonth",month),
                new SqlParameter("@text",text),
                new SqlParameter("@dep",dep),
            };
            DataTable dt = new DataTable();
            try {
                dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count < 1) {
                    dt = null;
                    mag = "数据查询失败，请联系管理员。";
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
            }
            return dt;
        }
        /// <summary>
        /// 更新【考核成绩结果】的单据状态为已审核
        /// </summary>
        public object UpdateExamResultApprove(string emCode, string year, string month, out string mag) {
            mag = string.Empty;
            string strSQL = @"Update FY_Performance_ExamineResult Set FStatus = 4 Where FEmCode = @FEmCode and FYear=@FYear and FMonth=@FMonth and FStatus=3";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FEmCode",emCode),
                new SqlParameter("@FYear",year),
                new SqlParameter("@FMonth",month),
            };
            object result = null;
            try {
                result = mSSQL.GetUpdateByText(strSQL, paras);
                if (Convert.ToInt32(result) != 1) {
                    result = null;
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 获取【部门进度统计】的结果集
        /// </summary>
        public DataSet GetDepartmentSchedule(out string mag, params object[] args) {
            mag = string.Empty;
            string strSQL = @"Usp_Performance_GetDepartment_Schedule";
            try {
                int year = Convert.ToInt32(args[0]);
                int month = Convert.ToInt32(args[1]);
                string department = args[2].ToString();
                SqlParameter[] paras = new SqlParameter[] {
                    new SqlParameter("@Year",year),
                    new SqlParameter("@Month",month),
                    new SqlParameter("@Depart",department),
                };
                return mSSQL.GetDataSetByProc(strSQL, paras);
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
    }
}