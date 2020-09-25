<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyReview.aspx.cs" Inherits="PerformanceWebApp.View.MyReview" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .lbtnBack {
            margin-left: 300px;
        }

        .headControl-margin {
            margin-left: 20px;
        }
        .txtQuery {
            margin-left:10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="width: 1000px; margin-left: 300px">
        <p style="background-color: lightskyblue; border-style: solid; border-width: 1px">
            <strong style="margin-left: 400px; font-size: x-large; font-weight: bold">复核评分管理</strong>
            <asp:LinkButton ID="lbtnBack" runat="server" OnClick="LinkButton1_Click" CssClass="lbtnBack">返回主页</asp:LinkButton>
        </p>
        <p>
            <asp:Label ID="Label3" runat="server" CssClass="auto-style4" Text="考核周期"></asp:Label>
            <asp:Label ID="labYear" runat="server" CssClass="auto-style4"></asp:Label>
            <asp:Label ID="Label1" runat="server" CssClass="auto-style4" Text="年"></asp:Label>
            <asp:Label ID="labMonth" runat="server" CssClass="auto-style4"></asp:Label>
            <asp:Label ID="Label2" runat="server" CssClass="auto-style4" Text="月"></asp:Label>
            <strong style="margin-left:10px;color:deepskyblue">|</strong>
            <asp:TextBox ID="txtQuery" runat="server" Width="100px" CssClass="txtQuery"></asp:TextBox>
            <asp:DropDownList ID="ddlBillStatus" runat="server" CssClass="headControl-margin">
                <asp:ListItem Value="-1">全 部</asp:ListItem>
                <asp:ListItem Value="0">未自评</asp:ListItem>
                <asp:ListItem Value="1">已自评</asp:ListItem>
                <asp:ListItem Value="2">修改自评分</asp:ListItem>
                <asp:ListItem Value="3">已复评</asp:ListItem>
                <asp:ListItem Value="4">已审核</asp:ListItem>
            </asp:DropDownList>
            <asp:Button ID="btnQuery" runat="server" Text="查  询" Width="80px" OnClick="btnQuery_Click" CssClass="headControl-margin" />
        </p>
        <hr />
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" PageSize="25" AutoGenerateColumns="False" Width="1000px" CellPadding="4"
            ForeColor="#333333" GridLines="Both" OnRowCommand="GridView1_RowCommand" OnPageIndexChanging="GridView1_PageIndexChanging">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="评分">
                    <ItemTemplate>
                        <asp:Button ID="btnGrade" runat="server" CommandName="btnGrade" Text="评分" />
                        <asp:Button ID="btnBacktoSelf" runat="server" CommandName="btnBacktoSelf" Enabled="False" Text="退回自评" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                </asp:TemplateField>
                <asp:BoundField DataField="FID" HeaderText="FID">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="序号">
                    <ItemTemplate>
                        <asp:Label runat="server" Text="<%# Container.DataItemIndex + 1 %>" Width="15px"> </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="10px" Wrap="False" />
                </asp:TemplateField>
                <asp:BoundField DataField="FEmCode" HeaderText="员工编号">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FEmName" HeaderText="员工名称">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FDpName" HeaderText="部门">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FSelfSum" HeaderText="自评总分">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FRepeSum" HeaderText="复评总分">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FExamineSum" HeaderText="总成绩">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FStatus_1" HeaderText="当前进度">
                    <ItemStyle HorizontalAlign="Center" Wrap="false" />
                </asp:BoundField>
                <asp:BoundField DataField="FIsDirector" HeaderText="主管级以上">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="fBillStatus" HeaderText="单据状态">
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="FEmpStatus" HeaderText="用户状态">
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
        <p>
            <asp:HiddenField ID="hiddenUrl" runat="server" />
            <asp:HiddenField ID="hiddEmName" runat="server" />
        </p>
    </form>
</body>
</html>
