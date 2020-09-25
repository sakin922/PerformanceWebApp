using System;
using System.Collections.Generic;


namespace PerformanceWebApp.Module {
    /// <summary>
    /// 员工实体类
    /// </summary>
    public class Employee {
        public string FID { get; set; }            //员工内码
        public string FExamerID { get; set; }   //评审人编号
        public string FExamerName { get; set; } //评审人名称
        public DateTime FCDate { get; set; }    //创建日期
        public DateTime FMDate { get; set; }    //修改日期
        public string FEmCode { get; set; }     //员工编号
        public string FEmName { get; set; }     //员工名称
        public string FPassword { get; set; }   //登陆密码
        public bool FIsDirector { get; set; }   //是否主管级
        public string FDepartmentID { get; set; }  //部门ID
        public string FDepartment { get; set; }  //部门
        public string FPositionID { get; set; }   //职位ID
        public string FPosition { get; set; }   //职位
        public decimal FBaseExamine { get; set; }   //原考核基数
        public decimal FCoefficient { get; set; }   //考核系数
        public bool FLockTB { get; set; }       //锁表标识
        public bool FCancellation { get; set; }   //作废
        public string FStatus { get; set; }        //员工状态 //0=正式，1=试用，2=离职，99=管理员
        public string FRank { get; set; }       //职能级别
        public string FDaiPGradeID { get; set; }    //代评分编号
        public string FDaiPGrade { get; set; }      //代评分名称
        public string FNewPwd { get; set; }   //新登陆密码
        public bool FIsDaiGrader { get; set; }   //是否代评分用户
    }
}