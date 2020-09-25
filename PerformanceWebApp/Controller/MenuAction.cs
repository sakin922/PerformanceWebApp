using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;

namespace PerformanceWebApp.Controller {
    [Description("主页面逻辑层")]
    public class MenuAction {
        private MSSQLHelper mSSQL = null;

        /// <summary>
        /// 绩效考核结果表（报表）
        /// </summary>
        public List<Module.ExamineResult> GetExamineResults(string emCode, string year, out string mag) {
            List<Module.ExamineResult> results = null;
            mag = string.Empty;

            if (string.IsNullOrEmpty(emCode)) {
                mag = "无法获取登陆信息！";
                return results;
            }

            string strSQL = @"Select t1.FID,t1.FCDate,t1.FMDate,t2.FEmName,t3.FEmName FExmName,t1.FYear,t1.FMonth,t1.FSelfSum,t1.FRepeSum,t1.FExamineSum
            ,t1.FStatus From FY_Performance_ExamineResult t1 Left Join FY_Performance_Employee t2 on t1.FEmCode=t2.FEmCode
			Left Join FY_Performance_Employee t3 on t2.FExamerID=t3.FEmCode Where FYear = @FYear and t1.FEmCode = @FEmCode and t1.FCancellation = 0 
            and t1.FLockTB = 0 and t1.FStatus <> 0";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FYear",year),
                new SqlParameter("@FEmCode",emCode),
            };
            try {
                mSSQL = new MSSQLHelper();
                DataTable dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count > 0) {
                    results = new List<Module.ExamineResult>();
                    for (int i = 0; i < dt.Rows.Count; i++) {
                        Module.ExamineResult result = new Module.ExamineResult() {
                            FID = (int)dt.Rows[i]["FID"],
                            FEmName = dt.Rows[i]["FEmName"].ToString(),
                            FExmName = dt.Rows[i]["FExmName"].ToString(),
                            FYear = dt.Rows[i]["FYear"].ToString(),
                            FMonth = dt.Rows[i]["FMonth"].ToString(),
                            FSelfSum = Convert.ToInt16(dt.Rows[i]["FSelfSum"]),
                            FRepeSum = Convert.ToInt16(dt.Rows[i]["FRepeSum"]),
                            FExamineSum = Convert.ToDecimal(dt.Rows[i]["FExamineSum"]),
                            FCDate = Convert.ToDateTime(dt.Rows[i]["FCDate"]),
                            FMDate = Convert.ToDateTime(dt.Rows[i]["FMDate"]),
                        };
                        switch ((int)dt.Rows[i]["FStatus"]) {
                            case 1:
                                result.FStatus = "已自评";
                                break;
                            case 2:
                                result.FStatus = "返回自评";
                                break;
                            case 3:
                                result.FStatus = "已复评";
                                break;
                            case 4:
                                result.FStatus = "已审核";
                                break;
                        }
                        results.Add(result);
                    }
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
            }
            return results;
        }
        /// <summary>
        /// 员工信息
        /// </summary>
        public bool GetEmployee(Module.Employee employee, out string mag) {
            mag = string.Empty;
            string strSQL = @"Select em.FEmName,exm.FEmName FExmName,dep.FDpName,pos.FFCName,pos.FRank 
            From FY_Performance_Employee em Left join FY_Performance_Department dep on em.FDepartment=dep.FID 
            Left Join FY_Performance_Position pos on em.FPosition=pos.FID  
            Left Join FY_Performance_Employee exm on em.FExamerID=exm.FEmCode Where em.FEmCode = @FEmCode";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FEmCode",employee.FEmCode),
            };
            try {
                mSSQL = new MSSQLHelper();
                DataTable dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count > 0) {
                    employee.FEmName = dt.Rows[0]["FEmName"].ToString();
                    employee.FExamerID = dt.Rows[0]["FExmName"].ToString();
                    employee.FDepartment = dt.Rows[0]["FDpName"].ToString();
                    employee.FPosition = dt.Rows[0]["FFCName"].ToString();
                    employee.FRank = dt.Rows[0]["FRank"].ToString();
                }
                else {
                    mag = "员工信息数据获取为空，请检查。";
                    return false;
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 绩效考核项目表
        /// </summary>
        public List<Module.ExamineDetail> GetExamineProject(bool FIsDirector, int emStatus, out string mag) {
            mSSQL = new MSSQLHelper();
            mag = string.Empty;
            List<Module.ExamineDetail> exDetails = null;
            string strSQL;
            //*是否代评分账号*
            if (emStatus != 3) {
                //*主管以下级别*
                if (!FIsDirector) {
                    strSQL = @"Select FProjectID,FProject,FEvaluate,FProject_1,FProject_2,FProject_3,FProject_4,FProject_5,0 FSelfGrade,0 FRepeGrade
                From FY_Performance_ExamineProject Where FUddirID<>0";
                }
                //*主管级别*
                else {
                    strSQL = @"Select FProjectID,FProject,FEvaluate,FProject_1,FProject_2,FProject_3,FProject_4,FProject_5,0 FSelfGrade,0 FRepeGrade
                From FY_Performance_ExamineProject Where FDirID<>0";
                }
            }
            else {
                strSQL = @"Select FProjectID,FProject,FEvaluate,FProject_1,FProject_2,FProject_3,FProject_4,FProject_5,0 FSelfGrade,0 FRepeGrade 
                From FY_Performance_ExamineProject Where FIsDaiGrade = 1";
            }
            try {
                exDetails = new List<Module.ExamineDetail>();
                DataTable dt = mSSQL.GetTableByText(strSQL);
                if (dt.Rows.Count > 0) {
                    for (int i = 0; i < dt.Rows.Count; i++) {
                        Module.ExamineDetail detail = new Module.ExamineDetail {
                            FProject_id = (int)dt.Rows[i]["FProjectID"],
                            FProject = dt.Rows[i]["FProject"].ToString(),
                            FEvaluate = dt.Rows[i]["FEvaluate"].ToString(),
                            FProject_1 = dt.Rows[i]["FProject_1"].ToString(),
                            FProject_2 = dt.Rows[i]["FProject_2"].ToString(),
                            FProject_3 = dt.Rows[i]["FProject_3"].ToString(),
                            FProject_4 = dt.Rows[i]["FProject_4"].ToString(),
                            FProject_5 = dt.Rows[i]["FProject_5"].ToString(),
                            FSelfGrade = (int)dt.Rows[i]["FSelfGrade"],
                            FRepeGrade = (int)dt.Rows[i]["FRepeGrade"],
                        };
                        exDetails.Add(detail);
                    }
                }
                else {
                    exDetails = null;
                    mag = "数据查询为空，请联系管理员检查。";
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
                exDetails = null;

            }
            return exDetails;
        }
        /// <summary>
        /// 获取单个考核结果
        /// </summary>
        public Module.ExamineResult GetExamineResult(int fid, out string mag) {
            mag = string.Empty;
            Module.ExamineResult result = null;
            string strSQL = @"Select a.FID,b.FEmName,c.FEmName FExamName,FYear,FMonth,FSelfSum,FRepeSum,FExamineSum,FExamMessage 
            From FY_Performance_ExamineResult a Left Join FY_Performance_Employee b on a.FEmCode=b.FEmCode 
            Left Join FY_Performance_Employee c on b.FExamerID=c.FEmCode 
            Where a.FID = @FID and A.FCancellation = 0";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FID",fid)
            };
            try {
                DataTable dt = new DataTable();
                dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count > 0) {
                    result = new Module.ExamineResult() {
                        FID = Convert.ToInt32(dt.Rows[0]["FID"]),
                        FEmName = dt.Rows[0]["FEmName"].ToString(),
                        FExmName = dt.Rows[0]["FExamName"].ToString(),
                        FYear = dt.Rows[0]["FYear"].ToString(),
                        FMonth = dt.Rows[0]["FMonth"].ToString(),
                        FSelfSum = Convert.ToInt16(dt.Rows[0]["FSelfSum"]),
                        FRepeSum = Convert.ToInt16(dt.Rows[0]["FRepeSum"]),
                        FExamineSum = Convert.ToDecimal(dt.Rows[0]["FExamineSum"]),
                        FExamMessage = dt.Rows[0]["FExamMessage"].ToString(),
                    };
                }
                else {
                    mag = "查询数据异常。";
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
            }
            return result;
        }
        /// <summary>
        /// 获取绩效考核个人评分明细表
        /// </summary>
        public List<Module.ExamineDetail> GetExamineDetails(int fid, out string mag) {
            mag = string.Empty;
            List<Module.ExamineDetail> details = null;
            string strSQL = @"Select FEmResultID,FProject_id,FSelfGrade,FRepeGrade From FY_Performance_Examine 
            Where FEmResultID = @FID and FCancellation = 0";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FID",fid)
            };
            try {
                mSSQL = new MSSQLHelper();
                DataTable dt = new DataTable();
                dt = mSSQL.GetTableByText(strSQL, paras);
                if (dt.Rows.Count > 0) {
                    details = new List<Module.ExamineDetail>();
                    for (int i = 0; i < dt.Rows.Count; i++) {
                        details.Add(
                            new Module.ExamineDetail() {
                                FEmResultID = Convert.ToInt32(dt.Rows[i]["FEmResultID"]),
                                FProject_id = Convert.ToInt32(dt.Rows[i]["FProject_id"]),
                                FSelfGrade = Convert.ToInt16(dt.Rows[i]["FSelfGrade"]),
                                FRepeGrade = Convert.ToInt16(dt.Rows[i]["FRepeGrade"]),
                            }
                        );
                    }
                }
                else {
                    mag = "查询数据异常。";
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
            }
            return details;
        }
        /// <summary>
        /// 新增绩效考核结果，返回FID值
        /// </summary>
        public object InsertExamResult(Module.ExamineResult exResult, out string mag) {
            mag = string.Empty;
            string strSQL = @"Insert Into FY_Performance_ExamineResult(FCDate,FMDate,FEmCode,FYear,FMonth,FSelfSum,FRepeSum,FExamineSum,FStatus) 
            Output inserted.FID 
            Values(@FCDate,@FMDate,@FEmCode,@FYear,@FMonth,@FSelfSum,@FRepeSum,@FExamineSum,1)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FCDate",exResult.FCDate),
                new SqlParameter("@FMDate",exResult.FMDate),
                new SqlParameter("@FEmCode",exResult.FEmCode),
                new SqlParameter("@FYear",exResult.FYear),
                new SqlParameter("@FMonth",exResult.FMonth),
                new SqlParameter("@FSelfSum",exResult.FSelfSum),
                new SqlParameter("@FRepeSum",exResult.FRepeSum),
                new SqlParameter("@FExamineSum",exResult.FExamineSum),
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
        /// 新增绩效考核明细
        /// </summary>
        public object InsertExamDetails(Module.ExamineDetail detail, out string mag) {
            mag = string.Empty;
            string strSQL = @"Insert Into FY_Performance_Examine(FEmResultID,FProject_id,FSelfGrade) 
            Values(@FID,@FProject_id,@FSelfGrade)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FID",detail.FEmResultID),
                new SqlParameter("@FProject_id",detail.FProject_id),
                new SqlParameter("@FSelfGrade",detail.FSelfGrade),
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
        /// 更新绩效考核结果表
        /// </summary>
        public object UpdateExamResult(Module.ExamineResult exResult, out string mag) {
            mag = string.Empty;
            string strSQL = @"Update FY_Performance_ExamineResult Set FMDate=@FMDate,FSelfSum=@FSelfSum,FRepeSum=@FRepeSum,FExamineSum=@FExamineSum
            ,FExamMessage=@FExamMessage,FStatus=@FStatus,FCoefficient=@FCoefficient Where FID = @FID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FMDate",exResult.FMDate),
                new SqlParameter("@FSelfSum",exResult.FSelfSum),
                new SqlParameter("@FRepeSum",exResult.FRepeSum),
                new SqlParameter("@FExamineSum",exResult.FExamineSum),
                new SqlParameter("@FExamMessage",exResult.FExamMessage),
                new SqlParameter("@FStatus",exResult.FStatus),
                new SqlParameter("@FCoefficient",exResult.FCoefficient),
                new SqlParameter("@FID",exResult.FID),
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
        /// 更新绩效考核明细表
        /// </summary>
        public object UpdateExamDetails(Module.ExamineDetail detail, out string mag) {
            mag = string.Empty;
            string strSQL = @"Update FY_Performance_Examine Set FSelfGrade=@FSelfGrade,FRepeGrade=@FRepeGrade Where FEmResultID=@FID and FProject_id=@FProject_id";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FSelfGrade",detail.FSelfGrade),
                new SqlParameter("@FRepeGrade",detail.FRepeGrade),
                new SqlParameter("@FID",detail.FEmResultID),
                new SqlParameter("@FProject_id",detail.FProject_id),
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
        /// 我的待评分明细表
        /// </summary>
        public DataTable GetGradeExamResults(string emCode, string year, string month, string text,string status, out string mag) {
            mag = string.Empty;
            string strSQL = @"Select FID,FEmCode,FEmName,FDpName,FSelfSum,FRepeSum,FExamineSum,FStatus_1,FEmpStatus,FIsDirector,fBillStatus From 
            (Select isnull(b.FID,0)FID,a.FEmCode,a.FEmName,c.FDpName,isnull(b.FSelfSum,0)FSelfSum,isnull(b.FRepeSum,0)FRepeSum,isnull(b.FExamineSum,0)FExamineSum
            ,case when b.FEmCode is null then '未自评' when b.FStatus=1 then '已自评' when b.FStatus=2 then '修改自评分' when b.FStatus=3 then '已复评' 
            when b.FStatus=4 then '已审核' else '' end as FStatus_1,isnull(b.FStatus,0) fBillStatus,a.FIsDirector,a.FStatus FEmpStatus 
            From FY_Performance_Employee a Left Join (Select FID,FEmCode,FSelfSum,FRepeSum,FExamineSum,FStatus From FY_Performance_ExamineResult 
            Where FYear=@FYear and FMonth=@FMonth) b on a.FEmCode=b.FEmCode Left Join FY_Performance_Department c on a.FDepartment=c.FID 
            Where (a.FCancellation=0 and a.FStatus not in (2,4)) and a.FExamerID = @emCode and (a.FEmCode like '%'+@text+'%' or a.FEmName like '%'+@text+'%' 
            or c.FDpName like '%'+@text+'%'))a Where a.FStatus_1 like '%'+@status+'%' Order By fBillStatus asc";          
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@emCode",emCode),
                new SqlParameter("@FYear",year),
                new SqlParameter("@FMonth",month),
                new SqlParameter("@text",text),
                new SqlParameter("@status",status),
            };
            DataTable dt = new DataTable();
            try {
                mSSQL = new MSSQLHelper();
                dt = mSSQL.GetTableByText(strSQL, paras);
                return dt;
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 验证是否存在当月的绩效记录
        /// </summary>
        public object VerifyExamRecord(string emCode, string year, string month, out string mag) {
            mag = string.Empty;
            string strSQL = @"Select 1 From FY_Performance_ExamineResult Where FEmCode = @FEmCode and FCancellation = 0 and (FYear=@year and FMonth=@month)";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FEmCode",emCode),
                new SqlParameter("@year",year),
                new SqlParameter("@month",month),
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
        /// 返回自评状态（已复评）
        /// </summary>
        public object UpdateExamResultStatus(string fid,out string mag) {
            mag = string.Empty;
            string strSQL = @"Update FY_Performance_ExamineResult Set FStatus = 2 Where FStatus = 3 and FID = @FID";
            SqlParameter[] paras = new SqlParameter[] {
                new SqlParameter("@FID",fid),
            };
            object result = null;
            try {
                mSSQL = new MSSQLHelper();
                result= mSSQL.GetUpdateByText(strSQL, paras);
                if (Convert.ToInt32(result) == 0) {
                    mag = "数据修改失败，请联系管理员。";
                    result = null;
                }
            }
            catch (Exception ex) {
                mag = ex.Message;
            }
            return result;
        }
    }
}