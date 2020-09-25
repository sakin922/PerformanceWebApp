<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="PerformanceWebApp.View.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style type="text/css">
        .auto-style1 {
            height: 751px;
        }

        .auto-style2 {
            margin-left: 409px;
        }

        .auto-style3 {
            height: 95px;
        }

        .auto-style4 {
            text-align: center;
        }

        .auto-style6 {
            width: 100%;
            height: 276px;
        }

        .auto-style8 {
            width: 344px;
            height: 23px;
        }

        .auto-style10 {
            text-align: right;
            width: 344px;
            height: 45px;
        }

        .auto-style11 {
            height: 45px;
            text-align: left;
        }

        .auto-style13 {
            height: 86px;
        }

        .auto-style15 {
            height: 23px;
        }

        .auto-style16 {
            text-align: right;
            width: 344px;
            height: 86px;
        }

        .auto-style18 {
            width: 400px;
            height: 54px;
        }

        .auto-style19 {
            width: 612px;
            height: 54px;
        }

        .auto-style20 {
            height: 40px;
            text-align: center;
        }

        .auto-style21 {
            width: 270px;
            height: 54px;
        }

        .auto-style22 {
            width: 296px;
            height: 54px;
        }
        .auto-style23 {
            height: 53px;
        }
        .auto-style24 {
            color: #006600;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" class="auto-style1" aria-autocomplete="none" dir="auto">
        <div class="auto-style3">
        </div>
        <div class="auto-style4">
            <asp:Panel ID="Panel1" runat="server" BackColor="White" CssClass="auto-style2" Height="485px" Width="731px">
                <table class="auto-style6">
                    <tr>
                        <td class="auto-style8"></td>
                        <td class="auto-style15"></td>
                    </tr>
                    <tr>
                        <td class="auto-style16"></td>
                        <td class="auto-style13"></td>
                    </tr>
                    <tr>
                        <td class="auto-style10">
                            <asp:Label ID="Label1" runat="server" Font-Size="X-Large" Text="账号(工号)："></asp:Label>
                        </td>
                        <td class="auto-style11">
                            <asp:TextBox ID="txtUser" runat="server" Font-Size="X-Large" Width="165px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUser" Display="Dynamic" ErrorMessage="账号不能为空" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style10">
                            <asp:Label ID="Label2" runat="server" Font-Size="X-Large" Text="密码(身份证后六位)："></asp:Label>
                        </td>
                        <td class="auto-style11">
                            <asp:TextBox ID="txtPwd" runat="server" Font-Size="X-Large" Width="165px" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPwd" ErrorMessage="密码不能为空" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <div class="auto-style20">
                    <br />
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </div>
                <table>
                    <tr>
                        <td class="auto-style19"></td>
                        <td class="auto-style21">
                            <asp:Button ID="btnLogin" runat="server" Font-Size="Large" Text="登  陆" OnClick="btnLogin_Click" />
                        </td>
                        <td class="auto-style22">
                            <asp:Button ID="btnCancel" runat="server" Font-Size="Large" Text="取  消" OnClick="btnCancel_Click" />
                        </td>
                        <td class="auto-style18"></td>
                    </tr>
                </table>
                <div class="auto-style23"><em>
                    <asp:Label ID="labMessage" runat="server" CssClass="auto-style24" Font-Bold="False" Font-Size="X-Large"></asp:Label>
                    </em></div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
