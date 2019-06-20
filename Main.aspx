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
            <div>
            <asp:Button ID="New_Quesitons" runat="server" Text="Get New Questions" OnClick="New_Quesitons_Click"/>
                <asp:Button ID="Get_My_Questions" runat="server" Text="My Questions" OnClick="Get_My_Questions_Click" />
                </div>
            <br />
            <div>
                <asp:GridView ID="Stack_Quesitons" runat="server" AutoGenerateColumns="False" DataKeyNames="Question_ID" DataSourceID="StackQuestions" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:HyperLinkField DataTextField="Question_ID" DataNavigateUrlFields="Question_ID" HeaderText="Question_ID" DataNavigateUrlFormatString="~/My_Questions.aspx?ID={0}" SortExpression="Question_ID" />
                        <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title" />
                        <asp:BoundField DataField="Current_Owner" HeaderText="Current_Owner" SortExpression="Current_Owner" />                     
                    </Columns>
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

                <asp:SqlDataSource ID="StackQuestions" runat="server" ConnectionString="<%$ ConnectionStrings:AcculynxConnectionString %>" SelectCommand="SELECT TOP (10) Question_ID, Title, Current_Owner FROM Question_List"></asp:SqlDataSource>

                <asp:GridView ID="My_Questions" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="GetUserQuestions" AllowPaging="True" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" Visible="false" />
                        <asp:BoundField DataField="Question_ID" HeaderText="Question_ID" SortExpression="Question_ID" />
                        <asp:BoundField DataField="Question_Information" HeaderText="Question_Information" SortExpression="Question_Information" />
                        <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username"/>
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <EmptyDataTemplate>There is no data for you, please look at some questions!</EmptyDataTemplate>
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
                <asp:SqlDataSource ID="GetUserQuestions" runat="server" ConnectionString="<%$ ConnectionStrings:AcculynxConnectionString %>" SelectCommand="SELECT Questions.ID, Questions.Question_ID, Questions.Question_Information, Users.Username FROM Questions INNER JOIN Users ON Questions.User_ID = Users.ID">
                </asp:SqlDataSource>
            </div>
            <asp:Label ID="ErrorMsg" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="UserID" runat="server" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>
