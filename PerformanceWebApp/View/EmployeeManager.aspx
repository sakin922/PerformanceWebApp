<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeManager.aspx.cs" Inherits="PerformanceWebApp.View.EmployeeManager" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server" style="text-align:center">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .lbtnBackMainPage {
            margin-left:700px;
            color:yellow;
        }
        .margin-div {
            margin-right:30px;
        }
        .btnCreateEmp {
            margin-left:30px;
        }
    </style>
</head>
<body style="width:1000px;height:900px;margin-left:350px">
    <form id="form1" runat="server">
        <div>
            <div style="background-color:royalblue;height:100px">
                <strong style="margin-left:20px; font-family:'Microsoft YaHei UI';font-weight:bold;font-size:larger;">员工信息管理</strong>
                <asp:LinkButton ID="lbtnBackMainPage" runat="server" OnClick="lbtnBackMainPage_Click" CssClass="lbtnBackMainPage">返回主页</asp:LinkButton>
            </div>
        </div>
        <div style="text-align:center;width:1000px; border-style:solid;border-width:1px; height:30px;">
           <asp:TextBox ID="txtQuery" runat="server"></asp:TextBox>
           <asp:Button ID="btnQuery" runat="server" Text="查  询" OnClick="btnQuery_Click" CssClass="margin-div"/>
            <strong>员工状态</strong>
            <asp:DropDownList ID="ddlStatus" runat="server" DataSourceID="EmpStatus" Height="18px" Width="83px" DataTextField="FStatusName" DataValueField="FStatusName" CssClass="margin-div">
            </asp:DropDownList>
            <strong>是否作废</strong>
            <asp:DropDownList ID="ddlIsCancel" runat="server" CssClass="margin-div">
                <asp:ListItem Selected="True" Value="0">否</asp:ListItem>
                <asp:ListItem Value="1">是</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnCreateEmp" runat="server" OnClick="btnCreateEmp_Click" Text="新增员工" Font-Size="Medium" ForeColor="Blue" Font-Bold="true" CssClass="btnCreateEmp"/>
            <asp:SqlDataSource ID="EmpStatus" runat="server" ConnectionString="<%$ ConnectionStrings:PANASIANConnectionString %>" 
                SelectCommand="SELECT - 1 AS FStatus, '全部' AS FStatusName UNION ALL SELECT DISTINCT FStatus, CASE WHEN emp.FStatus = 0 THEN '正式' 
                WHEN emp.FStatus = 1 THEN '试用' WHEN emp.FStatus = 2 THEN '离职' WHEN emp.FStatus = 3 THEN '代评分' WHEN emp.FStatus = 4 THEN '退休' 
                WHEN emp.FStatus = 98 THEN '管理员' WHEN emp.FStatus = 99 THEN 'HR权限' ELSE '未知' END AS FStatusName FROM FY_Performance_Employee AS emp">
            </asp:SqlDataSource>
        </div>
        <div style="margin-top:15px;">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="Both" Width="1000px" 
                AllowPaging="True" OnPageIndexChanging="GridView1_PageIndexChanging" OnSelectedIndexChanging="GridView1_SelectedIndexChanging" PageSize="20" 
                RowHeaderColumn="FID">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True">
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:CommandField>
                    <asp:BoundField DataField="FID" HeaderText="序号">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FEmCode" HeaderText="员工编号">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FEmName" HeaderText="员工名称">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FExamerID" HeaderText="评分人编号">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FExamerName" HeaderText="评分人名称">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FCDate" HeaderText="创建日期" DataFormatString="{0:d}">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FDepartment" HeaderText="部门">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FPosition" HeaderText="职能">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FDaiPGrade" HeaderText="代评分者">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FStatus" HeaderText="员工状态">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FCancellation" HeaderText="作废状态">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
