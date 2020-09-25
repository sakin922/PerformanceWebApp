using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PerformanceWebApp.View {
    public partial class Login : System.Web.UI.Page {
        private Module.Employee employee = null;
        private Controller.LoginAction loginAction = null;

        protected void Page_Load(object sender, EventArgs e) {
        }
        /// <summary>
        /// 【登陆】点击事件
        /// </summary>
        protected void btnLogin_Click(object sender, EventArgs e) {
            string mag = string.Empty;
            if (Session["Login_emoloyee"] != null) {
                mag = @"已有账号登陆，请先注销账号或点击重新登陆。";
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                return;
            }

            loginAction = new Controller.LoginAction();
            //*验证是否代评分员工*
            object isDai = null;
            try {
                isDai = loginAction.VerifyDaiGradeEmloye(txtUser.Text);
            }
            catch (Exception ex) {
                mag = ex.Message;
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
            }
            if (isDai != null) {
                mag = @"此账号为代评分账号，请用代评分账号登陆。";
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                return;
            }

            employee = new Module.Employee();
            employee = loginAction.GetEmployee(txtUser.Text, txtPwd.Text, out mag);
            if (employee == null) {
                labMessage.ForeColor = System.Drawing.Color.Red;
                labMessage.Text = @"错误提示： " + mag;
                return;
            }
            //*获取员工信息*
            Session["Login_emoloyee"] = employee;
            Session["Login_isDai"] = false;

            //*验证是否【代评分账号】*
            try {
                isDai = loginAction.VerifyDaiGrader(txtUser.Text);
            }
            catch (Exception ex) {
                mag = ex.Message;
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
            }
            if (isDai != null) {
                //*代评分员工*
                Session["Login_isDai"] = true;
            }

            labMessage.Text = @"登陆成功！";
            Response.Redirect("MenuView");
        }
        /// <summary>
        /// 清除页面
        /// </summary>
        private void ClearView() {
            txtUser.Text = string.Empty;
            txtPwd.Text = string.Empty;
            labMessage.Text = string.Empty;
        }
        /// <summary>
        /// 【取消】点击事件
        /// </summary>
        protected void btnCancel_Click(object sender, EventArgs e) {
            ClearView();
        }

    }
}