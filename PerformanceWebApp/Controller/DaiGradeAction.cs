using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace PerformanceWebApp.Controller {
    [Description("代评分逻辑层")]
    public class DaiGradeAction {
        private MSSQLHelper mSSQL = new MSSQLHelper();
        /// <summary>
        /// 获取代评分的记录信息
        /// </summary>
        public DataTable GetDaiGradeRecord(string daiGradeID, string year, string month, string text,out string mag) {
            mag = string.Empty;
            string strSQL = @"Select c.FID,a.FEmCode,b.FEmName,e.FDpName
            ,d.FFCName FPosition,c.FSelfSum,case when c.FStatus=1 then '已自评' when c.FStatus=2 then '修改自评' 
			when c.FStatus=3 then '已复评' when c.FStatus=4 then '已审核' else '' end as FStatus
			From FY_Performance_Employee_DaiGrade a 
            left join FY_Performance_Employee b on a.FEmCode=b.FEmCode left join FY_Performance_ExamineResult c on a.FEmCode=c.FEmCode 
            left join FY_Performance_Position d on b.FPosition=d.FID left join FY_Performance_Department e on b.FDepartment=e.FID 
            Where a.FDaiPGradeID = @daiGradeID and c.FYear = @FYear and c.FMonth = @Month and b.FStatus=3 and a.FCancellation=0 
            and (b.FEmCode like '%'+@text+'%' or b.FEmName like '%'+@text+'%' or d.FFCName like '%'+@text+'%' or e.FDpName like '%'+@text+'%')";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@daiGradeID",daiGradeID),
                new SqlParameter("@FYear",year),
                new SqlParameter("@Month",month),
                new SqlParameter("@text",text),
            };
            DataTable dt = null;
            try {
                dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count <= 0) {
                    dt = null;
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
            }
            return dt;
        }
        /// <summary>
        /// 新增的代评分信息
        /// </summary>
        public DataTable GetNewDaiGradeData(string daiGradeID, string year, string month, string text,out string mag) {
            mag = string.Empty;
            string strSQL = @"Select null FID,a.FEmCode,b.FEmName,e.FDpName
            ,c.FFCName FPosition,0 FSelfSum,0 FStatus From FY_Performance_Employee_DaiGrade a 
            left join FY_Performance_Employee b on a.FEmCode=b.FEmCode left join FY_Performance_Position c on b.FPosition=c.FID 
            left join FY_Performance_Department e on b.FDepartment=e.FID Where a.FDaiPGradeID = @daiGradeID and b.FStatus = 3 and a.FCancellation=0
			and (b.FEmCode like '%'+@text+'%' or b.FEmName like '%'+@text+'%' or c.FFCName like '%'+@text+'%' or e.FDpName like '%'+@text+'%')";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@daiGradeID",daiGradeID),
                new SqlParameter("@year",year),
                new SqlParameter("@month",month),
                new SqlParameter("@text",text),
            };
            DataTable dt = new DataTable();
            try {
                dt = mSSQL.GetTableByText(strSQL, paras);
            }
            catch (Exception ex) {
                mag = ex.Message;
                dt = null;
            }
            return dt;
        }
        /// <summary>
        /// 新增代评分绩效表
        /// </summary>
        public object InsertNewDaiGradeData(Module.ExamineResult result, out string mag) {
            mag = string.Empty;
            string strSQL = @"Insert Into FY_Performance_ExamineResult(FCDate,FMDate,FEmCode,FYear,FMonth,FSelfSum,FStatus) output inserted.FID 
            Values(@FCDate,@FMDate,@FEmCode,@FYear,@FMonth,@FSelfSum,@FStatus)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FCDate",result.FCDate),
                new SqlParameter("@FMDate",result.FMDate),
                new SqlParameter("@FEmCode",result.FEmCode),
                new SqlParameter("@FYear",result.FYear),
                new SqlParameter("@FMonth",result.FMonth),
                new SqlParameter("@FSelfSum",result.FSelfSum),
                new SqlParameter("@FStatus",result.FStatus),
            };
            try {
                return mSSQL.GetValueByText(strSQL, paras);
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 更新代评分绩效表
        /// </summary>
        public object UpdateDaiGradeRecord(Module.ExamineResult result, out string mag) {
            mag = string.Empty;
            string strSQL = @"Update FY_Performance_ExamineResult Set FMDate=@FMDate,FSelfSum=@FSelfSum,FExamineSum=@FExamineSum,FStatus=@FStatus Where FID = @FID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FMDate",result.FMDate),
                new SqlParameter("@FSelfSum",result.FSelfSum),
                new SqlParameter("@FExamineSum",result.FExamineSum),
                new SqlParameter("@FStatus",result.FStatus),
                new SqlParameter("@FID",result.FID),
            };
            try {
                return mSSQL.GetUpdateByText(strSQL, paras);
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 新增代评分明细表
        /// </summary>
        public object InsertNewDaiGradeDetail(Module.ExamineResult result, out string mag) {
            mag = string.Empty;
            string strSQL = @"Insert Into FY_Performance_Examine(FEmResultID,FProject_id,FSelfGrade) 
            Values(@FEmResultID,0,@FSelfGrade)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FEmResultID",result.FID),
                new SqlParameter("@FSelfGrade",result.FSelfSum),
            };
            try {
                return mSSQL.GetUpdateByText(strSQL, paras);
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 更新代评分明细表
        /// </summary>
        public object UpdateDaiGradeDetail(Module.ExamineResult result, out string mag) {
            mag = string.Empty;
            string strSQL = @"Update FY_Performance_Examine Set FSelfGrade = @FSelfGrade,FRepeGrade = @FRepeGrade Where FEmResultID = @FID and FProject_id = 0";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FSelfGrade",result.FSelfSum),
                new SqlParameter("@FRepeGrade",result.FRepeSum),
                new SqlParameter("@FID",result.FID),
            };
            try {
                return mSSQL.GetUpdateByText(strSQL, paras);
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        public Module.ExamineResult GetSelfGradeDetails(string fid, out string mag) {
            mag = string.Empty;
            string strSQL = @"select FID,FMDate,FStatus,FSelfSum,FRepeSum,FExamineSum from FY_Performance_ExamineResult where FID=@FID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FID",fid)
            };
            Module.ExamineResult examine = null;
            try {
                DataTable dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt != null && dt.Rows.Count > 0) {
                    for (int i = 0; i < dt.Rows.Count; i++) {
                         examine = new Module.ExamineResult {
                            FID = Convert.ToInt32(dt.Rows[i]["FID"]),
                            FMDate = DateTime.Now.Date,
                            FStatus = dt.Rows[i]["FStatus"].ToString(),
                            FSelfSum = Convert.ToInt32(dt.Rows[i]["FSelfSum"]),
                            FRepeSum = Convert.ToInt32(dt.Rows[i]["FRepeSum"]),
                            FExamineSum = Convert.ToDecimal(dt.Rows[i]["FExamineSum"]),
                        };
                    }
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
            }
            return examine;
        }
    }
}