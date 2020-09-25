<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamineManager.aspx.cs" Inherits="PerformanceWebApp.View.ExamineManager" MaintainScrollPositionOnPostback="True" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .auto-style4 {
            height: 26px;
            margin-right: 20px;
        }

        .auto-style5 {
            font-family: 微软雅黑;
            font-size: x-large;
            margin-left: 200px;
            width: 1200px;
        }

        .auto-style7 {
            font-size: large;
            margin-right: 20px;
        }

        .auto-style8 {
            height: 26px;
            width: 1161px;
        }

        .auto-style9 {
            font-family: 微软雅黑;
            font-size: large;
            margin-left: 200px;
        }

        .auto-style12 {
            margin-left: 20px;
        }

        .auto-style13 {
            font-size: small;
            font-weight: bold;
            font-family: 微软雅黑;
            margin-left: 40px;
        }

        .auto-style14 {
            margin-left: 200px;
        }
        .FooterStyle {
            text-align:center;            
        }
    </style>
</head>
<body >
    <form id="form1" runat="server" class="auto-style8">
        <p class="auto-style5">
            <strong style="margin-right: 900px">绩效奖金管理</strong>
            <asp:LinkButton ID="lbtnBack" runat="server" OnClick="lbtnBack_Click">返回主页</asp:LinkButton>
        </p>
        <p class="auto-style9">
            <asp:Label ID="Label3" runat="server" CssClass="auto-style4" Text="考核周期"></asp:Label>
            <asp:Label ID="labYear" runat="server" CssClass="auto-style4"></asp:Label>
            <asp:Label ID="Label1" runat="server" CssClass="auto-style4" Text="年"></asp:Label>
            <asp:Label ID="labMonth" runat="server" CssClass="auto-style4"></asp:Label>
            <asp:Label ID="Label2" runat="server" CssClass="auto-style4" Text="月"></asp:Label>
            <asp:TextBox ID="txtCurAttendDays" runat="server" CssClass="auto-style7" Width="49px" ReadOnly="True" Visible="False">21.75</asp:TextBox>
            总人数：<asp:Label ID="labTotalGuys" runat="server" CssClass="auto-style4" ForeColor="#009933">0</asp:Label>
            已完成：<asp:LinkButton ID="lbtnFinished" runat="server" OnClick="lbtnFinished_Click" CssClass="auto-style4">0</asp:LinkButton>
            未完成：<asp:LinkButton ID="lbtnUnfinished" runat="server" OnClick="lbtnUnfinished_Click">0</asp:LinkButton>
        </p>
        <hr style="margin-left: 200px; width: 1200px" />
        <p style="background-color: darkgreen; margin-left: 200px; width: 1200px">
            <asp:CheckBox ID="ckbSelectallRow" runat="server" Text="全选" AutoPostBack="True" OnCheckedChanged="ckbSelectallRow_CheckedChanged" CssClass="auto-style12" />
            <asp:TextBox ID="txtQuery" runat="server" Width="100px" CssClass="auto-style12"></asp:TextBox>
            <asp:DropDownList ID="ddlDepartment" runat="server" Width="100px" CssClass="auto-style12" DataSourceID="SqlDataSource1" DataTextField="FDpName" DataValueField="FID"></asp:DropDownList>
            <asp:Button ID="btnRefresh" runat="server" Text="查  询" OnClick="btnRefresh_Click" CssClass="auto-style12" />
            <asp:Button ID="btnSave" runat="server" Text="保  存" OnClick="btnSave_Click" Enabled="False" CssClass="auto-style12" />
            <asp:Button ID="btnFloatAmount" runat="server" Text="绩效浮动奖金" BackColor="#FFCC66" Width="109px" OnClick="btnFloatAmount_Click" CssClass="auto-style13" Height="31px" BorderWidth="1px" />
            <asp:Button ID="btnExportToFile" runat="server" Text="导出文件" CssClass="auto-style12" OnClick="btnExportToFile_Click" Visible="false" />
        </p>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:PANASIANConnectionString %>"
            SelectCommand="Select 0 FID,'' FDpName Union All Select Distinct FID,FDpName From FY_Performance_Department"></asp:SqlDataSource>
        <p style="background-color: darkgreen; color: lightyellow; margin-left: 200px; width: 1200px">
            <strong style="font-weight: bold; margin-right: 100px">部门统计</strong>
            <strong>总人数：</strong><asp:Label ID="labDepCount" runat="server" Text="0" CssClass="auto-style4"></asp:Label>
            <strong style="margin-right: 15px">|</strong>
            <strong>未完成绩效：</strong><asp:Label ID="lkbNoPer" runat="server" Text="0" CssClass="auto-style4" ForeColor="Orange"></asp:Label>
            <strong style="margin-right: 15px">|</strong>
            <strong>未完成复评：</strong><asp:LinkButton ID="lkbNoRepet" runat="server" Text="0" CssClass="auto-style4" ForeColor="Orange" OnClick="lkbNoRepet_Click"></asp:LinkButton>
            <strong style="margin-right: 15px">|</strong>
            <strong>未完成自评：</strong><asp:LinkButton ID="lkbNoSelf" runat="server" Text="0" CssClass="auto-style4" ForeColor="Orange" OnClick="lkbNoSelf_Click"></asp:LinkButton>
        </p>
        <hr style="margin-left: 200px; width: 1200px" />
        <asp:GridView ID="GridView1" runat="server" CssClass="auto-style14" Width="1200px" AutoGenerateColumns="False" CellPadding="3" BackColor="White"
            BorderColor="#CCCCCC" BorderStyle="Double" BorderWidth="1px" RowHeaderColumn="FID" ShowFooter="True" HorizontalAlign="Center">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkRow" runat="server" AutoPostBack="True" OnCheckedChanged="chkRow_CheckedChanged" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="序号">
                    <ItemTemplate>
                        <asp:Label runat="server" Text="<%# Container.DataItemIndex + 1 %>" Width="15px"> </asp:Label>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="10px" Wrap="False" />
                </asp:TemplateField>
                <asp:BoundField DataField="FID" HeaderText="FID">
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" Width="30px" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="FEmCode" HeaderText="编码">
                    <ItemStyle HorizontalAlign="Center" Width="50px" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="FEmName" HeaderText="姓名">
                    <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="FDpName" HeaderText="部门">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="FBaseExamine" HeaderText="原考核基数" DataFormatString="{0:F1}">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Width="60px" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="FCoefficient" HeaderText="绩效系数">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Width="60px" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="FBaseResult" HeaderText="基数结果" DataFormatString="{0:F1}">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Width="60px" Wrap="False" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="缺勤天数">
                    <ItemTemplate>
                        <asp:TextBox HorizontalAlign="Center" ID="txtAcAttend" runat="server" Height="20px" Width="50px" ReadOnly="True" Wrap="False">0</asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Width="80px" Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="迟到早退">
                    <ItemTemplate>
                        <asp:TextBox ID="txtAbsented" runat="server" Height="20px" Width="50px" ReadOnly="True">0</asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle HorizontalAlign="Center" Width="60px" Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="奖罚金额">
                    <ItemTemplate>
                        <asp:TextBox ID="txtFAmount" runat="server" Height="20px" Width="50px" ReadOnly="True" Wrap="False">0</asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Width="60px" Wrap="False" />
                </asp:TemplateField>
                <asp:BoundField DataField="FFloatAmount" HeaderText="绩效浮动奖金" DataFormatString="{0:F1}">
                    <HeaderStyle Wrap="False" />
                    <ItemStyle Width="80px" Wrap="False" HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="备注">
                    <ItemTemplate>
                        <asp:TextBox ID="txtRemark" runat="server" BorderStyle="None" Height="30px" TextMode="MultiLine" Width="350px" ReadOnly="True"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle Wrap="False" />
                </asp:TemplateField>
            </Columns>
            <FooterStyle BackColor="White" ForeColor="#000066" CssClass="FooterStyle"/>
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" Wrap="False" />
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#007DBB" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#00547E" />
        </asp:GridView>
    </form>
</body>
</html>
