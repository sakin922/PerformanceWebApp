using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace PerformanceWebApp.View {
    public partial class UnExecutiveExamine : System.Web.UI.Page {
        private Controller.MenuAction menuAction = new Controller.MenuAction();
        private List<Module.ExamineDetail> exmDetails = null;
        private Module.ExamineResult exResult = null;
        private Module.Employee employee = null;
        private int status = 0;
        private int fid = 0;

        protected void Page_Load(object sender, EventArgs e) {
            if (Session["Login_emoloyee"] == null) {
                Response.Redirect("Login");
                return;
            }

            //*获取上一个页面传递的Session参数*
            status = Convert.ToInt32(Session["ExResults_status"]);
            if (status > 0) {
                fid = Convert.ToInt32(Session["ExResults_fid"]);
            }

            if (!IsPostBack) {
                try {
                    if (string.IsNullOrEmpty(Request.UrlReferrer.ToString())) {
                        Response.Redirect("Login");
                    }
                    else {
                        hiddenUrl.Value = Request.UrlReferrer.ToString();
                    }
                }
                catch (Exception) {
                    Response.Redirect("Login");
                }

                //*获取登陆信息*
                this.employee = Session["Login_emoloyee"] as Module.Employee;
                string mag = string.Empty;

                //*判断是否待复评分*
                if (Session["MyReview_isDire"] != null) {
                    this.exmDetails = menuAction.GetExamineProject(Convert.ToBoolean(Session["MyReview_isDire"]), Convert.ToInt32(Session["MyReview_isDaiGrade"]), out mag);
                    //*清空待复评分的信息*
                    Session["MyReview_isDire"] = null;
                }
                else {
                    this.exmDetails = menuAction.GetExamineProject(this.employee.FIsDirector, Convert.ToInt32(this.employee.FStatus), out mag);
                }
                if (this.exmDetails == null) {
                    return;
                }

                //*信息隐藏控制*
                if (!Convert.ToBoolean(Session["HiddInform"])) {
                    //复评隐藏
                    GridView1.Columns[9].Visible = false;
                    txtReGradeSum.Visible = false;
                    //总成绩隐藏
                    txtTotalSum.Visible = false;
                    //评语隐藏
                    txtExmMessage.Visible = false;
                }

                //*项目数据存入Session，以便Postback后调用*
                Session["exmProject"] = this.exmDetails;
                GridView1.DataSource = null;
                GridView1.DataSource = this.exmDetails;
                GridView1.DataBind();

                //*获取登陆时的单据状态*
                switch (status) {
                    case 0:
                        labStatus.Text = "新增";
                        btnModify.Enabled = false;
                        GetExResultsDetails();
                        int rows = GridView1.Rows.Count;
                        for (int i = 0; i < rows; i++) {
                            TextBox txtExGrade = GridView1.Rows[i].FindControl("TxtExGrade") as TextBox;
                            txtExGrade.Enabled = false;
                        }
                        break;
                    case 1:
                        labStatus.Text = "已自评分";
                        //*领导评分*
                        if (Convert.ToBoolean(Session["HiddInform"])) {
                            btnCommit.Enabled = true;
                            btnModify.Enabled = false;
                            txtExmMessage.ReadOnly = false;
                        }
                        else {
                            btnCommit.Enabled = false;
                        }
                        GetExResultsDetails();
                        break;
                    case 2:
                        labStatus.Text = "修改自评分";
                        //*领导评分*
                        if (Convert.ToBoolean(Session["HiddInform"])) {
                            btnCommit.Enabled = false;
                            btnModify.Enabled = true;
                        }
                        else {
                            btnCommit.Enabled = false;
                        }
                        GetExResultsDetails();
                        break;
                    case 3:
                        labStatus.Text = "已复评分";
                        //*主管级别以下【修改】禁用*
                        if (!this.employee.FIsDirector || !Convert.ToBoolean(Session["HiddInform"])) {
                            btnModify.Enabled = false;
                        }
                        txtExmMessage.ReadOnly = true;
                        btnCommit.Enabled = false;
                        GetExResultsDetails();
                        break;
                    case 4:
                        labStatus.Text = "已审核";
                        btnCommit.Enabled = false;
                        btnModify.Enabled = false;
                        GetExResultsDetails();
                        break;
                    default:
                        break;
                }
            }
            else {
                //*新增状态*
                if (status == 0) {
                    int rows = GridView1.Rows.Count;
                    for (int i = 0; i < rows; i++) {
                        TextBox txtExGrade = GridView1.Rows[i].FindControl("TxtExGrade") as TextBox;
                        txtExGrade.Enabled = false;
                    }
                }
                if (status == 1 || status == 2) {
                    int rows = GridView1.Rows.Count;
                    exmDetails = Session["exmGrade"] as List<Module.ExamineDetail>;
                    for (int i = 0; i < rows; i++) {
                        for (int j = i; j < exmDetails.Count;) {
                            //*个人已评分*
                            if (!Convert.ToBoolean(Session["HiddInform"])) {
                                //TextBox textBox = GridView1.Rows[i].FindControl("TxtSelfGrade") as TextBox;
                                //textBox.Text = exmDetails[j].FSelfGrade.ToString();
                            }
                            //*领导评分*
                            else {
                                GridView1.Rows[i].Cells[8].Text = exmDetails[j].FSelfGrade.ToString();
                            }
                            break;
                        }
                    }
                }
                //*领导评分*
                if (status > 2 && status < 4) {
                    //*领导评分*
                    int rows = GridView1.Rows.Count;
                    exmDetails = Session["exmGrade"] as List<Module.ExamineDetail>;
                    for (int i = 0; i < rows; i++) {
                        for (int j = i; j < exmDetails.Count;) {
                            GridView1.Rows[i].Cells[8].Text = exmDetails[j].FSelfGrade.ToString();
                            //TextBox txtExGrade = GridView1.Rows[i].FindControl("TxtExGrade") as TextBox;
                            //txtExGrade.Text= exmDetails[j].FRepeGrade.ToString();
                            break;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取考核表信息
        /// </summary>
        private void GetExResultsDetails() {
            string mag = string.Empty;
            //*新增*
            if (status == 0) {
                labYear.Text = Session["ExYear"].ToString();
                labMonth.Text = Session["ExMonth"].ToString();
                labEmCode.Text = this.employee.FEmCode;
                labEmName.Text = this.employee.FEmName;
                labReExName.Text = this.employee.FExamerID;
            }
            else {
                labYear.Text = Session["ExResults_year"].ToString();
                labMonth.Text = Session["ExResults_month"].ToString();
            }
            //*个人评分*
            if (status == 1 || status == 2) {
                if (Session["MyReview_Employee"] != null) {
                    Module.ExamineResult myReviewEmployee = Session["MyReview_Employee"] as Module.ExamineResult;
                    labEmCode.Text = myReviewEmployee.FEmCode;
                    labEmName.Text = myReviewEmployee.FEmName;
                    labReExName.Text = myReviewEmployee.FExmName;
                }
                else {
                    labEmCode.Text = this.employee.FEmCode;
                    labEmName.Text = this.employee.FEmName;
                    labReExName.Text = this.employee.FExamerID;
                }
            }
            //*已复评分*
            if (status > 2) {
                if (Session["MyReview_Employee"] != null) {
                    Module.ExamineResult myReviewEmployee = Session["MyReview_Employee"] as Module.ExamineResult;
                    labEmCode.Text = myReviewEmployee.FEmCode;
                    labEmName.Text = myReviewEmployee.FEmName;
                    labReExName.Text = myReviewEmployee.FExmName;
                    Session["MyReview_Employee"] = null;
                }
                else {
                    labEmCode.Text = this.employee.FEmCode;
                    labEmName.Text = this.employee.FEmName;
                    labReExName.Text = this.employee.FExamerID;
                }
            }

            if (fid > 0) {
                exResult = new Module.ExamineResult();
                exmDetails = null;
                //*获取考核评分明细*
                exmDetails = menuAction.GetExamineDetails(fid, out mag);
                if (exmDetails == null) {
                    Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                    return;
                }

                //*分数明细存入Session，以便Postback后调用*
                Session["exmGrade"] = this.exmDetails;
                int rows = GridView1.Rows.Count;
                for (int i = 0; i < rows; i++) {
                    for (int j = i; j < exmDetails.Count;) {
                        if (status == 1) {
                            GridView1.Rows[i].Cells[8].Text = exmDetails[j].FSelfGrade.ToString();
                        }
                        else {
                            GridView1.Rows[i].Cells[8].Text = exmDetails[j].FSelfGrade.ToString();
                            GridView1.Rows[i].Cells[9].Text = exmDetails[j].FRepeGrade.ToString();
                        }
                        break;
                    }
                }
                //*获取考核评分结果*
                exResult = menuAction.GetExamineResult(fid, out mag);
                if (exResult == null) {
                    Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                    return;
                }
                txtSelfSum.Text = exResult.FSelfSum.ToString();
                txtReGradeSum.Text = exResult.FRepeSum.ToString();
                txtTotalSum.Text = exResult.FExamineSum.ToString();
                txtExmMessage.Text = exResult.FExamMessage;
            }
        }
        /// <summary>
        /// 【自评分数】数据变化时的激发事件
        /// </summary>
        protected void TxtSelfGrade_TextChanged(object sender, EventArgs e) {
            decimal _count = 0;
            TextBox textBox;
            for (int i = 0; i < GridView1.Rows.Count; i++) {
                textBox = GridView1.Rows[i].FindControl("TxtSelfGrade") as TextBox;
                if (!string.IsNullOrEmpty(textBox.Text)) {
                    try {
                        _count += Convert.ToDecimal(textBox.Text);
                    }
                    catch {
                        _count += 0;
                    }
                }
                txtSelfSum.Text = _count.ToString();
            }
            double tolReGrade = 0;
            double tolSelf = 0;
            tolSelf = Convert.ToDouble(string.IsNullOrEmpty(txtSelfSum.Text) ? "0" : txtSelfSum.Text) * 0.3;
            tolReGrade = Convert.ToDouble(string.IsNullOrEmpty(txtReGradeSum.Text) ? "0" : txtReGradeSum.Text) * 0.7;
            txtTotalSum.Text = (Convert.ToDecimal(tolSelf) + Convert.ToDecimal(tolReGrade)).ToString();
        }
        /// <summary>
        /// 【复评分数】数据变化时的激发事件
        /// </summary>
        protected void TxtExGrade_TextChanged(object sender, EventArgs e) {
            decimal _count = 0;
            TextBox textBox;
            for (int i = 0; i < GridView1.Rows.Count; i++) {
                textBox = GridView1.Rows[i].FindControl("TxtExGrade") as TextBox;
                if (!string.IsNullOrEmpty(textBox.Text)) {
                    try {
                        _count += Convert.ToDecimal(textBox.Text);
                    }
                    catch {
                        _count += 0;
                    }
                }
                txtReGradeSum.Text = _count.ToString();
            }
            double tolReGrade = 0;
            double tolSelf = 0;
            tolSelf = Convert.ToDouble(string.IsNullOrEmpty(txtSelfSum.Text) ? "0" : txtSelfSum.Text) * 0.3;
            tolReGrade = Convert.ToDouble(string.IsNullOrEmpty(txtReGradeSum.Text) ? "0" : txtReGradeSum.Text) * 0.7;
            txtTotalSum.Text = (Convert.ToDecimal(tolSelf) + Convert.ToDecimal(tolReGrade)).ToString();
        }
        /// <summary>
        /// 修改按钮点击事件
        /// </summary>
        protected void btnModify_Click(object sender, EventArgs e) {
            btnModify.Enabled = false;
            btnCommit.Enabled = true;

            //*重新加载GridView*
            GridView1.DataSource = null;
            GridView1.DataSource = Session["exmProject"] as List<Module.ExamineDetail>;
            GridView1.DataBind();

            //*已自评分状态*
            if (status == 1) {
                int rows = GridView1.Rows.Count;
                exmDetails = Session["exmGrade"] as List<Module.ExamineDetail>;
                for (int i = 0; i < rows; i++) {
                    for (int j = i; j < exmDetails.Count;) {
                        //*个人*
                        if (!Convert.ToBoolean(Session["HiddInform"])) {
                            TextBox textBox = GridView1.Rows[i].FindControl("TxtSelfGrade") as TextBox;
                            textBox.Text = exmDetails[j].FSelfGrade.ToString();
                        }
                        //*领导*
                        else {
                            GridView1.Rows[i].Cells[8].Text = exmDetails[j].FSelfGrade.ToString();
                        }
                        break;
                    }
                }
            }
            //*已复核评分*
            if (status == 3 || status == 2) {
                int rows = GridView1.Rows.Count;
                exmDetails = Session["exmGrade"] as List<Module.ExamineDetail>;
                if (exmDetails.Count == rows) {
                    for (int i = 0; i < rows; i++) {
                        //*个人修改*
                        if (!Convert.ToBoolean(Session["HiddInform"])) {
                            TextBox txtSelfGrade = GridView1.Rows[i].FindControl("TxtSelfGrade") as TextBox;
                            txtSelfGrade.Text = exmDetails[i].FSelfGrade.ToString();
                            txtSelfGrade.Enabled = true;
                        }
                        //*领导评分*
                        else {
                            GridView1.Rows[i].Cells[8].Text = exmDetails[i].FSelfGrade.ToString();
                            TextBox txtExGrade = GridView1.Rows[i].FindControl("TxtExGrade") as TextBox;
                            txtExGrade.Text = exmDetails[i].FRepeGrade.ToString();
                            txtExGrade.Enabled = true;
                            txtExmMessage.ReadOnly = false;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 【提交】点击事件
        /// </summary>
        protected void btnCommit_Click(object sender, EventArgs e) {
            bool success = false;
            string mag = string.Empty;
            Module.ExamineDetail detail = null;

            //**新增状态**
            if (status == 0) {
                //*新增记录表*
                exResult = new Module.ExamineResult() {
                    FCDate = DateTime.Now.Date,
                    FMDate = DateTime.Now.Date,
                    FEmCode = labEmCode.Text,
                    FYear = labYear.Text,
                    FMonth = labMonth.Text,
                    FSelfSum = Convert.ToInt32(string.IsNullOrEmpty(txtSelfSum.Text) ? "0" : txtSelfSum.Text),
                    FRepeSum = Convert.ToInt32(string.IsNullOrEmpty(txtReGradeSum.Text) ? "0" : txtReGradeSum.Text),
                    FExamineSum = Convert.ToDecimal(string.IsNullOrEmpty(txtTotalSum.Text) ? "0" : txtTotalSum.Text),
                };

                //*验证自评总分是否超出范围*
                if (exResult.FSelfSum > 100 || exResult.FSelfSum <= 0) {
                    mag = "自评总分超出正常范围，请重新填写后再提交。";
                    Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                    return;
                }
                object _result = menuAction.InsertExamResult(exResult, out mag);
                if (_result == null) {
                    Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                    return;
                }
                //*新增明细表*
                try {
                    int rowCount = GridView1.Rows.Count;
                    int _fid = Convert.ToInt32(_result);
                    if (rowCount > 0) {
                        for (int i = 0; i < rowCount; i++) {
                            TextBox text = GridView1.Rows[i].FindControl("TxtSelfGrade") as TextBox;
                            detail = null;
                            detail = new Module.ExamineDetail() {
                                FEmResultID = Convert.ToInt32(_fid),
                                FProject_id = i + 1,
                                //FSelfGrade = Convert.ToInt32(string.IsNullOrEmpty(text.Text) ? "0" : text.Text),
                            };
                            try {
                                detail.FSelfGrade = Convert.ToInt32(string.IsNullOrEmpty(text.Text) ? "0" : text.Text);
                            }
                            catch {
                                detail.FSelfGrade = 0;
                            }
                            int x = (int)menuAction.InsertExamDetails(detail, out mag);
                            if (x != 1) {
                                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                                return;
                            }
                        }
                    }
                }
                catch (Exception ex) {
                    mag = ex.Message;
                    Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                    return;
                }
                //*过程已通过*
                success = true;
            }
            //更新状态
            else {
                if (this.fid > 0) {
                    //*复评状态下，绩效评语为必填*
                    if (status >= 1 && string.IsNullOrEmpty(txtExmMessage.Text) && txtExmMessage.Visible) {
                        mag = "绩效评语为必填项，请重新填写后再提交。";
                        Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                        return;
                    }

                    //*结果表*
                    exResult = new Module.ExamineResult() {
                        FMDate = DateTime.Now.Date,
                        FSelfSum = Convert.ToInt32(string.IsNullOrEmpty(txtSelfSum.Text) ? "0" : txtSelfSum.Text),
                        FRepeSum = Convert.ToInt32(string.IsNullOrEmpty(txtReGradeSum.Text) ? "0" : txtReGradeSum.Text),
                        FExamineSum = Convert.ToDecimal(string.IsNullOrEmpty(txtTotalSum.Text) ? "0" : txtTotalSum.Text),
                        FExamMessage = string.IsNullOrEmpty(txtExmMessage.Text) ? "" : txtExmMessage.Text,
                        FStatus = this.status.ToString(),
                        FID = this.fid,
                    };

                    //*复评分，状态=3*
                    if (Convert.ToBoolean(Session["HiddInform"])) {
                        exResult.FStatus = "3";
                    }
                    //*返回自评，状态=3*
                    if (this.status == 2) {
                        exResult.FStatus = "3";
                    }

                    //*验证复评总分是否超出范围*
                    if (exResult.FRepeSum > 100 || exResult.FRepeSum < 0) {
                        mag = "复评总分超出正常范围，请重新填写后再提交。";
                        Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                        return;
                    }
                    //*计算绩效系数*
                    if (exResult.FExamineSum >= 95 && exResult.FExamineSum <= 100) {
                        exResult.FCoefficient = 1.2M;
                    }
                    else if (exResult.FExamineSum >= 90 && exResult.FExamineSum < 95) {
                        exResult.FCoefficient = 1.1M;
                    }
                    else if (exResult.FExamineSum >= 80 && exResult.FExamineSum < 90) {
                        exResult.FCoefficient = 1.0M;
                    }
                    else if (exResult.FExamineSum >= 70 && exResult.FExamineSum < 80) {
                        exResult.FCoefficient = 0.8M;
                    }
                    else if (exResult.FExamineSum >= 60 && exResult.FExamineSum < 70) {
                        exResult.FCoefficient = 0.5M;
                    }
                    else {
                        exResult.FCoefficient = 0;
                    }

                    int result = (int)menuAction.UpdateExamResult(exResult, out mag);
                    if (result != 1) {
                        Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                        return;
                    }
                    //*明细表*
                    try {
                        int rowCount = GridView1.Rows.Count;
                        if (rowCount > 0) {
                            for (int i = 0; i < rowCount; i++) {
                                //*待复评分*
                                if (status >= 1) {
                                    detail = null;
                                    detail = new Module.ExamineDetail() {
                                        FEmResultID = Convert.ToInt32(this.fid),
                                        FProject_id = i + 1,
                                    };

                                    //*代评分用户 FProject_id改为0*
                                    if (Convert.ToInt32(Session["MyReview_isDaiGrade"]) == 3) {
                                        detail.FProject_id = 0;
                                    }

                                    //*排除 返回自评 的状态，否则复评分会被清零*
                                    if (status != 2) {
                                        try {
                                            detail.FRepeGrade = Convert.ToInt32(GridView1.Rows[i].Cells[9].Text);
                                        }
                                        catch {
                                            TextBox ExGrade = GridView1.Rows[i].FindControl("TxtExGrade") as TextBox;
                                            detail.FRepeGrade = Convert.ToInt32(string.IsNullOrEmpty(ExGrade.Text) ? "0" : ExGrade.Text);
                                        }
                                    }
                                    else {
                                        exmDetails = Session["exmGrade"] as List<Module.ExamineDetail>;
                                        foreach (Module.ExamineDetail item in exmDetails) {
                                            if (detail.FProject_id.Equals(item.FProject_id)) {
                                                detail.FRepeGrade = item.FRepeGrade;
                                            }
                                        }
                                    }

                                    try {
                                        detail.FSelfGrade = Convert.ToInt32(GridView1.Rows[i].Cells[8].Text);
                                    }
                                    catch {
                                        TextBox SelfGrade = GridView1.Rows[i].FindControl("TxtSelfGrade") as TextBox;
                                        detail.FSelfGrade = Convert.ToInt32(string.IsNullOrEmpty(SelfGrade.Text) ? "0" : SelfGrade.Text);
                                    }
                                }

                                int x = (int)menuAction.UpdateExamDetails(detail, out mag);
                                if (x != 1) {
                                    Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                                    return;
                                }
                            }
                        }
                    }
                    catch (Exception ex) {
                        Response.Write($@"<script language=javaScript>alert('{ex.Message}');</script>");
                        return;
                    }
                    //*过程已通过*
                    success = true;
                }
            }
            if (success) {
                mag = "数据保存成功，请返回主页。";
                if (Convert.ToInt32(Session["MyReview_isDaiGrade"]) == 3) {
                    Response.Write($"<script language='javascript'>alert('{mag}'); location='MyReview';</script>");
                }
                else {
                    Response.Write($"<script language='javascript'>alert('{mag}'); location='{hiddenUrl.Value}';</script>");
                }
            }
            else {
                mag = "数据保存失败，请联系管理员查找原因！";
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                return;
            }
        }
        /// <summary>
        /// 返回主页
        /// </summary>
        protected void lbtnBack_Click(object sender, EventArgs e) {
            Session["HiddInform"] = false;
            this.Dispose();
            Response.Redirect(!string.IsNullOrEmpty(hiddenUrl.Value) ? hiddenUrl.Value : "MenuView.aspx");
        }

    }
}