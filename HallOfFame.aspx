<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HallOfFame.aspx.cs" Inherits="AccuLynx_Code_Challenge.HallOfFame" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" href="Main_Style_Sheet.css" />
    <title>Hall Of Fame</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="ErrorMsg" runat="server" Visible="false" ForeColor="RED"></asp:Label>
        </div>
            <div style="height: 54px"><asp:Label runat="server" Text="AccuLynx Code Challenge" CssClass="title"></asp:Label></div>
            <div style="width:100%; height: 74px;">
            <asp:Label ID="Label1" runat="server" CssClass="Username"></asp:Label>
                <asp:Menu ID="Menu1" CssClass="menuBar" runat="server" BackColor="#F7F6F3" Orientation="Horizontal" DynamicHorizontalOffset="2" Font-Names="Verdana" Font-Size="1em" ForeColor="#7C6F57" StaticSubMenuIndent="10px">
                    <DynamicHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <DynamicMenuStyle BackColor="#F7F6F3" />
                    <DynamicSelectedStyle BackColor="#5D7B9D" />
                    <Items>
                        
                        <asp:MenuItem NavigateUrl="~/Main.aspx" Text="Home" Value="Home"></asp:MenuItem>
                        
                        <asp:MenuItem Text="Help" Value="Help" NavigateUrl="~/Help.aspx"></asp:MenuItem>
                        <asp:MenuItem NavigateUrl="~/HallOfFame.aspx" Text="Top Users" Value="Top Users"></asp:MenuItem>
                    </Items>
                    <StaticHoverStyle BackColor="#7C6F57" ForeColor="White" />
                    <StaticMenuItemStyle HorizontalPadding="5px" VerticalPadding="2px" />
                    <StaticSelectedStyle BackColor="#5D7B9D" />
                </asp:Menu>
            </div>
        <div>
            <asp:Label ID="Label2" runat="server" Text="TOP USERS" Font-Size="2.5em"></asp:Label>
            </div>
        <div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="4" DataKeyNames="ID" DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" />
                    <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
                    <asp:BoundField DataField="Created_Date" HeaderText="Created_Date" SortExpression="Created_Date" />
                    <asp:BoundField DataField="Correct_Answers" HeaderText="Correct_Answers" SortExpression="Correct_Answers" />
                    <asp:BoundField DataField="Total_Score" HeaderText="Total_Score" SortExpression="Total_Score" />
                </Columns>
                <EmptyDataTemplate>There is no users yet! Answer some questions!</EmptyDataTemplate>
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:AcculynxConnectionString %>" SelectCommand="SELECT TOP (5) ID, Username, Created_Date, Correct_Answers, MAX(Total_Score) AS Total_Score FROM Users GROUP BY ID, Username, Created_Date, Correct_Answers ORDER BY Total_Score DESC, ID DESC, Username DESC, Created_Date DESC, Correct_Answers DESC"></asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
