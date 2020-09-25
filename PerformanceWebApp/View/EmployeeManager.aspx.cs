using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PerformanceWebApp.View {
    public partial class EmployeeManager : System.Web.UI.Page {
        private Controller.EmpManageAction empManage = new Controller.EmpManageAction();
        private List<Module.Employee> employees = null;
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                string mag = string.Empty;
                string[] paras = new string[] { ddlIsCancel.SelectedValue,ddlStatus.SelectedValue, txtQuery.Text };
                if (ddlStatus.SelectedValue == "全部") {
                    paras[1] = string.Empty;
                }
                employees = empManage.GetEmployees(out mag, paras);
                if (employees == null) {
                    Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                    return;
                }
                Session["Empmanager_DataSource"] = employees;
                GridView1.DataSource = null;
                GridView1.DataSource = employees;
                GridView1.DataBind();
                GridView1.Columns[11].Visible = false;
            }
        }
        /// <summary>
        /// 分页事件
        /// </summary>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            GridView1.PageIndex = e.NewPageIndex;
            GridView1.DataSource = null;
            GridView1.DataSource = Session["Empmanager_DataSource"];
            GridView1.DataBind();
            GridView1.Columns[11].Visible = false;
        }
        /// <summary>
        /// 返回主页
        /// </summary>
        protected void lbtnBackMainPage_Click(object sender, EventArgs e) {
            Session["Empmanager_DataSource"] = null;
            Session["Empmanager_employee"] = null;
            Session["HiddInform"] = false;
            this.Dispose();
            Response.Redirect("MenuView.aspx");
        }
        /// <summary>
        /// 查询 
        /// </summary>
        protected void btnQuery_Click(object sender, EventArgs e) {
            string[] paras = new string[] { ddlIsCancel.SelectedValue,ddlStatus.SelectedValue, txtQuery.Text };
            if (ddlStatus.SelectedValue == "全部") {
                paras[1] = string.Empty;
            }
            employees = empManage.GetEmployees(out string mag, paras);
            if (employees == null) {
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                return;
            }
            Session["Empmanager_DataSource"] = employees;
            GridView1.DataSource = null;
            GridView1.DataSource = employees;
            GridView1.DataBind();
            GridView1.Columns[11].Visible = false;
        }
        /// <summary>
        /// 行内控件事件--选择
        /// </summary>
        protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e) {
            int row = e.NewSelectedIndex;
            string _fid = GridView1.Rows[row].Cells[1].Text;
            var employees = Session["Empmanager_DataSource"] as List<Module.Employee>;
            foreach (var employee in employees) {
                if (_fid == employee.FID) {
                    Session["Empmanager_employee"] = employee;
                }
            }            
            Session["EmployeInfomation_Status"] = "2";
            string url = @"EmployeInfomation.aspx";
            Response.Redirect(url);
        }
        /// <summary>
        /// 新增员工
        /// </summary>
        protected void btnCreateEmp_Click(object sender, EventArgs e) {
            Session["EmployeInfomation_Status"] = 1;
            Response.Redirect("EmployeInfomation.aspx");
        }
    }
}