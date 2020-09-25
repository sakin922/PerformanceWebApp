using System;
using System.ComponentModel;

namespace PerformanceWebApp.Module {
    [Description("考核评分结果表")]
    public class ExamineResult {
        public int FID { get; set; }                //FID
        public DateTime? FCDate { get; set; }        //创建日期
        public DateTime? FMDate { get; set; }        //修改日期
        public string FEmCode { get; set; }         //被考核人ID
        public string FEmName { get; set; }         //被考核人
        public string FExmName { get; set; }       //评分人
        public string FYear { get; set; }           //年份
        public string FMonth { get; set; }          //月份
        public bool FLockTB { get; set; }           //锁表标识
        public bool FCancellation { get; set; }      //作废
        public int FSelfSum { get; set; }        //自评分数总分
        public int FRepeSum { get; set; }        //复评分数总分
        public decimal FExamineSum { get; set; }     //考核总分
        public decimal FCoefficient { get; set; }     //考核系数
        public string FExamMessage { get; set; }     //绩效评语
        public string FStatus { get; set; }            //单据状态 //0=无效，1=自评，2=复评
        public string FEmpStatus { get; set; }       //用户状态
    }

    [Description("考核评分明细表")]
    public class ExamineDetail {
        public int FEmResultID { get; set; }        //结果表ID
        public int FProject_id { get; set; }        //项目ID
        public string FProject { get; set; }        //项目名称
        public string FEvaluate { get; set; }       //评价因素
        public string FProject_1 { get; set; }      //项目1
        public string FProject_2 { get; set; }      //项目2
        public string FProject_3 { get; set; }      //项目3
        public string FProject_4 { get; set; }      //项目4
        public string FProject_5 { get; set; }      //项目5
        public int FSelfGrade { get; set; }     //自评分数
        public int FRepeGrade { get; set; }     //复评分数
        public bool FLockTB { get; set; }           //锁表标识
        public bool FCancellation { get; set; }      //作废
    }
}