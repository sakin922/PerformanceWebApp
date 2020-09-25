<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeInfomation.aspx.cs" Inherits="PerformanceWebApp.View.EmployeInfomation" MaintainScrollPositionOnPostback="True" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>h2 员工信息维护</title>
    <style type="text/css">
        .head-div {
            width: 780px;
            height: 80px;
            margin-left: 500px;
            margin-top: 50px;
            background-color: darkgreen;
            border: outset;
            padding: 10px 10px 10px 10px;
        }

        .table {
            font-size: medium;
            font-family: 'Microsoft YaHei';
            margin-left: 500px;
            margin-top: 30px;
            width: 800px;
            height: 500px;
            border: outset;
            padding: 10px 10px 10px 10px;
        }

        .table-td-lable {
            text-align: center;
            width: auto;
            font-weight: bold;
        }

        .table-td-textbox {
            text-align: left;
            width: auto
        }

        .table-ddl {
            width: 150px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="head-div">
            <strong style="font-size: x-large; font-weight: bold; margin-left: 10px; margin-right: 380px; color: yellow">员工信息维护</strong>
            <asp:LinkButton ID="lbtnBackMainPage" runat="server" Font-Bold="true" ForeColor="Red" OnClick="lbtnBackMainPage_Click">返回员工信息管理</asp:LinkButton>
        </div>
        <div>
        </div>
        <table class="table">
            <tr>
                <td class="table-td-lable">
                    <strong>员工编号</strong>
                </td>
                <td class="table-td-textbox">
                    <asp:TextBox runat="server" ID="txtEmCode" />
                    <asp:Label ID="Label1" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
                <td class="table-td-lable">
                    <strong>员工名称</strong>
                </td>
                <td class="table-td-textbox">
                    <asp:TextBox runat="server" ID="txtEmName" />
                    <asp:Label ID="Label2" runat="server" Text="*" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="table-td-lable">
                    <strong>复评人编码</strong>
                </td>
                <td class="table-td-textbox">
                    <asp:TextBox runat="server" ID="txtReGradeID" Text="" ReadOnly="true" ForeColor="Blue" />
                    <asp:Label ID="Label3" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    <asp:Button runat="server" Text="查询" ID="btnReGradeQuery" OnClick="btnReGradeQuery_Click" />
                </td>
                <td class="table-td-lable">
                    <strong>复评人名称</strong>
                </td>
                <td class="table-td-textbox">
                    <asp:TextBox runat="server" ID="txtReGradeName" ReadOnly="true" ForeColor="Blue" />
                </td>
            </tr>
            <tr>
                <td class="table-td-lable">
                    <strong>部门</strong>
                </td>
                <td class="table-td-textbox">
                    <asp:TextBox runat="server" ID="txtDepartment" Text="" ReadOnly="true" ForeColor="Blue"></asp:TextBox>
                    <asp:Label ID="Label4" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    <asp:Button runat="server" ID="btnDepQuery" Text="查询" OnClick="btnDepQuery_Click" />
                </td>

                <td class="table-td-lable">
                    <strong>职位</strong>
                </td>
                <td class="table-td-textbox">
                    <asp:TextBox runat="server" ID="txtPosition" Text="" ReadOnly="true" ForeColor="Blue"></asp:TextBox>
                    <asp:Label ID="Label5" runat="server" Text="*" ForeColor="Red"></asp:Label>
                    <asp:Button ID="btnPosQuery" runat="server" Text="查询" OnClick="btnPosQuery_Click" />
                </td>
            </tr>
            <tr>
                <td class="table-td-lable">
                    <strong>状态</strong>
                </td>
                <td class="table-td-textbox">
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="table-ddl" DataSourceID="EmStatus" DataTextField="FStatusName" DataValueField="FStatus" AutoPostBack="true" />
                    <asp:SqlDataSource ID="EmStatus" runat="server" ConnectionString="<%$ ConnectionStrings:PANASIANConnectionString %>"
                        SelectCommand="SELECT DISTINCT FStatus, CASE WHEN emp.FStatus = 0 THEN '正式' 
                        WHEN emp.FStatus = 1 THEN '试用' WHEN emp.FStatus = 2 THEN '离职' WHEN emp.FStatus = 3 THEN '代评分' WHEN emp.FStatus = 4 THEN '退休' 
                        WHEN emp.FStatus = 98 THEN '管理员' WHEN emp.FStatus = 99 THEN 'HR权限' ELSE '未知' END AS FStatusName FROM FY_Performance_Employee AS emp"></asp:SqlDataSource>
                </td>
                <td class="table-td-lable">
                    <strong>选择代评分者</strong>
                </td>
                <td class="table-td-textbox">
                    <asp:TextBox ID="txtDaiGradeCode" runat="server" ReadOnly="true" Width="70px" />
                    <asp:TextBox ID="txtDaiGradeName" runat="server" ReadOnly="true" Width="100px" />
                    <asp:Button ID="btnDaiGradeQuery" runat="server" Text="查询" OnClick="btnDaiGradeQuery_Click" />
                </td>
            </tr>
            <tr>
                <td class="table-td-lable">
                    <strong>修改密码</strong>
                </td>
                <td class="table-td-textbox">
                    <asp:CheckBox ID="chkPassword" runat="server" Text="启用" AutoPostBack="true" />
                </td>
                <td class="table-td-lable">
                    <strong>是否代评分用户</strong>
                </td>
                <td class="table-td-textbox">
                    <asp:RadioButton ID="rbtDaiGradeYes" runat="server" Text="是" GroupName="DaiGrade" AutoPostBack="true" />
                    <asp:RadioButton ID="rbtDaiGradeNo" runat="server" Text="否" GroupName="DaiGrade" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td class="table-td-lable">
                    <asp:Label ID="Label11" runat="server" Text="原密码"></asp:Label>
                </td>
                <td class="table-td-textbox">
                    <asp:TextBox ID="txtOldPwd" runat="server" TextMode="SingleLine" ReadOnly="true"></asp:TextBox>
                </td>
                <td class="table-td-lable">
                    <asp:Label ID="Label9" runat="server" Text="是否复评分用户"></asp:Label>
                </td>
                <td class="table-td-textbox">
                    <asp:RadioButton ID="rbtReGraderYes" runat="server" Text="是" GroupName="ReGrader" AutoPostBack="true" />
                    <asp:RadioButton ID="rbtReGraderNo" runat="server" Text="否" GroupName="ReGrader" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td class="table-td-lable">
                    <asp:Label ID="Label12" runat="server" Text="新密码"></asp:Label>
                </td>
                <td class="table-td-textbox">
                    <asp:TextBox ID="txtNewPwd" runat="server" TextMode="SingleLine" ReadOnly="true"></asp:TextBox>
                </td>
                <td class="table-td-lable"></td>
                <td class="table-td-textbox"></td>
            </tr>
            <tr>
                <td class="table-td-lable">
                    <strong>创建日期</strong>
                </td>
                <td class="table-td-textbox">
                    <asp:Label ID="labCreateDate" runat="server" Text="2020-08-31"></asp:Label>
                </td>
                <td class="table-td-lable">
                    <strong>修改日期</strong>
                </td>
                <td class="table-td-textbox">
                    <asp:Label ID="labModiDate" runat="server" Text="2020-08-31"></asp:Label>
                </td>
            </tr>
            <tr style="height: 70px">
                <td>
                    <asp:HiddenField runat="server" ID="hdfDepID" />
                </td>
                <td>
                    <asp:HiddenField runat="server" ID="hdfPosID" />
                </td>
                <td>
                    <asp:HiddenField runat="server" ID="hdfFID" />
                </td>
            </tr>
            <tr>
                <td class="table-td-lable"></td>
                <td class="table-td-lable"></td>
                <td class="table-td-lable">
                    <asp:Button ID="btnSave" runat="server" Text="保 存" OnClick="btnSave_Click" Font-Size="Medium" Font-Bold="true" />
                </td>
                <td class="table-td-lable">
                    <asp:Button ID="btnChongzhi" runat="server" Text="重 置" Font-Size="Medium" Font-Bold="true"  Enabled="false"/>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
