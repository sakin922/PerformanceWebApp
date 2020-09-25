<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnExecutiveExamine.aspx.cs" Inherits="PerformanceWebApp.View.UnExecutiveExamine" MaintainScrollPositionOnPostback="True" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .auto-style1 {
            font-size: xx-large;
            font-family: 微软雅黑;
            height: 60px;
            margin-top: 5px;
            text-align:right;
        }

        .auto-style5 {
            margin-left: 100px;
            width: 1200px;
        }

        .auto-style7 {
            width:100%;
        }

        .auto-style11 {
            width: 100%;
        }

        .auto-style12 {
            border-style: solid;
            height: 33px;
            font-size: smaller;
        }

        .auto-style13 {
            width: 82px;
            font-size: medium;
        }

        .auto-style14 {
            width: 1207px;
            font-size: smaller;
        }

        .auto-style15 {
            width: 142px;
            height: 33px;
            font-size: medium;
        }

        .auto-style16 {
            width: 82px;
            height: 33px;
        }

        .auto-style17 {
            height: 33px;
        }

        .auto-style19 {
            font-size: medium;
        }

        .auto-style20 {
            height:120px;
            width: 100%;
        }

        .auto-style21 {
            width: 90px;
            font-family:'Microsoft YaHei';
            font-size:large;
        }

        .auto-style23 {
            width: 142px;
            font-size: medium;
        }

        .auto-style25 {
            text-align:right;
        }

        .auto-style26 {
            margin-left:20px;
        }

        .auto-style27 {
            font-family:'Microsoft YaHei';
            font-size: large;
        }

        .auto-style28 {
            font-size: x-large;
            margin-right: 900px;
            margin-left: 20px;
        }
        .auto-style29 {
            margin-left:200px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="auto-style5">
        <div class="auto-style1">
            <strong>
                <asp:LinkButton ID="lbtnBack" runat="server" CssClass="auto-style27" OnClick="lbtnBack_Click">返回</asp:LinkButton>
            </strong>
        </div>
        <div class="auto-style25">
            <strong>
            <span>状态：</span>
            <asp:Label ID="labStatus" runat="server"></asp:Label>
            </strong>
        </div>
        <hr />
        <div class="auto-style27">
            <span>考核周期：</span>
            <asp:Label ID="labYear" runat="server"></asp:Label>
            <span>年</span>
            <asp:Label ID="labMonth" runat="server"></asp:Label>
            <span>月</span>
            <span>被考核人：</span>
            <asp:Label ID="labEmName" runat="server"></asp:Label>
            <asp:Label ID="labEmCode" runat="server" Visible="False"></asp:Label>
            <span>复评人：</span>
            <asp:Label ID="labReExName" runat="server"></asp:Label>
            <span><strong class="auto-style26" style="color:deepskyblue">|</strong></span>
            <asp:Button ID="btnCommit" runat="server" Text="提交" Width="68px" CssClass="auto-style26 auto-style29" OnClick="btnCommit_Click" />
            <asp:Button ID="btnModify" runat="server" Text="修改" Width="69px" CssClass="auto-style26" OnClick="btnModify_Click" />
        </div>

        <br />
        <div class="auto-style7">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Caption="绩效考核表" CellPadding="4" Width="100%" ForeColor="#333333" GridLines="Both" ShowFooter="True">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" BorderStyle="Solid" BorderWidth="1px" />
                <Columns>
                    <asp:BoundField DataField="FID" HeaderText="FID" ReadOnly="True" Visible="False" />
                    <asp:BoundField DataField="FProject" HeaderText="项目" ReadOnly="True">
                        <HeaderStyle VerticalAlign="Top" HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FEvaluate" HeaderText="评价因素" ReadOnly="True">
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FProject_1" HeaderText="项目1" ReadOnly="True" />
                    <asp:BoundField DataField="FProject_2" HeaderText="项目2" ReadOnly="True" />
                    <asp:BoundField DataField="FProject_3" HeaderText="项目3" ReadOnly="True" />
                    <asp:BoundField DataField="FProject_4" HeaderText="项目4" ReadOnly="True" />
                    <asp:BoundField DataField="FProject_5" HeaderText="项目5" ReadOnly="True" />
                    <asp:TemplateField HeaderText="自评分数">
                        <EditItemTemplate>
                            <asp:TextBox ID="TxtSelfGradeEdit" runat="server" Height="16px" Text='<%# Bind("FSelfGrade") %>' TextMode="Number" Width="40px"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TxtSelfGrade" runat="server" Height="15px" Width="40px" Wrap="False" AutoPostBack="True" OnTextChanged="TxtSelfGrade_TextChanged" TextMode="Number"></asp:TextBox>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="复评分数">
                        <EditItemTemplate>
                            <asp:TextBox ID="TxtExGradeEdit" runat="server" Height="15px" Text='<%# Bind("FRepeGrade") %>' TextMode="Number" Width="40px"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:TextBox ID="TxtExGrade" runat="server" Height="15px" Width="40px" AutoPostBack="True" OnTextChanged="TxtExGrade_TextChanged" TextMode="Number"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" ForeColor="White" Font-Bold="True" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" VerticalAlign="Middle" />
                <RowStyle ForeColor="#333333" BackColor="#F7F6F3" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
            <table class="auto-style11">
                <tr>
                    <td class="auto-style14" style="border-style: solid; border-width: 1px">考核等级：90（优秀） 90&gt;评分≥80（良好） 80&gt;评分≥70（合格） 70&gt;评分≥60（有待改进） 60&gt;评分（不合格）</td>
                    <td class="auto-style23" style="border-style: solid; border-width: 1px;">各项分数合计</td>
                    <td class="auto-style13" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtSelfSum" runat="server" BackColor="#FFFF66" Height="32px" ReadOnly="True" TextMode="Number" Width="80px" Wrap="False"></asp:TextBox>
                    </td>
                    <td class="auto-style19" style="border-style: solid; border-width: 1px">
                        <asp:TextBox ID="txtReGradeSum" runat="server" BackColor="#FFFF66" Height="32px" ReadOnly="True" TextMode="Number" Width="80px" Wrap="False"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style12" style="border-style: ridge; border-width: 1px"><span class="auto-style19">考核得分计算方法： 评定结果＝员工自评分×30%＋复核评分×70%</span></td>
                    <td class="auto-style15" style="border-style: ridge; border-width: 1px;">考核总分</td>
                    <td class="auto-style16" style="border-style: ridge; border-width: 1px"></td>
                    <td class="auto-style17" style="border-style: ridge; border-width: 1px">
                        <asp:TextBox ID="txtTotalSum" runat="server" BackColor="Yellow" Height="32px" ReadOnly="True" TextMode="Number" Width="80px" Wrap="False"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table class="auto-style20">
                <tr>
                    <td class="auto-style21" style="border-style: ridge; border-width: 1px; text-align: center;">绩效评语</td>
                    <td >
                        <asp:TextBox ID="txtExmMessage" runat="server" BorderStyle="Solid" BorderWidth="1px" Height="120px" Font-Size="Larger" TextMode="MultiLine" Width="100%" ReadOnly="True"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <br />
            <asp:HiddenField ID="hiddenUrl" runat="server" />
        </div>
    </form>
</body>
</html>
