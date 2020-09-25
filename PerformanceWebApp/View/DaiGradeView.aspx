<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DaiGradeView.aspx.cs" Inherits="PerformanceWebApp.View.DaiGradeView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .head-margin {
            margin-left: 20px;
        }

        .lbtnBack {
            margin-left: 300px;
        }

        .button {
            font-family: 'Microsoft YaHei';
            font-weight: bolder;
            margin-left: 20px;
            width: 75px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="margin-left: 450px; width: 800px">
        <p style="background-color: deepskyblue; width: 800px">
            <strong style="font-weight: bolder; font-size: larger; margin-left: 300px">代评分管理</strong>
            <asp:LinkButton ID="lbtnBack" runat="server" OnClick="LinkButton1_Click" CssClass="lbtnBack">返回主页</asp:LinkButton>
        </p>
        <p>
            <asp:Label ID="Label3" runat="server" CssClass="auto-style4" Text="考核周期"></asp:Label>
            <asp:Label ID="labYear" runat="server" CssClass="auto-style4"></asp:Label>
            <asp:Label ID="Label1" runat="server" CssClass="auto-style4" Text="年"></asp:Label>
            <asp:Label ID="labMonth" runat="server" CssClass="auto-style4"></asp:Label>
            <asp:Label ID="Label2" runat="server" CssClass="auto-style4" Text="月"></asp:Label>
            <asp:TextBox ID="txtQuery" runat="server" Width="120px" CssClass="head-margin"></asp:TextBox>
            <asp:Button ID="btnQuery" runat="server" CssClass="button" Text="查  询" OnClick="btnQuery_Click" />
            <asp:Button ID="btnCommit" runat="server" OnClienClick="return confirm(LinkButton1_Click)" OnClick="btnCommit_Click" Text="提  交" CssClass="button" />
            <asp:Button ID="btnModify" runat="server" Text="修  改" OnClick="btnModify_Click" CssClass="button" />
        </p>
        <hr />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333"
            GridLines="Both" Width="800px" RowHeaderColumn="FID">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:TemplateField HeaderText="序号">
                    <ItemTemplate>
                        <asp:Label runat="server" Text="<%# Container.DataItemIndex + 1 %>" Width="15px"> </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="10px" Wrap="False" />
                </asp:TemplateField>
                <asp:BoundField DataField="FID" HeaderText="FID">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FEmCode" HeaderText="员工编号">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FEmName" HeaderText="员工名称">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FDpName" HeaderText="部门">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FPosition" HeaderText="职务">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FStatus" HeaderText="单据状态">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FSelfSum" HeaderText="自评总分" NullDisplayText="0">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="自评总分">
                    <ItemTemplate>
                        <asp:TextBox ID="TxtDaiSelfGrade" runat="server" Width="60px" Wrap="False" TextMode="Number"></asp:TextBox>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
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
        <asp:HiddenField ID="hiddenUrl" runat="server" />
    </form>
</body>
</html>
