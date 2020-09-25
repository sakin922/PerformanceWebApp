using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;


namespace PerformanceWebApp.View {
    [Description("员工信息明细")]
    public partial class EmployeInfomation : System.Web.UI.Page {
        private Module.Employee employee_nom = null;

        protected void Page_Load(object sender, EventArgs e) {
            txtNewPwd.BorderColor = txtDaiGradeCode.BorderColor = txtDaiGradeName.BorderColor = txtEmCode.BorderColor = txtEmName.BorderColor = System.Drawing.Color.Black;
            if (!IsPostBack) {
                string _status = string.Empty;
                try {
                    _status = Session["EmployeInfomation_Status"].ToString();
                }
                catch {
                    _status = string.Empty;
                }
                //**新增状态**
                if (_status == "1" || string.IsNullOrEmpty(_status)) {
                    //*绑定状态下拉框数据*
                    ddlStatus.DataBind();
                    labCreateDate.Text = DateTime.Now.ToShortDateString();
                    labModiDate.Text = DateTime.Now.ToShortDateString();

                    //*标识单据：Treu=新增，False=修改*
                    Session["Employee_New"] = true; 
                }
                //**修改状态**
                if (_status == "2") {
                    if (Session["Empmanager_employee"] != null) {
                        employee_nom = Session["Empmanager_employee"] as Module.Employee;
                        this.ModelsMapToPage(employee_nom);
                        Session["Employee_New"] = false;
                    }
                }
                //**子页面回传状态**
                if (_status == "3") {
                    if (Session["Empmanager_employee"] != null) {
                        employee_nom = Session["Empmanager_employee"] as Module.Employee;
                        this.ModelsMapToPage(employee_nom);
                    }
                }
            }
            //**处理控件状态**
            this.PageControlHandle();
        }
        /// <summary>
        /// 实体类数据映射到页面
        /// </summary>
        private void ModelsMapToPage(Module.Employee employee) {
            //*绑定状态下拉框数据*
            ddlStatus.DataBind();
            if (ddlStatus.Items.Count > 0) {
                foreach (ListItem item in ddlStatus.Items) {
                    if (item.Text == employee.FStatus) {
                        item.Selected = true;
                    }
                }
            }
            //*是否代评分用户*
            if (!employee.FIsDaiGrader) {
                rbtDaiGradeNo.Checked = true;
            }
            else {
                rbtDaiGradeYes.Checked = true;
            }
            //*复评分用户*
            if (!employee.FIsDirector) {
                rbtReGraderNo.Checked = true;
            }
            else {
                rbtReGraderYes.Checked = true;
            }
            //*密码*
            if (!string.IsNullOrEmpty(employee.FNewPwd)) {
                chkPassword.Checked = true;
                txtNewPwd.Text = employee.FNewPwd;
            }
            hdfFID.Value = employee.FID;
            txtDaiGradeCode.Text = employee.FDaiPGradeID;
            txtDaiGradeName.Text = employee.FDaiPGrade;
            txtEmCode.Text = employee.FEmCode;
            txtEmName.Text = employee.FEmName;
            txtReGradeID.Text = employee.FExamerID;
            txtReGradeName.Text = employee.FExamerName;
            hdfDepID.Value = employee.FDepartmentID;
            hdfPosID.Value = employee.FPositionID;
            txtDepartment.Text = employee.FDepartment;
            txtPosition.Text = employee.FPosition;
            labCreateDate.Text = employee.FCDate.ToShortDateString();
            labModiDate.Text = employee.FMDate.ToShortDateString();
            txtOldPwd.Text = employee.FPassword;
        }
        /// <summary>
        /// 页面数据映射到实体类
        /// </summary>
        private void PageMapToModels(Module.Employee employee) {
            employee = null;
            employee = new Module.Employee();
            try {
                employee.FID = hdfFID.Value;
                employee.FEmCode = txtEmCode.Text;
                employee.FEmName = txtEmName.Text;
                employee.FExamerID = txtReGradeID.Text;
                employee.FExamerName = txtReGradeName.Text;
                employee.FDepartment = txtDepartment.Text;
                employee.FDepartmentID = hdfDepID.Value;
                employee.FPosition = txtPosition.Text;
                employee.FPositionID = hdfPosID.Value;
                employee.FCDate = Convert.ToDateTime(labCreateDate.Text);
                employee.FMDate = Convert.ToDateTime(labModiDate.Text);
                employee.FStatus = ddlStatus.SelectedItem.Text;
                employee.FPassword = txtOldPwd.Text;
                employee.FNewPwd = txtNewPwd.Text;
                //*代评分者*
                employee.FDaiPGradeID = txtDaiGradeCode.Text;
                employee.FDaiPGrade = txtDaiGradeName.Text;
                //*是否代评分用户*
                employee.FIsDaiGrader = rbtDaiGradeYes.Checked ? true : false;
                //*是否复评用户*
                employee.FIsDirector = rbtReGraderYes.Checked ? true : false;
                Session["Empmanager_employee"] = employee;
            }
            catch (Exception ex) {
                Response.Write($"<script> alert('数据转换出错：{ex.Message}');</script>");
                employee = null;
            }
        }
        /// <summary>
        /// 员工信息更新
        /// </summary>
        private Module.Employee UpdatePageToModel(out string mag) {
            mag = string.Empty;
            try {
                Module.Employee employee = new Module.Employee {
                    FID = hdfFID.Value,
                    FEmCode = txtEmCode.Text,
                    FEmName = txtEmName.Text,
                    FExamerID = txtReGradeID.Text,
                    FExamerName = txtReGradeName.Text,
                    FDepartmentID = hdfDepID.Value,
                    FDepartment = txtDepartment.Text,
                    FPositionID = hdfPosID.Value,
                    FPosition = txtPosition.Text,
                    FStatus = ddlStatus.SelectedValue,
                    FIsDaiGrader = rbtDaiGradeYes.Checked,
                    FIsDirector = rbtReGraderYes.Checked,
                    FCDate = Convert.ToDateTime(labCreateDate.Text).Date,
                    FMDate = DateTime.Now.Date,
                };
                //*获取新密码*
                if (chkPassword.Checked) {
                    employee.FPassword = txtNewPwd.Text;
                }
                else {
                    employee.FPassword = txtOldPwd.Text;
                }
                //*获取代评分用户*
                if (employee.FIsDaiGrader) {
                    employee.FDaiPGradeID = txtDaiGradeCode.Text;
                    employee.FDaiPGrade = txtDaiGradeName.Text;
                }
                return employee;
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 页面上的控件处理
        /// </summary>
        private void PageControlHandle() {
            //**代评分**
            if (ddlStatus.SelectedItem.Value == "3") {
                btnDaiGradeQuery.Enabled = true;
            }
            else {
                btnDaiGradeQuery.Enabled = false;
                txtDaiGradeCode.Text = txtDaiGradeName.Text = string.Empty;
            }

            //**修改密码**
            if (chkPassword.Checked) {
                txtNewPwd.ReadOnly = false;
            }
            else {
                txtNewPwd.ReadOnly = true;
                txtNewPwd.Text = string.Empty;
            }

            //**保存**
            if (Session["Employee_New"] == null) {
                btnSave.Enabled = false;
            }
            else {
                btnSave.Enabled = true;
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e) {
            string mag = string.Empty;
            employee_nom = null;
            bool success = true;
            
            //**获取当前数据状态：新增/修改**
            bool isNew = Convert.ToBoolean(Session["Employee_New"]);

            //**校验【保存】时数据输入是否有效**
            if (success) {
                if (!this.ValidateSaveInput(isNew, out mag)) {
                    success = false;
                }
            }
            //**加载页面数据**
            if (success) {
                employee_nom = this.UpdatePageToModel(out mag);
                if (employee_nom == null) {
                    success = false;
                }
            }
            //**数据处理**
            if (success) {
                Controller.EmpManageAction action = new Controller.EmpManageAction();
                //**新增员工信息**
                if (isNew) {
                    if (!action.InsertEmployeeDetail(out mag, employee_nom)) {
                        success=false;
                    }
                }
                //**更新员工信息表**
                else {
                    if (!action.UpdateEmployeeDetail(employee_nom, out mag)) {
                        success = false;
                    }
                }
                if (success) {
                    //*检验是否代评分用户--返回FCancellation状态*
                    bool result = action.ValidateNewDaiGrade(out mag, out int status, new string[] { txtEmCode.Text });
                    //*异常*
                    if (!result && status == -1) {
                        success = false;
                    }
                    if (success) {
                        //**状态 = 代评分**
                        if (employee_nom.FStatus == "3") {
                            //*新增代评分表记录*
                            if (!result && status == 0) {
                                if (!action.InsertDaiGradeRecord(out mag, new string[] { txtReGradeID.Text, txtEmCode.Text, txtDaiGradeCode.Text })) {
                                    success = false;
                                }
                            }
                            //*修改代评分记录*
                            if (result) {
                                if (!action.UpdateDaiGradeRecord(out mag, new string[] { txtReGradeID.Text, txtDaiGradeCode.Text, txtEmCode.Text })) {
                                    success = false;
                                }
                            }
                        }
                        //**状态 != 代评分 -- 数据状态改为失效**
                        else {
                            if (result && status != 1) {
                                if (!action.UpdateDaiGradeRecordStatus(out mag, new string[] { txtEmCode.Text })) {
                                    success = false;
                                }
                            }
                        }
                    }
                }
            }
            //**执行结果**
            if (!success) {
                Response.Write($@"<script>alert('{mag}');</script>");
                return;
            }
            else {
                mag += @"数据更新成功。";
                //*清空待修改的数据 Session*
                Session["Empmanager_employee"] = null;
                Session["EmployeInfomation_Status"] = null;
                if (isNew) {
                    //*单据标识改为 修改*
                    Response.Write($@"<script>alert('{mag}'),location='EmployeeManager.aspx';</script>");
                }
                else {
                    Response.Write($@"<script>alert('{mag}');</script>");
                }
            }
        }
        /// <summary>
        /// 返回员工信息管理
        /// </summary>
        protected void lbtnBackMainPage_Click(object sender, EventArgs e) {
            //*清空待修改的数据 Session*
            Session["Empmanager_employee"] = null;
            Session["EmployeInfomation_Status"] = null;
            Session["Employee_New"] = null;
            Response.Redirect("EmployeeManager.aspx");
        }
        /// <summary>
        /// 复评分人查询
        /// </summary>
        protected void btnReGradeQuery_Click(object sender, EventArgs e) {
            Session["EmployeInfomation_Status"] = 3;
            this.PageMapToModels(this.employee_nom);
            this.Response.Write(@"<script> window.open('RepetGrader.aspx', 'RepetGrader', 'top=100,left=600,width=1000px,height=800px,resizable=no,location=no') </script>");
        }
        /// <summary>
        /// 部门查询
        /// </summary>
        protected void btnDepQuery_Click(object sender, EventArgs e) {
            Session["EmployeInfomation_Status"] = 3;
            this.PageMapToModels(this.employee_nom);
            this.Response.Write(@"<script> window.open('DepartmentInfo.aspx', 'DepartmentInfo', 'top=100,left=600,width=1000px,height=800px,resizable=no,location=no') </script>");
        }
        /// <summary>
        /// 职位查询
        /// </summary>
        protected void btnPosQuery_Click(object sender, EventArgs e) {
            Session["EmployeInfomation_Status"] = 3;
            this.PageMapToModels(this.employee_nom);
            this.Response.Write(@"<script> window.open('PositionInfo.aspx', 'PositionInfo', 'top=100,left=600,width=1000px,height=800px,resizable=no,location=no') </script>");
        }
        /// <summary>
        /// 代评分人查询
        /// </summary>
        protected void btnDaiGradeQuery_Click(object sender, EventArgs e) {
            Session["EmployeInfomation_Status"] = 3;
            this.PageMapToModels(this.employee_nom);
            this.Response.Write(@"<script> window.open('DaiGradeInfo.aspx', 'DaiGradeInfo', 'top=100,left=600,width=1000px,height=800px,resizable=no,location=no') </script>");
        }
        /// <summary>
        /// 校验【保存】时数据输入是否有效
        /// </summary>
        private bool ValidateSaveInput(bool isNew,out string mag) {
            mag = string.Empty;
            bool success = true;
            //*修改密码或者新增状态：新密码不能为空*
            if (chkPassword.Checked || isNew) {
                if (string.IsNullOrEmpty(txtNewPwd.Text)) {
                    success = false;
                    txtNewPwd.BorderColor = System.Drawing.Color.OrangeRed;
                    mag += @"修改密码或新增用户时，新密码不能为空。\n";
                }
            }
            //*选择代评分者*
            if (ddlStatus.SelectedValue == "3") {
                if (string.IsNullOrEmpty(txtDaiGradeCode.Text)) {
                    success = false;
                    txtDaiGradeCode.BorderColor = txtDaiGradeName.BorderColor = System.Drawing.Color.OrangeRed;
                    mag += @"选择代评分者信息输入有误，请检查。\n";
                }
            }
            //*员工编号、名称*
            if (string.IsNullOrEmpty(txtEmCode.Text) || string.IsNullOrEmpty(txtEmName.Text)) {
                success = false;
                txtEmCode.BorderColor = txtEmName.BorderColor = System.Drawing.Color.OrangeRed;
                mag += @"员工编号、名称是必填项，请确认。\n";
            }
            //*必填项验证*
            if (string.IsNullOrEmpty(txtReGradeID.Text) || string.IsNullOrEmpty(txtDepartment.Text) || string.IsNullOrEmpty(txtPosition.Text)) {
                success = false;
                mag += @"带标记的必填项不能为空。\n";
            }
            //**新增状态验证**
            if (isNew) {
                Controller.EmpManageAction action = new Controller.EmpManageAction();
                //*验证员工编号是否已经存在*
                object x = action.ExistsTheSameEmployeeCode(out string existsEmcode, txtEmCode.Text);
                if (x == null && !string.IsNullOrEmpty(existsEmcode)) {
                    success = false;
                    mag += (existsEmcode + "\n");
                }
                if (x != null) {
                    mag += $@"存在相同的员工编号【{x.ToString()}】";
                    success = false;
                }
            }
            //*是否代评分者与代评分用户冲突*
            if (ddlStatus.SelectedValue == "3" && rbtDaiGradeYes.Checked) {
                success = false;
                mag += @"【被代评分者】不能兼容【代评分用户】。 \n";
            }
            return success;
        }
    }
}