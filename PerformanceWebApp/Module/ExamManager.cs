using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;


namespace PerformanceWebApp.Module {
    [Description("绩效管理实体类")]
    public class ExamManager {
        public string FID { get; set; }            //FID
        public DateTime FCDate { get; set; }    //日期
        public string FEmCode { get; set; }     //员工编号
        public string FDepartment { get; set; }    //部门ID
        public string FYear { get; set; }          //期间年
        public string FMonth { get; set; }         //期间月
        public double FBaseExamine { get; set; }   //原考核基数
        public double FCoefficient { get; set; }   //考核系数
        public double FBaseResult { get; set; }   //基数结果
        public double FReAttend { get; set; }   //应出勤（天）
        public double FAcAttend { get; set; }   //实际出勤（天）
        public double FAbsented { get; set; }   //缺勤扣罚（元）
        public double FAmount { get; set; }   //奖惩金额（元）
        public decimal FFloatAmount { get; set; }   //绩效浮动奖金（元）
        public string FRemark { get; set; }     //备注
        public bool FLockTB { get; set; }       //锁表标识
        public bool FCancellation { get; set; }   //作废
        public bool FStatus { get; set; }        //单据状态：0=未完成，1=已完成
    }
}