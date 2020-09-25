using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using Aspose.Cells;

namespace PerformanceWebApp.View {
    public partial class ExamineManager : System.Web.UI.Page {
        private Controller.ExamManageAction manageAction = new Controller.ExamManageAction();
        private bool isNewData = true;          //是否新增/修改

        protected void Page_Load(object sender, EventArgs e) {
            if (Session["isNewData"] != null) {
                isNewData = Convert.ToBoolean(Session["isNewData"]);
            }

            if (!IsPostBack) {
                labYear.Text = Session["ExYear"].ToString();
                labMonth.Text = Session["ExMonth"].ToString();
                ddlDepartment.DataBind();
                //*GridView数据绑定*
                if (!this.GridViewDataBind(isNewData, out string mag)) {
                    Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                }
                //*加载【统计考核人数】数据*
                GetPeopleCount();
                //*是否开启导出文件*
                btnExportToFile.Visible = !isNewData ? true : false;
            }
        }
        /// <summary>
        /// GridView数据绑定
        /// </summary>
        private bool GridViewDataBind(bool isNew, out string mag) {
            GridView1.Columns[2].Visible = true;
            GridView1.DataSource = null;
            if (isNew) {
                DataTable dt = manageAction.GetExamInformation(labYear.Text, labMonth.Text, txtQuery.Text, ddlDepartment.SelectedItem.Text, txtCurAttendDays.Text, out mag);
                if (dt != null) {
                    if (dt.Rows.Count > 0) {
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        for (int i = 0; i < dt.Rows.Count; i++) {
                            foreach (GridViewRow row in GridView1.Rows) {
                                //*处理未复核评分的用户*
                                if (dt.Rows[i]["FID"].ToString() == "0" && dt.Rows[i]["FID"].ToString() == row.Cells[2].Text) {
                                    CheckBox checkBox = row.FindControl("chkRow") as CheckBox;
                                    checkBox.Enabled = false;
                                    row.ForeColor = System.Drawing.Color.Red;
                                }

                                if (dt.Rows[i]["FEmCode"].ToString() == row.Cells[3].Text) {
                                    TextBox txtRemark = row.FindControl("txtRemark") as TextBox;
                                    txtRemark.Text = dt.Rows[i]["FExamMessage"].ToString();
                                }
                            }
                        }
                        GridView1.Columns[2].Visible = false;
                        return true;
                    }
                }
            }
            else {
                DataTable dt = manageAction.GetExamInfoFinished(labYear.Text, labMonth.Text, txtQuery.Text, ddlDepartment.SelectedItem.Text, out mag);
                if (dt != null) {
                    if (dt.Rows.Count > 0) {
                        #region GridView合计列
                        decimal baseExam = 0;
                        decimal resultExam = 0;
                        decimal flotExam = 0;
                        decimal queQin = 0;
                        decimal chiDao = 0;
                        decimal jiangFa = 0;
                        #endregion
                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                        for (int i = 0; i < dt.Rows.Count; i++) {
                            foreach (GridViewRow row in GridView1.Rows) {
                                if (dt.Rows[i]["FID"].ToString() == row.Cells[2].Text) {
                                    row.Cells[9].Text = dt.Rows[i]["FAcAttend"].ToString();
                                    row.Cells[10].Text = dt.Rows[i]["FAbsented"].ToString();
                                    row.Cells[11].Text = dt.Rows[i]["FAmount"].ToString();
                                    row.Cells[13].Text = dt.Rows[i]["FRemark"].ToString();
                                }
                            }
                            //*汇总列*
                            baseExam += Convert.ToDecimal(dt.Rows[i]["FBaseExamine"]);
                            resultExam += Convert.ToDecimal(dt.Rows[i]["FBaseResult"]);
                            flotExam += Convert.ToDecimal(dt.Rows[i]["FFloatAmount"]);
                            queQin += Convert.ToDecimal(dt.Rows[i]["FAcAttend"]);
                            chiDao += Convert.ToDecimal(dt.Rows[i]["FAbsented"]);
                            jiangFa += Convert.ToDecimal(dt.Rows[i]["FAmount"]);
                        }
                        //**绑定汇总行**
                        GridViewRow gvr = GridView1.FooterRow;
                        gvr.BackColor = System.Drawing.Color.Yellow;
                        gvr.Cells[3].Text = "合计";
                        gvr.Cells[6].Text = Math.Round(baseExam, 1).ToString();
                        gvr.Cells[8].Text = Math.Round(resultExam, 1).ToString();
                        gvr.Cells[12].Text = Math.Round(flotExam, 1).ToString();
                        gvr.Cells[9].Text = Math.Round(queQin, 1).ToString();
                        gvr.Cells[10].Text = Math.Round(chiDao, 1).ToString();
                        gvr.Cells[11].Text = Math.Round(jiangFa, 1).ToString();
                        GridView1.Columns[2].Visible = false;
                        return true;
                    }
                }
            }
            GridView1.DataBind();
            mag = "查询的数据为空。";
            return false;
        }
        /// <summary>
        /// 加载【统计考核人数】数据
        /// </summary>
        private void GetPeopleCount() {
            DataTable dtCount = manageAction.GetPeopleCount(labYear.Text, labMonth.Text, out string mag);
            if (dtCount == null) {
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                return;
            }
            labTotalGuys.Text = dtCount.Rows[0]["FTotal"].ToString();
            lbtnFinished.Text = dtCount.Rows[0]["FFinished"].ToString();
            lbtnUnfinished.Text = dtCount.Rows[0]["FUnfinished"].ToString();
        }
        /// 返回主页
        /// </summary>
        protected void lbtnBack_Click(object sender, EventArgs e) {
            Session["GetDepartmentSchedule"] = null;
            Session["HiddInform"] = false;
            this.Dispose();
            Response.Redirect("MenuView.aspx");
        }
        /// <summary>
        /// 查询
        /// </summary>
        protected void btnRefresh_Click(object sender, EventArgs e) {
            if (!this.GridViewDataBind(isNewData, out string mag)) {
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
            }
            ckbSelectallRow.Checked = false;
            //**加载【总统计考核人数】**
            GetPeopleCount();
            //**加载【部门统计考核人数】**
            if (ddlDepartment.SelectedValue == "0") {
                labDepCount.Text = lkbNoPer.Text = lkbNoRepet.Text = lkbNoSelf.Text = "0";
            }
            else {
                if (GridView1.Rows.Count > 0) {
                    object[] paras = new object[] { labYear.Text, labMonth.Text, ddlDepartment.SelectedItem.Text };
                    DataSet ds = manageAction.GetDepartmentSchedule(out mag, paras);
                    if (ds == null) {
                        Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                        return;
                    }
                    DataTable dt = ds.Tables[0];
                    labDepCount.Text = dt.Rows[0].ItemArray[0].ToString();
                    lkbNoPer.Text = dt.Rows[0].ItemArray[1].ToString();
                    lkbNoRepet.Text = dt.Rows[0].ItemArray[2].ToString();
                    lkbNoSelf.Text = dt.Rows[0].ItemArray[3].ToString();
                    Session["GetDepartmentSchedule"] = null;
                    Session["GetDepartmentSchedule"] = ds;
                }
            }
            btnSave.Enabled = false;
        }
        /// <summary>
        /// 行内CheckBox选中激发事件
        /// </summary>
        protected void chkRow_CheckedChanged(object sender, EventArgs e) {
            foreach (GridViewRow row in GridView1.Rows) {
                Control ctl = row.FindControl("chkRow");
                CheckBox check = ctl as CheckBox;
                TextBox txtAcAttend = row.FindControl("txtAcAttend") as TextBox;
                TextBox txtAbsented = row.FindControl("txtAbsented") as TextBox;
                TextBox txtFAmount = row.FindControl("txtFAmount") as TextBox;
                TextBox txtRemark = row.FindControl("txtRemark") as TextBox;
                string _acattend = row.Cells[9].Text;
                string _absented = row.Cells[10].Text;
                string _fAmount = row.Cells[11].Text;
                string _remark = row.Cells[13].Text;
                //选中状态--新增
                if (check.Checked && isNewData) {
                    txtAcAttend.ReadOnly = txtAbsented.ReadOnly = txtFAmount.ReadOnly = txtRemark.ReadOnly = false;
                    row.ForeColor = System.Drawing.Color.DarkGoldenrod;
                }
                //选中状态--修改
                if (check.Checked && !isNewData) {
                    row.ForeColor = System.Drawing.Color.DarkGoldenrod;
                    txtAcAttend.Text = row.Cells[9].Text;
                    txtAbsented.Text = row.Cells[10].Text;
                    txtFAmount.Text = row.Cells[11].Text;
                    txtRemark.Text = row.Cells[13].Text;
                    txtAcAttend.ReadOnly = txtAbsented.ReadOnly = txtFAmount.ReadOnly = txtRemark.ReadOnly = false;
                }
                //非选中状态--新增
                if (!check.Checked && isNewData) {
                    if (!txtAcAttend.ReadOnly) {
                        txtAcAttend.ReadOnly = txtAbsented.ReadOnly = txtFAmount.ReadOnly = txtRemark.ReadOnly = true;
                        txtAcAttend.Text = txtAbsented.Text = txtFAmount.Text = "0";
                        row.Cells[12].Text = "0";
                        row.BackColor = System.Drawing.Color.White; ;
                        row.ForeColor = System.Drawing.Color.Black; ;
                    }
                }
                //非选中状态--修改
                if (!check.Checked && !isNewData) {
                    row.Cells[9].Text = _acattend;
                    row.Cells[10].Text = _absented;
                    row.Cells[11].Text = _fAmount;
                    row.Cells[13].Text = _remark;
                    row.BackColor = System.Drawing.Color.White; ;
                    row.ForeColor = System.Drawing.Color.Black; ;
                }
            }
        }
        /// <summary>
        /// 【全选】触发事件
        /// </summary>
        protected void ckbSelectallRow_CheckedChanged(object sender, EventArgs e) {
            if (ckbSelectallRow.Checked) {
                foreach (GridViewRow row in GridView1.Rows) {
                    Control ctl = row.FindControl("chkRow");
                    CheckBox check = ctl as CheckBox;
                    check.Checked = true;
                }
                chkRow_CheckedChanged(null, null);
            }
            else {
                foreach (GridViewRow row in GridView1.Rows) {
                    Control ctl = row.FindControl("chkRow");
                    CheckBox check = ctl as CheckBox;
                    check.Checked = false;
                }
                chkRow_CheckedChanged(null, null);
            }
        }
        /// <summary>
        /// 计算绩效浮动奖金
        /// </summary>
        protected void btnFloatAmount_Click(object sender, EventArgs e) {
            bool isChecked = false;
            string mag = string.Empty;
            foreach (GridViewRow row in GridView1.Rows) {
                Control ctl = row.FindControl("chkRow");
                CheckBox check = ctl as CheckBox;
                if (check.Checked) {
                    TextBox txtMissDuty = row.FindControl("txtAcAttend") as TextBox;
                    TextBox txtAbsented = row.FindControl("txtAbsented") as TextBox;
                    TextBox txtAmount = row.FindControl("txtFAmount") as TextBox;
                    //**计算【绩效浮动奖金】**
                    try {
                        double baseReult = Convert.ToDouble(row.Cells[8].Text);         //基数结果
                        double reDuty = Convert.ToDouble(txtCurAttendDays.Text);        //应出勤天数
                        double missDuty = Convert.ToDouble(string.IsNullOrEmpty(txtMissDuty.Text) ? "0" : txtMissDuty.Text);           //缺勤勤天数
                        double amount = Convert.ToDouble(string.IsNullOrEmpty(txtAmount.Text) ? "0" : txtAmount.Text);               //奖罚金额
                        double absented = Convert.ToDouble(string.IsNullOrEmpty(txtAbsented.Text) ? "0" : txtAbsented.Text);           //迟到早退
                        decimal exAmount = CountExAmount(baseReult, reDuty, missDuty, amount, absented);
                        exAmount = Math.Round(exAmount, 2); //保留2位小数
                        row.Cells[12].Text = exAmount.ToString();
                        isChecked = true;
                    }
                    catch (Exception ex) {
                        mag = ex.Message;
                        btnSave.Enabled = false;
                        isChecked = false;
                    }
                }
                else {
                    row.Visible = false;
                }
            }
            if (!isChecked) {
                if (string.IsNullOrEmpty(mag)) {
                    mag = "请先勾选需要计算的行。";
                }
                Response.Write($@"<script language=javaScript>alert('【绩效浮动奖金】计算有误：{mag}');window.location.href('ExamineManager.aspx');</script>");
                btnSave.Enabled = false;
                return;
            }
            else {
                Response.Write($@"<script language=javaScript>alert('[计算完成]');</script>");
                btnSave.Enabled = true;
            }
        }
        /// <summary>
        /// 计算【绩效浮动奖金】
        /// </summary>
        /// <param name="baseReult">基数结果（元）</param>
        /// <param name="standDays">标准每月天数</param>
        /// <param name="missDuty">缺出勤（天）</param>
        /// <param name="amount">奖罚金额（元）</param>
        /// <param name="absented">迟到早退（元）</param>
        /// <returns></returns>
        private decimal CountExAmount(double baseReult, double standDays, double missDuty, double amount, double absented) {
            /***********************计算逻辑***************************************************************************
                 【绩效浮动奖金】 = A：(每日标准金额);B(实际当月金额);C(奖罚金额);D(迟到早退);E(绩效考核奖金（结果）)

                  A --======= 基数结果 / 21.75(标准天数)
                  B --======= 基数结果 - (A * 缺勤天数)
                  E --======= B + C - D
            *********************************************************************************************************/
            double exAmount = 0;        //*绩效浮动奖金*
            double dayAmount = 0;       //每日标准金额   
            double actMoAmount = 0;     //实际当月金额    
            dayAmount = baseReult / standDays;
            actMoAmount = baseReult - (dayAmount * missDuty);
            exAmount = actMoAmount + amount - absented;

            try {
                return Convert.ToDecimal(exAmount);
            }
            catch {
                return 0;
            }
        }
        /// <summary>
        /// 检验数据行中的文本框输入
        /// </summary>
        private bool ValidateTextBox(string txtControl, string txtBoxName, out string mag) {
            mag = string.Empty;
            foreach (GridViewRow row in GridView1.Rows) {
                Control ctl = row.FindControl("chkRow");
                CheckBox check = ctl as CheckBox;
                if (check.Checked) {
                    TextBox txtBox = row.FindControl(txtControl) as TextBox;
                    try {
                        txtBox.Text = Convert.ToDouble(txtBox.Text).ToString();
                    }
                    catch {
                        mag = $@"【{txtBoxName}】数值输入有误，请重新输入。";
                        txtBox.Focus();
                        txtBox.Text = "0";
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 保存
        /// </summary>
        protected void btnSave_Click(object sender, EventArgs e) {
            string mag = string.Empty;
            int count = 0;
            bool isChecked = false;

            foreach (GridViewRow row in GridView1.Rows) {
                Control ctl = row.FindControl("chkRow");
                CheckBox check = ctl as CheckBox;
                if (check.Checked) {
                    TextBox txtAcAttend = row.FindControl("txtAcAttend") as TextBox;
                    TextBox txtAbsented = row.FindControl("txtAbsented") as TextBox;
                    TextBox txtFAmount = row.FindControl("txtFAmount") as TextBox;
                    TextBox txtRemark = row.FindControl("txtRemark") as TextBox;
                    Module.ExamManager exam = new Module.ExamManager {
                        FCDate = DateTime.Now.Date,
                        FID = row.Cells[2].Text,
                        FEmCode = row.Cells[3].Text,
                        FYear = labYear.Text,
                        FMonth = labMonth.Text,
                        FBaseExamine = Convert.ToDouble(row.Cells[6].Text),
                        FCoefficient = Convert.ToDouble(row.Cells[7].Text),
                        FBaseResult = Convert.ToDouble(row.Cells[8].Text),
                        FFloatAmount = Convert.ToDecimal(row.Cells[12].Text),
                        FStatus = true,
                    };
                    exam.FAcAttend = Convert.ToDouble(string.IsNullOrEmpty(txtAcAttend.Text) ? "0" : txtAcAttend.Text);
                    exam.FAbsented = Convert.ToDouble(string.IsNullOrEmpty(txtAbsented.Text) ? "0" : txtAbsented.Text);
                    exam.FAmount = Convert.ToDouble(string.IsNullOrEmpty(txtFAmount.Text) ? "0" : txtFAmount.Text);
                    exam.FRemark = txtRemark.Text;
                    //**验证是否完成绩效计算**
                    if (exam.FBaseResult != 0 && exam.FFloatAmount == 0) {
                        string saveOk = string.Empty;
                        if (isChecked) {
                            saveOk = $@"已完成【{count}】条记录保存，";
                        }
                        mag = @"请先完成【绩效浮动奖金】计算。";
                        Response.Write($@"<script language=javaScript>alert('{saveOk}编号：【{exam.FEmCode}】数据保存失败，原因： {mag}');</script>");
                        btnRefresh_Click(null, null);
                        return;
                    }

                    //**新增状态*
                    if (isNewData) {
                        //*插入新记录*
                        object result = manageAction.InsertExamInformation(exam, out mag);
                        if (result == null) {
                            Response.Write($@"<script language=javaScript>alert('已完成更新{count}条记录，【{exam.FEmCode}】数据新增失败');</script>");
                            btnRefresh_Click(null, null);
                            return;
                        }
                        else {
                            //*更新【考核成绩结果】的单据状态为已审核*
                            result = null;
                            result = manageAction.UpdateExamResultApprove(exam.FEmCode, exam.FYear, exam.FMonth, out mag);
                            if (result == null) {
                                Response.Write($@"<script language=javaScript>alert('【绩效结果表】审核状态修改失败，原因： {mag}');</script>");
                                btnRefresh_Click(null, null);
                                return;
                            }
                        }
                        count += 1;
                        isChecked = true;
                    }
                    //**修改状态**
                    if (!isNewData) {
                        exam.FID = row.Cells[2].Text;
                        object result = manageAction.UpdateExamInformation(exam, out mag);
                        if (result == null) {
                            Response.Write($@"<script language=javaScript>alert('{count}条记录更新完成，【{exam.FEmCode}】更新失败。');</script>");
                            btnRefresh_Click(null, null);
                            return;
                        }
                        count += 1;
                        isChecked = true;
                    }
                }
            }
            if (!isChecked) {
                mag = $"请先选择要保存的行。";
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                return;
            }
            else {
                mag = $"数据更新成功，共更新【{count.ToString()}】条数据。";
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                btnRefresh_Click(null, null);
                btnSave.Enabled = false;
            }
        }
        /// <summary>
        /// 已完成
        /// </summary>
        protected void lbtnFinished_Click(object sender, EventArgs e) {
            Session["isNewData"] = false;
            this.Response.Redirect("ExamineManager.aspx");
        }
        /// <summary>
        /// 未完成
        /// </summary>
        protected void lbtnUnfinished_Click(object sender, EventArgs e) {
            Session["isNewData"] = true;
            this.Response.Redirect("ExamineManager.aspx");
        }
        /// <summary>
        /// 导出文件
        /// </summary>
        protected void btnExportToFile_Click(object sender, EventArgs e) {
            if (GridView1.Rows.Count > 0) {
                using (DataTable dt = new DataTable()) {
                    dt.Columns.Add("序号", typeof(string));
                    dt.Columns.Add("编码", typeof(string));
                    dt.Columns.Add("姓名", typeof(string));
                    dt.Columns.Add("部门", typeof(string));
                    dt.Columns.Add("原考核基数", typeof(decimal));
                    dt.Columns.Add("绩效系数", typeof(decimal));
                    dt.Columns.Add("基数结果", typeof(decimal));
                    dt.Columns.Add("缺勤天数", typeof(double));
                    dt.Columns.Add("迟到早退", typeof(double));
                    dt.Columns.Add("奖罚金额", typeof(decimal));
                    dt.Columns.Add("绩效浮动奖金", typeof(decimal));
                    dt.Columns.Add("备注", typeof(string));
                    DataRow dr = null;
                    for (int i = 0; i < GridView1.Rows.Count; i++) {
                        dr = dt.NewRow();
                        dr["序号"] = GridView1.Rows[i].Cells[2].Text;
                        dr["编码"] = GridView1.Rows[i].Cells[3].Text;
                        dr["姓名"] = GridView1.Rows[i].Cells[4].Text;
                        dr["部门"] = GridView1.Rows[i].Cells[5].Text;
                        dr["原考核基数"] = Convert.ToDecimal(GridView1.Rows[i].Cells[6].Text);
                        dr["绩效系数"] = Convert.ToDecimal(GridView1.Rows[i].Cells[7].Text);
                        dr["基数结果"] = Convert.ToDecimal(GridView1.Rows[i].Cells[8].Text);
                        dr["缺勤天数"] = Convert.ToDouble(GridView1.Rows[i].Cells[9].Text);
                        dr["迟到早退"] = Convert.ToDouble(GridView1.Rows[i].Cells[10].Text);
                        dr["奖罚金额"] = Convert.ToDecimal(GridView1.Rows[i].Cells[11].Text);
                        dr["绩效浮动奖金"] = Convert.ToDecimal(GridView1.Rows[i].Cells[12].Text);
                        dr["备注"] = GridView1.Rows[i].Cells[13].Text;
                        dt.Rows.Add(dr);
                        dr = null;
                    }
                    //**汇总行**
                    dr = dt.NewRow();
                    dr["编码"] = GridView1.FooterRow.Cells[3].Text;
                    dr["原考核基数"] = GridView1.FooterRow.Cells[6].Text;
                    dr["基数结果"] = GridView1.FooterRow.Cells[8].Text;
                    dr["绩效浮动奖金"] = GridView1.FooterRow.Cells[12].Text;
                    dr["缺勤天数"] = GridView1.FooterRow.Cells[9].Text;
                    dr["迟到早退"] = GridView1.FooterRow.Cells[10].Text;
                    dr["奖罚金额"] = GridView1.FooterRow.Cells[11].Text;
                    dt.Rows.Add(dr);

                    MemoryStream ms = this.DataTableToExcel(dt, out string mag);
                    if (ms == null) {
                        Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                        return;
                    }
                    byte[] byt = ms.ToArray();
                    Response.ClearContent();
                    Response.AppendHeader("Content-Disposition", "attachment;filename =" + HttpUtility.UrlEncode("测试.xls", Encoding.UTF8));
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.BinaryWrite(byt);
                    Response.Flush();
                    Response.End();
                }
            }
            else {
                Response.Write($@"<script language=javaScript>alert('没有可以导出的数据行。');</script>");
                return;
            }
        }
        /// <summary>
        /// 转换成DataTable导出Excel
        /// </summary>
        private MemoryStream DataTableToExcel(DataTable table, out string mag) {
            mag = string.Empty;
            try {
                Workbook workbook = new Workbook();//*工作簿*
                Worksheet worksheet = workbook.Worksheets[0];//*页*
                Cells cells = worksheet.Cells;//*单元格*                   
                int _colnums = table.Columns.Count;//*数据总列数*
                int _rows = table.Rows.Count;//*数据总行数*
                //**生成列头**
                for (int i = 0; i < _colnums; i++) {
                    cells[0, i].PutValue(table.Columns[i].ColumnName);
                }
                //**生成行数据**
                for (int i = 0; i < _rows; i++) {
                    for (int k = 0; k < _colnums; k++) {
                        cells[1 + i, k].PutValue(table.Rows[i][k]);//*添加数据*
                    }
                }
                worksheet.AutoFitColumns();
                return workbook.SaveToStream();
            }
            catch (Exception ex) {
                mag = ex.Message;
                return null;
            }
        }
        /// <summary>
        /// 未完成复评
        /// </summary>
        protected void lkbNoRepet_Click(object sender, EventArgs e) {
            if (!this.GridViewDataBind(isNewData, out string mag)) {
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                return;
            }
            //**校验是否有效查询**
            if (lkbNoRepet.Text == "0" || string.IsNullOrEmpty(ddlDepartment.SelectedItem.Text)) {
                mag = @"请先选择【部门】或 检查【未完成复评】是否 = 0";
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                return;
            }
            Session["ScheduleStatus"] = "2";
            this.Response.Write(@"<script language=javaScript> window.open('DepartmentSchedule.aspx', 'DepartmentSchedule', 'top=100px,left=500px,width=500px,height=600px,resizable=no,location=no') </script>");
        }
        /// <summary>
        /// 未完成自评
        /// </summary>
        protected void lkbNoSelf_Click(object sender, EventArgs e) {
            if (!this.GridViewDataBind(isNewData, out string mag)) {
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                return;
            }
            //**校验是否有效查询**
            if (lkbNoSelf.Text == "0" || string.IsNullOrEmpty(ddlDepartment.SelectedItem.Text)) {
                mag = @"请先选择【部门】或 检查【未完成自评】是否 = 0";
                Response.Write($@"<script language=javaScript>alert('{mag}');</script>");
                return;
            }
            Session["ScheduleStatus"] = "3";
            this.Response.Write(@"<script language=javaScript> window.open('DepartmentSchedule.aspx', 'DepartmentSchedule', 'top=100px,left=500px,width=500px,height=600px,resizable=no,location=no') </script>");
        }
        /// <summary>
        /// GridView数据绑定时激发事件
        /// </summary>
    }
}