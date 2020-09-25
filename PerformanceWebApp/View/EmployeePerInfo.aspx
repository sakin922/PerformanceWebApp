<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeePerInfo.aspx.cs" Inherits="PerformanceWebApp.View.EmployeePerInfo" MaintainScrollPositionOnPostback="True" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .auto-style2 {
            height: 356px;
            width: 1138px;
        }

        .auto-style3 {
            margin-left: 933px;
            height: 18px;
        }

        .auto-style4 {
            font-family: 微软雅黑;
            font-size: x-large;
        }

        .auto-style5 {
            font-family: 微软雅黑;
            font-size: medium;
        }

        .auto-style6 {
            margin-left: 0px;
            font-size: small;
        }

        .auto-style7 {
            font-size: small;
        }

        .auto-style9 {
            width: 1114px;
            margin-left: 7px;
        }

        .auto-style10 {
            margin-left: 720px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="auto-style2">
        <p class="auto-style3">
            <asp:LinkButton ID="lbtnBack" runat="server" OnClick="lbtnBack_Click">返回主页</asp:LinkButton>
            <br />
        </p>
        <span class="auto-style4"><strong>员工绩效原考核基数</strong></span><p class="auto-style9">

            <asp:Label ID="Label3" runat="server" CssClass="auto-style5" Text="考核周期"></asp:Label>
            <asp:Label ID="labYear" runat="server" CssClass="auto-style5"></asp:Label>
            <asp:Label ID="Label1" runat="server" CssClass="auto-style5" Text="年"></asp:Label>
            <asp:Label ID="labMonth" runat="server" CssClass="auto-style5"></asp:Label>
            <asp:Label ID="Label2" runat="server" CssClass="auto-style5" Text="月"></asp:Label>
            &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;        
            <asp:Label ID="Label4" runat="server" Text="员工" CssClass="auto-style5"></asp:Label>
            <asp:TextBox ID="txtQuery" runat="server" Width="115px" CssClass="auto-style6"></asp:TextBox>
            <asp:Button ID="btnQuery" runat="server" Text="查  询" CssClass="auto-style7" Height="21px" Width="69px" OnClick="btnQuery_Click" />
        </p>
        <p class="auto-style10">
            <asp:Button ID="btnSave" runat="server" Font-Names="黑体" OnClienClick="return confirm(LinkButton1_Click)" Text="保  存" OnClick="btnSave_Click" Enabled="False" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnRebuil" runat="server" Font-Names="黑体" OnClienClick="return confirm(LinkButton1_Click)" Text="重  置" OnClick="btnRebuil_Click" />
        </p>

        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Height="16px" Width="1127px" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanging="GridView1_SelectedIndexChanging" ShowFooter="True">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:CommandField ButtonType="Button" ShowSelectButton="True">
                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:CommandField>
                <asp:BoundField DataField="FID" HeaderText="序号" NullDisplayText="-1">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FEmCode" HeaderText="员工编号">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FEmName" HeaderText="员工名称">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="考核基数">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("FBaseExamine") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:TextBox ID="TxtBaseNum" runat="server" Width="65px" Wrap="False" ReadOnly="True"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TxtBaseNum" Display="Dynamic" ErrorMessage="请输入正确的值" ForeColor="Red" SetFocusOnError="True" ValidationExpression="^[0-9]+(.[0-9]{2})?$" ValidationGroup="ValDecimals_2"></asp:RegularExpressionValidator>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:TemplateField>
                <asp:BoundField DataField="FCoefficient" HeaderText="考核系数">
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FDepartment" HeaderText="部门">
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="FPosition" HeaderText="职务">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                </asp:BoundField>
                <asp:BoundField DataField="FRank" HeaderText="员工等级" NullDisplayText="0.00">
                    <ItemStyle HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
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
    </form>
</body>
</html>
