using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace PerformanceWebApp.View {
    public partial class MyReview : System.Web.UI.Page {
        private Controller.MenuAction menuAction;
        private Module.Employee employee = null;

        protected void Page_Load(object sender, EventArgs e) {
            if (Session["Login_emoloyee"] == null) {
                Response.Redirect("Login");
                return;
            }
            //*获取登陆信息*
            this.employee = Session["Login_emoloyee"] as Module.Employee;
            hiddEmName.Value = this.employee.FEmName;
            Session["HiddInform"] = false;

            if (!IsPostBack) {
                labYear.Text = Session["ExYear"].ToString();
                labMonth.Text = Session["ExMonth"].ToString();
                hiddenUrl.Value = Request.UrlReferrer.ToString();
                //**数据绑定**
                GridDataBind();
            }
        }
        private void GridDataBind() {
            if (!GridView1.Columns[1].Visible && !GridView1.Columns[10].Visible && !GridView1.Columns[11].Visible && !GridView1.Columns[12].Visible) {
                GridView1.Columns[1].Visible = GridView1.Columns[10].Visible = GridView1.Columns[11].Visible = GridView1.Columns[12].Visible = true;
            }

            if (Session["PageIndex"] != null) {
                try {
                    GridView1.PageIndex = Convert.ToInt32(Session["PageIndex"]);
                }
                catch {
                    GridView1.PageIndex = 0;
                }
            }

            //*获取【进度状态】*
            string _billStatus = string.Empty;
            if (ddlBillStatus.SelectedValue != "-1") {
                _billStatus = ddlBillStatus.SelectedItem.Text;
            }
            menuAction = new Controller.MenuAction();
            DataTable dt = menuAction.GetGradeExamResults(this.employee.FEmCode, labYear.Text, labMonth.Text, txtQuery.Text, _billStatus, out string mag);
            if (dt == null) {
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                return;
            }
            if (dt.Rows.Count > 0) {
                Session["DataSource"] = dt;
            }
            GridView1.DataSource = null;
            GridView1.DataSource = dt;
            GridView1.DataBind();
           
            for (int i = 0; i < GridView1.Rows.Count; i++) {
                //*退回自评*
                if (GridView1.Rows[i].Cells[11].Text == "3") {
                    Button btn = GridView1.Rows[i].FindControl("btnBacktoSelf") as Button;
                    btn.Enabled = true;
                }
                //*未自评分、已审核、待复评*
                if (GridView1.Rows[i].Cells[11].Text == "0" || GridView1.Rows[i].Cells[11].Text == "4" || GridView1.Rows[i].Cells[11].Text == "2") {
                    if (GridView1.Rows[i].Cells[11].Text == "0") {
                        GridView1.Rows[i].ForeColor = System.Drawing.Color.Red;
                    }
                    if (GridView1.Rows[i].Cells[11].Text == "4") {
                        GridView1.Rows[i].ForeColor = System.Drawing.Color.DarkGreen;
                    }
                    Button btn= GridView1.Rows[i].FindControl("btnGrade") as Button;
                    btn.Enabled = false;
                }
            }
            GridView1.Columns[1].Visible = false;
            GridView1.Columns[10].Visible = false;
            GridView1.Columns[11].Visible = false;
            GridView1.Columns[12].Visible = false;
        }
        /// <summary>
        /// 【返回主页】
        /// </summary>
        protected void LinkButton1_Click(object sender, EventArgs e) {
            Session["HiddInform"] = false;
            this.Dispose();
            Response.Redirect("MenuView.aspx");
        }
        /// <summary>
        /// GridView1模板控件激发事件--【评分】
        /// </summary>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
            Session["HiddInform"] = true;
            Session["ExResults_year"] = labYear.Text;
            Session["ExResults_month"] = labMonth.Text;
            if (e.CommandName == "btnGrade") {
                GridViewRow grv = (GridViewRow)((Button)e.CommandSource).Parent.Parent;
                Module.ExamineResult employee = new Module.ExamineResult() {
                    FEmCode = GridView1.Rows[grv.RowIndex].Cells[3].Text,
                    FEmName = GridView1.Rows[grv.RowIndex].Cells[4].Text,                    
                    FEmpStatus = GridView1.Rows[grv.RowIndex].Cells[12].Text,
                    FStatus = GridView1.Rows[grv.RowIndex].Cells[11].Text,
                    FExmName = hiddEmName.Value,
                };
                //*单据状态*
                Session["ExResults_status"] = Convert.ToInt32(GridView1.Rows[grv.RowIndex].Cells[11].Text);
                //*单据信息*
                Session["MyReview_Employee"] = employee;
                //单据ID
                Session["ExResults_fid"] = Convert.ToInt32(GridView1.Rows[grv.RowIndex].Cells[1].Text);
                //主管级别
                Session["MyReview_isDire"] = Convert.ToBoolean(GridView1.Rows[grv.RowIndex].Cells[10].Text);
                //是否代评分用户
                Session["MyReview_isDaiGrade"] = Convert.ToInt32(GridView1.Rows[grv.RowIndex].Cells[12].Text);
                Response.Redirect("UnExecutiveExamine.aspx");
            }

            //*退回自评分*
            if (e.CommandName == "btnBacktoSelf") {
                GridViewRow grv = (GridViewRow)((Button)e.CommandSource).Parent.Parent;
                string _fid = GridView1.Rows[grv.RowIndex].Cells[1].Text;
                menuAction = new Controller.MenuAction();
                object x = menuAction.UpdateExamResultStatus(_fid, out string mag);
                if (x == null) {
                    Response.Write($"<script> alert('{mag}');location='MyReview.aspx' </script>");
                }
                else {
                    mag = "状态已修改为【修改自评分】状态，请提醒自评用户修改并提交。";
                    Response.Write($"<script> alert('{mag}');location='MyReview.aspx' </script>");
                }
            }
        }
        /// <summary>
        /// 分页激发事件
        /// </summary>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            GridView1.PageIndex = e.NewPageIndex;
            Session["PageIndex"] = GridView1.PageIndex;
            GridDataBind();
        }
        /// <summary>
        /// 查询
        /// </summary>
        protected void btnQuery_Click(object sender, EventArgs e) {
            //**数据绑定**
            GridDataBind();
        }
    }
}