<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PositionInfo.aspx.cs" Inherits="PerformanceWebApp.View.PositionInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>职位查询表</title>
    <style type="text/css">
        text {
            text-align:center;
            font-size:x-large;
            color:plum;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" style="width:600px;height:800px">
        <div style="text-align:right">
            <a href="#" onclick="javascript:; window.close()">关闭</a>
        </div>
        <div style="text-align:center;font-family:'Microsoft YaHei';font-weight:bold">
            <strong >
                职位查询表
            </strong>
        </div>
        <div style="margin-left:100px;margin-bottom:10px;margin-top:10px">
            <asp:TextBox ID="txtQuery" runat="server"></asp:TextBox>
            <asp:Button ID="btnQuery" runat="server" Text="查  询" OnClick="btnQuery_Click"/>
        </div>
        <div style="margin-left:20px;">
            <asp:Label runat="server" BackColor="#666666" Width="560px" Height="1px"></asp:Label>
        </div>
        <div style="margin:10px 10px 5px 100px;font-family:'Microsoft YaHei';">
            <asp:GridView runat="server" ID="GridView1" AllowPaging="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#336666" 
                BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal" Height="600px" Width="400px" OnPageIndexChanging="GridView1_PageIndexChanging"
                OnRowCommand="GridView1_RowCommand" >
                <Columns>
                    <asp:ButtonField ButtonType="Button" Text="选择" CommandName="btnSelected">
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:ButtonField>
                    <asp:BoundField DataField="FID" HeaderText="FID">
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FFCName" HeaderText="职位">
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                </Columns>
                <FooterStyle BackColor="White" ForeColor="#333333" />
                <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                <PagerSettings PageButtonCount="20" />
                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="White" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                <SortedAscendingHeaderStyle BackColor="#487575" />
                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                <SortedDescendingHeaderStyle BackColor="#275353" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
