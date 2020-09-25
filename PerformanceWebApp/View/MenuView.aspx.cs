using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PerformanceWebApp.View {
    public partial class MenuView : System.Web.UI.Page {
        private Module.Employee employee = null;
        private Controller.MenuAction menuAction = new Controller.MenuAction();
        private List<Module.ExamineResult> exResults = null;

        protected void Page_Load(object sender, EventArgs e) {
            if (Session["Login_emoloyee"] == null) {
                this.Page.Dispose();
                Response.Redirect("Login");
            }
            Session["ExYear"] = ddlExYear.SelectedValue;
            Session["ExMonth"] = ddlExMonth.SelectedValue;

            this.employee = Session["Login_emoloyee"] as Module.Employee;
            bool isDalGrade = Convert.ToBoolean(Session["Login_isDai"]);
            if (!IsPostBack) {
                Session["ExYear"] = null;
                Session["ExMonth"] = null;
                //*当前考核周期*
                ddlExYear.Items.Clear();
                int _exYear = (DateTime.Now.Year - 2);
                for (int i = 0; i < 2; i++) {
                    _exYear += 1;
                    ListItem item = new ListItem(_exYear.ToString(), _exYear.ToString());
                    ddlExYear.Items.Add(item);
                }
                ddlExYear.SelectedValue = DateTime.Now.Year.ToString();
                //*获取当前【月份】-- 本月25日开始，方可跳到当月，25日之前跳回上一个月*
                ddlExMonth.SelectedValue = this.GetCurrentMonth().ToString();
                //ddlExMonth.SelectedValue = DateTime.Now.Month.ToString();
                Session["ExYear"] = ddlExYear.SelectedValue;
                Session["ExMonth"] = ddlExMonth.SelectedValue;

                //*期间年控件*
                ddlYear.Items.Clear();
                int _year = (DateTime.Now.Year);
                ListItem _yearItem = new ListItem(_year.ToString(), _year.ToString());
                ddlYear.Items.Add(_yearItem);
                for (int i = 0; i < 2; i++) {
                    _year -= 1;
                    _yearItem = new ListItem(_year.ToString(), _year.ToString());
                    ddlYear.Items.Add(_yearItem);
                }
                if (ddlYear.Items.Count > 0) {
                    if (Session["MenuView_PerYear"] == null) {
                        ddlYear.SelectedValue = DateTime.Now.Year.ToString();
                    }
                    else {
                        ddlYear.SelectedValue = Session["MenuView_PerYear"].ToString();
                    }
                }

                ClearControl();
                //*验证是否【主管级】*
                if (employee.FIsDirector) {
                    lbtnUnDire.Visible = false;
                }
                else {
                    lbtnDire.Visible = false;
                    lbtnMyGrade.Visible = false;
                }
                //*检验是否【管理员】*
                if (employee.FStatus == "98") {
                    labAccount.Text = "管理员";
                    lbtnEmpManager.Visible = true;
                }
                else if (employee.FStatus == "99") {
                    labAccount.Text = "HR管理员";
                    lbtnBaseExam.Visible = true;
                    lbtnExamManager.Visible = true;
                    lbtnExamReport.Visible = true;
                    lbtnEmpManager.Visible = true;
                }
                else {
                    labAccount.Text = "普通用户";
                }
                //*验证是否【代评分用户】*
                if (isDalGrade) {
                    lbtnDaiGrade.Visible = true;
                }

                string mag = string.Empty;
                if (!menuAction.GetEmployee(employee, out mag)) {
                    labMessage.Text = mag;
                    return;
                }
                GetDataBind(out mag);
                if (this.exResults == null || !string.IsNullOrEmpty(mag)) {
                    labMessage.Text = mag;
                    return;
                }
                GridView1.DataSource = null;
                GridView1.DataSource = this.exResults;
                GridView1.DataBind();
                Session["MenuView_exResults"] = this.exResults;
            }
            Session["MenuView_PerYear"] = null;
        }
        /// <summary>
        /// 清空控件
        /// </summary>
        private void ClearControl() {
            labEmName.Text = labExamer.Text = labPost.Text = labDepartment.Text = string.Empty;
        }
        /// <summary>
        /// 员工信息
        /// </summary>
        private void GetDataBind(out string mag) {
            mag = string.Empty;
            labDepartment.Text = employee.FDepartment;
            labPost.Text = employee.FPosition;
            labPolevel.Text = employee.FRank;
            labEmName.Text = employee.FEmName;
            labExamer.Text = employee.FExamerID;
            this.exResults = menuAction.GetExamineResults(employee.FEmCode, ddlYear.SelectedValue, out mag);
        }
        /// <summary>
        /// 主管级以下绩效考核表
        /// </summary>
        protected void lbtnUnDire_Click(object sender, EventArgs e) {
            object verify = menuAction.VerifyExamRecord(employee.FEmCode, Session["ExYear"].ToString(), Session["ExMonth"].ToString(), out string mag);
            if (verify != null) {
                if (string.IsNullOrEmpty(mag)) {
                    mag = $"已存在【{Session["ExMonth"]}月】的考核绩效表，不允许重复创建。";
                }
                Response.Write($"<script> alert('{mag}');location='MenuView.aspx' </script>");
                return;
            }
            Session["ExResults_status"] = 0;
            Response.Redirect("UnExecutiveExamine.aspx");
        }
        /// <summary>
        /// 主管级以上绩效考核表
        /// </summary>
        protected void lbtnDire_Click(object sender, EventArgs e) {
            object verify = menuAction.VerifyExamRecord(employee.FEmCode, Session["ExYear"].ToString(), Session["ExMonth"].ToString(), out string mag);
            if (verify != null) {
                if (string.IsNullOrEmpty(mag)) {
                    mag = $"已存在【{Session["ExMonth"]}月】的考核绩效表，不允许重复创建。";
                }
                Response.Write($"<script> alert('{mag}');location='MenuView.aspx' </script>");
                return;
            }
            Session["ExResults_status"] = 0;
            Response.Redirect("UnExecutiveExamine.aspx");
        }
        /// <summary>
        /// 我的待复核评分
        /// </summary>
        protected void lbtnMyGrade_Click(object sender, EventArgs e) {
            Response.Redirect("MyReview.aspx");
        }
        /// <summary>
        /// GridView行【查看】事件
        /// </summary>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
            //*获取按钮控件的当前rowindex *
            GridViewRow gvr = (GridViewRow)((LinkButton)e.CommandSource).Parent.Parent;
            if (e.CommandName == "查看") {
                Session["ExResults_fid"] = Convert.ToInt32(GridView1.Rows[gvr.RowIndex].Cells[1].Text);
                if (GridView1.Rows[gvr.RowIndex].Cells[9].Text == "已自评") {
                    Session["ExResults_status"] = 1;
                }
                if (GridView1.Rows[gvr.RowIndex].Cells[9].Text == "返回自评") {
                    Session["ExResults_status"] = 2;
                }
                if (GridView1.Rows[gvr.RowIndex].Cells[9].Text == "已复评") {
                    Session["ExResults_status"] = 3;
                }
                if (GridView1.Rows[gvr.RowIndex].Cells[9].Text == "已审核") {
                    Session["ExResults_status"] = 4;
                }
                this.exResults = Session["MenuView_exResults"] as List<Module.ExamineResult>;
                if (exResults == null) {
                    Response.Write($"<script> alert('获取数据失败。');location='MenuView.aspx' </script>");
                    return;
                }
                foreach (var item in exResults) {
                    if ((int)Session["ExResults_fid"] == item.FID) {
                        Session["ExResults_year"] = item.FYear;
                        Session["ExResults_month"] = item.FMonth;
                    }
                }
                Session["HiddInform"] = false;
                Response.Redirect("UnExecutiveExamine.aspx");
            }
        }
        /// <summary>
        /// 注销
        /// </summary>
        protected void lbtnReLogin_Click(object sender, EventArgs e) {
            Session["HiddInform"] = false;
            this.Page.Dispose();
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Login");
        }
        /// <summary>
        /// 代评分信息录入
        /// </summary>
        protected void lbtnDaiGrade_Click(object sender, EventArgs e) {
            //*代评分人*
            Session["daiGradeID"] = this.employee.FEmCode.ToString();
            Response.Redirect("DaiGradeView.aspx");
        }
        /// <summary>
        /// 员工原考核基数管理
        /// </summary>
        protected void lbtnBaseExam_Click(object sender, EventArgs e) {
            Response.Redirect("EmployeePerInfo.aspx");
        }
        /// <summary>
        /// 绩效考核管理
        /// </summary>
        protected void lbtnExamManager_Click(object sender, EventArgs e) {
            Session["isNewData"] = true;
            Response.Redirect("ExamineManager.aspx");
        }
        /// <summary>
        /// 刷新
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e) {
            Session["MenuView_PerYear"] = ddlYear.SelectedValue;
            Response.Redirect("MenuView.aspx");
        }
        /// <summary>
        /// 当前考核年选择事件
        /// </summary>
        protected void ddlExYear_SelectedIndexChanged(object sender, EventArgs e) {
            Session["ExYear"] = ddlExYear.SelectedValue;
            Session["ExMonth"] = ddlExMonth.SelectedValue;
            UpdatePanel1.Update();
        }
        /// <summary>
        /// 当前考核月选择事件
        /// </summary>
        protected void ddlExMonth_SelectedIndexChanged(object sender, EventArgs e) {
            Session["ExYear"] = ddlExYear.SelectedValue;
            Session["ExMonth"] = ddlExMonth.SelectedValue;
            UpdatePanel1.Update();
        }
        /// <summary>
        /// 绩效考核信息报表
        /// </summary>
        protected void lbtnExamReport_Click(object sender, EventArgs e) {
            Session["isNewData"] = false;
            Response.Redirect("ExamineManager.aspx");
        }
        /// <summary>
        /// 员工信息管理
        /// </summary>
        protected void lbtnEmpManager_Click(object sender, EventArgs e) {
            Response.Redirect("EmployeeManager.aspx");
        }
        /// <summary>
        /// 获取当前【月份】-- 本月25日开始，方可跳到当月，25日之前跳回上一个月
        /// </summary>
        private int GetCurrentMonth() {
            int curMonth = DateTime.Now.Month;
            int curDay = DateTime.Now.Day;
            if (curDay < 25) {
                curMonth = DateTime.Now.AddMonths(-1).Month;
            }
            return curMonth;
        }
    }
}