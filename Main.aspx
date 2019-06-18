<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="AccuLynx_Code_Challenge.Main" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="Main_Style_Sheet.css" />
    <title>Main.aspx</title>
</head>
<body>
    <form id="form1" runat="server">
            <div><asp:Label runat="server" Text="AccuLynx Code Challenge" CssClass="title"></asp:Label></div>
            <div>
            <asp:Label ID="Label1" runat="server" CssClass="Username"></asp:Label>
                <asp:Menu ID="Menu1" CssClass="menuBar" runat="server" BackColor="#F7F6F3" Orientation="Horizontal" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="1em" ForeColor="#7C6F57" StaticSubMenuIndent="10px">
                    <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <DynamicMenuStyle BackColor="#F7F6F3" />
                    <DynamicSelectedStyle BackColor="#5D7B9D" />
                    <Items>
                        <asp:MenuItem NavigateUrl="~/My_Questions.aspx" Text="My Questions" Value="My Questions"></asp:MenuItem>
                        <asp:MenuItem Text="Help" Value="Help" NavigateUrl="~/Help.aspx"></asp:MenuItem>
                    </Items>
                    <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <StaticSelectedStyle BackColor="#5D7B9D" />
                </asp:Menu>
            </div>
        <div>
            <!--This is where we will pull in your info!-->
            
        </div>
    </form>
</body>
</html>
