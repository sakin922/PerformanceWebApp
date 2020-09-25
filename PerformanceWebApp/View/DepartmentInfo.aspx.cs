using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace PerformanceWebApp.View {
    public partial class DepartmentInfo : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                BindData();
            }
        }
        /// <summary>
        /// 数据准备
        /// </summary>
        private void BindData() {
            Controller.EmpManageAction emp = new Controller.EmpManageAction();
            string mag = string.Empty;
            string[] paras = new string[] { txtQuery.Text };
            DataTable dt = emp.GetDepartment(out mag, paras);
            if (dt == null) {
                this.Response.Write($@"<script> alert('{mag}') </script>");
            }
            GridView1.DataSource = null;
            GridView1.DataSource = dt;
            GridView1.DataBind();
        }
        /// <summary>
        /// 分页
        /// </summary>
        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            GridView1.PageIndex = e.NewPageIndex;
            BindData();
        }
        /// <summary>
        /// 行激发事件
        /// </summary>
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e) {
            if (e.CommandName == "btnSelected") {
                if (Session["Empmanager_employee"] != null) {
                    int rowIndex = Convert.ToInt32(e.CommandArgument);
                    string[] paras = new string[] { GridView1.Rows[rowIndex].Cells[1].Text, GridView1.Rows[rowIndex].Cells[2].Text };
                    var model = Session["Empmanager_employee"] as Module.Employee;
                    model.FDepartmentID = paras[0];
                    model.FDepartment = paras[1];
                    Session["Empmanager_employee"] = model;
                }
                Session["EmployeInfomation_Status"] = "3";
                this.Response.Write(@"<script>window.opener.location.href = window.opener.location.href;if (window.opener.progressWindow) 
                    window.opener.progressWindow.close();window.close();</script>");
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        protected void btnQuery_Click(object sender, EventArgs e) {
            BindData();
        }
    }
}