using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace PerformanceWebApp.View {
    public partial class DaiGradeView : System.Web.UI.Page {

        private Controller.DaiGradeAction daiGradeAction = new Controller.DaiGradeAction();
        private int billStatus = 0; //0=null,1=新增，2=已评分
        private Module.ExamineResult examineResult = null;

        protected void Page_Load(object sender, EventArgs e) {
            //*代评分人*
            if (!IsPostBack) {
                string mag = string.Empty;
                string daiID = Session["daiGradeID"].ToString();
                labYear.Text = Session["ExYear"].ToString();
                labMonth.Text = Session["ExMonth"].ToString();
                hiddenUrl.Value = Request.UrlReferrer.ToString();
                Session["gridSource"] = daiGradeAction.GetDaiGradeRecord(daiID, labYear.Text, labMonth.Text, txtQuery.Text, out mag);
                if (Session["gridSource"] == null) {
                    txtQuery.Enabled = false;
                    btnQuery.Enabled = false;
                    ErorrAndReturn(mag);
                    Session["gridSource"] = daiGradeAction.GetNewDaiGradeData(daiID, labYear.Text, labMonth.Text, txtQuery.Text, out mag);
                    ErorrAndReturn(mag);
                    Session["table_isNew"] = true;
                }
                else {
                    Session["table_isNew"] = false;
                }
                GridDataBind();
            }
            this.billStatus = Convert.ToInt32(Session["billStatus"]);
            //if (billStatus == 1) {
            //    GridView1.Columns[0].Visible = false;
            //}
        }
        /// <summary>
        /// 数据绑定方法
        /// </summary>
        private void GridDataBind() {
            DataTable dt = Session["gridSource"] as DataTable;
            bool table_isNew = Convert.ToBoolean(Session["table_isNew"]);
            if (table_isNew) {
                if (dt.Rows.Count > 0) {
                    GridView1.DataSource = null;
                    GridView1.DataSource = dt;
                }
                btnModify.Enabled = false;
                Session["billStatus"] = 1;
            }
            else {
                GridView1.DataSource = null;
                GridView1.DataSource = dt;
                GridView1.Columns[8].Visible = false;
                btnCommit.Enabled = false;
                btnModify.Enabled = true;
                Session["billStatus"] = 2;
            }
            GridView1.DataBind();
            foreach (GridViewRow gvr in GridView1.Rows) {
                if (gvr.Cells[6].Text == "修改自评")
                    gvr.ForeColor = System.Drawing.Color.DarkOrange;
                if (gvr.Cells[6].Text == "已复评" || gvr.Cells[6].Text == "已审核")
                    gvr.ForeColor = System.Drawing.Color.DarkBlue;
            }
            GridView1.Columns[1].Visible = false;
        }
        /// <summary>
        /// 出错并终止执行
        /// </summary>
        private void ErorrAndReturn(string mag) {
            if (!string.IsNullOrEmpty(mag)) {
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                return;
            }
        }
        /// <summary>
        /// 返回主页
        /// </summary>
        protected void LinkButton1_Click(object sender, EventArgs e) {
            Session["HiddInform"] = false;
            this.Dispose();
            Response.Redirect("MenuView.aspx");
        }
        /// <summary>
        /// 提交
        /// </summary>
        protected void btnCommit_Click(object sender, EventArgs e) {
            string mag = string.Empty;
            object result = null;
            bool success = false;
            int rows = 0;

            //*判断billStatus状态*
            switch (this.billStatus) {
                //新增
                case 1:
                    rows = GridView1.Rows.Count;
                    for (int i = 0; i < rows; i++) {
                        examineResult = new Module.ExamineResult {
                            FCDate = DateTime.Now.Date,
                            FMDate = DateTime.Now.Date,
                            FEmCode = GridView1.Rows[i].Cells[2].Text,
                            FYear = labYear.Text,
                            FMonth = labMonth.Text,
                            FStatus = "1".ToString(),
                        };
                        TextBox txtSeSum = GridView1.Rows[i].FindControl("TxtDaiSelfGrade") as TextBox;
                        examineResult.FSelfSum = Convert.ToInt32(string.IsNullOrEmpty(txtSeSum.Text) ? "0" : txtSeSum.Text);
                        //*0-100分范围检验*
                        if (examineResult.FSelfSum < 0 || examineResult.FSelfSum > 100) {
                            mag = "评分超出0-100的范围，请重新填写。";
                            txtSeSum.Focus();
                            Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                            return;
                        }
                        result = daiGradeAction.InsertNewDaiGradeData(examineResult, out mag);
                        if (result == null || !string.IsNullOrEmpty(mag)) {
                            Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                            return;
                        }
                        //*新增明细表*
                        try {
                            examineResult.FID = Convert.ToInt32(result);
                            object x = daiGradeAction.InsertNewDaiGradeDetail(examineResult, out mag);
                            if ((int)x != 1 || !string.IsNullOrEmpty(mag)) {
                                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                                return;
                            }
                        }
                        catch (Exception ex) {
                            mag = ex.Message;
                            Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                            return;
                        }
                    }
                    success = true;
                    break;
                //修改自评
                case 2:
                    rows = GridView1.Rows.Count;
                    for (int i = 0; i < rows; i++) {
                        TextBox textBox = GridView1.Rows[i].FindControl("TxtDaiSelfGrade") as TextBox;
                        if (!string.IsNullOrEmpty(textBox.Text)) {
                            examineResult = daiGradeAction.GetSelfGradeDetails(GridView1.Rows[i].Cells[1].Text, out mag);
                            if (examineResult == null) {
                                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                                return;
                            }
                            try {
                                examineResult.FSelfSum = Convert.ToInt32(textBox.Text);
                            }
                            catch {
                                mag = "分数输入不正确，请输入整数。";
                                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                                return;
                            }

                            //*重新获取【总成绩】*
                            examineResult.FExamineSum = Convert.ToDecimal((examineResult.FSelfSum * 0.3) + (examineResult.FRepeSum * 0.7));

                            //*0-100分范围检验*
                            if (examineResult.FSelfSum < 0 || examineResult.FSelfSum > 100) {
                                mag = "评分超出0-100的范围，请重新填写。";
                                textBox.Focus();
                                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                                return;
                            }
                            //*单据状态修改为【已复评】*
                            if (examineResult.FStatus == "2") {
                                examineResult.FStatus = "3";
                            }

                            result = daiGradeAction.UpdateDaiGradeRecord(examineResult, out mag);
                            if (result == null || !string.IsNullOrEmpty(mag)) {
                                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                                return;
                            }
                            //*更新明细*
                            object x = daiGradeAction.UpdateDaiGradeDetail(examineResult, out mag);
                            if ((int)x != 1 || !string.IsNullOrEmpty(mag)) {
                                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                                return;
                            }
                        }
                    }
                    success = true;
                    break;
                default:
                    success = false;
                    break;
            }
            if (success) {
                mag = "数据提交成功。";
                //Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                Response.Write($@"<script languge='javascript'>alert('{mag}'); window.location.href='DaiGradeView.aspx'</script>");
            }
            else {
                mag = "数据提交失败，请联系管理员。";
                Response.Write($"<script language='javascript'>alert('{mag}')");
            }
            btnModify.Enabled = true;
        }
        /// <summary>
        /// 修改
        /// </summary>
        protected void btnModify_Click(object sender, EventArgs e) {
            try {
                GridView1.Columns[8].Visible = true;
                btnCommit.Enabled = true;
                btnModify.Enabled = false;
                for (int i = 0; i < GridView1.Rows.Count; i++) {
                    string _status = GridView1.Rows[i].Cells[6].Text;
                    if (_status == "已复评" || _status == "已审核") {
                        GridView1.Rows[i].Cells[8].Enabled = false;
                    }
                }
            }
            catch (Exception ex) {
                Response.Write($@"<script language=javaScript>alert('{ex.Message}');</script>");
                return;
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        protected void btnQuery_Click(object sender, EventArgs e) {
            string daiID = Session["daiGradeID"].ToString();
            string mag = string.Empty;
            Session["gridSource"] = daiGradeAction.GetDaiGradeRecord(daiID, labYear.Text, labMonth.Text, txtQuery.Text, out mag);
            if (Session["gridSource"] == null) {
                ErorrAndReturn(mag);
                Session["gridSource"] = daiGradeAction.GetNewDaiGradeData(daiID, labYear.Text, labMonth.Text, txtQuery.Text, out mag);
                ErorrAndReturn(mag);
                Session["table_isNew"] = true;
            }
            else {
                Session["table_isNew"] = false;
            }
            GridDataBind();
            this.billStatus = Convert.ToInt32(Session["billStatus"]);
        }
    }
}