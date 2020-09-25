<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DepartmentSchedule.aspx.cs" Inherits="PerformanceWebApp.View.DepartmentSchedule" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>部门考核进度监控</title>
    <style type="text/css">
        .labTitle {
            margin-left: 10px;
            font-size: large;
            font-weight: bold;
            color: darkgreen;
        }

        .lkbClose {
            margin-left: 220px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="margin-left: 50px; width: 400px">
        <div style="background-color: darkkhaki">
            <asp:Label ID="labTitle" runat="server" Text="" CssClass="labTitle"></asp:Label>
            <asp:LinkButton ID="lkbClose" runat="server" Text="关闭" CssClass="lkbClose" OnClick="lkbClose_Click"></asp:LinkButton>
        </div>
        <hr />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="Solid" 
            BorderWidth="1px" CellPadding="3" CellSpacing="2" GridLines="Both" Width="400px">
            <Columns>
<%--                <asp:BoundField DataField="FID" ReadOnly="True" HeaderText="序号">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>--%>
                <asp:TemplateField HeaderText="序号">
                    <ItemTemplate>
                        <asp:Label runat="server" Text="<%# Container.DataItemIndex + 1 %>" > </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:TemplateField>
                <asp:BoundField DataField="FEmCode" ReadOnly="True" HeaderText="员工编码">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="FEmName" ReadOnly="True" HeaderText="员工姓名">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="FExamer" ReadOnly="True" HeaderText="复评者">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="FStatus" ReadOnly="True" HeaderText="状态">
                    <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:BoundField>
            </Columns>
            <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
            <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
            <PagerStyle ForeColor="#8C4510" HorizontalAlign="Center" />
            <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#FFF1D4" />
            <SortedAscendingHeaderStyle BackColor="#B95C30" />
            <SortedDescendingCellStyle BackColor="#F1E5CE" />
            <SortedDescendingHeaderStyle BackColor="#93451F" />
        </asp:GridView>
    </form>
</body>
</html>
