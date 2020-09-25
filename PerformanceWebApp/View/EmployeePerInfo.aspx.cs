using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;

namespace PerformanceWebApp.View {
    [Description("员工原考核基数")]
    public partial class EmployeePerInfo : System.Web.UI.Page {
        private Controller.ExamManageAction manageAction = new Controller.ExamManageAction();
        private List<Module.Employee> employees = null;
        protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                labYear.Text = DateTime.Now.Year.ToString();
                labMonth.Text = DateTime.Now.Month.ToString();
                employees = manageAction.GetBaseExamineInfo(txtQuery.Text, out string mag);
                if (employees == null) {
                    Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                    return;
                }
                Session["gridSource"] = employees;
                Session["gridSource_old"] = employees;
                employees = null;
                this.GridViewDataBind(Session["gridSource"]);
            }
            btnSave.Enabled = false;
            foreach (GridViewRow row in GridView1.Rows) {
                TextBox textBox = row.FindControl("TxtBaseNum") as TextBox;
                textBox.ReadOnly = true;
            }
        }
        /// <summary>
        /// GridView数据绑定
        /// </summary>
        private void GridViewDataBind(object source) {
            GridView1.DataSource = null;
            GridView1.DataSource = source;
            GridView1.DataBind();
            //**绑定（原考核基数）**
            if (GridView1.Rows.Count > 0) {
                employees = source as List<Module.Employee>;
                for (int i = 0; i < GridView1.Rows.Count; i++) {
                    TextBox text = GridView1.Rows[i].FindControl("TxtBaseNum") as TextBox;
                    foreach (Module.Employee item in employees) {
                        if (item.FID == GridView1.Rows[i].Cells[1].Text) {
                            text.Text = item.FBaseExamine.ToString();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        protected void btnQuery_Click(object sender, EventArgs e) {
            Session["gridSource"] = null;
            Session["gridSource"] = manageAction.GetBaseExamineInfo(txtQuery.Text, out string mag);
            if (Session["gridSource"] == null) {
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                return;
            }
            this.GridViewDataBind(Session["gridSource"]);
            Session["gridSource_old"] = Session["gridSource"];
        }
        /// <summary>
        /// 返回主页
        /// </summary>
        protected void lbtnBack_Click(object sender, EventArgs e) {
            Session["HiddInform"] = false;
            this.Dispose();
            Response.Redirect("MenuView.aspx");
        }
        /// <summary>
        /// 重置
        /// </summary>
        protected void btnRebuil_Click(object sender, EventArgs e) {
            this.GridViewDataBind(Session["gridSource_old"]);
        }
        /// <summary>
        /// 保存
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e) {
            string mag = string.Empty;
            bool success = true;

            if (GridView1.SelectedIndex != -1) {
                int row = GridView1.SelectedIndex;
                string emCode = GridView1.Rows[row].Cells[2].Text;
                TextBox textBox = GridView1.Rows[row].FindControl("TxtBaseNum") as TextBox;
                decimal value = 0.00M;
                try {
                    value = Convert.ToDecimal(string.IsNullOrEmpty(textBox.Text) ? "0" : textBox.Text);
                }
                catch {
                    textBox.Focus();
                    success = false;
                    mag = "数值输入有误，请重新输入。";
                    Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                }
                if (success) {
                    object result = null;
                    //*查询该行是否新增行，如果result不为空，就是新数据*
                    result = manageAction.CheckNullEmployee(emCode, out mag);
                    if (result != null) {
                        result = null;
                        //*新增数据*
                        result = manageAction.InsertBaseExamineValue(emCode, value, out mag);
                        if (result == null) {
                            mag = "数据更新失败。" + mag;
                            Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                            return;
                        }
                        if (Convert.ToInt32(result) == 0) {
                            mag = "数据更新失败。" + mag;
                            Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                            return;
                        }
                        success = true;
                    }
                    else {
                        result = null;
                        //*更新数据*
                        result= manageAction.UpdateBaseExamineValue(emCode, value, out mag);
                        if (result == null) {
                            mag = "数据更新失败。" + mag;
                            Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                            return;
                        }
                        if (Convert.ToInt32(result) == 0) {
                            mag = "数据更新失败。" + mag;
                            Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                            return;
                        }
                        success = true;
                    }
                }
                if (success) {
                    mag = "数据更新成功。";
                    Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                    btnQuery_Click(null, null);
                }
            }
            else {
                mag = "请先选择要修改的行。";
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
            }
        }
        /// <summary>
        /// GridView--选择行
        /// </summary>
        protected void GridView1_SelectedIndexChanging(object sender, GridViewSelectEventArgs e) {
            this.GridViewDataBind(Session["gridSource_old"]);
            if (GridView1.SelectedIndex != e.NewSelectedIndex) {
                TextBox textBox = GridView1.Rows[e.NewSelectedIndex].FindControl("TxtBaseNum") as TextBox;
                textBox.ReadOnly = false;
                GridView1.Rows[e.NewSelectedIndex].BackColor = System.Drawing.Color.Red;
                btnSave.Enabled = true;
            }
        }
    }
}