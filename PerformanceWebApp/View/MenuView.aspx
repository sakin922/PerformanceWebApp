<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuView.aspx.cs" Inherits="PerformanceWebApp.View.MenuView" MaintainScrollPositionOnPostback="True" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .form {
            width: 1200px;
            margin-left: 100px;
        }

        .topArea {
            font-family: 'Microsoft YaHei UI';
            font-weight: bold;
            border-width:1px;
            font-size: small;
            color: bisque;
            background-color: #009933;
            height: 100px;
            width: 1200px;
            text-align:left;
        }

        .lbtnReLogin {
            font-size: medium;
            color: darkblue;
            margin-left: 500px;
        }

        .labelmargin {
            margin-left: 10px;
            margin-right: 20px;
        }

        .dropDownList {
            margin-left: 10px;
            margin-right: 10px;
        }

        .linkButton-nav {
            font-weight: bold;
            font-family: 'Microsoft YaHei UI';
            font-size: x-large;
        }

        .td-Margin {
            height: 50px;
            text-align: left;
        }

        .table-form2 {
            width: 850px;
        }

        .span {
            font-family: 'Microsoft YaHei UI';
            font-weight: bold;
            font-size: medium;
            margin-right: 20px;
        }

        .gridView1 {
            width: 880px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="form">
        <div class="topArea" style="border: solid;border-width:1px;">
            <span>部门</span>
            <asp:Label ID="labDepartment" runat="server" CssClass="labelmargin"></asp:Label>
            <span>职务</span>
            <asp:Label ID="labPost" runat="server" CssClass="labelmargin"></asp:Label>
            <asp:Label ID="labPolevel" runat="server" Visible="False"></asp:Label>
            <span>编号</span>
            <asp:Label ID="labAccount" runat="server" CssClass="labelmargin"></asp:Label>
            <span>员工</span>
            <asp:Label ID="labEmName" runat="server" CssClass="labelmargin"></asp:Label>
            <span>复评人</span>
            <asp:Label ID="labExamer" runat="server" CssClass="labelmargin"></asp:Label>
            <asp:LinkButton ID="lbtnReLogin" runat="server" OnClick="lbtnReLogin_Click" CssClass="lbtnReLogin">注销</asp:LinkButton>
            <div>
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            </div>
        </div>
        <hr />
        <table style="width: 1150px">
            <tr>
                <td style="width: 300px; border: solid;border-width:1px;">
                    <table style="width: 300px;">
                        <tr>
                            <td style="height: 80px; vertical-align: top">
                                <div style="margin-bottom: 35px">
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <span>当前考核周期</span>
                                            <asp:DropDownList ID="ddlExYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlExYear_SelectedIndexChanged" CssClass="dropDownList">
                                                <asp:ListItem>2020</asp:ListItem>
                                            </asp:DropDownList>
                                            <span>年</span>
                                            <asp:DropDownList ID="ddlExMonth" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlExMonth_SelectedIndexChanged" CssClass="dropDownList">
                                                <asp:ListItem Value="1">1月</asp:ListItem>
                                                <asp:ListItem Value="2">2月</asp:ListItem>
                                                <asp:ListItem Value="3">3月</asp:ListItem>
                                                <asp:ListItem Value="4">4月</asp:ListItem>
                                                <asp:ListItem Value="5">5月</asp:ListItem>
                                                <asp:ListItem Value="6">6月</asp:ListItem>
                                                <asp:ListItem Value="7">7月</asp:ListItem>
                                                <asp:ListItem Value="8">8月</asp:ListItem>
                                                <asp:ListItem Value="9">9月</asp:ListItem>
                                                <asp:ListItem Value="10">10月</asp:ListItem>
                                                <asp:ListItem Value="11">11月</asp:ListItem>
                                                <asp:ListItem Value="12">12月</asp:ListItem>
                                            </asp:DropDownList>
                                            <span>月</span>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td class="td-Margin">
                                <asp:LinkButton ID="lbtnUnDire" runat="server" OnClick="lbtnUnDire_Click" CommandName="lbtnUnDire" CssClass="linkButton-nav">主管级以下绩效考核表</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="td-Margin">
                                <asp:LinkButton ID="lbtnDire" runat="server" OnClick="lbtnDire_Click" CommandName="lbtnDire" CssClass="linkButton-nav">主管级以上绩效考核表</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="td-Margin">
                                <asp:LinkButton ID="lbtnMyGrade" runat="server" OnClick="lbtnMyGrade_Click" CssClass="linkButton-nav">待复核评分</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="td-Margin">
                                <asp:LinkButton ID="lbtnDaiGrade" runat="server" OnClick="lbtnDaiGrade_Click" Visible="False" CssClass="linkButton-nav">代评分信息录入</asp:LinkButton>

                            </td>
                        </tr>
                        <tr>
                            <td class="td-Margin">
                                <asp:LinkButton ID="lbtnBaseExam" runat="server" OnClick="lbtnBaseExam_Click" Visible="False" CssClass="linkButton-nav">员工原考核基数管理</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="td-Margin">
                                <asp:LinkButton ID="lbtnExamManager" runat="server" OnClick="lbtnExamManager_Click" Visible="False" CssClass="linkButton-nav">绩效考核数据录入</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="td-Margin">
                                <asp:LinkButton ID="lbtnExamReport" runat="server" OnClick="lbtnExamReport_Click" Visible="False" CssClass="linkButton-nav">绩效考核信息报表</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td class="td-Margin">
                                <asp:LinkButton ID="lbtnEmpManager" runat="server" OnClick="lbtnEmpManager_Click" Visible="False" CssClass="linkButton-nav">员工信息管理</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 850px; vertical-align: top; border: solid;border-width:1px;" >
                    <h3 style="margin-left: 20px">历史考核记录查询</h3>
                    <hr />
                    <div>
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="dropDownList"></asp:DropDownList>
                        <span class="span">年</span>
                        <asp:Button ID="btnRefresh" runat="server" Text="查  询" OnClick="btnRefresh_Click" />
                        <br />
                        <asp:Label ID="labMessage" runat="server" CssClass="span"></asp:Label>
                    </div>
                    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False" HorizontalAlign="Center" OnRowCommand="GridView1_RowCommand"
                        BackColor="White" BorderColor="White" BorderStyle="Ridge" BorderWidth="1px" CellPadding="3" CellSpacing="1" GridLines="Both" CssClass="gridView1">
                        <Columns>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnItemCheck" runat="server" CommandName="查看">查 看</asp:LinkButton>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="FID" HeaderText="行号" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FYear" HeaderText="年份">
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FMonth" HeaderText="月份">
                                <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FEmName" HeaderText="自评" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FSelfSum" HeaderText="自评总分" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FExmName" HeaderText="复评" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="FRepeSum" HeaderText="复评总分" ReadOnly="True" Visible="False" />
                            <asp:BoundField DataField="FExamineSum" HeaderText="考核总分" ReadOnly="True" Visible="False" />
                            <asp:BoundField DataField="FStatus" HeaderText="单据状态" ReadOnly="True">
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#C6C3C6" ForeColor="Black" />
                        <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#E7E7FF" />
                        <PagerStyle BackColor="#C6C3C6" ForeColor="Black" HorizontalAlign="Right" />
                        <RowStyle BackColor="#DEDFDE" ForeColor="Black" />
                        <SelectedRowStyle BackColor="#9471DE" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#594B9C" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#33276A" />
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
