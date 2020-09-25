using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PerformanceWebApp.View {
    public partial class DepartmentSchedule : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                switch (Session["ScheduleStatus"].ToString()) {
                    //*未完成绩效*
                    case "1":
                        labTitle.Text = "未完成绩效";
                        break;
                    //*未完成复评*
                    case "2":
                        labTitle.Text = "未完成复评";
                        break;
                    //*未完成自评*
                    case "3":
                        labTitle.Text = "未完成自评";
                        break;
                    default:
                        break;
                }
                this.GridViewBinding(Session["ScheduleStatus"].ToString());
            }
        }
        /// <summary>
        /// 数据绑定--GridView
        /// </summary>
        private void GridViewBinding(string status) {
            if (Session["GetDepartmentSchedule"] != null) {
                DataSet ds = Session["GetDepartmentSchedule"] as DataSet;
                DataTable dt = null;
                if (status == "1") {
                    dt = ds.Tables[1];
                }
                if (status == "2") {
                    dt = ds.Tables[2];
                }
                if (status == "3") {
                    dt = ds.Tables[3];
                }
                if (dt != null) {
                    GridView1.DataSource = null;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                }
                dt.Dispose();
                ds.Dispose();
            }
        }
        /// <summary>
        /// 关闭
        /// </summary>
        protected void lkbClose_Click(object sender, EventArgs e) {
            Session["ScheduleStatus"] = null;
            this.Response.Write(@"<script> window.close() </script>");
        }
    }
}